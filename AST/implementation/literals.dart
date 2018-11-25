import 'package:analyzer/dart/ast/ast.dart';
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
    var stringValue = literal.toString();
    stringValue = stringValue.substring(1, stringValue.length - 1);

    if (stringValue.length == 1 || (stringValue.startsWith('\\') && stringValue.length == 2))
      return "'$stringValue'";
    else
      return '"$stringValue"';
  }

  static String processListLiteral(ListLiteral literal) {
    var csharp = '';
    for (var entity in literal.childEntities) {
      csharp += Implementation.processEntity(entity);
    }
    return csharp;
  }
}
