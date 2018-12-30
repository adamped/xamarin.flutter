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

import 'config.dart';
import 'signature/frame.dart';
import 'naming.dart';
import 'packageResolver.dart';

main() async {
  // 1) Get all directories and all files
  var outputPath = Directory('..\\FlutterSDK\\src');

  print("Directory: " + Config.sourcePath);
  var contents = await dirContents(Directory(Config.sourcePath));

  PhysicalResourceProvider resourceProvider = PhysicalResourceProvider.INSTANCE;
  DartSdk sdk = new FolderBasedDartSdk(
      resourceProvider, resourceProvider.getFolder(Config.DartSdkPath));
  var flutterSdk = Config.sourcePath
      .substring(0, Config.sourcePath.lastIndexOf('\\'));

  var resolvers = [
    new DartUriResolver(sdk),
    Config.isTestbed ? null : new DartUriResolver(embeddedResolver(resourceProvider, flutterSdk)),
    new ResourceUriResolver(resourceProvider),
    Config.isTestbed ? null :packageResolver(
        resourceProvider, 'flutter', resourceProvider.getFolder(flutterSdk)),
  ];

  AnalysisContext context = AnalysisEngine.instance.createAnalysisContext()
    ..sourceFactory = new SourceFactory(resolvers);

  // Clear old output directory
  if (await outputPath.exists()) await outputPath.delete(recursive: true);
  await outputPath.create();

  // Iterate trough all files
  // Each file is transpiled and written as one file with its own namespace
  // the file contains all classes, interfaces, enums and delegates of the source file
  // HACK: just removing Cupertino for initial alpha version
  for (var item in contents.where((x) {
    return !x.toString().contains('cupertino');
  })) {
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
      var namespaceParts =
          Naming.namespacePartsFromIdentifier(element.library.identifier);
      var namespaceDartName =
          Naming.namespaceFromIdentifier(element.library.identifier);
      var code = Frame.printNamespace(element, namespaceDartName);

      var file = new File(
          "${outputPath.absolute.path}\\${namespaceParts.join("\\")}.cs");
      if (!await file.exists()) await file.create(recursive: true);
      await file.writeAsString(code);
      print("Wrote ${file.path} to file.");
    }
  }
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
