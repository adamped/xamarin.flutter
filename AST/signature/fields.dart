import 'package:analyzer/dart/element/element.dart';
import '../implementation/implementation.dart';
import '../naming.dart';

class Fields {
  static String printField(FieldElement element) {
    var baseField = getBaseFieldInClass(element);
    var code = new StringBuffer();

    if (element.hasProtected == true) code.write("protected ");
    if (element.isPublic == true) code.write("public ");
    if (element.isPrivate == true) code.write("private ");
    if (element.hasOverride == true) code.write("override ");
    if (element.isPrivate == false && element.hasOverride == false)
      code.write("virtual ");

    // type + name
    code.write(printTypeAndName(baseField));

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

  static FieldElement getBaseFieldInClass(FieldElement element) {
    if (element.enclosingElement == null ||
        element.enclosingElement.allSupertypes.length == 0) return element;

    FieldElement fieldInSupertype;
    for (var supertype in element.enclosingElement.allSupertypes
        .where((st) => st is ClassElement)
        .cast<ClassElement>()
        .where((st) => st.fields.length > 0)) {
      fieldInSupertype = supertype.fields.firstWhere(
          (field) => field.displayName == element.displayName,
          orElse: () => null);
      if (fieldInSupertype != null) {
        // Found method this method extends from
        break;
      }
    }

    if (fieldInSupertype != null) {
      return getBaseFieldInClass(fieldInSupertype);
    } else
      return element;
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
    var name = getFieldName(element);
    if (name == Naming.nameWithTypeParameters(element.enclosingElement, false))
      name = name + "Value";

    if (name == "_onChanged") {
      print("test");
      type = Naming.getVariableType(element, VariableType.Field);
    }
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
