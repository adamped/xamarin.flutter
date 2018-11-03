import 'package:analyzer/dart/element/element.dart';

import 'comments.dart';
import 'config.dart';
import 'fields.dart';
import 'methods.dart';
import 'naming.dart';

class Frame {
  static String printNamespace(
      CompilationUnitElement element, String namespace) {
    var code = new StringBuffer();
    code.writeln(printImports(element));
    code.writeln("namespace ${namespace}{");
    // Mixins (Interface + Implementation)
    for (var type in element.types.where((t) => isMixin(t))) {
      code.writeln(printInterface(type));
    }

    // Classes (Without mixins)
    for (var type in element.types) {
      code.writeln(printClass(type));
    }
    // Enums
    for (var type in element.enums) {
      code.writeln(printEnum(type));
    }

    code.writeln("}");
    return code.toString();
  }

  static String printImports(CompilationUnitElement element) {
    var imports = new List<String>()..add("System");
    for (var import in element.enclosingElement.imports) {
      // Skip dart:core, we import System instead.
      if (import.importedLibrary != null &&
          import.importedLibrary.displayName == "dart.core") continue;
      var name = import.uri;

      // Check other default imports
      switch (name) {
        case "package:flutter/foundation.dart":
        case "dart:ui":
        case "dart:math":
          continue;
      }

      // Check if import is within the FlutterSDK library and modify the import
      if (import.importedLibrary != null &&
          import.importedLibrary.identifier
              .replaceAll("/", "\\")
              .contains(Config.directoryPath)) {
        name =
            Naming.namespaceFromIdentifier(import.importedLibrary.identifier);
      }
      imports.add(name);
    }
    return imports.map((import) => "using ${import};").join("\n");
  }

  static String printEnum(ClassElement element) {
    var name = element.name;

    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (element.hasProtected == true || element.isPrivate == true)
      code.write("internal ");
    if (element.isPublic == true) code.write("public ");

    code.writeln("enum ${name}{");
    code.writeln("");
    for (var value in element.fields.where((e) => e.isEnumConstant)) {
      Comments.appendComment(code, value);
      code.writeln(value.name + ",");
    }
    code.writeln("}");
    return code.toString();
  }

  static bool isMixin(ClassElement element) {
    return element.name.contains("Mixin");
  }

  static String printClass(ClassElement element) {
    var handleAsMixing = isMixin(element);

    var name = Naming.nameWithTypeParameters(element, false);
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (element.hasProtected == true || element.isPrivate == true)
      code.write("internal ");
    if (element.isPublic == true) code.write("public ");
    if (element.isAbstract == true && !handleAsMixing) code.write("abstract ");
    if (element.hasSealed == true) code.write("sealed ");

    code.write("class ${name}");

    // Add base class, interfaces, mixin interfaces
    var hasBaseClass =
        element.supertype != null && element.supertype.name != "Object";
    var base = new List<String>();
    if (hasBaseClass) {
      var baseClass = Naming.nameWithTypeArguments(element.supertype, false);
      base.add(baseClass);
    }
    // Add mixin interfaces
    for (var mxin in element.mixins) {
      base.add(Naming.nameWithTypeArguments(mxin, true));
    }
    // add its interface if class is a mixin
    if (handleAsMixing) {
      base.add(Naming.mixinInterfaceName(element));
    }

    if (base.length > 0) code.write(" : " + base.join(","));

    code.writeln("{\n");

    code.writeln("#region mixin methods");
    var addedByMxin = new List<MethodElement>();
    var mxinMethods = new List<MethodElement>();
    // Add mixin fields and method implementations
    for (var mxin in element.mixins.reversed) {
      var fieldName = mxin.name;
      var mxinNameWithTypes = Naming.nameWithTypeArguments(mxin, false);
      code.writeln(
          "private ${mxinNameWithTypes} ${fieldName} = new ${mxinNameWithTypes}();");
      for (var mxinMethod in mxin.methods.where((method) =>
          method.isPublic &&
          !mxinMethods.any((existingMethod) =>
              existingMethod.toString() == method.toString()))) {
        // Store which methods are already implemented to avoid multiple declarations of the same method
        mxinMethods.add(mxinMethod);

        // Check if a method in this class overrides the mixin method
        // Use the method body of the overriding method in this case
        var overrideElement = element.methods.firstWhere(
            (method) => method.name == mxinMethod.name,
            orElse: () => null);
        // Store the overriding method to avoid adding it again when adding the other methods
        if (overrideElement != null) addedByMxin.add(overrideElement);

        code.writeln(Methods.printMxinMethod(
            mxinMethod, fieldName, overrideElement, element));
      }
    }
    code.writeln("#endregion\n");

    code.writeln("#region fields");
    for (var field in element.fields) {
      code.writeln(Fields.printField(field));
    }
    code.writeln("#endregion\n");

    code.writeln("#region methods");
    // Add methods that are not already handled as mixin method overrides
    for (var method
        in element.methods.where((method) => !addedByMxin.contains(method))) {
      code.writeln(Methods.printMethod(method, handleAsMixing,
          Methods.overridesParentBaseMethod(method, element)));
    }
    code.writeln("#endregion\n");

    code.writeln("}");
    return code.toString();
  }

  static String printInterface(ClassElement element) {
    var name = Naming.nameWithTypeParameters(element, true);
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (element.hasProtected == true || element.isPrivate == true)
      code.write("internal ");
    if (element.isPublic == true) code.write("public ");

    code.write("interface ${name}{\n");
    for (var method in element.methods
        .where((method) => method.isPublic || method.hasProtected)) {
      var methodName = Naming.nameWithTypeParameters(method, false);
      code.writeln(Methods.methodSignature(method, methodName) + ";");
    }
    code.writeln("}");
    return code.toString();
  }
}
