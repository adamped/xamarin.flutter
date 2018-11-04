import 'package:analyzer/dart/ast/token.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/dart/element/element.dart';

import 'config.dart';

class Naming {
  static List<String> namespacePartsFromIdentifier(String identifier) {
    var namespacePath = identifier
        .replaceAll(
            "file:///" + Config.directoryPath.replaceAll("\\", "/") + "/", "")
        .replaceAll(".dart", "");
    var splittedPath = namespacePath.split('/');
    return splittedPath;
  }

  static String namespaceFromIdentifier(String identifier) {
    var parts = namespacePartsFromIdentifier(identifier);

    //Dont include the filename in the namespace
    parts.removeLast();
    return Config.rootNamespace + "." + parts.join(".");
  }

  static String nameWithTypeArguments(
      ParameterizedType element, bool isInterface) {
    var name = element.name;
    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in element.typeArguments) {
      // TODO format name
      typeArguments.add(argument.name);
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String nameWithTypeParameters(
      TypeParameterizedElement element, bool isInterface) {
    var name = element.name;
    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in element.typeParameters) {
      // TODO format name
      typeArguments.add(argument.name);
    }
    if (typeArguments.length > 0) {
      name += "<${typeArguments.join(",")}>";
    }
    return name;
  }

  static String parameterNameWithTypeParameters(
      ParameterElement element, bool isInterface) {
    var name = element.type.name;
    if (isInterface) name = "I" + name;
    var typeArguments = new List<String>();
    for (var argument in element.typeParameters) {
      // TODO format name
      typeArguments.add(argument.name);
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
    return element.returnType.displayName;
  }

  static String tokenToText(Token token) {
    var text = token.lexeme;
    //if (token.next != null && token.next.isEof == false )
    //  text += tokenToText(token.next);
    return text;
  }

  static String getVariableType(VariableElement element) {
    if (element.type is InterfaceType) {
      if (element.type.displayName == "Animation<T>") {
        print("test");
      }
      return element.type.displayName;
    }
    if (element.type is TypeParameterType) {
      if (element.type.displayName == "Animation<T>") {
        print("test");
      }
      return element.type.displayName;
    }
    if (element.computeNode() == null) {
      //TODO Find the correct type here
      return "object";
    }
    return tokenToText(element.computeNode().beginToken);
  }
}
