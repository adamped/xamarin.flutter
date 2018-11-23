import 'package:analyzer/dart/ast/ast.dart';
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
