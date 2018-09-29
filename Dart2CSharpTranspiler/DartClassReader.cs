using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Transpiler
{
    public static class DartClassReader
    {
        public static DartFile Construct(string filename, string folder, string dart)
        {

            dart = dart.RemoveComments()
                       .RemoveLines()
                       .CleanDoubleSpace();

            var file = new DartFile()
            {
                Name = filename,
                Classes = new List<DartClass>(),
                Imports = GetImports(dart),
                Folder = folder
            };

            foreach (var section in GetClasses(dart))
            {
                var name = GetClassName(section.Value);
                var type = GetVisibilityType(name);
                var constraints = GetGeneric(name);
                var clearName = GetClearName(name);
                var sections = GetSections(section.Value);
                ProcessSections(sections, clearName);

                file.Classes.Add(new DartClass()
                {
                    Extends = GetExtends(section.Value, name),
                    Implements = GetImplements(section.Value, name),
                    Sections = sections,
                    RawName = name,
                    Name = clearName,
                    VisibilityType = type,
                    IsAbstract = IsAbstract(dart),
                    IsImmutable = IsImmutable(dart),
                    Raw = section.Value.Trim(),
                    Filename = filename,
                    GenericConstraints = constraints
                });
            }

            // File level properties, typedefs, enums, methods etc.
            file.Sections = GetFileLevelSections(dart, file);
            ProcessSections(file.Sections, "");

            // Combine all sections
            var sectionList = new List<Section>();
            sectionList.AddRange(file.Sections);
            foreach (var item in file.Classes)
                sectionList.AddRange(item.Sections);

            return file;
        }

        public static IList<Section> GetFileLevelSections(string dart, DartFile file)
        {
            foreach (var item in file.Classes)
                dart = dart.Replace(item.Raw, "");

            foreach (var import in file.Imports)
                dart = dart.Replace(import.Raw, "");

            return GetSections("{ " + dart.Trim() + " }");
        }

        public static bool IsAbstract(string dart)
        {
            return dart.Trim().StartsWith("abstract ");
        }

        public static bool IsImmutable(string dart)
        {
            return dart.Trim().StartsWith("@immutable ");
        }

        public static IList<DartImport> GetImports(string dart)
        {
            var list = new List<DartImport>();

            var tmp = dart;
            var marker = "import ";
            while (tmp.IndexOf(marker) != -1)
            {

                var index = tmp.IndexOf(marker);
                var value = tmp.Substring(index, tmp.IndexOf(';', index) + 1 - index);

                var import = new DartImport()
                {
                    Raw = value
                };

                var endIndex = value.LastIndexOf("'");
                var name = value.Substring(value.IndexOf(marker) + marker.Length, endIndex - value.IndexOf(marker) - marker.Length);
                import.Complete = name.Trim('\'');

                // Get split ScopedVariables
                var end = value.Substring(endIndex + 1).Replace(";", "").Trim();
                if (end.Contains("show "))
                    import.ScopedVariables = end.Substring(end.IndexOf("show ") + 5).Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (end.Contains("as "))
                {
                    import.Alias = end.Substring(end.IndexOf("as ") + 3).TrimEnd(';');
                    if (import.Alias.Contains(' '))
                        import.Alias = import.Alias.Substring(0, import.Alias.IndexOf(' '));
                }

                // Split Complete
                if (!import.Complete.Contains(":"))
                {
                    import.Type = Type.File;
                    import.Name = import.Complete;
                }
                else
                {
                    var start = import.Complete.Substring(0, import.Complete.IndexOf(":"));
                    switch (start)
                    {
                        case "dart":
                            import.Type = Type.Dart;
                            break;
                        case "package":
                            import.Type = Type.Package;
                            break;
                        default:
                            throw new Exception($"Unknown prefix to import: {start}");
                    }

                    import.Name = import.Complete.Substring(import.Complete.IndexOf(":") + 1);
                }

                list.Add(import);

                tmp = tmp.Substring(0, index) + tmp.Substring(tmp.IndexOf(';', index) + 1);
            }

            return list;
        }

        public static void ProcessSections(IList<Section> sections, string className)
        {
            foreach (var section in sections)
            {
                var value = section.Raw.Replace(" (", "(").Trim();

                if (value.StartsWith("export "))
                {
                    section.Type = SectionType.Export;
                    section.ReturnType = "";
                    section.Parameters = new List<Parameter>();
                    section.Name = value.Split(' ')[1];
                    continue;
                }

                if (value.StartsWith("@visibleForTesting"))
                {
                    var startIndex = value.IndexOf("@visibleForTesting");
                    value = value.Substring(startIndex + 18);
                }

                if (value.Contains("@Deprecated("))
                {
                    var startIndex = value.IndexOf("@Deprecated(") + 12;
                    section.Attribute = section.Attribute | MethodAttribute.Deprecated;
                    section.DeprecatedMessage = value.Substring(startIndex, value.IndexOf(")", startIndex) - startIndex);
                    value = value.Substring(0, startIndex - 12) + value.Substring(value.IndexOf(")", startIndex) + 1);
                }

                value = value.Trim();

                if (value.StartsWith("static "))
                {
                    section.IsStatic = true;
                    value = value.Substring(7);
                }

                if (value.StartsWith("const "))
                {
                    section.IsConstant = true;
                    value = value.Substring(6);
                }

                if (value.StartsWith("factory "))
                {
                    section.IsFactory = true;
                    value = value.Substring(8);
                }

                var split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (split[0].Contains("(") && split[0].Substring(0, split[0].IndexOf("(")).Replace("._", "") == className) // e.g. Animation( or _Animation(
                {
                    section.Type = SectionType.Constructor;
                    section.Name = split[0].Substring(0, split[0].IndexOf("("));
                }
                else if (split[0].Contains("(")  // e.g. Animation.unbounded(										
                    && split[0].Contains(".")
                    && split[0].IndexOf("(") > split[0].IndexOf(".")
                    && split[0].Substring(0, split[0].IndexOf("(")).Substring(0, split[0].IndexOf(".")) == className)
                {
                    section.Type = SectionType.Constructor;
                    section.Name = split[0].Substring(0, split[0].IndexOf("("));
                }
                else if (split.Length > 1 && split[1] == "operator")
                {
                    section.Name = split[1];
                    section.ReturnType = split[0];
                    section.Type = SectionType.Operator;
                }
                else if (value.IndexOf("(") > 0 && value.Substring(0, value.IndexOf("(")).Contains(" set ") || value.StartsWith("set "))
                {
                    section.Type = SectionType.PropertySet;

                    var index = 0;
                    foreach (var item in split)
                    {
                        if (item == "set")
                            break;

                        index++;
                    }

                    section.Name = split[index + 1];
                    if (section.Name.Contains("("))
                        section.Name = section.Name.Substring(0, section.Name.IndexOf("("));

                    var method = Util.GetContainerCode(value, '(', ')');

                    section.ReturnType = method.SplitViaCharacter(' ')[0];
                }
                else if (value.Contains(" get "))
                {
                    section.Type = SectionType.PropertyGet;
                    var index = 0;
                    foreach (var item in split)
                    {
                        if (item == "get")
                            break;

                        index++;
                    }
                    section.Name = split[index + 1];
                    if (section.Name.Contains(";"))
                        section.Name = section.Name.Substring(0, section.Name.IndexOf(";"));
                    else
                    {
                        // Auto assignment
                        section.RawCode = value.Substring(value.IndexOf(section.Name + " ") + section.Name.Length + 1);
                    }
                    section.ReturnType = GetReturnType(split, index - 1);
                }
                else if (value.StartsWith("typedef "))
                {
                    section.Type = SectionType.Typedef;
                    section.Name = split[2];
                }
                else if (value.StartsWith("enum "))
                {
                    section.Type = SectionType.Enum;
                    section.Name = value.Substring(value.IndexOf("enum ") + 5, value.IndexOf("{") - value.IndexOf("enum ") - 5);
                }
                else if (!value.Contains("(")
                     || (value.Contains("=")
                          && (value.IndexOf("(") > value.IndexOf("="))))
                {
                    section.Type = SectionType.Field;

                    var index = 0;
                    foreach (var item in split)
                    {
                        if (item.Contains(";"))
                            break;

                        if (item == "=")
                        {
                            index--;
                            break;
                        }

                        index++;
                    }

                    section.Name = split[index].TrimEnd(';');

                    section.ReturnType = GetReturnType(split, index - 1);
                }
                else
                {
                    section.Type = SectionType.Method;
                    var index = 0;
                    foreach (var item in split)
                    {
                        if (item.Contains("("))
                            break;

                        index++;
                    }
                    section.Name = split[index].Substring(0, split[index].IndexOf("("));

                    section.ReturnType = GetReturnType(split, index - 1);
                }

                foreach (var item in split)
                {
                    switch (item.Trim())
                    {
                        case "@override":
                            section.Attribute = section.Attribute | MethodAttribute.Override;
                            break;
                        case "@protected":
                            section.Attribute = section.Attribute | MethodAttribute.Protected;
                            break;
                        case "@immutable":
                            section.Attribute = section.Attribute | MethodAttribute.Immutable;
                            break;
                        case "abstract":
                            section.IsAbstract = true;
                            break;
                        case "final":
                            section.IsFinal = true;
                            break;
                    }
                }

                section.Name = section.Name.Trim();

                // Constructor Super Call
                if (value.Contains("super("))
                    section.SuperCall = GetParameters(value.Substring(value.IndexOf("super(")), true).parameters;

                if (section.Type == SectionType.Constructor)
                    section.ConstructorRawCode = GetConstructorCode(value);

                // Get Visibility
                if (section.Name.StartsWith("_"))
                    section.VisibilityType = VisibilityType.Private;
                else
                    section.VisibilityType = VisibilityType.Public;

                // Get Lines of code
                if (section.Type == SectionType.Method
                    || section.Type == SectionType.PropertySet
                    || section.Type == SectionType.Constructor
                    || section.Type == SectionType.Enum)
                {
                    var openIndex = value.IndexOf(")") == -1 ? 0 : value.IndexOf(")");
                    var openBrackets = value.IndexOf("{", openIndex);
                    if (openBrackets == -1)
                        section.NoBody = true;
                    else
                    {
                        var methodBracketStart = 0;
                        if (value.IndexOf("(") != -1)
                        {
                            var parameterContainer = Util.GetContainerCode(value, '(', ')');
                            methodBracketStart = value.IndexOf(parameterContainer) + parameterContainer.Length;
                        }
                        section.RawCode = Util.GetContainerCode(value.Substring(methodBracketStart), '{', '}').Trim();
                    }
                }

                if (section.Type == SectionType.Field)
                {
                    if (value.Contains(" = "))
                    {
                        section.RawCode = value.Substring(value.IndexOf(" = ") + 3).Trim();
                    }
                    else
                        section.NoBody = true;
                }

                if (section.Type == SectionType.Constructor
                    || section.Type == SectionType.Method)
                {
                    if (section.Name == "showDialog<T>")
                        section.Name.ToString();

                    var (parameters, rawParameter) = GetParameters(section.Raw);
                    section.Parameters = parameters;
                    section.RawParameter = rawParameter;
                }
                else
                    section.Parameters = new List<Parameter>();
            }
        }

        private static string GetConstructorCode(string value)
        {
            var match = Regex.Matches(value, @"[\s\S]+?\(").First(); // Should always have at least 1.

            var index = 0;
            var openCount = 0;
            foreach (var c in value.Substring(match.Length))
            {
                if (c == ')' && openCount == 0)
                    break;

                if (c == ')' && openCount != 0)
                    openCount -= 1;

                if (c == '(')
                    openCount += 1;

                index++;
            }

            // Index is now just after the constructor, e.g. Animation()<- here
            value = value.Substring(index);

            if (value.Contains(":"))
            {
                value = value.Substring(value.IndexOf(':') + 1);

                if (value.Contains("super("))
                    value = value.Substring(0, value.LastIndexOf("super("));

                return value;
            }
            else
                return string.Empty;
        }

        private static string GetReturnType(string[] split, int startIndex)
        {
            var returnType = split[startIndex];

            var index = 0;
            if (returnType.Contains(">") && !returnType.Contains('<'))
                for (var i = startIndex - 1; i >= 0; i--)
                {
                    returnType = split[i] + returnType;
                    if (split[i].Contains('<'))
                        break;
                }

            if (returnType.Contains('<') && !returnType.Contains('>'))
                foreach (var item in split)
                {
                    if (index > startIndex)
                    {
                        returnType += item;
                        if (item.Contains('>'))
                            break;
                    }

                    index++;
                }

            return returnType;
        }

        public static (List<Parameter> parameters, string rawParameter) GetParameters(string raw, bool isSuper = false)
        {
            var list = new List<Parameter>();

            var processed = raw;

            if (processed.Contains("@Deprecated"))
            {
                // TODO: this will wipe out all @Deprecated.
                // It doesn't account for brackets internal to strings.
                // Multiple Scenarios:
                // (Widget child, @Deprecated('Message')double val)
                // (@Deprecated('Message'){ Widget child, double val })
                var startIndex = processed.IndexOf("@Deprecated");
                processed = processed.Substring(0, startIndex) + processed.Substring(processed.IndexOf(')', startIndex) + 1);
            }
            var match = Regex.Matches(processed, @"[\s\S]+?\(").First(); // Should always have at least 1.

            var index = 0;
            var openCount = 0;
            foreach (var c in processed.Substring(match.Length))
            {
                if (c == ')' && openCount == 0)
                    break;

                if (c == ')' && openCount != 0)
                    openCount -= 1;

                if (c == '(')
                    openCount += 1;

                index++;
            }

            var rawParmeter = processed.Substring(match.Length, index);

            var parameterString = rawParmeter;

            if (parameterString.Contains("{"))
            {
                // These are optional parameters
                var optional = parameterString.Substring(parameterString.IndexOf("{") + 1, parameterString.LastIndexOf("}") - 1 - parameterString.IndexOf("{"));
                Extract(optional.SplitViaCharacter(','), false);

                // Remove optional parameters from parameter string
                parameterString = parameterString.Substring(0, parameterString.IndexOf("{"))
                                + parameterString.Substring(parameterString.LastIndexOf("}") + 1);
            }

            // These are required parameters
            if (parameterString.Trim().Length > 0)
                Extract(parameterString.SplitViaCharacter(','), true);

            void Extract(string[] parameters, bool required)
            {
                // Combine parameters if accidentally split

                // Bizzaro parameters, e.g. T F(E e)
                var cleanedParameter = CleanParameters(parameters, '(', ')', " ", false);

                // Array as default value e.g. this.Something: const <int> [1, 2, 3]
                cleanedParameter = CleanParameters(cleanedParameter.ToArray(), '[', ']', ", ", true);

                List<string> CleanParameters(string[] parameterList, char start, char end, string sep, bool checkAssignment = false)
                {
                    var openParameter = false;
                    var newParameter = new List<string>();
                    var tmpJoin = "";
                    var assignment = false;

                    if (checkAssignment == false)
                        assignment = true;

                    foreach (var split in parameterList)
                    {
                        if (split.Contains(":") && checkAssignment)
                            assignment = true;

                        if (split.Contains(start) && !split.Contains(end) && assignment)
                        {
                            openParameter = true;
                            tmpJoin += split;
                        }
                        else if (openParameter && !split.Contains(end) && assignment)
                        {
                            tmpJoin += sep + split;
                        }
                        else if (openParameter && split.Contains(end) && assignment)
                        {
                            tmpJoin += sep + split;
                            openParameter = false;
                            newParameter.Add(tmpJoin);
                            tmpJoin = "";
                        }
                        else
                            newParameter.Add(split);
                    }

                    return newParameter;
                }

                parameters = cleanedParameter.ToArray();

                foreach (var parameter in parameters)
                {
                    if (string.IsNullOrWhiteSpace(parameter))
                        continue;

                    var value = parameter.Trim();

                    var isDeprecated = false;
                    var isFinal = false;

                    if (value.StartsWith("@Deprecated"))
                    {
                        value = value.Substring(value.IndexOf(')') + 1).Trim();
                        isDeprecated = true;
                    }

                    var isRequiredFlag = false;

                    if (value.StartsWith("@required "))
                    {
                        value = value.Substring(value.IndexOf("@required ") + 10);
                        isRequiredFlag = true;
                    }

                    if (value.StartsWith("final "))
                    {
                        value = value.Substring(value.IndexOf("final ") + 6);
                        isFinal = true;
                    }

                    var isOptional = value.StartsWith('[') || value.EndsWith(']');

                    value = value.TrimStart('[').TrimEnd(']').Replace(" =", "=");

                    var isCovariant = false;
                    var isNoType = false;

                    if (value.Contains("covariant "))
                    {
                        value = value.Replace("covariant ", "");
                        isCovariant = true;
                    }

                    value = value.Replace(":", ": ").Replace(" :", ":").Replace(", ", "");

                    var nextSplit = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var openBracket = false;
                    var newSplit = new List<string>();
                    var tmp = "";
                    foreach (var split in nextSplit)
                    {
                        if (split.Contains("(") && !split.Contains(")"))
                        {
                            openBracket = true;
                            tmp += split;
                        }
                        else if (openBracket && !split.Contains(')'))
                        {
                            tmp += " " + split;
                        }
                        else if (openBracket && split.Contains(')'))
                        {
                            tmp += " " + split;
                            openBracket = false;
                            newSplit.Add(tmp);
                            tmp = "";
                        }
                        else
                            newSplit.Add(split);
                    }

                    nextSplit = newSplit.ToArray();

                    var isAutoAssign = false;
                    string defaultValue = null;
                    string type = null;

                    var isDefaultAssign = false;
                    var splitIndexAssign = 0;
                    var count = 0;
                    foreach (var split in nextSplit)
                    {
                        if (split.Contains(':') || split.Contains('='))
                        {
                            isDefaultAssign = true;
                            splitIndexAssign = count;
                            break;
                        }
                        count++;
                    }

                    var name = "";
                    if (isDefaultAssign)
                    {
                        name = nextSplit[splitIndexAssign].Trim().TrimEnd(':').TrimEnd('=');
                        defaultValue = nextSplit[splitIndexAssign + 1];
                        if (splitIndexAssign > 0)
                            type = nextSplit[splitIndexAssign - 1].Trim();
                        else
                            isNoType = true;
                    }
                    else if (nextSplit.Count() == 2)
                    {
                        type = nextSplit[0].Trim();
                        name = nextSplit[1].Trim();
                    }

                    if (string.IsNullOrEmpty(name) && nextSplit.Length == 1)
                    {
                        name = nextSplit[0].Trim();
                        isNoType = true;
                    }

                    if (name.Contains("this."))
                    {
                        name = name.Replace("this.", "");
                        isAutoAssign = true;
                    }

                    var isValue = false;
                    string valueObject = null;

                    var isFlutterParam = false;

                    if (!string.IsNullOrEmpty(parameter) && string.IsNullOrEmpty(name))
                    {
                        // This part is pre-vetter Flutter parameter values
                        // Just to make sure the parameter extraction is working
                        // correctly
                        var p = parameter.Trim();
                        if (p == "dx * 2.0 - 1.0"
                            || p == "dy * 2.0 - 1.0"
                            || p == "value ?? TextEditingValue.empty"
                            || p == "void painter(Canvas canvas  Size size  double thumbOffset  double thumbExtent)")
                            isFlutterParam = true;

                        isValue = true;
                        valueObject = parameter.Trim();

                        if (!isFlutterParam)
                            isFlutterParam = true;
                    }

                    if (name == "'" || name == "WidgetBuilder>{" || name == "}" || name == ")" || (isAutoAssign == false && isNoType == false && isValue == false && string.IsNullOrEmpty(type) && isFlutterParam == false))
                        throw new Exception("A non auto assigned property, with no confirmed isNoType doesn't have a type");

                    if (isAutoAssign == false && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(defaultValue) && !isSuper)
                        throw new Exception("A null type must be auto-assigned");

                    list.Add(new Parameter()
                    {
                        IsOptional = !required || isOptional,
                        IsRequired = isRequiredFlag || required,
                        IsAutoAssign = isAutoAssign,
                        IsCovariant = isCovariant,
                        IsDeprecated = isDeprecated,
                        IsFinal = isFinal,
                        DefaultValue = defaultValue,
                        Name = name,
                        Value = valueObject,
                        IsValue = isValue,
                        Type = type
                    });

                }
            }

            return (list, rawParmeter);
        }

     
        public static string GetClearName(string name)
        {
            if (name.Contains("<"))
                name = name.Substring(0, name.IndexOf("<"));

            return name;
        }

        public static Extend GetExtends(string value, string className)
        {
            Extend extend = null;

            value = value.Substring(value.IndexOf(className) + className.Length);
            var keyword = " extends ";
            if (value.Contains(keyword))
            {
                var tmp = value.Substring(value.IndexOf(keyword) + keyword.Length);

                var otherKeyword = " implements ";
                if (tmp.Contains(otherKeyword))
                    tmp = tmp.Substring(0, tmp.IndexOf(otherKeyword));
                else if (tmp.Contains(" {"))
                    tmp = tmp.Substring(0, tmp.IndexOf(" {"));

                if (tmp.Contains(" with "))
                {
                    extend = new Extend()
                    {
                        Name = tmp.Substring(0, tmp.IndexOf(" with ")),
                        Mixins = tmp.Substring(tmp.IndexOf(" with ") + 6).Replace(" ", "").SplitViaCharacter(',').ToList()
                    };
                }
                else
                {
                    extend = new Extend()
                    {
                        Name = tmp
                    };
                }
            }

            return extend;
        }

        public static List<string> GetImplements(string value, string className)
        {
            var list = new List<string>();

            value = value.Substring(value.IndexOf(className) + className.Length);
            var keyword = " implements ";
            if (value.Contains(keyword))
            {
                var tmp = value.Substring(value.IndexOf(keyword) + keyword.Length);

                var otherKeyword = " extends ";
                if (tmp.Contains(otherKeyword))
                    tmp = tmp.Substring(0, tmp.IndexOf(otherKeyword));
                else if (tmp.Contains(" {"))
                    tmp = tmp.Substring(0, tmp.IndexOf(" {"));

                list.AddRange(tmp.Replace(" ", "").SplitViaCharacter(','));
            }

            return list;
        }

        public static IDictionary<string, string> GetGeneric(string name)
        {
            if (name.Contains("<"))
            {
                var list = new Dictionary<string, string>();

                name = name.Substring(name.IndexOf("<") + 1);
                name = name.Substring(0, name.LastIndexOf(">"));

                var split = name.SplitViaCharacter(',');

                foreach (var value in split)
                {
                    if (value.Contains(" extends "))
                    {
                        var n = value.Trim().Substring(0, value.Trim().IndexOf(" "));
                        var extends = value.Trim().Substring(value.Trim().IndexOf(" extends ") + 9);
                        list.Add(n.Trim(), extends.Trim());
                    }
                    else
                    {
                        list.Add(value, null);
                    }
                }

                return list;
            }

            return new Dictionary<string, string>();
        }

        public static VisibilityType GetVisibilityType(string name)
        {
            if (name.StartsWith("_"))
                return VisibilityType.Private;
            else
                return VisibilityType.Public;
        }

        public static Dictionary<int, string> GetClasses(string dart)
        {
            var list = new Dictionary<int, string>();
            var index = 0;

            var startPrefix = "class ";
            var opening = '{';
            var ending = '}';

            while (dart.IndexOf(startPrefix, index) != -1)
            {
                var startIndex = dart.IndexOf(startPrefix, index);

                // Find previous class definitions
                var found = false;
                var previousIndex = -1;
                while (!found)
                {
                    var checkIndex = startIndex + previousIndex;
                    if (checkIndex >= 0)
                    {
                        var previousChar = Convert.ToChar(dart.Substring(checkIndex, 1));

                        if (previousChar == ';' || previousChar == '}' || previousChar == ')')
                        {
                            startIndex = checkIndex + 1;
                            found = true;
                        }

                    }
                    else
                        found = true;

                    previousIndex--;
                }


                if (startIndex > 0)
                {
                    var enterIndex = dart.Substring(0, startIndex).LastIndexOf('\n');
                    if (enterIndex > 0)
                        startIndex = enterIndex + 1;
                }

                var endingFound = 0;
                var started = false;
                var length = 0;
                foreach (var c in dart.Substring(startIndex))
                {
                    if (c == opening)
                    {
                        endingFound -= 1;
                        started = true;
                    }
                    else if (c == ending)
                        endingFound += 1;

                    length++;

                    if (endingFound == 0 && started)
                        break;
                }

                list.Add(index + startIndex, dart.Substring(startIndex, length));

                index = startIndex + length;
            }

            return list;
        }

        public static string GetClassName(string value)
        {
            var prefix = "class ";
            var index = value.IndexOf(prefix);

            var startIndex = index + prefix.Length;
            var tmp = value.Substring(startIndex, value.IndexOf(" ", startIndex) - startIndex);

            if (tmp.Contains("<"))
            {
                var container = $"<{Util.GetContainerCode(value, '<', '>')}>";
                tmp = value.Substring(startIndex, value.IndexOf(container) + container.Length + 1 - startIndex);
            }

            return tmp.Trim();
        }

        public static List<Section> GetSections(string dart)
        {
            var list = new List<Section>();

            // Strip class
            dart = dart.Substring(dart.IndexOf("{") + 1).Trim();
            dart = dart.Substring(0, dart.LastIndexOf("}"));

            var tmp = dart;
            while (tmp.IndexOf("{") != -1 || tmp.IndexOf(";") != -1) // Find either { or ;
            {
                var insideString = false;
                var openCurlyFound = 0;
                var curlyStarted = false;
                var bracketFound = 0;
                var index = 0;
                char previousChar = ' ';
                char stringMarker = ' ';

                foreach (var c in tmp)
                {
                    if ((c == '"' || c == '\'') && previousChar != '\\')
                    {
                        if (!insideString)
                        {
                            insideString = true;
                            stringMarker = c;
                        }
                        else if (c == stringMarker)
                        {
                            insideString = false;
                            stringMarker = ' ';
                        }
                    }

                    if (!insideString)
                    {
                        if (c == '(' && curlyStarted == false)
                            bracketFound += 1;

                        if (c == ')' && curlyStarted == false)
                            bracketFound -= 1;

                        if (c == '{' && bracketFound == 0)
                        {
                            openCurlyFound += 1;
                            curlyStarted = true;
                        }

                        if (c == '}' && bracketFound == 0)
                            openCurlyFound -= 1;

                        if (openCurlyFound == 0 && curlyStarted)
                        {
                            var endIndex = index + 1;
                            if (tmp.Length > endIndex && tmp.Substring(endIndex, 1) == ";") // Could have {};
                                endIndex++;

                            var section = tmp.Substring(0, endIndex);
                            list.Add(new Section() { Raw = section.Trim() });
                            tmp = tmp.Substring(endIndex);
                            break;
                        }

                        if (c == ';' && curlyStarted == false && bracketFound == 0)
                        {
                            var section = tmp.Substring(0, index + 1);
                            tmp = tmp.Substring(index + 1);
                            list.Add(new Section() { Raw = section.Trim() });
                            break;
                        }
                    }

                    previousChar = c;
                    index++;
                }
            }

            return list;
        }

        /// <summary>
        /// MUST call before removing lines. Requires end \n to signify end
        /// </summary>
        /// <param name="dart"></param>
        /// <returns></returns>
        public static string RemoveComments(this string dart)
        {
            var newDart = "";
            var insideString = false;
            var previousChar = ' ';
            var insideComment = false;
            char stringMarker = ' ';

            foreach (var c in dart)
            {
                if ((c == '"' || c == '\'') && previousChar != '\\' && !insideComment)
                {
                    if (!insideString)
                    {
                        insideString = true;
                        stringMarker = c;
                    }
                    else if (c == stringMarker)
                    {
                        insideString = false;
                        stringMarker = ' ';
                    }
                }

                if (!insideString)
                    if (c == '/' && previousChar == '/')
                    {
                        newDart = newDart.TrimEnd('/');
                        insideComment = true;
                    }

                if (c == '\n' && insideComment)
                    insideComment = false;

                if (!insideComment)
                    newDart += c;

                previousChar = c;
            }

            return newDart;
        }

        public static string RemoveLines(this string dart)
        {
            return dart.Replace("\r", "").Replace("\n", " ");
        }

        public static string CleanDoubleSpace(this string dart)
        {
            // Will be shooting myself in the foot if double space is inside a string
            // TODO: update to account for this, if I come across it

            while (dart.IndexOf("  ") != -1)
                dart = dart.Replace("  ", " ");

            return dart;
        }
    }

}