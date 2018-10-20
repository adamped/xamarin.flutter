using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dart2CSharpTranspiler.Writer;
using Microsoft.CodeAnalysis;
using Transpiler;

namespace Dart2CSharpTranspiler
{
    class Program
    {
        static void Main(string[] args)
        {  
            string source = @"..\..\..\..\flutter\lib\src";
            string destination = @"..\..\..\..\FlutterSDK\src";
            string modelStorage = @"..\..\..\..\model.json";

            DartModel model = null;

            if (File.Exists(modelStorage))
                model = JsonConvert.DeserializeObject<DartModel>(File.ReadAllText(modelStorage));

            if (model == null)
            {
                model = BuildDartModel(source);
                File.WriteAllText(modelStorage, JsonConvert.SerializeObject(model));
            }

            PostProcessDart(model);

            CreateCSharpFiles(model, destination);

            List<DartFile> dartFiles = model.SelectMany(x => x.Value).ToList();

            Console.WriteLine($"Files: {dartFiles.Count.ToString("N0")}");
            Console.WriteLine($"File Sections: {dartFiles.Sum(x => x.Sections.Count).ToString("N0")}");
            Console.WriteLine($"File Imports: {dartFiles.Sum(x => x.Imports.Count).ToString("N0")}");
            Console.WriteLine($"Classes: {dartFiles.Sum(x => x.Classes.Count).ToString("N0")}");
            Console.WriteLine($"Class Sections: {dartFiles.Sum(x => x.Classes.Select(y => y.Sections.Count).Sum()).ToString("N0")}");

            //Console.ReadLine();
        }

        private static Dictionary<string, DartClass> ClassList { get; set; }

        public static DartClass GetDartClass(string name)
        {
            if (name.Contains("<"))
                name = name.Substring(0, name.IndexOf("<")) + "<>";
            var baseClass = ClassList.SingleOrDefault(x => x.Key.EndsWith($".{name}")).Value;

            if (baseClass == null)
            {
                // Attempt a Generic incase it was an @optionalTypeArgs
                baseClass = ClassList.SingleOrDefault(x => x.Key.EndsWith($".{name}<>")).Value;
            }

            return baseClass;
        }

        private static DartModel BuildDartModel(string source)
        {
            var model = new DartModel();

            foreach (var sourceDirectory in Directory.GetDirectories(source))
            {
                var folder = sourceDirectory.Replace(source, "").TrimStart('\\');

                foreach (var sourceFilePath in Directory.GetFiles(sourceDirectory))
                {
                    // Create Model from Dart File
                    var sourceFileName = sourceFilePath.Replace(sourceDirectory, "").TrimStart('\\');
                    var dart = File.ReadAllText(sourceFilePath);
                    var file = DartClassReader.Construct(sourceFileName, folder, dart);

                    if (model.ContainsKey(folder))
                        model[folder].Add(file);
                    else
                    {
                        var list = new List<DartFile>();
                        model.Add(folder, list);
                        list.Add(file);
                    }
                }
            }

            return model;
        }

        private static void CreateCSharpFiles(DartModel model, string destination)
        {
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }

            var writer = new CSharpWriter(model, "FlutterSDK");

            foreach (var folder in model)
            {
                var destinationFolder = Path.Combine(destination, NormalizationHelper.CamelCase(folder.Key));

                // Check Folder
                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);

                foreach (var file in folder.Value)
                {
                    
                    // Create CSharp File
                    var namespaceDeclaration = writer.GenerateFileSyntaxTree(file);
                    var code = namespaceDeclaration
                        .NormalizeWhitespace()
                        .ToFullString();
                    var destinationFileName = $"{NormalizationHelper.NormalizeTypeName(file.Name)}.cs";

                    var destinationPath = Path.Combine(destinationFolder, destinationFileName);


                    if (!File.Exists(destinationFileName))
                        File.Create(destinationPath).Close();

                    File.WriteAllText(destinationPath, code);
                }
            }
        }

        private static void PostProcessDart(DartModel model)
        {
            ClassList = new Dictionary<string, DartClass>();

            foreach ((string folder, IList<DartFile> file) in model)
                foreach (var @class in file.SelectMany(x => x.Classes))
                {
                    var name = @class.RawName;
                    if (name.Contains("<"))
                        name = name.Substring(0, name.IndexOf("<")) + "<>";

                    ClassList.Add($"{folder}.{name.Replace("<T>", "<>")}", @class);
                }

            InheritanceCalculation(ClassList);

            PropertyCombination(ClassList);

            foreach (var file in model.Values.SelectMany(x => x))
            {
                foreach (var item in file.Sections.Where(x => x.Type == SectionType.Enum))
                    if (!CSharpWriter.Enums.Contains(item.Name))
                        CSharpWriter.Enums.Add(item.Name);
            }
        }

        private static void PropertyCombination(Dictionary<string, DartClass> classList)
        {
            foreach ((string key, DartClass item) in classList)
            {
                var propertyList = new Dictionary<string, IList<Section>>();

                foreach (var section in item.Sections)
                {
                    if (section.Type == SectionType.PropertyGet
                        || section.Type == SectionType.PropertySet
                        || section.Type == SectionType.Field)
                    {
                        if (propertyList.ContainsKey(section.Name))
                            propertyList[section.Name].Add(section);
                        else
                            propertyList.Add(section.Name, new List<Section>() { section });
                    }
                }

                foreach ((string name, IList<Section> sections) in propertyList)
                {
                    var isProperty = false;
                    foreach (var section in sections)
                        if (section.Type == SectionType.PropertyGet || section.Type == SectionType.PropertySet)
                            isProperty = true;

                    if (isProperty)
                    {
                        var pName = name;
                        if (pName == "event")
                            pName = "@event";

                        if (pName == "delegate")
                            pName = "@delegate";

                        var property = new Property()
                        {
                            Name = pName,
                            Visibility = name.StartsWith("_") ? VisibilityType.Private : VisibilityType.Public
                        };

                        foreach (var section in sections)
                        {
                            if (section.Type == SectionType.Field)
                                section.IsPropertyBacking = true;
                            else if (section.Type == SectionType.PropertyGet)
                                property.GetRawCode = section.RawCode;
                            else if (section.Type == SectionType.PropertySet)
                                property.SetRawCode = section.RawCode;

                            if (string.IsNullOrEmpty(property.ReturnType))
                                property.ReturnType = section.ReturnType;
                            else if (property.ReturnType.Replace(", ", ",") != section.ReturnType.Replace(", ", ","))
                                throw new Exception("Return Types don't match on property");
                        }

                        item.Properties.Add(property);
                    }

                }

            }
        }

        private static void InheritanceCalculation(Dictionary<string, DartClass> classList)
        {
            foreach ((string key, DartClass item) in classList)
            {
                if (item.Extends != null && !DartFoundationClass(item.Extends.Name))
                {
                    var baseClass = GetDartClass(item.Extends.Name);
                    if(baseClass == null)
                        continue;

                    foreach (var method in item.Sections.Where(x => x.Type == SectionType.Method))
                    {
                        var baseMethod = baseClass.Sections.SingleOrDefault(x => x.Name == method.Name);
                        if (baseMethod != null)
                        {
                            baseMethod.IsVirtual = true;
                            method.IsOverride = true;
                        }
                    }
                }
            }
        }

        private static bool DartFoundationClass(string className)
        {
            if (className.Contains("<"))
                className = className.Substring(0, className.IndexOf("<"));

            switch (className)
            {
                case "AssertionError":
                case "IterableBase":
                case "Iterable":
                case "Color":
                case "Size":
                case "Comparable":
                case "Object":
                    return true;
                default:
                    return false;
            }
        }
    }
}