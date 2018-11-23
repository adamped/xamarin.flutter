import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/syntactic_entity.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:front_end/src/scanner/token.dart';
import '../naming.dart';
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
    for (var entity in statement.childEntities) csharp += Implementation.processEntity(entity);
    return csharp;
  }
}
