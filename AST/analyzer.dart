import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/ast/token.dart';
import 'package:analyzer/file_system/physical_file_system.dart';
import 'dart:io';
import 'serialization.dart' as god;
import 'dart:async';

import 'dart:io';

import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/ast/visitor.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/file_system/physical_file_system.dart';
import 'package:analyzer/src/context/builder.dart';
import 'package:analyzer/src/dart/sdk/sdk.dart';
import 'package:analyzer/src/file_system/file_system.dart';
import 'package:analyzer/src/generated/engine.dart';
import 'package:analyzer/src/generated/sdk.dart' show DartSdk;
import 'package:analyzer/src/generated/source.dart';
import 'package:analyzer/src/generated/source_io.dart';
import 'package:analyzer/src/source/package_map_resolver.dart';
import 'package:analyzer/src/source/source_resource.dart';

main() async {
  // 1) Get all directories and all files
  var directoryPath = Directory('..\\flutter\\lib\\src').absolute.path;
  directoryPath = directoryPath.replaceAll('\\AST\\..', '');

  var contents = await dirContents(Directory(directoryPath));

  PhysicalResourceProvider resourceProvider = PhysicalResourceProvider.INSTANCE;
  DartSdk sdk = new FolderBasedDartSdk(
      resourceProvider, resourceProvider.getFolder('D:\\Dart\\dart-sdk'));

  var resolvers = [
    new DartUriResolver(sdk),
    new ResourceUriResolver(resourceProvider)
  ];

  AnalysisContext context = AnalysisEngine.instance.createAnalysisContext()
    ..sourceFactory = new SourceFactory(resolvers);

  // for (var item in contents) {
  // FileSystemEntityType type = await FileSystemEntity.type(item.path);
  // if (type == FileSystemEntityType.file && item.path.endsWith('dart')) {
  Source source = new FileSource(resourceProvider.getFile(
      'C:\\Users\\Adam\\source\\repos\\xamarin.flutter\\flutter\\lib\\src\\animation\\animation.dart'));
  ChangeSet changeSet = new ChangeSet()..addedSource(source);
  context.applyChanges(changeSet);
  LibraryElement libElement = context.computeLibraryElement(source);
  CompilationUnit resolvedUnit =
      context.resolveCompilationUnit(source, libElement);

  // 2) AST and Encode each file and place out corresponding .ast.json
  // print(item.path);
  // var ast = parseDartFile(item.path, parseFunctionBodies: true);

  var test = resolvedUnit.declaredElement;

  // var simplified = new SimpleCompilationUnit()
  //   ..beginToken = simplify(ast.beginToken);

  // // 3) Serialize and output AST to same directory
   String serialized = god.serializeModel(test);

  // var fileName = item.path.substring(item.path.lastIndexOf('\\') + 1);
  // fileName = fileName.substring(0, fileName.lastIndexOf('.'));
  // var filePath = item.path.substring(0, item.path.lastIndexOf('\\'));
  // var newFileName = filePath + '\\' + fileName + '.ast.json';
  // if (File(newFileName).existsSync()) File(newFileName).deleteSync();
  // new File(newFileName).writeAsStringSync(serialized);
  // }
  //}
}

int count = 0;

List<SimpleToken> simplify(Token token, {bool initial = false}) {
  var currentToken = token;
  var list = new List<SimpleToken>();

  while (currentToken != null) {
    var newToken = new SimpleToken()
      ..lexeme = currentToken.lexeme
      ..type = createSimpleType(currentToken.type)
      ..isKeyword = currentToken.isKeyword;

    // Tmp hack - if recursion goes for too long on big files it just throws a stack overflow exception
    if (count > 6000) {
      count = 0;
      newToken.hasFailed = true;
      list.add(newToken);
      currentToken = null; // end
    } else
      newToken.hasFailed = false;

    count = count + 1;

    list.add(newToken);

    if (currentToken != null &&
        currentToken.next != null &&
        !currentToken.isEof)
      currentToken = currentToken.next;
    else
      currentToken = null;
  }

  count = 0;
  return list;
}

SimpleTokenType createSimpleType(TokenType type) {
  return new SimpleTokenType()
    ..name = type.name
    ..isKeyword = type.isKeyword;
}

class SimpleToken {
  SimpleTokenType type;
  bool isKeyword;
  String lexeme;
  bool hasFailed;
}

class SimpleTokenType {
  String name;
  bool isKeyword;
}

class SimpleCompilationUnit {
  List<SimpleToken> beginToken;
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
