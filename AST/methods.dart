import 'package:analyzer/dart/element/element.dart';

import 'comments.dart';
import 'naming.dart';

class Methods {
  static bool isSameSignature(MethodElement m1, MethodElement m2) {
    return methodSignature(m1, m1.name) == methodSignature(m2, m2.name);
  }

  static bool overridesBaseMethod(MethodElement method, ClassElement element) {
    if (element.methods.any((m) => isSameSignature(m, method)) ||
        overridesParentBaseMethod(method, element)) return true;

    return false;
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
    var name = Naming.nameWithTypeParameters(element, false);
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (insideMixin) {
      if (element.isPrivate)
        code.write("private ");
      else
        code.write("public ");
    } else {
      if (element.hasProtected == true) code.write("protected ");
      if (element.isPublic == true) code.write("public ");
      if (element.isAbstract == true) code.write("abstract ");
      if (element.isPrivate == true) code.write("private ");
      if (element.hasOverride == true) code.write("override ");
      if (element.hasSealed == true)
        code.write("sealed ");
      // Add virtual as default key if method is not already abstract since all methods are virtual in dart
      else if (element.hasOverride == false &&
          element.isAbstract == false &&
          element.isPrivate == false) code.write("virtual ");
    }

    code.write(methodSignature(element, name));

    if (element.isAbstract && !insideMixin) {
      code.writeln(";");
    } else {
      code.writeln("{");
      code.writeln(printMethodBody(element));
      code.writeln("}");
    }

    return code.toString();
  }

  static String printMxinMethod(MethodElement element, String mxinFieldName,
      MethodElement overrideMethod, ClassElement classElement) {
    var name = Naming.nameWithTypeParameters(element, false);

    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (element.isPublic == true) code.write("public ");
    if (element.hasProtected == true) code.write("protected ");
    if (element.hasSealed == true) code.write("sealed ");
    if (overridesParentBaseMethod(element, classElement))
      code.write("override ");

    code.write(methodSignature(element, name));

    code.writeln("{");
    if (overrideMethod == null) {
      code.writeln(
          "${mxinFieldName}.${name}(${element.parameters.map((p) => p.name).join(",")});");
    } else {
      code.writeln(printMethodBody(element));
    }
    code.writeln("}");

    return code.toString();
  }

  static String methodSignature(MethodElement element, String name) {
    var returnType = element.returnType.displayName;

    var parameters = element.parameters.map((p) {
      var parameterName = p.name;
      var parameterType = p.computeNode().beginToken.lexeme;
      return parameterType + " " + parameterName;
    });
    var parameter = parameters == null ? "" : parameters.join(",");
    return "${returnType} ${name}(${parameter})";
  }

  static String printMethodBody(MethodElement element) {
    var bodyLines =
        element.computeNode().body.childEntities.first.toString().split("\n");
    return bodyLines.map((l) => "// ${l}\n").join() +
        "throw new NotImplementedException();";
  }
}
