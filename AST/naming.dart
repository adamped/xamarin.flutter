import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/ast/token.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/dart/element/element.dart';

import 'config.dart';

class Naming {
  static List<String> namespacePartsFromIdentifier(String identifier) {
    var namespacePath = identifier
        .replaceAll(
            "file:///" + Config.flutterSourcePath.replaceAll("\\", "/") + "/",
            "")
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

    //Dont include the filename in the namespace
    //parts.removeLast();
    return Config.rootNamespace + "." + parts.join(".");
  }

  static String nameWithTypeArguments(
      ParameterizedType element, bool isInterface) {
    var name = element.name;
    name = getFormattedName(name, NameStyle.UpperCamelCase);
    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in element.typeArguments) {
      typeArguments.add(getDartTypeName(argument));
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String interfaceTypeName(InterfaceType element) {
    var name = element.name;
    name = getFormattedName(name, NameStyle.UpperCamelCase);
    var typeArguments = new List<String>();
    for (var argument in element.typeArguments) {
      typeArguments.add(getDartTypeName(argument));
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

  static String mixinInterfaceName(ClassElement mxin) {
    var name = nameWithTypeParameters(mxin, true);
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
    }

    var formattedName = getFormattedTypeName(returnName);
    
    if (!(returnType is TypeParameterType) && returnType.element != null) {
      var library = returnType.element.library;
      if (library != null &&
          !Config.ignoredImports.contains(library.identifier) && 
          !["bool", "double", "object", "void", "string", "T"].contains(formattedName)) {
        var namespace = namespaceFromIdentifier(library.identifier);
        formattedName = namespace + "." + formattedName;
      }
    } 

    return formattedName;
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

  static String getDartTypeName(DartType type) {
    String typeName = "object";
    if (type is InterfaceType) {
      typeName = nameWithTypeArguments(type, false);
    } else if (type is TypeParameterType) {
      typeName = type.displayName;
    }
    var formattedName = getFormattedTypeName(typeName);

    if (!(type is TypeParameterType) && type.element != null) {
      var library = type.element.library;
      if (library != null &&
          !Config.ignoredImports.contains(library.identifier) &&
          formattedName != "object") {
        var namespace = namespaceFromIdentifier(library.identifier);
        formattedName = namespace + "." + formattedName;
      }
    } 

    return formattedName;
  }

  static String getVariableType(VariableElement element, VariableType type) {
    String typeName = "object";
    if (element.type is InterfaceType) {
      typeName = nameWithTypeArguments(element.type, false);
    } else if (element.type is TypeParameterType) {
      typeName = element.type.displayName;
    } else if (element.computeNode() != null) {
      switch (type) {
        case VariableType.Field:
          typeName =
              tokenToText(element.computeNode().beginToken.previous, true)
                  .split(" ")
                  .first;
          break;
        case VariableType.Parameter:
          typeName = tokenToText(element.computeNode().endToken.previous, true)
              .split(" ")
              .last;
          break;
      }
    }
    var formattedName = getFormattedTypeName(typeName);
    var library = element.type.element.library;
    if (!( element.type is TypeParameterType) &&  library != null &&
        !Config.ignoredImports.contains(library.identifier) &&
        formattedName != "object") {
      var namespace = namespaceFromIdentifier(library.identifier);
      formattedName = namespace + "." + formattedName;
    }
    return formattedName;
  }

  static String getTypeParameterName(TypeParameterElement element) {
    String typeName = "object";

    if (element.type is InterfaceType) {
      typeName = interfaceTypeName(element.type as InterfaceType);
    }
    typeName = element.type.displayName;
    return getFormattedTypeName(typeName);
  }

  static String getFormattedTypeName(String typeName) {
    var formattedName = typeName;
    if (formattedName.startsWith("ui."))
      formattedName = formattedName.replaceAll("ui.", "");

    if (formattedName.startsWith("Set"))
      formattedName = formattedName.replaceAll("Set", "HashSet");
    if (formattedName.startsWith("Map"))
      formattedName = formattedName.replaceAll("Map", "Dictionary");

    switch (formattedName.toLowerCase()) {
      case "httpclientresponse":
        return "HttpResponseMessage";
      case "shader":
        return "SKShader";
      case "enginelayer":
        return "NativeEngineLayer";
      case "void":
      case "bool":
      case "int":
      case "object":
      case "double":
      case "string":
        return formattedName.toLowerCase();
      case "dynamic":
        return "object";
      case "duration":
        return "TimeSpan";
      case "future<void>":
      case "future<null>":
        return "Task";
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
    if (formattedName == null ||
        formattedName.length == 0 ||
        formattedName == "_" ||
        formattedName == "-") {
      return "";
    } else
      formattedName = formattedName.replaceAll("_", "").replaceAll("-", "");

    if (style != NameStyle.LeadingUnderscoreLowerCamelCase) {
      formattedName = escapeFixedWords(formattedName);
    }
    if (formattedName == "==") formattedName = "equals";
    if (formattedName == "~/") formattedName = "divideIntegerResultOperator";
    if (formattedName == "*") formattedName = "multiplyOperator";
    if (formattedName == "/") formattedName = "divideOperator";
    if (formattedName == "%") formattedName = "moduloOperator";
    if (formattedName == "+") formattedName = "addOperator";
    if (formattedName == "-") formattedName = "subtractOperator";
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
        formattedName = "_" + lowerCamelCase(formattedName);
        break;
    }
    return formattedName;
  }

  static String lowerCamelCase(String name) {
    return name[0].toLowerCase() + name.replaceRange(0, 1, "");
  }

  static String upperCamelCase(String name) {
    return name[0].toUpperCase() + name.replaceRange(0, 1, "");
  }

  static String escapeFixedWords(String word) {
    var lowerName = word.toLowerCase();
    if (["event", "object", "delegate", "byte", "fixed", "checked"]
        .any((x) => lowerName == x))
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
