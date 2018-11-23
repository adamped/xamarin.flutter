import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/syntactic_entity.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:front_end/src/scanner/token.dart';
import '../naming.dart';
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
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }
}
