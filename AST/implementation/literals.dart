import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/syntactic_entity.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:front_end/src/scanner/token.dart';
import '../naming.dart';
import 'implementation.dart';

class Literals {
  static String processNullLiteral(NullLiteral literal) {
    var csharp = "";
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processDoubleLiteral(DoubleLiteral literal) {
    var csharp = "";
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processBooleanLiteral(BooleanLiteral literal) {
    var csharp = "";
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processIntegerLiteral(IntegerLiteral literal) {
    var csharp = "";
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processMapLiteral(MapLiteral literal) {
    var csharp = "";
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processMapLiteralEntry(MapLiteralEntry entry) {
    var csharp = "";
    for (var entity in entry.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }

  static String processSimpleStringLiteral(SimpleStringLiteral literal) {
    return '"${literal.stringValue}"';
  }

  static String processListLiteral(ListLiteral literal) {
    var csharp = "";
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }
}
