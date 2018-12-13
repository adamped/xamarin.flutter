import 'package:analyzer/dart/ast/ast.dart';
import 'implementation.dart';

class Loops {
  static String processForStatement(ForStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processWhileStatement(WhileStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processDoStatement(DoStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processForEachStatement(ForEachStatement statement) {
    var csharp = "";
    for (var entity in statement.childEntities) {
      if (entity.toString() == 'for')
        csharp += 'foreach';
      else
        csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }
}
