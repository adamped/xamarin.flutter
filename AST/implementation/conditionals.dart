import 'package:analyzer/dart/ast/ast.dart';
import 'implementation.dart';

class Conditionals {
  static String processConditionalExpression(ConditionalExpression expression) {
    var csharp = "";
    for (var entity in expression.childEntities) {
      csharp += Implementation.processEntity(entity);
    }

    return csharp;
  }

  static String processSwitchStatement(SwitchStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += Implementation.processEntity(entity);
    }

    return csharp;
  }

  static String processSwitchCase(SwitchCase switchCase) {
    var csharp = "";

    for (var entity in switchCase.childEntities) {
      csharp += Implementation.processEntity(entity);
    }

    return csharp;
  }

  static String processSwitchDefault(SwitchDefault switchDefault) {
    var csharp = "";

    for (var entity in switchDefault.childEntities) {
      csharp += Implementation.processEntity(entity);
    }

    return csharp;
  }

  static String processContinueStatement(ContinueStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      csharp += Implementation.processEntity(entity);
    }

    return csharp;
  }

  static String processIfStatement(IfStatement statement) {
    var csharp = "";

    for (var entity in statement.childEntities) {
      if (entity is Block) Implementation.startCastMapping = true;

      csharp += Implementation.processEntity(entity);

      // Clear cast mapping after BlockImpl - TODO: Could backfire if embedded if statements
      if (entity is Block) {
        Implementation.castMapping.clear();
        Implementation.startCastMapping = false;
      }
    }
    return csharp;
  }
}
