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
    // TODO: Detect if IsExpression, then set _castMapping only during BlockImpl
    for (var entity in statement.childEntities) 
    csharp += Implementation.processEntity(entity);
    return csharp;
  }
}
