import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/src/dart/element/member.dart';
import '../implementation/implementation.dart';
import '../naming.dart';
import '../types.dart';

class Fields {
  static bool containsGenericPart(DartType type) {
    var element = type.element;
    if (element is TypeParameterElement) return true;
    if (type is InterfaceType) {
      var hasTypedTypeArguments =
          type.typeArguments.any((argument) => argument is TypeParameterType);
      if (hasTypedTypeArguments) {
        return true;
      }
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
    if (element.isPrivate == true) code.write("internal ");
    if (element.hasOverride == true) code.write("override ");
    if (element.hasOverride == false) code.write("virtual ");

    // type + name
    if (containsGenericPart(element.type)) {
      code.write(printTypeAndName(element));
    } else {
      code.write(printTypeAndName(baseField));
    }

    if (code.toString().contains("UiKitViewController")) {
      print("ouch");
    }

    var hasGetter = element.getter != null;
    var hasSetter = element.setter != null;

    if (hasGetter || hasSetter) {
      code.write("{");
      var implementedGetter = false;
      // getter
      if (hasGetter) {
        var getterNode = element.getter.computeNode();
        if (getterNode == null)
          code.write("get;");
        else {
          code.write("get {${Implementation.fieldBody(element.getter)}}");
          implementedGetter = true;
        }
      }
      // setter
      if (hasSetter) {
        var setterNode = element.setter.computeNode();
        if (setterNode == null)
          code.write("set;");
        else {
          code.write("set {${Implementation.fieldBody(element.setter)}}");
        }
      } else {
        if (implementedGetter)
          code.write("set { ${Implementation.fieldBody(element.setter)} }");
        else
          code.write("set;"); // For static auto initialization of variables
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
      var typeParameter = implementedClass.typeParameters.firstWhere(
          (tp) => element.type.displayName.contains(tp.type.displayName));
      var type = implementedClass.typeArguments[
          implementedClass.typeParameters.indexOf(typeParameter)];
      //TODO: Not sure if this is 100% correct, could have false positives,
      //but so far seems to be working well.
      if (type.name == 'T')
        code.write('${type.name} $name');
      else
        code.write("${element.type.name}<${type.name}> $name");
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
      } else {
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
    var name = getFieldName(element);
    if (name == Naming.nameWithTypeParameters(element.enclosingElement, false))
      name = name + "Value";

    var type = Types.getVariableType(element, VariableType.Field);

    return "${type} ${name}";
  }

  static String printImplementedTypeAndName(
      FieldElement element, ClassElement supertypeThatProvidesField) {
    var type = Types.getVariableType(element, VariableType.Field);
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
