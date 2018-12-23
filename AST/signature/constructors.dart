import 'package:analyzer/dart/element/element.dart';

import '../implementation/implementation.dart';
import '../naming.dart';
import 'methods.dart';
import '../config.dart';

class Constructors {
  static void printConstructor(
      StringBuffer code, ConstructorElement constructor, String generics) {
    if (constructor.enclosingElement is ClassElement) {
      var isStatic = false;
      var className = constructor.enclosingElement.name;

      var parameters = Methods.printParameter(constructor, null, null, null);
      if (constructor.name == '')
        code.writeln('public ${className}($parameters)');
      else if (constructor.name == '_')
        code.writeln('internal ${className}($parameters)');
      else if (constructor.name.startsWith('_'))
        code.writeln('internal ${className}($parameters)');
      else // I'm named, hence we are turing into static methods that return an instance
      {
        isStatic = true;
        code.writeln('private ${className}($parameters)');
      }

      // Base class call
      if (constructor.redirectedConstructor != null) {
        var baseCall = constructor.redirectedConstructor;
        var baseParameters = Methods.printParameterNames(baseCall);

        code.writeln(': base($baseParameters)');
      }

      // Fill out Constructor body
      var node = constructor.computeNode();
      if (node != null) {
        var body = '';

        if (Config.includeConstructorImplementations)
          body = Implementation.MethodBody(node.body);
        else
          body = '{ throw new NotImplementedException(); }';

        // Add auto assignments if any
        var autoAssignment = Methods.printAutoParameters(constructor, className);
        if (autoAssignment.isNotEmpty)
          body = '{\n' + autoAssignment + '\n' + body.substring(2);

        // Normal constructor body
        code.writeln(body);

        if (isStatic) {
          code.writeln(
              'public static ${className}$generics ${Naming.upperCamelCase(constructor.name)}($parameters)');
          var parameterNames = Methods.printParameterNames(constructor);
          // Call private constructor
          code.writeln(
              '{\nvar instance = new ${className}$generics($parameterNames);\nreturn instance;\n}\n');
        }
        
      } else
        code.writeln('{ }');
    } else
      throw new AssertionError(
          'A constructor is not inside a ClassElement, that should not happen.');
  }
}