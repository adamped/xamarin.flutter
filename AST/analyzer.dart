import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/ast/token.dart';
import 'dart:io';
import 'serialization.dart' as god;
import 'dart:async';

main() async {
  // 1) Get all directories and all files
  var contents = await dirContents(Directory('..\\flutter\\lib\\src'));

  for (var item in contents) {
    FileSystemEntityType type = await FileSystemEntity.type(item.path);
    if (type == FileSystemEntityType.file && item.path.endsWith('dart')) {
      // 2) AST and Encode each file and place out corresponding .ast.json
      var src = await new File(item.path).readAsString();
      print(item.path);

      var ast = parseCompilationUnit(src, parseFunctionBodies: true);
      print("Parsed");
      var simplified = new SimpleCompilationUnit()
        ..beginToken = simplify(ast.beginToken);
      print("Simplified");

      // 3) Serialize and output AST to same directory
      String serialized = god.serializeModel(simplified);

      var fileName = item.path.substring(item.path.lastIndexOf('\\') + 1);
      fileName = fileName.substring(0, fileName.lastIndexOf('.'));
      var filePath = item.path.substring(0, item.path.lastIndexOf('\\'));
      var newFileName = filePath + '\\' + fileName + '.ast.json';
      if (File(newFileName).existsSync()) File(newFileName).deleteSync();
      await new File(newFileName).writeAsString(serialized);
      print("Serialized");
    }
  }
}

SimpleToken simplify(Token token) {
  var newToken = new SimpleToken()
  ..lexeme = token.lexeme
  ..type = createSimpleType(token.type)
  ..isKeyword = token.isKeyword;

  if (!token.isEof) newToken.next = simplify(token.next);

  return newToken;
}

SimpleTokenType createSimpleType(TokenType type) {
  return new SimpleTokenType()
    ..name = type.name
    ..isKeyword = type.isKeyword;
}

class SimpleToken {
  SimpleTokenType type;
  SimpleToken next;
  bool isKeyword;
  String lexeme;
}

class SimpleTokenType {
  String name;
  bool isKeyword;
}

class SimpleCompilationUnit {
  SimpleToken beginToken;
}

Future<List<FileSystemEntity>> dirContents(Directory directory) async {
  Completer<List<FileSystemEntity>> completer =
      new Completer<List<FileSystemEntity>>();
  var files = <FileSystemEntity>[];
  print(directory);

  var exists = await directory.exists();
  if (exists) {
    print("exits");

    var stream = directory
        .list(recursive: true, followLinks: false)
        .listen((FileSystemEntity entity) {
      files.add(entity);
      print(entity.path);
    });

    stream.onDone(() => completer.complete(files));
  }

  return completer.future;
}
