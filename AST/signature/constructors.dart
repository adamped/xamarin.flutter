import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/src/dart/element/element.dart';
import 'package:analyzer/dart/ast/ast.dart';
import '../implementation/implementation.dart';
import '../naming.dart';
import 'methods.dart';
import '../config.dart';
import 'package:front_end/src/scanner/token.dart';

class Constructors {
  static void printConstructor(
      StringBuffer code, ConstructorElementImpl constructor, String generics) {
    if (constructor.enclosingElement is ClassElement) {
      var isFactory = false;
      var className = constructor.enclosingElement.name;
      var constructorName = constructor.name;
      var callsBaseCtor = constructor.redirectedConstructor != null || (constructor.constantInitializers != null && constructor.constantInitializers.length > 0);

      var parameters = Methods.printParameter(constructor, null, null);
      // normal constructors do not have any special key chars
      if (constructorName == '')
        code.writeln('public ${className}($parameters)');
      // internal classes start with an underscore in dart
      else if (constructorName == '_' || constructorName.startsWith('_'))
        code.writeln('internal ${className}($parameters)');
      else // I'm named, hence we are turing into static methods that return an instance
      {
        isFactory = true;
        code.writeln(
            'public static ${className}$generics ${Naming.upperCamelCase(constructorName)}($parameters)');
      }

      // Base class call
      if (callsBaseCtor && !isFactory) {
        code.writeln(': base(${getBaseParameters(constructor)})');
      }

      var instanceName = isFactory ? "instance" : "this";

      // Fill out Constructor body
      var node = constructor.computeNode();
      if (node != null) {
        var body = '{\n';

        if (isFactory) {
          // Insert initialization if this is a factory method
          body += 'var instance =';
          if (callsBaseCtor) {
            var parameters = getBaseParameters(constructor);
            //TODO Get the correct constructor name in case this class does not call its own constructor!
            body += 'new ${className}$generics(${parameters});';
          } else
            body += 'new ${className}$generics();';
        }

        // Add auto assignments if any
        var autoAssignment = Methods.printAutoParameters(constructor, className,
            instanceName: instanceName);
        if (autoAssignment.isNotEmpty) body += autoAssignment;

        // add logic and closing curly brace
        if (Config.includeConstructorImplementations)
          body = Implementation.MethodBody(node.body).substring(2);
        else
          body += 'throw new NotImplementedException(); }';

        // Normal constructor body
        code.writeln(body);
      } else
        code.writeln('{ }');
    } else
      throw new AssertionError(
          'A constructor is not inside a ClassElement, that should not happen.');
  }

  static String getBaseParameters(ConstructorElement constructor) {
    // Get parameters
    var parameters = "";
    if (constructor is ConstructorElementImpl &&
        constructor.constantInitializers.length > 0) {
      // :)
      var constantInitializer = constructor.constantInitializers.first;
      var argumentList = constantInitializer.childEntities
          .where((x) => x is ArgumentList);
      if (argumentList != null && argumentList.length > 0) {
        ArgumentList list = argumentList.firstWhere((x) => x is ArgumentList);
        int count = 0;
        parameters = list.childEntities
            .where((argument) =>
                argument is! BeginToken && argument is! SimpleToken)
            .map((argument) {
              var parameter = Naming.escapeFixedWords(Implementation.processEntity(argument)).trim();
              if (parameter == 'null' && constructor.redirectedConstructor.parameters[count].type.displayName == 'T')
              {
                  // Can't pass null to generic type in C# (you can in Dart).
                  parameter = 'default(T)';
              }
              count += 1;
              return parameter;
               })
            .join(",");
      }
    }
    return parameters;
  }
}
