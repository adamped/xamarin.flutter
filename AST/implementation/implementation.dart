import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/syntactic_entity.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:front_end/src/scanner/token.dart';
import '../naming.dart';

/// Provides methods to transpile the body of elements
class Implementation {
  static String MethodBody(MethodElement element) {
    var body = element.computeNode().body;

    if (body is EmptyFunctionBody) {
      return '{}'; // No code;
    } else if (body is BlockFunctionBody) {
      return processBlockFunction(body);
    } else if (body is ExpressionFunctionBody) {
      return processExpressionFunction(body);
    } else {
      // Nothing comes here, so I have implemented all for now.
      // But this is here in case something in the future appears
      // and needs to be accounted for.
      return "\nthrow new NotImplementedException();\n";
    }
  }

  static String processExpressionFunction(ExpressionFunctionBody body) {
    var rawBody = "";
    for (var child in body.childEntities) {
      rawBody += processEntity(child);
    }
    return rawBody;
  }

  static String processBlockFunction(BlockFunctionBody body) {
    var rawBody = "\n";
    for (var child in body.childEntities) {
      if (child is Block) {
        for (var entity in child.childEntities) {
          rawBody += processEntity(entity) + "\n";
        }
      } else if (child is KeywordToken) {
        rawBody += child.toString() + "\n";
      } else if (child is SimpleToken) {
        rawBody += child.toString() + "\n";
      } else
        rawBody += "\n// Block Function type not dealt with $child";
    }

    return rawBody + "\n";
  }

  static String processEntity(SyntacticEntity entity) {
    if (entity is BeginToken) {
      return entity.lexeme;
    } else if (entity is KeywordToken) {
      return processToken(entity);
    } else if (entity is SimpleToken) {
      return entity.lexeme;
    } else if (entity is SimpleIdentifier) {
      return processSimpleIdentifier(entity);
    } else if (entity is ThisExpression) {
      return entity.toString();
    } else if (entity is NullLiteral) {
      return entity.toString();
    } else if (entity is DoubleLiteral) {
      return entity.toString();
    } else if (entity is BooleanLiteral) {
      return entity.toString();
    } else if (entity is IntegerLiteral) {
      return entity.toString();
    } else if (entity is SimpleStringLiteral) {
      return processSimpleStringLiteral(entity);
    } else if (entity is ArgumentList) {
      return processArgumentList(entity);
    } else if (entity is MapLiteral) {
      return entity.toString();
    } else if (entity is PrefixedIdentifier) {
      return processPrefixedIdentifier(entity);
    } else if (entity is DeclaredIdentifier) {
      return entity.toString();
    } else if (entity is PrefixExpression) {
      return entity.toString();
    } else if (entity is AdjacentStrings) {
      return entity.toString();
    } else if (entity is Label) {
      return entity.toString();
    } else if (entity is MethodInvocation) {
      return processMethodInvocation(entity);
    } else if (entity is FunctionExpression) {
      return entity.toString();
    } else if (entity is ParenthesizedExpression) {
      return processParenthesizedExpression(entity);
    } else if (entity is IndexExpression) {
      return entity.toString();
    } else if (entity is BinaryExpression) {
      return processBinaryExpression(entity);
    } else if (entity is AwaitExpression) {
      return entity.toString();
    } else if (entity is ConditionalExpression) {
      return processConditionalExpression(entity);
    } else if (entity is StringInterpolation) {
      return processStringInterpolation(entity);
    } else if (entity is InterpolationExpression) {
      return processInterpolationExpression(entity);
    } else if (entity is InstanceCreationExpression) {
      return entity.toString();
    } else if (entity is PropertyAccess) {
      return entity.toString();
    } else if (entity is AssignmentExpression) {
      return processAssignmentExpression(entity);
    } else if (entity is AssertStatement) {
      return ""; // I just ignore assert statements at the moment
    } else if (entity is ReturnStatement) {
      return processReturnStatement(entity);
    } else if (entity is VariableDeclaration) {
      return entity.toString();
    } else if (entity is VariableDeclarationStatement) {
      return processVariableDeclarationStatement(entity);
    } else if (entity is VariableDeclarationList) {
      return processVariableDeclarationList(entity);
    } else if (entity is SwitchStatement) {
      return processSwitchStatement(entity);
    } else if (entity is SwitchCase) {
      return processSwitchCase(entity);
    } else if (entity is BreakStatement) {
      return entity.toString();
    } else if (entity is SwitchDefault) {
      return processSwitchDefault(entity);
    } else if (entity is ContinueStatement) {
      return processContinueStatement(entity);
    } else if (entity is IfStatement) {
      return processIfStatement(entity);
    } else if (entity is IsExpression) {
      return entity.toString();
    } else if (entity is CascadeExpression) {
      return entity.toString();
    } else if (entity is ExpressionStatement) {
      return processExpressionStatement(entity);
    } else if (entity is SuperExpression) {
      return processSuperExpression(entity);
    } else if (entity is ThrowExpression) {
      return entity.toString();
    } else if (entity is WhileStatement) {
      return processWhileStatement(entity);
    } else if (entity is ForEachStatement) {
      return processForEachStatement(entity);
    } else if (entity is ForStatement) {
      return processForStatement(entity);
    } else if (entity is ListLiteral) {
      return entity.toString();
    } else if (entity is FunctionDeclarationStatement) {
      return entity.toString();
    } else if (entity is TryStatement) {
      return entity.toString();
    } else if (entity is DoStatement) {
      return entity.toString();
    } else if (entity is YieldStatement) {
      return entity.toString();
    } else if (entity is PostfixExpression) {
      return entity.toString();
    } else if (entity is AsExpression) {
      return entity.toString();
    } else if (entity is NamedExpression) {
      return entity.toString();
    } else if (entity is TypeName) {
      return processTypeName(entity);
    } else if (entity is Block) {
      var raw = "";
      for (var item in entity.childEntities) {
        raw += processEntity(item) + "\n";
      }
      return raw;
    } else {
      return entity.toString();
    }
  }

  static String processToken(KeywordToken keyword) {
    var newKeyword = keyword.keyword.lexeme;

    if (newKeyword == "super") return "base";
    if (newKeyword == "final")
      return ""; // Rarely an equivalence of const in this flutter scenario.
    return newKeyword + " ";
  }

  static String processSuperExpression(SuperExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processArgumentList(ArgumentList list) {
    var csharp = "";
    for (var entity in list.childEntities) {
      if (csharp.endsWith(', ') && entity.toString() == ')')
        csharp = csharp.substring(0, csharp.length - 2);
      csharp += processEntity(entity);
      if (entity.toString() != '(' && entity.toString() != ')') csharp += ', ';
    }
    return csharp;
  }

  static String processParenthesizedExpression(
      ParenthesizedExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processBinaryExpression(BinaryExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processIfStatement(IfStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) csharp += processEntity(entity);
    return csharp;
  }

  static String processForStatement(ForStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processWhileStatement(WhileStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processDoStatement(DoStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processForEachStatement(ForEachStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processReturnStatement(ReturnStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processMethodInvocation(MethodInvocation invocation) {
    var csharp = "";
    for (var entity in invocation.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processSimpleIdentifier(SimpleIdentifier identifier) {
    var csharp = "";
    if (identifier.staticElement is ParameterElement) // e.g. child
    {
      csharp += identifier.name;
    } else if (identifier.staticElement
        is MethodElement) // e.g animate // Can't seem to get MethodMember
    {
      csharp += processMethodElement(identifier.staticElement);
    } else if (identifier.staticElement is FunctionElement) {
      csharp += processFunctionElement(identifier.staticElement);
    } else if (identifier.staticElement is LocalVariableElement) {
      csharp += identifier.name;
    } else if (identifier.staticElement is PropertyAccessorElement) {
      csharp += processPropertyAccessorElement(identifier.staticElement);
    } else {
      csharp += identifier.name;
    }
    return csharp;
  }

  static String processPrefixedIdentifier(PrefixedIdentifier identifier) {
    var csharp = "";
    for (var entity in identifier.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processMethodElement(MethodElement element) {
    return Naming.upperCamelCase(element.displayName);
  }

  static String processFunctionElement(FunctionElement element) {
    return Naming.upperCamelCase(element.displayName);
  }

  static String processPropertyAccessorElement(
      PropertyAccessorElement element) {
    return Naming.upperCamelCase(element.displayName);
  }

  static String processStringInterpolation(StringInterpolation interpolation) {
    var csharp = '\$"';

    for (var entity in interpolation.childEntities) {
      if (entity is StringLiteral)
        csharp += entity.stringValue;
      else if (entity is InterpolationString)
        csharp += entity.value;
      else if (entity is InterpolationExpression) {
        var stringValue = '{';
        for (var item in entity.childEntities) {
          if (!(item is BeginToken) &&
              item.toString() != '}' &&
              item.toString() != '\$') stringValue += processEntity(item);
        }
        csharp += stringValue + '}';
      } else
        csharp += '{' + processEntity(entity) + '}';
    }

    return csharp + '"';
  }

  static String processInterpolationExpression(
      InterpolationExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += entity.toString();
    }
    return csharp;
  }

  static String processSwitchStatement(SwitchStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processSwitchCase(SwitchCase switchCase) {
    var csharp = "";

    for (var entity in switchCase.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processSwitchDefault(SwitchDefault switchDefault) {
    var csharp = "";

    for (var entity in switchDefault.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processContinueStatement(ContinueStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processExpressionStatement(ExpressionStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processConditionalExpression(ConditionalExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processAssignmentExpression(AssignmentExpression expression) {
    var csharp = "";
    // expression.childEntities[1].lexeme == ??=
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processSimpleStringLiteral(SimpleStringLiteral literal) {
    return '"${literal.stringValue}"';
  }

  static String processVariableDeclarationStatement(
      VariableDeclarationStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processVariableDeclarationList(VariableDeclarationList list) {
    var csharp = "";

    var type = "";
    for (var entity in list.childEntities) {
      if (entity is TypeName)
        type = processEntity(entity);
      else {
        csharp += ' ' + processEntity(entity);
      }
    }

    if (csharp.contains('='))
      return "$type $csharp";
    else
      return "$type $csharp = default($type)";
  }

  static String processTypeName(TypeName name) {
    return Naming.getFormattedTypeName(name.toString());
  }

  static String FieldBody(PropertyAccessorElement element) {
    var body = element.computeNode();
    var bodyLines = Naming.tokenToText(body.beginToken, false).split("\n");
    var rawBody = bodyLines.map((l) => "${l}\n").join();

    // Transpile logic comes here
    var transpiledBody = rawBody + "throw new NotImplementedException();";
    return transpiledBody;
  }

   static String FunctionBody(FunctionElement element) {
    // TODO
    return "throw new NotImplementedException();";  
  }
}
