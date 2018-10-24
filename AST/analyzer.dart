import 'package:analyzer/analyzer.dart';
import 'package:analyzer/file_system/physical_file_system.dart';
import 'dart:io';
import 'dart:async';
import 'package:analyzer/dart/ast/ast.dart';
import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/src/dart/sdk/sdk.dart';
import 'package:analyzer/src/file_system/file_system.dart';
import 'package:analyzer/src/generated/engine.dart';
import 'package:analyzer/src/generated/sdk.dart' show DartSdk;
import 'package:analyzer/src/generated/source.dart';
import 'package:analyzer/src/generated/source_io.dart';
import 'package:analyzer/src/source/source_resource.dart';

main() async {
  // 1) Get all directories and all files
  var directoryPath = Directory('..\\flutter\\lib\\src').absolute.path;
  var outputPath = Directory('..\\FlutterSDK\\src');
  var rootNamespace = "FlutterSDK";
  directoryPath = directoryPath.replaceAll('\\AST\\..', '');

  var contents = await dirContents(Directory(directoryPath));

  PhysicalResourceProvider resourceProvider = PhysicalResourceProvider.INSTANCE;
  DartSdk sdk = new FolderBasedDartSdk(resourceProvider,
      resourceProvider.getFolder('C:\\Program Files\\Dart\\dart-sdk'));

  var resolvers = [
    new DartUriResolver(sdk),
    new ResourceUriResolver(resourceProvider)
  ];

  AnalysisContext context = AnalysisEngine.instance.createAnalysisContext()
    ..sourceFactory = new SourceFactory(resolvers);

  // Clear old output directory
  if (await outputPath.exists()) await outputPath.delete(recursive: true);
  await outputPath.create();

  for (var item in contents) {
    FileSystemEntityType type = await FileSystemEntity.type(item.path);
    if (type == FileSystemEntityType.file && item.path.endsWith('dart')) {
      print(item.path);
      Source source = new FileSource(resourceProvider.getFile(item.path));
      ChangeSet changeSet = new ChangeSet()..addedSource(source);
      context.applyChanges(changeSet);
      LibraryElement libElement = context.computeLibraryElement(source);
      CompilationUnit resolvedUnit =
          context.resolveCompilationUnit(source, libElement);

      var element = resolvedUnit.declaredElement;

      var namespacePath = item.path
          .replaceAll(directoryPath + "\\", "")
          .replaceAll(".dart", "");
      var namespaceParts = namespacePath.split('\\');
      var namespaceDartName = rootNamespace + "." + namespaceParts.join(".");
      var code = printNamespace(element, namespaceDartName);

      var file = new File(
          "${outputPath.absolute.path}\\${namespaceParts.join("\\")}.cs");
      if (!await file.exists()) await file.create(recursive: true);
      await file.writeAsString(code);
    }
  }
}

void appendComment(StringBuffer buffer, Element element) {
  var dartComment = element.documentationComment;
  if (dartComment == null || dartComment == "") return;
 
  buffer.writeln("/// <Summary>");
  buffer.writeln(element.documentationComment);
  buffer.writeln("/// </Summary>");
}

String printNamespace(CompilationUnitElement element, String namespace) {
  var code = new StringBuffer();
  code.writeln("namespace ${namespace}{");
  // Content
  for (var type in element.types) {
    code.writeln(printClass(type));
  }

  code.writeln("}");
  return code.toString();
}

String printClass(ClassElement element) { 
  var name = element.name;

  var code = new StringBuffer();
  code.writeln("");
  appendComment(code, element);

  if (element.isAbstract) code.write("abstract ");

  code.writeln("class ${name}{");
  code.writeln("");
  for (var method in element.methods) {
    code.writeln(printMethod(method));
  }
  code.writeln("}");
  return code.toString();
}

String printMethod(MethodElement element) { 
  var name = element.name;
  var returnType = element.returnType.displayName;
  var body = element.computeNode().body.childEntities.first.toString();

  var code = new StringBuffer();
  code.writeln("");
  appendComment(code, element);

  if (element.isAbstract) code.write("abstract ");

  code.write("${returnType} ${name}()");

  if (element.isAbstract) {
    code.writeln(";");
  } else {
    code.writeln("{");
    code.writeln(body);
    code.writeln("");
    code.writeln("\tthrow new NotImplementedException();");
    code.writeln("}");
  }

  return code.toString();
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
