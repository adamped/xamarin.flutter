import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/src/dart/element/element.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/src/dart/element/member.dart';

import '../comments.dart';
import '../naming.dart';
import 'constructors.dart';
import 'fields.dart';
import 'methods.dart';

class Classes {
  static String printClass(ClassElement element) {
    var implementWithInterface = element.isMixin || element.isAbstract;
    var name = Naming.nameWithTypeParameters(element, false);
    var code = new StringBuffer();
    code.writeln("");
    Comments.appendComment(code, element);    
    code.write("public ");
    if (element.isAbstract == true && !implementWithInterface)
      code.write("abstract ");
    if (element.hasSealed == true) code.write("sealed ");

    code.write("class ${name}");

    var generics = '';
    if (name.contains('<'))
      generics = name.substring(name.indexOf('<'), name.lastIndexOf('>') + 1);

    // Add base class, interfaces, mixin interfaces
    var hasBaseClass =
        element.supertype != null && element.supertype.name != "Object";
    var inheritions = new List<String>();
    if (hasBaseClass) {
      inheritions.add(Naming.className(element.supertype));
    }

    // Add interfaces
    for (var interface in element.interfaces) {
      inheritions.add(Naming.nameWithTypeArguments(interface, true));
    }

    // Add mixin interfaces
    for (var mxin in element.mixins) {
      inheritions.add(Naming.nameWithTypeArguments(mxin, true));
    }

    if (inheritions.length > 0) code.write(" : " + inheritions.join(","));

    code.writeln("\n{");

    code.writeln("#region constructors");

    // Add constructors
    for (var constructor in element.constructors) {
      Constructors.printConstructor(code, constructor, generics);
    }
    code.writeln("#endregion\n");

    // Add fields and methods
    printFieldsAndMethods(code, element, implementWithInterface);

    code.writeln("}");
    return code.toString();
  }

  static void printFieldsAndMethods(StringBuffer code, ClassElement element,
      bool implementWithInterface) {
   
    code.writeln("#region fields");
    // Add fields that are not already handled as implementation overrides
    for (var field
        in element.fields) {
      code.writeln(Fields.printField(field));
    }
    code.writeln("#endregion\n");

    code.writeln("#region methods");

    // Add methods that are not already handled as implementation overrides
    for (var method in element.methods) {
      code.writeln(Methods.printMethod(method,
          Methods.overridesParentBaseMethod(method, element)));
    }
    code.writeln("#endregion");
  }
 
  static String printMixin(ClassElement element)
  {
    var name = Naming.nameWithTypeParameters(element, true);
    var code = new StringBuffer();
    code.writeln("");

    var rawName = name.substring(1); // Name without `I` at front

    // Start Mixin Interface
    code.write("public interface $name");

    // Inherits
    var mixinInheritance = element.allSupertypes.where((x) { return x.displayName != 'Object';}).map((f) { return Naming.nameWithTypeParameters(f.element, true); }).join(',');
    
    if (mixinInheritance.isNotEmpty)
      code.write(': $mixinInheritance');

    code.writeln('{}\n'); 
    // End Mixin Interface


    // Start Instance class 
    var mixinInstanceInheritance = element.allSupertypes.where((x) { return x.displayName != 'Object';}).map((f) { return Naming.nameWithTypeParameters(f.element, true).substring(1); }).join(',');
    
    code.write('public class ${rawName}'); 

    if (mixinInstanceInheritance.isNotEmpty)
      code.write(': $mixinInstanceInheritance');

    code.writeln('{');

    // Fields and Methods
    
    // Add fields
    for (var field
        in element.fields) {
      code.writeln(Fields.printField(field));
    }
  
    // Add methods
    for (var method in element.methods) {
      code.writeln(Methods.printMethod(method,
          Methods.overridesParentBaseMethod(method, element)));
    }    
   
    code.writeln('}'); // End Instance Class

    // Mixin static extensions class 
    code.writeln('public static class ${name}Mixin {');

    // Instance holding for mixins
    code.writeln('static System.Runtime.CompilerServices.ConditionalWeakTable<$name, $rawName> _table = new System.Runtime.CompilerServices.ConditionalWeakTable<$name, $rawName>();');

    // Extension Methods

    // Add Fields
    for (var field
        in element.fields.where((x) { return x.isPublic; })) {
          var typeAndName = Fields.printTypeAndName(field);
          var fieldName = Fields.getFieldName(field);
      code.writeln('public static ${typeAndName}Property(this $name instance) => _table.GetOrCreateValue(instance).$fieldName;');
    }

     // Add Methods
    for (var method
        in element.methods.where((x) { return x.isPublic; })) {
          var methodName = Methods.getMethodName(method);
          var signature = Methods.methodSignature(method, null, false, '', null, 'this $name instance');
          
          code.writeln('public static ${signature} => _table.GetOrCreateValue(instance).$methodName();');     
    }

    code.writeln('}'); // End Mixin Class

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
      var baseMethod = Methods.getBaseMethodInClass(method);
      code.writeln(
          Methods.methodSignature(baseMethod, method, false) + ";");
    }

    for (var field in element.fields
        .where((field) => field.isPublic || field.hasProtected)) {
      
      var baseField = Fields.getBaseFieldInClass(field);
      code.writeln(Fields.getFieldSignature(baseField));
    }

    code.writeln("}");
    return code.toString();
  }
}
