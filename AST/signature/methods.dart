import 'package:analyzer/src/dart/element/member.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';

import '../implementation/implementation.dart';
import '../comments.dart';
import '../naming.dart';

class Methods {
  static bool isSameSignature(MethodElement m1, MethodElement m2) {
    return methodSignature(m1, m1, null) == methodSignature(m2, m2, null);
  }

  static bool overridesBaseMethod(MethodElement method, ClassElement element) {
    if (element.methods.any((m) => isSameSignature(m, method)) ||
        overridesParentBaseMethod(method, element)) return true;

    return false;
  }

  static MethodElement getBaseMethodInClass(MethodElement element) {
    if (element.enclosingElement == null ||
        element.enclosingElement.allSupertypes.length == 0) return element;

    MethodElement methodInSupertype;
    for (var supertype in element.enclosingElement.allSupertypes
        .where((st) => st.methods.length > 0)) {
      methodInSupertype = supertype.methods.firstWhere(
          (method) => method.displayName == element.displayName,
          orElse: () => null);
      if (methodInSupertype != null) {
        // Found method this method extends from
        break;
      }
    }

    if (methodInSupertype != null) {
      return getBaseMethodInClass(methodInSupertype);
    } else if (element is MethodMember) {
      return element.baseElement;
    } else
      return element;
  }

  static bool overridesParentBaseMethod(
      MethodElement method, ClassElement element) {
    for (var superType
        in element.allSupertypes.where((st) => !element.mixins.contains(st))) {
      if (overridesBaseMethod(method, superType.element)) return true;
    }
    return false;
  }

  static String printMethod(
      MethodElement element, bool insideMixin, bool isOverride) {
    var baseMethod = getBaseMethodInClass(element);

    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    var isOverride = element.hasOverride == true && baseMethod != element;

    if (element.isPublic == true)
      code.write("public ");
    else if (element.hasProtected == true || (element.isPrivate && isOverride))
      code.write("protected ");
    else if (element.isPrivate == true) code.write("private ");
    if (isOverride) code.write("override ");
    if (element.hasSealed == true)
      code.write("sealed ");
    // Add virtual as default key if method is not already an override since all methods are virtual in dart
    else if (element.hasOverride == false && element.isPrivate == false)
      code.write("virtual ");

    code.write(methodSignature(baseMethod, element, null));
    code.writeln(Implementation.MethodBody(element.computeNode().body));

    return code.toString();
  }

  static String printImplementedMethod(
      MethodElement element,
      String implementationInstanceName,
      InterfaceType implementedClass,
      MethodElement overrideMethod,
      ClassElement classElement) {
    var baseMethod = implementedClass.element.isMixin
        ? element
        : getBaseMethodInClass(element);

    var name = Naming.nameWithTypeParameters(element, false);
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (element.isPublic == true) code.write("public ");
    if (element.hasProtected == true) code.write("protected ");
    if (element.hasSealed == true) code.write("sealed ");
    code.write("virtual ");

    code.write(methodSignature(baseMethod, element, implementedClass));

    if (overrideMethod == null) {
      code.write("{");
      if (baseMethod.returnType.displayName != "void") code.write("return ");
      code.writeln(
          "${implementationInstanceName}.${name}(${baseMethod.parameters.map((p) => Naming.getFormattedName(p.name, NameStyle.LowerCamelCase)).join(",")});}");
    } else {
      code.writeln(
          Implementation.MethodBody(overrideMethod.computeNode().body));
    }

    return code.toString();
  }

  static bool containsGenericPart(DartType type) {
    var element = type.element;
    if (element is TypeParameterElement) return true;
    if (type is ParameterElement) {
      print("t");
    }
    return false;
  }

  static String methodSignature(MethodElement element,
      MethodElement overridenElement, InterfaceType implementedClass) {
    var methodName = Naming.nameWithTypeParameters(element, false);
    if (methodName ==
        Naming.nameWithTypeParameters(element.enclosingElement, false))
      methodName = "Self" + methodName;

    methodName = Naming.getFormattedName(
        methodName,
        element.isPrivate
            ? NameStyle.LeadingUnderscoreLowerCamelCase
            : NameStyle.UpperCamelCase);

    var parameter = printParameter(element, overridenElement, implementedClass);
    var returnType = Naming.getReturnType(element);
    var typeParameter = "";

    // Check if the method has a generic return value
    if (element.returnType.element is TypeParameterElement) {
      returnType = Naming.getDartTypeName(overridenElement.returnType);
    }

    return "${returnType} ${methodName}${typeParameter}(${parameter})";
  }

  static String printParameter(FunctionTypedElement element,
      FunctionTypedElement overridenElement, InterfaceType implementedClass) {
    // Parameter
    var parameters = element.parameters.map((p) {
      // Name
      var parameterName =
          Naming.getFormattedName(p.name, NameStyle.LowerCamelCase);
      if (parameterName == "")
        parameterName = "p" + (element.parameters.indexOf(p) + 1).toString();

      if (parameterName == 'decimal') parameterName = '@decimal';
      if (parameterName == 'object') parameterName = '@object';
      if (parameterName == 'byte') parameterName == '@byte';

      // Type
      var parameterType =
          Naming.getVariableType(p, VariableType.Parameter).split(" ").last;

      if (p.type.element is TypeParameterElement && overridenElement != null) {
        var actualParameterSignature = overridenElement
            .parameters[element.parameters.indexWhere((x) => x.name == p.name)];
        parameterType = Naming.getVariableType(
                actualParameterSignature, VariableType.Parameter)
            .split(" ")
            .last;
      }

      if (parameterType == "@") {
        parameterType = "object";
      }
      var parameterSignature = parameterType + " " + parameterName;

      if (p.hasRequired) {
        parameterSignature = "[NotNull] " + parameterSignature;
      }

      if (p.isOptional) {
        parameterSignature += " = default(${parameterType})";
      }
      return parameterSignature;
    });
    return parameters == null ? "" : parameters.join(",");
  }
}
