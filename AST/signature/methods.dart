import 'package:analyzer/analyzer.dart';
import 'package:analyzer/src/dart/element/member.dart';
import 'package:analyzer/src/dart/element/element.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/dart/ast/ast.dart';
import 'package:front_end/src/scanner/token.dart';

import '../implementation/implementation.dart';
import '../comments.dart';
import '../naming.dart';
import '../types.dart';

class Methods {
  static bool isSameSignature(MethodElement m1, MethodElement m2) {
    return methodSignature(m1, m1, null, false) ==
        methodSignature(m2, m2, null, false);
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
    for (var superType in element.allSupertypes.where(
        (st) => st.displayName != "Object" && !element.mixins.contains(st))) {
      if (overridesBaseMethod(method, superType.element)) return true;
    }
    return false;
  }

  static String printMethod(
      MethodElement element, bool insideMixin, bool isOverride,
      [String inheritedType = '']) {
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

    code.write(
        methodSignature(baseMethod, element, null, isOverride, inheritedType));
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

    code.write(methodSignature(baseMethod, element, implementedClass, false));

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

  static String methodSignature(
      MethodElement element,
      MethodElement overridenElement,
      InterfaceType implementedClass,
      bool isOverride,
      [String inheritedType = '']) {
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
    var returnTypeName = Naming.getReturnType(element);
    var returnType = element.returnType;
    var typeParameter = "";

    // Check if the method has a generic return value
    if (returnType is TypeParameterElement) {
      returnTypeName = Types.getDartTypeName(overridenElement.returnType);
    }

    // Check if the method return type has type arguments with generic values
    if (returnType is InterfaceType) {
      var hasTypedTypeArguments = returnType.typeArguments
          .any((argument) => argument is TypeParameterType);
      if (hasTypedTypeArguments) {
        returnTypeName = Naming.nameWithTypeArguments(returnType, false);
      }
    }

    if (returnTypeName.contains(
            "List<FlutterSDK.Widgets.Widgetinspector.LocationCount>") ||
        returnTypeName.contains(
            "List<FlutterSDK.Widgets.Widgetinspector.DiagnosticsPathNode>")) {
      print("ups");
    }

    if (isOverride && returnTypeName == 'T') {
      returnTypeName = inheritedType;
    }

    return "${returnTypeName} ${methodName}${typeParameter}(${parameter})";
  }

  static String printAutoParameters(FunctionTypedElement element) {
    return element.parameters.where((x) {
      return x.isInitializingFormal == true;
    }).map((p) {
      var variableName = '';

      if (p.name.startsWith('_'))
        variableName = p.name;
      else
        variableName =
            Naming.getFormattedName(p.name, NameStyle.UpperCamelCase);

      return 'this.$variableName = ' +
          Naming.getFormattedName(p.name, NameStyle.LowerCamelCase) +
          ';';
    }).join('\n');
  }

  /// This function returns a comma delimited list that
  static String printParameterNames(FunctionTypedElement element) {
    return element.parameters.map((p) {
      var parameterName =
          Naming.getFormattedName(p.name, NameStyle.LowerCamelCase);
      if (parameterName == "")
        parameterName = "p" + (element.parameters.indexOf(p) + 1).toString();

      return parameterName;
    }).join(', ');
  }

  static String printParameter(FunctionTypedElement method,
      FunctionTypedElement overridenMethod, InterfaceType implementedClass) {
    // Parameter
    var parameters = method.parameters.map((p) {
      // Name
      var parameterName =
          Naming.getFormattedName(p.name, NameStyle.LowerCamelCase);
      if (parameterName == "")
        parameterName = "p" + (method.parameters.indexOf(p) + 1).toString();


      if (parameterName.toLowerCase().contains('onswitchedtrain'))
      parameterName = parameterName;
      var parameterType =
          Types.getParameterType(p, method, overridenMethod, implementedClass);

      if (parameterType == null) {
        parameterType = "object";
      }

      var parameterSignature = parameterType + " " + parameterName;

      // Add keys
      // Required
      if (p.hasRequired || p.toString().contains("@required")) {
        parameterSignature = "[NotNull] " + parameterSignature;
      }

      // Optional
      if (p.isOptional) {
        var defaultValue = "default(${parameterType})";

        // Get correct default value
        //if (p is ConstVariableElement && p.defaultValueCode != null) {
        //   defaultValue = p.defaultValueCode;
        // }

        parameterSignature += " = ${defaultValue}";
      }
      return parameterSignature;
    });
    return parameters == null ? "" : parameters.join(",");
  }
}
