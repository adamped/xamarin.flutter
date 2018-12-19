import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/src/dart/element/type.dart';
import 'package:analyzer/analyzer.dart';
import 'package:analyzer/src/dart/element/element.dart';
import 'package:analyzer/dart/ast/ast.dart';
import 'package:front_end/src/base/syntactic_entity.dart';
import 'package:front_end/src/scanner/token.dart';

import 'config.dart';
import 'implementation/implementation.dart';
import 'naming.dart';

class Types {
  static String getDartTypeName(DartType type) {
    String typeName = "object";
    if (type is InterfaceType) {
      typeName = Naming.nameWithTypeArguments(type, false);
    } else if (type is TypeParameterType) {
      typeName = type.displayName;
    }
    var formattedName = Naming.getFormattedTypeName(typeName);

    if (!(type is TypeParameterType) && type.element != null) {
      var library = type.element.library;
      if (library != null &&
          !Config.ignoredImports.contains(library.identifier) &&
          formattedName != "object") {
        var namespace = Naming.namespaceFromIdentifier(library.identifier);
        formattedName = namespace + "." + formattedName;
      }
    }

    return formattedName;
  }

  static String getVariableType(VariableElement element, VariableType type) {
    String typeName = "object";
    var elementType = element.type;

    // Function type
    if (elementType is FunctionTypeImpl) {
      return handleFunctionType(elementType);
      // Interface type
    } else if (elementType is InterfaceType) {
      typeName = Naming.nameWithTypeArguments(element.type, false);
    } else if (elementType is TypeParameterType) {
      typeName = elementType.displayName;
    } else if (element.computeNode() != null) {
      switch (type) {
        case VariableType.Field:
          typeName = Naming.tokenToText(
                  element.computeNode().beginToken.previous, true)
              .split(" ")
              .first;
          break;
        case VariableType.Parameter:
          typeName =
              Naming.tokenToText(element.computeNode().endToken.previous, true)
                  .split(" ")
                  .last;
          break;
      }
    }

    return addNamespace(elementType, typeName);
  }

  static String addNamespace(DartType type, String typeName) {
    var formattedName = Naming.getFormattedTypeName(typeName);
    var library = type.element.library;
    if (!(type is TypeParameterType) &&
        library != null &&
        !Config.ignoredImports.contains(library.identifier) &&
        formattedName != "object") {
      var namespace = Naming.namespaceFromIdentifier(library.identifier);
      formattedName = namespace + "." + formattedName;
    }
    return formattedName;
  }

  static String handleFunctionType(FunctionTypeImpl elementType) {
    if (elementType.newPrune != null) {
      // We only expect one newPrune
      if (elementType.newPrune.length != 1) {
        throw new AssertionError(
            'Never accounted for elementType to have more than 1 newPrune value.');
      }

      var typeName = elementType.newPrune[0].displayName;

      var typeAndNamespace = addNamespace(elementType, typeName);

      if (elementType.typeArguments.length > 0) {
        var typeParameters = elementType.typeArguments.map((p) {         
          return p.displayName;
        }).join(',');
        typeAndNamespace = '$typeAndNamespace<${typeParameters}>';
      }

      return typeAndNamespace;
    }
    // Type did not have newPrune, resolve the func or action manually

    var parameterTypes = '';
    if (elementType.normalParameterTypes == null)
      throw new AssertionError('Its null');
    if (elementType.normalParameterTypes != null) {
      parameterTypes = elementType.normalParameterTypes.map((p) {
        if (p is InterfaceType) {
          var type = Naming.getFormattedTypeName(p.name);
          var typeArguments = p.typeArguments.map((t) {
            return Naming.getFormattedTypeName(t.displayName);
          }).join(',');
          if (typeArguments.isEmpty)
            return type;
          else
            return type + '<$typeArguments>';
        } else
          return Naming.getFormattedTypeName(p.displayName);
      }).join(',');
    }

    // Remove all spaces
    parameterTypes = parameterTypes.replaceAll(' ', '');

    if (elementType.returnType is VoidType) {
      // This is an Action
      if (parameterTypes.isEmpty)
        return 'Action';
      else
        return 'Action<$parameterTypes>';
    } else {
      // This is a Function
      var returnType = elementType.returnType.name;
      if (parameterTypes.isNotEmpty)
        return 'Func<$returnType,$parameterTypes>';
      else
        return 'Func<$returnType>';
    }
  }

  static String getParameterType(
      ParameterElement parameter,
      FunctionTypedElement method,
      FunctionTypedElement overridenMethod,
      InterfaceType implementedClass) {
    // Type

    //if (parameterType == 'object' && !parameter.toString().contains('dynamic'))
    //  parameterType = '';

    // if parameter is generic, we use the parameter signature of the method that got overriden
    if (parameter.type.element is TypeParameterElement &&
        overridenMethod != null) {
      parameter = overridenMethod.parameters[
          method.parameters.indexWhere((x) => x.name == parameter.name)];
    }

    if (parameter.toString().toLowerCase().contains('onchanged')) {
      parameter.toString();
    }

    var parameterType =
        getVariableType(parameter, VariableType.Parameter).split(" ").last;

    if (parameterType == "@") {
      parameterType = "object";
    }

    if (parameterType == 'object' && !(parameter.type is DynamicTypeImpl)) {
      var computedParameter = parameter.computeNode();

      parameterType = getTypeFromComputedNodeEntities(
          parameter, computedParameter.childEntities, false);
    }

    return parameterType;
  }

  static String getTypeFromComputedNodeEntities(ParameterElement parameter,
      Iterable<SyntacticEntity> childEntities, bool isRequired) {
    // Skip the '@required' key, the c# key will be added later
    var firstEntityName = childEntities.first.toString();
    if (["@required", "covariant"]
        .any((ignored) => firstEntityName == ignored)) {
      childEntities = childEntities.skip(1);
    }

    // this first element should be the element, that represents the type
    var expectedTypeEntity = childEntities.elementAt(0);

    // One child entity, parameter might be wrapped
    if (childEntities.length == 1) {
      if (expectedTypeEntity is SimpleFormalParameter) {
        return getTypeFromComputedNodeEntities(
            parameter, expectedTypeEntity.childEntities, isRequired);
      } else {
        throw new AssertionError("Parameter uses unexpected type.");
      }
      // Two child entities, type + name
    } else if (childEntities.length == 2) {
      if (expectedTypeEntity is TypeName) {
        // Check if this is a prefixed identifier eg. 'ui.actualType'
        if (expectedTypeEntity.childEntities.length == 1 &&
            expectedTypeEntity.childEntities.first is PrefixedIdentifier) {
          var prefixedIdentifier =
              expectedTypeEntity.childEntities.first as PrefixedIdentifier;
          return prefixedIdentifier.childEntities.last.toString();
        } else
          return Implementation.processTypeName(expectedTypeEntity);
      } else {
        throw new AssertionError("Parameter uses unexpected type.");
      }
    }

    // More then two entities, this can have multiple reasons

    // Check if this is an auto assigning parameter (eg. this._aPropertyName)
    if (expectedTypeEntity is KeywordToken &&
        expectedTypeEntity.lexeme == "this") {
      // Get the field that is initialized by this parameter and use its type
      var containingMethod = parameter.enclosingElement as FunctionTypedElement;
      var containingClass = containingMethod.enclosingElement as ClassElement;
      var field = containingClass.fields.firstWhere(
          (f) => f.displayName == parameter.name,
          orElse: () => null);
      if (field != null && field is FieldElementImpl) {
        var computed = field.computeNode();
        var parent = computed.parent;
        if (parent is VariableDeclarationList) {
          var type = parent.type;

          if (type is TypeName) return Implementation.processEntity(type);
        }
        return getDartTypeName(field.type);
      } else {
        throw new AssertionError(
            "Could not find field for initialization parameter.");
      }
      // Check if the extra entities contain the default value
    } else if (parameter.isOptional) {
      if (expectedTypeEntity is TypeName) {
        return Implementation.processTypeName(childEntities.first);
      } else if (expectedTypeEntity is SimpleFormalParameter) {
        return getTypeFromComputedNodeEntities(
            parameter, expectedTypeEntity.childEntities, isRequired);
      } else {
        throw new AssertionError("Optional parameter uses unexpected type.");
      }
    }

    throw new AssertionError(
        "Could not resolve parameter type from computed node.");
  }
}
