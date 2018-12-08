import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/src/dart/element/member.dart';
import '../implementation/implementation.dart';
import '../naming.dart';

class Fields {
  static bool containsGenericPart(DartType type) {
    var element = type.element;
    if (element is TypeParameterElement) return true;
    if (type is ParameterElement) {
      print("t");
    }
    return false;
  }

  static FieldElement getBaseFieldInClass(FieldElement element) {
    if (element.enclosingElement == null ||
        element.enclosingElement.allSupertypes.length == 0) return element;

    FieldElement fieldInSupertype;
    for (var supertype in (element.enclosingElement.allSupertypes
          ..addAll(element.enclosingElement.superclassConstraints))
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
    } else if (element is FieldMember) {
      return element.baseElement;
    } else
      return element;
  }

  static String printField(FieldElement element) {
    var baseField = getBaseFieldInClass(element);
    var code = new StringBuffer();

    if (element.hasProtected == true) code.write("protected ");
    if (element.isPublic == true) code.write("public ");
    if (element.isPrivate == true) code.write("protected internal ");
    if (element.hasOverride == true) code.write("override ");
    if (element.hasOverride == false) code.write("virtual ");

    var implementedClass = element.enclosingElement.allSupertypes.firstWhere(
        (superClass) => superClass.element.fields
            .any((field) => field.displayName == element.displayName),
        orElse: () => null);

    // type + name
    if (containsGenericPart(element.type) &&
        implementedClass != null &&
        implementedClass.typeParameters
            .any((tp) => tp.type.displayName == element.type.displayName)) {
      var typeParameter = implementedClass.typeParameters
          .firstWhere((tp) => tp.type.displayName == element.type.displayName);
      var type = implementedClass.typeArguments[
          implementedClass.typeParameters.indexOf(typeParameter)];
      var name = getFieldName(element);
      if (name ==
          Naming.nameWithTypeParameters(element.enclosingElement, false))
        name = name + "Value";
      code.write("${type} ${name}");
    } else {
      code.write(printTypeAndName(baseField));
    }

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
      FieldElement element,
      FieldElement overridingElement,
      InterfaceType implementedClass,
      String implementedFieldName) {
    var code = new StringBuffer();
    var elementForSignature =
        overridingElement != null ? overridingElement : element;

    if (elementForSignature.hasProtected == true) code.write("protected ");
    if (elementForSignature.isPublic == true) code.write("public ");
    code.write("virtual ");

    // type + name
    var name = getFieldName(element);
    if (name == Naming.nameWithTypeParameters(element.enclosingElement, false))
      name = name + "Value";

    if (containsGenericPart(element.type)) {
      var typeParameter = implementedClass.typeParameters
          .firstWhere((tp) => tp.type == element.type);
      var type = implementedClass.typeArguments[
          implementedClass.typeParameters.indexOf(typeParameter)];
      code.write("${type} ${name}");
    } else {
      code.write(printTypeAndName(element));
    }

    var hasGetter = elementForSignature.getter != null;
    var hasSetter = elementForSignature.setter != null;

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
    return "${type} ${name}";
  }

  static String printImplementedTypeAndName(
      FieldElement element, ClassElement supertypeThatProvidesField) {
    var type = Naming.getVariableType(element, VariableType.Field);
    var name = getFieldName(element);
    if (name == Naming.nameWithTypeParameters(element.enclosingElement, false))
      name = name + "Value";

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
