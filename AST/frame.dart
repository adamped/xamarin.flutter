import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';

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

    // Interaces for mxins and abstract classes
    for (var type in element.types.where((t) => t.isAbstract == true)) {
      code.writeln(printInterface(type));
    }

    // Add mixins and their interfaces
    for (var mxin in element.mixins) {
      code.writeln(printInterface(mxin));
      code.writeln(printClass(mxin));
    }

    // Classes
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
        case "dart:ui":
        case "dart:async":
        case "dart:math":
        case "package:meta/meta.dart":
          continue;
      }

      if (name.contains("package:flutter/")) {
        name = name.replaceAll("package:flutter/", "");
        name = name.replaceAll(".dart", "");
        name = Config.rootNamespace + "." + name;
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
    return element.isMixin;
  }

  static String printClass(ClassElement element) {
    var implementWithInterface = isMixin(element) || element.isAbstract;

    var name = Naming.nameWithTypeParameters(element, false);
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if (element.hasProtected == true || element.isPrivate == true)
      code.write("internal ");
    if (element.isPublic == true) code.write("public ");
    if (element.isAbstract == true && !implementWithInterface)
      code.write("abstract ");
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

    // Add interfaces
    for (var interface in element.interfaces) {
      base.add(Naming.nameWithTypeArguments(interface, true));
    }

    // Add mixin interfaces
    for (var mxin in element.mixins) {
      base.add(Naming.nameWithTypeArguments(mxin, true));
    }
    // add its interface if class is a mixin
    if (implementWithInterface) {
      base.add(Naming.mixinInterfaceName(element));
    }

    if (base.length > 0) code.write(" : " + base.join(","));

    code.writeln("{\n");

    addFieldsMethods(code, element, implementWithInterface);

    code.writeln("}");
    return code.toString();
  }

  static void addFieldsMethods(
      StringBuffer code, ClassElement element, bool implementWithInterface) {
    // Add mixin fields and method implementations
    code.writeln("#region inherited methods and fields");
    var addedByMxin = new List<ClassMemberElement>();
    var implementedVariables = new List<ClassMemberElement>();

    code.writeln("#region inherited from mixins");
    for (var implementedMixin in element.mixins.reversed) {
      code.writeln(implementedInstanceName(implementedMixin));

      addImplementedFields(code, implementedMixin.element, element,
          implementedVariables, addedByMxin);
      addImplementedMethods(code, implementedMixin.element, element,
          implementedVariables, addedByMxin);
    }
    code.writeln("#endregion");

    code.writeln("#region inherited from abstract class interfaces");
    for (var implementedClass in element.interfaces.reversed) {
      code.writeln(implementedInstanceName(implementedClass));

      addImplementedFields(code, implementedClass.element, element,
          implementedVariables, addedByMxin);
      addImplementedMethods(code, implementedClass.element, element,
          implementedVariables, addedByMxin);
    }
    code.writeln("#endregion");
    code.writeln("#endregion\n");

    code.writeln("#region fields");

    // Add fields that are not already handled as implementation overrides
    for (var field
        in element.fields.where((method) => !addedByMxin.contains(method))) {
      code.writeln(Fields.printField(field));
    }
    code.writeln("#endregion\n");

    // Add methods that are not already handled as mixin method overrides
    for (var method
        in element.methods.where((method) => !addedByMxin.contains(method))) {
      code.writeln(Methods.printMethod(method, implementWithInterface,
          Methods.overridesParentBaseMethod(method, element)));
    }
  }

  static String implementedInstanceName(InterfaceType element) {
    var implementedTypeName = element.name;
    var mxinNameWithTypes = Naming.nameWithTypeArguments(element, false);
    if(mxinNameWithTypes.contains("AnimationWithParentMixin")){
      print("test");
    }
    // Add instance of the implemented class
    // The implementations of the interface will call the methods provided by this instance
    return "private ${mxinNameWithTypes} ${implementedTypeName} = new ${mxinNameWithTypes}();";
  }

  static void addImplementedFields(
      StringBuffer code,
      ClassElement implementedType,
      ClassElement implementingType,
      List<ClassMemberElement> mxinMethods,
      List<ClassMemberElement> overridenImplementedMethods) {
    var implementedTypeName = implementedType.name;

    for (var implementedField in implementedType.fields.where((field) =>
        field.isPublic &&
        !mxinMethods.any((existingMethod) =>
            existingMethod.toString() == field.toString()))) {
      // Store which methods are already implemented to avoid multiple declarations of the same method
      mxinMethods.add(implementedField);

      // Check if a field in this class overrides the implemented method
      // Use the method body of the overriding field in this case
      var overrideElement = implementingType.fields.firstWhere(
          (method) => method.name == implementedField.name,
          orElse: () => null);
      // Store the overriding field to avoid adding it again when adding the other fields
      if (overrideElement != null)
        overridenImplementedMethods.add(overrideElement);

      code.writeln(
        // Pass the overriden element to get the correct field signature
          Fields.printImplementedField(overrideElement != null ? overrideElement : implementedField, implementedTypeName));
    }
  }

  static void addImplementedMethods(
      StringBuffer code,
      ClassElement implementedType,
      ClassElement implementingType,
      List<ClassMemberElement> mxinMethods,
      List<ClassMemberElement> addedByMxin) {
    var implementedTypeName = implementedType.name;

    for (var implementedMethod in implementedType.methods.where((method) =>
        method.isPublic &&
        !mxinMethods.any((existingMethod) =>
            existingMethod.toString() == method.toString()))) {
      // Store which methods are already implemented to avoid multiple declarations of the same method
      mxinMethods.add(implementedMethod);

      // Check if a method in this class overrides the implemented method
      // Use the method body of the overriding method in this case
      var overrideElement = implementingType.methods.firstWhere(
          (method) => method.name == implementedMethod.name,
          orElse: () => null);
      // Store the overriding method to avoid adding it again when adding the other methods
      if (overrideElement != null) addedByMxin.add(overrideElement);

      code.writeln(Methods.printMxinMethod(
        // Pass the overriden element to get the correct method signature
          overrideElement != null ? overrideElement : implementedMethod, implementedTypeName, overrideElement, implementingType));
    }
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

    for (var field in element.fields
        .where((field) => field.isPublic || field.hasProtected)) {
      code.writeln(Fields.getFieldSignature(field));
    }

    code.writeln("}");
    return code.toString();
  }
}
