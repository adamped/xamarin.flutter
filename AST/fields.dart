import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/element/element.dart';

import 'naming.dart';

class Fields {
  static String printField(FieldElement element) {
    var code = new StringBuffer();

    if (element.hasProtected == true) code.write("protected ");
    if (element.isPublic == true) code.write("public ");
    if (element.isPrivate == true) code.write("private ");
    if (element.hasOverride == true) code.write("override ");
    if (element.isPrivate == false && element.hasOverride == false)
      code.write("virtual ");

    // type + name
    code.write("${Naming.getVariableType(element)} ${element.name} ");

    var hasGetter = element.getter != null;
    var hasSetter = element.setter != null;

    if (hasGetter || hasSetter) {
      code.write("{");
// getter
      if (hasGetter) {
        var getterNode = element.getter.computeNode();
        if (getterNode == null)
          code.write("get;");
        else {
          code.write("get {${printFieldBody(getterNode)}}");
        }
      }
      // setter
      if (hasSetter) {
        var setterNode = element.setter.computeNode();
        if (setterNode == null)
          code.write("set;");
        else {
          code.write("set {${printFieldBody(setterNode)}}");
        }
      }
      code.write("}");
    } else
      code.write(";");

    return code.toString();
  }

  static String printImplementedField(
      FieldElement element, String implementedFieldName) {
    var code = new StringBuffer();

    if (element.hasProtected == true) code.write("protected ");
    if (element.isPublic == true) code.write("public ");
    code.write("virtual ");

    if (element.type.displayName == "Animation<T>") {
      print("test");
    }

    // type + name
    var type = Naming.getVariableType(element);
    var name = element.name;
    code.write("${type} ${name} ");

    var hasGetter = element.getter != null;
    var hasSetter = element.setter != null;

    if (hasGetter || hasSetter) {
      code.write("{");
// getter
      if (hasGetter) {
        code.write("get => ${implementedFieldName}.${name};");
      }
      // setter
      if (hasSetter) {
        code.write("set => ${implementedFieldName}.${name} = value;");
      }
      code.write("}");
    } else
      code.write(";");

    return code.toString();
  }

  static String getFieldSignature(FieldElement element) {
    var code = new StringBuffer();

    // type + name
    code.write("${Naming.getVariableType(element)} ${element.name} ");

    var hasGetter = element.getter != null;
    var hasSetter = element.setter != null;

    if (hasGetter || hasSetter) {
      code.write("{");
// getter
      if (hasGetter) {
        code.write("get;");
      }
      // setter
      if (hasSetter) {
        code.write("set;");
      }
      code.write("}");
    } else
      code.write(";");

    return code.toString();
  }

  static String printFieldBody(AstNode element) {
    var bodyLines = Naming.tokenToText(element.beginToken).split("\n");
    return bodyLines.map((l) => "// ${l}\n").join() +
        "\nthrow new NotImplementedException();";
  }
}
