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
    // TODO: Will most likely use processBlockFunction but add a => to it
    return "\n";
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
      return entity.lexeme +
          " "; //TODO: might need to do some keyword switching.
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
      return entity.toString();
    } else if (entity is ArgumentList) {
      return entity.toString();
    } else if (entity is MapLiteral) {
      return entity.toString();
    } else if (entity is PrefixedIdentifier) {
      return entity.toString();
    } else if (entity is PrefixExpression) {
      return entity.toString();
    } else if (entity is AdjacentStrings) {
      return entity.toString();
    } else if (entity is MethodInvocation) {
      return processMethodInvocation(entity);
    } else if (entity is FunctionExpression) {
      return entity.toString();
    } else if (entity is ParenthesizedExpression) {
      return entity.toString();
    } else if (entity is IndexExpression) {
      return entity.toString();
    } else if (entity is BinaryExpression) {
      return entity.toString();
    } else if (entity is AwaitExpression) {
      return entity.toString();
    } else if (entity is ConditionalExpression) {
      return entity.toString();
    } else if (entity is StringInterpolation) {
      return entity.toString();
    } else if (entity is InstanceCreationExpression) {
      return entity.toString();
    } else if (entity is PropertyAccess) {
      return entity.toString();
    } else if (entity is AssignmentExpression) {
      return entity.toString();
    } else if (entity is AssertStatement) {
      return ""; // I just ignore assert statements at the moment
    } else if (entity is ReturnStatement) {
      return processReturnStatement(entity);
    } else if (entity is VariableDeclarationStatement) {
      return entity.toString();
    } else if (entity is SwitchStatement) {
      return entity.toString();
    } else if (entity is IfStatement) {
      return entity.toString();
    } else if (entity is IsExpression) {
      return entity.toString();
    } else if (entity is CascadeExpression) {
      return entity.toString();
    } else if (entity is ExpressionStatement) {
      return entity.toString();
    } else if (entity is SuperExpression) {
      return entity.toString();
    } else if (entity is WhileStatement) {
      return entity.toString();
    } else if (entity is ForEachStatement) {
      return entity.toString();
    } else if (entity is ForStatement) {
      return entity.toString();
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
      csharp += Naming.upperCamelCase(identifier.name);
    } else if (identifier.staticElement is LocalVariableElement) {
      csharp += identifier.name + " ";
    } else {
      csharp += identifier.name + " ";
    }
    return csharp;
  }

  static String FieldBody(PropertyAccessorElement element) {
    var body = element.computeNode();
    var bodyLines = Naming.tokenToText(body.beginToken, false).split("\n");
    var rawBody = bodyLines.map((l) => "// ${l}\n").join();

    // Transpile logic comes here
    var transpiledBody = rawBody + "throw new NotImplementedException();";
    return transpiledBody;
  }

   static String FunctionBody(FunctionElement element) {
    // TODO
    return "throw new NotImplementedException();";  
  }
}
