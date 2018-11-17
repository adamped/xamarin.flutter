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
      return ""; // No code;
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
    // TODO: Will most likely use processBlockFunction
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
      return "";
    } else if (entity is SimpleToken) {
      return entity.toString();
    } else if (entity is AssertStatement) {
      return ""; // I just ignore assert statements at the moment
    } else if (entity is ReturnStatement) {
      return entity.toString();
    } else if (entity is VariableDeclarationStatement) {
      return entity.toString();
    } else if (entity is SwitchStatement) {
      return entity.toString();
    } else if (entity is IfStatement) {
      return entity.toString();
    } else if (entity is ExpressionStatement) {
      return entity.toString();
    } else if (entity is WhileStatement) {
      return entity.toString();
    } else if (entity is ForEachStatement) {
      return entity.toString();
    } else if (entity is ForStatement) {
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
