import 'package:analyzer/dart/element/element.dart';

import '../implementation/implementation.dart';
import '../naming.dart';

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
    code.write(printTypeAndName(element));

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
          code.write("get {${Implementation.FieldBody(element.getter)}}");
        }
      }
      // setter
      if (hasSetter) {
        var setterNode = element.setter.computeNode();
        if (setterNode == null)
          code.write("set;");
        else {
          code.write("set {${Implementation.FieldBody(element.setter)}}");
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

    // type + name
    code.write(printTypeAndName(element));
    var name = getFieldName(element);

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
    code.write(printTypeAndName(element));

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

  static String printTypeAndName(FieldElement element) {
    var type = Naming.getVariableType(element, VariableType.Field);
    if(type.contains("override")){
      print("tes");
    }
    var name = getFieldName(element);
    return "${type} ${name}";
  }

  static String getFieldName(FieldElement element) {
    return Naming.getFormattedName(
        element.name,
        element.isPrivate
            ? NameStyle.LeadingUnderscoreLowerCamelCase
            : NameStyle.UpperCamelCase);
  } 
}
