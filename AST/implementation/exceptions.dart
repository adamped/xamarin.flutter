import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/syntactic_entity.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:front_end/src/scanner/token.dart';
import '../naming.dart';
import 'implementation.dart';

class Exceptions {
  static String processTryStatement(TryStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processThrowExpression(ThrowExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processCatchClause(CatchClause clause) {
    var csharp = "";
    for (var entity in clause.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }
}
