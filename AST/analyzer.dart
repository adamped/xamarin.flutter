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

      var simplified = new SimpleCompilationUnit()
        ..beginToken = simplify(ast.beginToken, ast.beginToken, initial: true);

      // 3) Serialize and output AST to same directory
      String serialized = god.serializeModel(simplified);

      var fileName = item.path.substring(item.path.lastIndexOf('\\') + 1);
      fileName = fileName.substring(0, fileName.lastIndexOf('.'));
      var filePath = item.path.substring(0, item.path.lastIndexOf('\\'));
      var newFileName = filePath + '\\' + fileName + '.ast.json';
      if (File(newFileName).existsSync()) File(newFileName).deleteSync();
      await new File(newFileName).writeAsString(serialized);
    }
  }
}

int count = 0;

SimpleToken simplify(Token startToken, Token token, {bool initial = false}) {
  var newToken = new SimpleToken()
    ..lexeme = token.lexeme
    ..type = createSimpleType(token.type)
    ..isKeyword = token.isKeyword;

  // Tmp hack - if recursion goes for too long on big files it just throws a stack overflow exception
  if (count > 6000) {
    count = 0;
    newToken.hasFailed = true;
    return newToken;
  }

  count = count + 1;
  if (token.next != null && !token.isEof && (startToken != token || initial))
    newToken.next = simplify(startToken, token.next);

  count = 0;
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
  bool hasFailed;
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
    var stream = directory
        .list(recursive: true, followLinks: false)
        .listen((FileSystemEntity entity) {
      files.add(entity);
    });

    stream.onDone(() => completer.complete(files));
  }

  return completer.future;
}
