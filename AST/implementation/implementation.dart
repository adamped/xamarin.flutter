import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/syntactic_entity.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:front_end/src/scanner/token.dart';
import '../naming.dart';
import 'loops.dart';
import 'exceptions.dart';
import 'literals.dart';
import 'conditionals.dart';

/// Provides methods to transpile the body of elements
class Implementation {
  static String MethodBody(MethodElement element) {
    var body = element.computeNode().body;

    if (body is EmptyFunctionBody) {
      var parent = body.parent;
      if (parent is MethodDeclaration &&
          parent.returnType != null &&
          parent.returnType.toString() != 'void')
        return '{ \nreturn default(${processEntity(parent.returnType)}); \n}';
      else
        return '{ \n}'; // No code;
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
      return processThisExpression(entity);
    } else if (entity is NullLiteral) {
      return Literals.processNullLiteral(entity);
    } else if (entity is DoubleLiteral) {
      return Literals.processDoubleLiteral(entity);
    } else if (entity is BooleanLiteral) {
      return Literals.processBooleanLiteral(entity);
    } else if (entity is IntegerLiteral) {
      return Literals.processIntegerLiteral(entity);
    } else if (entity is SimpleStringLiteral) {
      return Literals.processSimpleStringLiteral(entity);
    } else if (entity is ArgumentList) {
      return processArgumentList(entity);
    } else if (entity is MapLiteral) {
      return Literals.processMapLiteral(entity);
    } else if (entity is PrefixedIdentifier) {
      return processPrefixedIdentifier(entity);
    } else if (entity is DeclaredIdentifier) {
      return processDeclaredIdentifier(entity);
    } else if (entity is PrefixExpression) {
      return processPrefixExpression(entity);
    } else if (entity is AdjacentStrings) {
      return processAdjacentString(entity);
    } else if (entity is Label) {
      return processLabel(entity);
    } else if (entity is MethodInvocation) {
      return processMethodInvocation(entity);
    } else if (entity is FunctionExpression) {
      return processFunctionExpression(entity);
    } else if (entity is ParenthesizedExpression) {
      return processParenthesizedExpression(entity);
    } else if (entity is IndexExpression) {
      return processIndexExpression(entity);
    } else if (entity is BinaryExpression) {
      return processBinaryExpression(entity);
    } else if (entity is AwaitExpression) {
      return processAwaitExpression(entity);
    } else if (entity is ConditionalExpression) {
      return Conditionals.processConditionalExpression(entity);
    } else if (entity is StringInterpolation) {
      return processStringInterpolation(entity);
    } else if (entity is InterpolationExpression) {
      return processInterpolationExpression(entity);
    } else if (entity is InstanceCreationExpression) {
      return processInstanceCreationExpression(entity);
    } else if (entity is ConstructorName) {
      return processConstructorName(entity);
    } else if (entity is PropertyAccess) {
      return processPropertyAccess(entity);
    } else if (entity is AssignmentExpression) {
      return processAssignmentExpression(entity);
    } else if (entity is AssertStatement) {
      return ""; // I just ignore assert statements at the moment
    } else if (entity is ReturnStatement) {
      return processReturnStatement(entity);
    } else if (entity is VariableDeclaration) {
      return processVariableDeclaration(entity);
    } else if (entity is VariableDeclarationStatement) {
      return processVariableDeclarationStatement(entity);
    } else if (entity is VariableDeclarationList) {
      return processVariableDeclarationList(entity);
    } else if (entity is SwitchStatement) {
      return Conditionals.processSwitchStatement(entity);
    } else if (entity is SwitchCase) {
      return Conditionals.processSwitchCase(entity);
    } else if (entity is BreakStatement) {
      return entity.toString();
    } else if (entity is SwitchDefault) {
      return Conditionals.processSwitchDefault(entity);
    } else if (entity is ContinueStatement) {
      return Conditionals.processContinueStatement(entity);
    } else if (entity is IfStatement) {
      return Conditionals.processIfStatement(entity);
    } else if (entity is IsExpression) {
      return processIsExpression(entity);
    } else if (entity is CascadeExpression) {
      return processCascadeExpression(entity);
    } else if (entity is ExpressionStatement) {
      return processExpressionStatement(entity);
    } else if (entity is SuperExpression) {
      return processSuperExpression(entity);
    } else if (entity is ThrowExpression) {
      return Exceptions.processThrowExpression(entity);
    } else if (entity is WhileStatement) {
      return Loops.processWhileStatement(entity);
    } else if (entity is ForEachStatement) {
      return Loops.processForEachStatement(entity);
    } else if (entity is ForStatement) {
      return Loops.processForStatement(entity);
    } else if (entity is ListLiteral) {
      return Literals.processListLiteral(entity);
    } else if (entity is FormalParameterList) {
      return processFormalParameterList(entity);
    } else if (entity is TypeArgumentList) {
      return processTypeArgumentList(entity);
    } else if (entity is SimpleFormalParameter) {
      return processSimpleFormalParameter(entity);
    } else if (entity is FunctionDeclarationStatement) {
      return processFunctionDeclarationStatement(entity);
    } else if (entity is TryStatement) {
      return Exceptions.processTryStatement(entity);
    } else if (entity is CatchClause) {
      return Exceptions.processCatchClause(entity);
    } else if (entity is DoStatement) {
      return Loops.processDoStatement(entity);
    } else if (entity is YieldStatement) {
      return processYieldStatement(entity);
    } else if (entity is PostfixExpression) {
      return processPostfixExpression(entity);
    } else if (entity is AsExpression) {
      return processAsExpression(entity);
    } else if (entity is NamedExpression) {
      return processNamedExpression(entity);
    } else if (entity is TypeName) {
      return processTypeName(entity);
    } else if (entity is Block) {
      return processBlock(entity);
    } else if (entity is BlockFunctionBody) {
      return processBlockFunctionBody(entity);
    } else if (entity is ExpressionFunctionBody) {
      return processExpressionFunctionBody(entity);
    } else if (entity is FunctionExpressionInvocation) {
      return processFunctionExpressionInvocation(entity);
    } else if (entity is FunctionDeclaration) {
      return processFunctionDeclaration(entity);
    } else if (entity is MapLiteralEntry) {
      return Literals.processMapLiteralEntry(entity);
    } else {
      // This should never be hit. If it is, it means you aren't correctly processing something.
      return entity.toString();
    }
  }

  static String processPropertyAccess(PropertyAccess access) {
    var csharp = "";
    for (var entity in access.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processFunctionDeclaration(FunctionDeclaration declaration) {
    var csharp = "";
    for (var entity in declaration.childEntities) {
      csharp += processEntity(entity) + ' ';
    }
    return csharp;
  }

  static String processFunctionExpressionInvocation(
      FunctionExpressionInvocation invocation) {
    var csharp = "";
    for (var entity in invocation.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processExpressionFunctionBody(ExpressionFunctionBody body) {
    var csharp = "";
    for (var entity in body.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processTypeArgumentList(TypeArgumentList list) {
    if (list.childEntities.length < 2)
      throw new AssertionError(
          'There should always be brackets around the arguments.');

    var csharp = list.childEntities
        .where((f) => f is TypeName)
        .map((f) => processEntity(f))
        .join(',');

    var entities = list.childEntities.toList();

    var first = entities[0];
    var end = entities.last;
    return '$first$csharp$end';
  }

  static String processBlockFunctionBody(BlockFunctionBody body) {
    var csharp = "";
    for (var entity in body.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processSimpleFormalParameter(SimpleFormalParameter parameter) {
    return parameter.childEntities.map((f) => processEntity(f)).join(' ');
  }

  static String processFormalParameterList(FormalParameterList list) {
    var csharp = "";
    var hasParameter = false;
    for (var entity in list.childEntities) {
      if (entity.toString() == ')' && hasParameter == true)
        csharp = csharp.substring(0, csharp.length - 2);
      csharp += processEntity(entity);
      if (entity is SimpleFormalParameter) {
        csharp += ', ';
        hasParameter = true;
      }
    }
    return csharp;
  }

  static String processAdjacentString(AdjacentStrings string) {
    var csharp = "";
    for (var entity in string.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processDeclaredIdentifier(DeclaredIdentifier identifier) {
    var csharp = "";
    for (var entity in identifier.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processLabel(Label label) {
    var csharp = "";
    for (var entity in label.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processPrefixExpression(PrefixExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processFunctionExpression(FunctionExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
      // TODO: need to find a way when it's only a function expression not an actual method
      if (entity is FormalParameterList) csharp += ' => ';
    }
    return csharp;
  }

  static String processIndexExpression(IndexExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processIsExpression(IsExpression expression) {
    var csharp = '';
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity) + ' ';
    }
    return csharp;
  }

  static String processCascadeExpression(CascadeExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      // If SimpleIdentifier, then it isn't creating anything or a new assignment
      // its just a variable, and we can ignore it, because the cascades will
      // put it in front anyway.
      if (!(entity is SimpleIdentifier)) csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processFunctionDeclarationStatement(
      FunctionDeclarationStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processYieldStatement(YieldStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processPostfixExpression(PostfixExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processAsExpression(AsExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processNamedExpression(NamedExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processBlock(Block block) {
    var csharp = "";
    for (var item in block.childEntities) {
      csharp += processEntity(item) + "\n";
    }
    return csharp;
  }

  static String processToken(KeywordToken keyword) {
    var newKeyword = keyword.keyword.lexeme;

    if (newKeyword == "super") return "base";
    if (newKeyword == "final" || newKeyword == 'const')
      return ""; // Rarely an equivalence of final or const inside a method, because flutter has compiled consts, whereas C# does not.

    if (newKeyword == 'is') newKeyword = ' ' + newKeyword;
    if (newKeyword == 'as') newKeyword = ' ' + newKeyword;
    if (newKeyword == 'in') newKeyword = ' ' + newKeyword;

    return newKeyword + " ";
  }

  static String processVariableDeclaration(VariableDeclaration declaration) {
    var csharp = "";
    for (var entity in declaration.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processSuperExpression(SuperExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processAwaitExpression(AwaitExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processThisExpression(ThisExpression expression) {
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

  static String processInstanceCreationExpression(
      InstanceCreationExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += processEntity(entity);
    }
    // TODO: No such thing as named constructors in C#
    // Will need to look at Static Method calls without the new.
    if (!csharp.startsWith('new ')) csharp = 'new ' + csharp;
    return csharp;
  }

  static String processConstructorName(ConstructorName name) {
    var csharp = "";
    for (var entity in name.childEntities) {
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

  static String processReturnStatement(ReturnStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }
    return csharp;
  }

  static String processMethodInvocation(MethodInvocation invocation) {
    var csharp = "";

    if (invocation.isCascaded) {
      csharp +=
          processEntity(invocation.parent.childEntities.toList()[0]) + '.';
      for (var entity in invocation.childEntities) {
        if (entity.toString() != '..') csharp += processEntity(entity);
      }
    } else {
      for (var entity in invocation.childEntities) {
        csharp += processEntity(entity);
      }
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
      csharp += Naming.upperCamelCase(identifier.name);
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
    var name = element.displayName;

    if (name == "toUpperCase") return "ToUpper";
    if (name == "toLowerCase") return "ToLower";
    if (name == "trimRight") return "TrimEnd";
    if (name == "trimLeft") return "TrimStart";

    return Naming.upperCamelCase(name);
  }

  static String processFunctionElement(FunctionElement element) {
    return Naming.upperCamelCase(element.displayName);
  }

  static String processPropertyAccessorElement(
      PropertyAccessorElement element) {
    var name = element.displayName;

    if (name == "inMicroseconds") return "InMicroseconds()";
    if (name == "isFinite") return "IsFinite()";
    if (name == 'runtimeType') return 'GetType()';

    return Naming.upperCamelCase(name);
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

  static String processExpressionStatement(ExpressionStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += processEntity(entity);
    }

    return csharp;
  }

  static String processAssignmentExpression(AssignmentExpression expression) {
    var csharp = "";
    var list = expression.childEntities.toList();
    if (list.length != 3)
      throw new AssertionError('Should always have 3 parts');

    var nullCheckAssignment = false;
    var token = list[1] as SimpleToken;
    if (token != null && token.lexeme == '??=') nullCheckAssignment = true;

    if (nullCheckAssignment) {
      var variable = processEntity(list[0]);
      csharp += variable + ' = (' + variable + ' == null ';
      csharp += '? ' + processEntity(list[2]) + ' ';
      csharp += ': ' + variable + ' )';
    } else {
      for (var entity in expression.childEntities) {
        csharp += processEntity(entity);
      }
    }

    return csharp;
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
