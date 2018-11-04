import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';

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

    if (element.hasProtected == true) code.write("protected ");
    if (element.isPublic == true) code.write("public ");
    if (element.isPrivate == true) code.write("private ");
    if (element.hasOverride == true) code.write("override ");
    if (element.hasSealed == true)
      code.write("sealed ");
    // Add virtual as default key if method is not already abstract since all methods are virtual in dart
    else if (element.hasOverride == false && element.isPrivate == false)
      code.write("virtual ");

    if (name == "addListener") {
      print("test");
    }

    code.write(methodSignature(element, name));

    code.writeln("{");
    code.writeln(printMethodBody(element.computeNode().body));
    code.writeln("}");

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
      code.writeln(printMethodBody(element.computeNode().body));
    }
    code.writeln("}");

    return code.toString();
  }

  static String methodSignature(MethodElement element, String name) {
    var parameters = element.parameters.map((p) {
      var parameterName = p.name;
      var parameterType = Naming.getVariableType(p);
      return parameterType + " " + parameterName;
    });
    var parameter = parameters == null ? "" : parameters.join(",");
    return "${Naming.getReturnType(element)} ${name}(${parameter})";
  }

  static String printMethodBody(FunctionBody element) {
    var bodyLines = Naming.tokenToText(element.beginToken).split("\n");
    return bodyLines.map((l) => "// ${l}\n").join() +
        "throw new NotImplementedException();";
  }
}
