import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/ast/token.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/dart/element/element.dart';

import 'config.dart';
import 'types.dart';

class Naming {
  static List<String> namespacePartsFromIdentifier(String identifier) {
    var namespacePath = identifier
        .replaceAll(
            "file:///" + Config.sourcePath.replaceAll("\\", "/") + "/", "")
        .replaceAll(".dart", "")
        .replaceAll("package:flutter/src/", "");

    var splittedPath = namespacePath
        .split('/')
        .map<String>((part) => getFormattedName(part, NameStyle.UpperCamelCase))
        .toList();
    return splittedPath;
  }

  static String namespaceFromIdentifier(String identifier) {
    var parts = namespacePartsFromIdentifier(identifier);

    return Config.rootNamespace + "." + parts.join(".");
  }

  static String nameWithTypeArguments(
      ParameterizedType type, bool isInterface) {
    var name = type.name;
    name = getFormattedName(name, NameStyle.UpperCamelCase);

    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in type.typeArguments) {
      typeArguments.add(Types.getDartTypeName(argument));
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String interfaceTypeName(InterfaceType type) {
    var name = type.name;
    name = getFormattedName(name, NameStyle.UpperCamelCase);

    var typeArguments = new List<String>();
    for (var argument in type.typeArguments) {
      typeArguments.add(Types.getDartTypeName(argument));
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String nameWithTypeParameters(
      TypeParameterizedElement element, bool isInterface) {
    var name = element.name;
    name = getFormattedName(name, NameStyle.UpperCamelCase);

    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in element.typeParameters) {
      typeArguments.add(getTypeParameterName(argument));
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String parameterTypeWithTypeParameters(
      ParameterElement element, bool isInterface) {
    var name = element.type.name;
    name = getFormattedName(name, NameStyle.UpperCamelCase);
    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in element.typeParameters) {
      typeArguments.add(getTypeParameterName(argument));
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String getReturnType(FunctionTypedElement element) {
    var returnType = element.returnType;
    var returnName = returnType.displayName;
    var test = element.computeNode();
    if (returnType is InterfaceType) {
      returnName = interfaceTypeName(returnType);
    } else if (test is MethodDeclaration) {
      if (test.returnType is TypeName) {
        var t = test.returnType as TypeName;
        returnName = t.name.name;
      }
    } else if (test is FunctionDeclaration) {
      if (test.returnType is TypeName) {
        var t = test.returnType as TypeName;
        returnName = t.name.name;
      }
    }

    var formattedName = getFormattedTypeName(returnName);

    if (!(returnType is TypeParameterType) && returnType.element != null) {
      var library = returnType.element.library;
      if (library != null &&
          !Config.ignoredImports.contains(library.identifier) &&
          !["bool", "double", "object", "void", "string", "T"]
              .contains(formattedName)) {
        var namespace = namespaceFromIdentifier(library.identifier);
        formattedName = namespace + "." + formattedName;
      }
    }

    return formattedName;
  }

  static String className(InterfaceType type) {
    var baseClass = nameWithTypeArguments(type, false);
    if (type.element.library != null &&
        type.element.library.identifier != null &&
        !Config.ignoredImports.contains(type.element.library.identifier)) {
      var namespace = namespaceFromIdentifier(type.element.library.identifier);
      baseClass = "${namespace}.${baseClass}";
    }
    return baseClass;
  }

  static String tokenToText(Token token, bool backwards) {
    if (token.isEof == true ||
        token.keyword != null ||
        (token.isKeywordOrIdentifier == false && backwards)) return "";

    var text = token.lexeme;
    if (backwards) {
      if (token.previous != null)
        text += " " + tokenToText(token.previous, backwards);
    } else {
      if (token.next != null) text += " " + tokenToText(token.next, backwards);
    }

    return text;
  }

  static String getTypeParameterName(TypeParameterElement element) {
    String typeName = "object";

    if (element.type is InterfaceType) {
      typeName = interfaceTypeName(element.type as InterfaceType);
    }
    typeName = element.type.displayName;
    return getFormattedTypeName(typeName);
  }

  // TODO: I think this should be rebuilt and accept DartType
  // Then work from there.
  static String getFormattedTypeName(String typeName) {
    var formattedName = typeName;

    // if (typeName == 'ui.Image' || typeName == 'Image')
    //   return 'SKImage';

    if (formattedName.toLowerCase().startsWith("ui."))
      formattedName = formattedName.substring(3, formattedName.length - 3);

    if (formattedName == "Set" || formattedName.startsWith('Set<'))
      formattedName = 'HashSet' + formattedName.substring(3);
    if (formattedName.startsWith("Map"))
      formattedName = formattedName.replaceAll("Map", "Dictionary");

    switch (formattedName.toLowerCase()) {
      case "float8list":
      case "float16list":
      case "float32list":
        return "List<double>";
      case "float64list":
        return "List<float>";
      case "int32list":
        return "List<uint>";
      case "httpclientresponse":
        return "HttpResponseMessage";
      case "shader":
        return "SKShader";
      // case "image": // This doesn't work properly because there is an Image class in the FlutterSDK. But we do need something here for inside method bodies.
      //   return "SKImage";
      case "enginelayer":
        return "NativeEngineLayer";
      case "frameinfo":
        return "SKCodecFrameInfo";
      case "codec":
        return "SKCodec";
      case "void":
      case "bool":
      case "int":
      case "object":
      case "double":
      case "string":
        return formattedName.toLowerCase();
      case "dynamic": //TODO: we need to fix this. Dynamic is normally hiding a proper type. (AP - Might have fixed, will have to check later)
        return "object";
      case "duration":
        return "TimeSpan";
      case "future<void>":
      case "future<null>":
        return "Future";
      // case "iterable":
      //   return "List";
      case "clip":
        return "FlutterBinding.UI.Clip";
      case "paragraph":
        return "FlutterBinding.UI.Paragraph";
      default:
        formattedName =
            getFormattedName(formattedName, NameStyle.UpperCamelCase);
        if (formattedName == "")
          return "object";
        else
          return formattedName;
    }
  }

  static String getFormattedName(String originalName, NameStyle style) {
    var formattedName = originalName;
    if (formattedName == null || formattedName.length == 0) {
      return "";
    } else if (formattedName == "-") {
      formattedName = "subtractOperator";
    } else {
      // We can't do this for names with namespaces already, since it doesn't quite work.
      if (!formattedName.contains('.')) {
        var reAddUnderscore = formattedName.startsWith("_");
        formattedName = formattedName.replaceAll("_", "").replaceAll("-", "");
        if (reAddUnderscore) {
          formattedName = "_" + formattedName;
        }
      }
    }
    if (style != NameStyle.LeadingUnderscoreLowerCamelCase) {
      formattedName = escapeFixedWords(formattedName);
    }

    // rename operators
    if (formattedName == "==") formattedName = "equals";
    if (formattedName == "~/") formattedName = "divideIntegerResultOperator";
    if (formattedName == "*") formattedName = "multiplyOperator";
    if (formattedName == "/") formattedName = "divideOperator";
    if (formattedName == "%") formattedName = "moduloOperator";
    if (formattedName == "+") formattedName = "addOperator";
    if (formattedName == "[]") formattedName = "indexOfOperator";
    if (formattedName == "[]=") formattedName = "insertAtOperator";

    switch (style) {
      case NameStyle.LowerCamelCase:
        formattedName = lowerCamelCase(formattedName);
        break;
      case NameStyle.UpperCamelCase:
        formattedName = upperCamelCase(formattedName);
        break;
      case NameStyle.LeadingUnderscoreLowerCamelCase:
        if (!formattedName.startsWith("_"))
          formattedName = "_" + lowerCamelCase(formattedName);
        break;
    }
    return formattedName;
  }

  static String lowerCamelCase(String name) {
    if (name.length == 1) return name;

    var underscore = name.startsWith('_');

    if (underscore) name = name.substring(1);

    var newName = name[0].toLowerCase() + name.replaceRange(0, 1, "");

    if (underscore) newName = '_' + newName;

    return newName;
  }

  static String upperCamelCase(String name) {
    if (name.length <= 1) return name;

    var underscore = name.startsWith('_');

    if (underscore) name = name.substring(1);

    var newName = name[0].toUpperCase() + name.replaceRange(0, 1, "");

    if (underscore) newName = '_' + newName;

    return newName;
  }

  static String escapeFixedWords(String word) {
    var lowerName = word.toLowerCase();
    if ([
      "event",
      "object",
      "delegate",
      "byte",
      "fixed",
      "checked",
      "base",
      "decimal",
      "byte"
    ].any((x) => lowerName == x))
      return "@" + word;
    else
      return word;
  }

  static String DefaultClassName(CompilationUnitElement element) {
    var name = element.library.identifier
            .replaceAll(".dart", "")
            .replaceAll(".g", "")
            .split("/")
            .last +
        "DefaultClass";
    return getFormattedName(name, NameStyle.UpperCamelCase);
  }
}

enum NameStyle {
  LowerCamelCase,
  UpperCamelCase,
  LeadingUnderscoreLowerCamelCase
}

enum NameType { Name, Type }

enum VariableType { Field, Parameter }
