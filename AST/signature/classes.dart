import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';

import '../comments.dart';
import '../naming.dart';
import 'fields.dart';
import 'methods.dart';

class Classes { 
  static String printClass(ClassElement element) {
    var implementWithInterface = element.isMixin || element.isAbstract;  
    var name = Naming.nameWithTypeParameters(element, false); 
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);

    if(name == "WidgetInspectorService")
    {
      print("PaintingBinding");
    }

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
    var inheritions = new List<String>();
    if (hasBaseClass) {
      var baseClass = Naming.nameWithTypeArguments(element.supertype, false);
      inheritions.add(baseClass);
    }

    // Add interfaces
    for (var interface in element.interfaces) {
      inheritions.add(Naming.nameWithTypeArguments(interface, true));
    }

    // Add mixin interfaces
    for (var mxin in element.mixins) {
      inheritions.add(Naming.nameWithTypeArguments(mxin, true));
    }

    // Add superclasses of mixins 
    for (var mixinSuperclass in element.superclassConstraints.where((c) => c.displayName != "@Object" && c.displayName != "object" && c.displayName != "Object")) {
      inheritions.add(Naming.nameWithTypeArguments(mixinSuperclass, true));
    }

    // add its interface if class is a mixin
    if (implementWithInterface) {
      inheritions.add(Naming.mixinInterfaceName(element));
    }

    if (inheritions.length > 0) code.write(" : " + inheritions.join(","));

    code.writeln("{\n");

    printFieldsAndMethods(code, element, implementWithInterface);

    code.writeln("}");
    return code.toString();
  }

  static void printFieldsAndMethods(
      StringBuffer code, ClassElement element, bool implementWithInterface) {
    // Add mixin fields and method implementations
    code.writeln("#region inherited methods and fields");
    var overridenImplementations = new List<ClassMemberElement>();
    var implementedVariables = new List<ClassMemberElement>();

    // Add mixin implementations
    if (element.mixins.length > 0) {
      code.writeln("#region inherited from mixins");
      for (var implementedMixin in element.mixins.reversed) {
        code.writeln("#region inherited from ${implementedMixin.name}");
        code.writeln(implementedInstanceName(implementedMixin));

        addImplementedFields(code, implementedMixin.element, element,
            implementedVariables, overridenImplementations);
        addImplementedMethods(code, implementedMixin.element, element,
            implementedVariables, overridenImplementations);
        code.writeln("#endregion\n");
      }
      code.writeln("#endregion\n");
    }

    // Add interface implementations
    var extendingClasses = element.interfaces.toList();
    extendingClasses.addAll(element.superclassConstraints.where((c) => c.name != "Object"));
    if (extendingClasses.length > 0) {
      code.writeln("#region inherited from interfaces");
      for (var implementedClass in extendingClasses.reversed) {
        code.writeln("#region inherited from ${implementedClass.name}");
        code.writeln(implementedInstanceName(implementedClass));

        addImplementedFields(code, implementedClass.element, element,
            implementedVariables, overridenImplementations);
        addImplementedMethods(code, implementedClass.element, element,
            implementedVariables, overridenImplementations);
        code.writeln("#endregion\n");
      }
      code.writeln("#endregion\n");
    }
    code.writeln("#endregion\n");

    code.writeln("#region fields");
    // Add fields that are not already handled as implementation overrides
    for (var field
        in element.fields.where((f) => !overridenImplementations.contains(f))) {
      code.writeln(Fields.printField(field));
    }
    code.writeln("#endregion\n");

    code.writeln("#region methods");
    // Add methods that are not already handled as implementation overrides
    for (var method in element.methods
        .where((m) => !overridenImplementations.contains(m))) {
      code.writeln(Methods.printMethod(method, implementWithInterface,
          Methods.overridesParentBaseMethod(method, element)));
    }
    code.writeln("#endregion");
  }

  static String implementedInstanceName(InterfaceType element) {
    var implementedTypeName = element.name;
    var mxinNameWithTypes = Naming.nameWithTypeArguments(element, false);
    if (mxinNameWithTypes.contains("AnimationWithParentMixin")) {
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
          Fields.printImplementedField(
              overrideElement != null ? overrideElement : implementedField,
              implementedTypeName));
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
      var overridingMethod = implementingType.methods.firstWhere(
          (method) => method.name == implementedMethod.name,
          orElse: () => null);
      // Store the overriding method to avoid adding it again when adding the other methods
      if (overridingMethod != null) addedByMxin.add(overridingMethod);

      code.writeln(Methods.printImplementedMethod(
          // Pass the overriden element to get the correct method modifiers
          overridingMethod != null ? overridingMethod : implementedMethod,
          implementedTypeName,
          overridingMethod,
          implementingType));
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
      code.writeln(Methods.methodSignature(method) + ";");
    }

    for (var field in element.fields
        .where((field) => field.isPublic || field.hasProtected)) {
      code.writeln(Fields.getFieldSignature(field));
    }

    code.writeln("}");
    return code.toString();
  }
}
