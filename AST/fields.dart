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
    code.write("${element.type.displayName} ${element.name} ");

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
          code.write("set ${printFieldBody(setterNode)}");
        }
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
