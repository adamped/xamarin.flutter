import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/element/type.dart';
import 'package:analyzer/file_system/physical_file_system.dart';
import 'dart:io';
import 'serialization.dart' as serialize;
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
import 'model.dart';

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

      var unit = new ResolvedCompilationUnit()
        ..enums = convertClassElements(element.enums)
        ..types = convertClassElements(element.types)
        ..accessors = convertAccessors(element.accessors);

      convertElement(unit, element);

      String serialized = serialize.serializeModel(unit);

      var fileName = item.path.substring(item.path.lastIndexOf('\\') + 1);
      fileName = fileName.substring(0, fileName.lastIndexOf('.'));
      var filePath = item.path.substring(0, item.path.lastIndexOf('\\'));
      var newFileName = filePath + '\\' + fileName + '.sm.json';

      if (File(newFileName).existsSync()) File(newFileName).deleteSync();
      new File(newFileName).writeAsStringSync(serialized);
    }
  }
}

List<ResolvedPropertyAccessorElement> convertAccessors(
    List<PropertyAccessorElement> elements) {
  List<ResolvedPropertyAccessorElement> list = [];

  for (var item in elements) {
    var accessor = new ResolvedPropertyAccessorElement()
      ..isGetter = item.isGetter
      ..isSetter = item.isSetter;
    convertExecutableElement(accessor, item);

    list.add(accessor);
  }

  return list;
}

List<ResolvedClassElement> convertClassElements(List<ClassElement> elements) {
  List<ResolvedClassElement> list = [];

  for (var item in elements) {
    var resolved = ResolvedClassElement()
      ..isAbstract = item.isAbstract
      ..isEnum = item.isEnum
      ..isMixin = item.isMixin
      ..isMixinApplication = item.isMixinApplication
      ..isOrInheritsProxy = item.isOrInheritsProxy
      ..isProxy = item.isProxy
      ..isValidMixin = item.isValidMixin
      ..hasNonFinalField = item.hasNonFinalField
      ..hasReferenceToSuper = item.hasReferenceToSuper
      ..hasStaticMember = item.hasStaticMember
      ..mixins = mixinList(item.mixins)
      ..methods = methodList(item.methods);

    convertElement(resolved, item);

    list.add(resolved);
  }

  return list;
}

List<ResolvedDartType> mixinList(List<InterfaceType> mixins) {
  List<ResolvedDartType> list = [];
  for (var item in mixins) {
    list.add(new ResolvedDartType()..displayName = item.displayName);
  }

  return list;
}

List<ResolvedMethodElement> methodList(List<MethodElement> methods) {
  List<ResolvedMethodElement> list = [];
  for (var item in methods) {
    list.add(new ResolvedMethodElement()
      ..hasImplicitReturnType = item.hasImplicitReturnType
      ..isAbstract = item.isAbstract
      ..isAsynchronous = item.isAsynchronous
      ..isExternal = item.isExternal
      ..isGenerator = item.isGenerator
      ..isOperator = item.isOperator
      ..isStatic = item.isStatic
      ..isSynchronous = item.isSynchronous
      ..parameters = parameterList(item.parameters)
      ..returnType =
          (new ResolvedDartType()..displayName = item.returnType.displayName)
      ..type =
          (new ResolvedFunctionType()..displayName = item.type.displayName));
  }

  return list;
}

List<ResolvedParameterElement> parameterList(
    List<ParameterElement> parameters) {
  List<ResolvedParameterElement> list = [];
  for (var item in parameters) {
    list.add(new ResolvedParameterElement()..displayName = item.displayName);
  }

  return list;
}

convertExecutableElement(
    ResolvedExecutableElement item, ExecutableElement element) {
  item.hasImplicitReturnType = element.hasImplicitReturnType;
  item.isAbstract = element.isAbstract;
  item.isAsynchronous = element.isAsynchronous;
  item.isExternal = element.isExternal;
  item.isGenerator = element.isGenerator;
  item.isOperator = element.isOperator;
  item.isStatic = element.isStatic;
  item.isSynchronous = element.isSynchronous;
}

convertElement(ResolvedElement item, Element element) {
  item.displayName = element.displayName;
  item.documentationComment = element.documentationComment;
  item.hasAlwaysThrows = element.hasAlwaysThrows;
  item.hasDeprecated = element.hasDeprecated;
  item.hasFactory = element.hasFactory;
  item.hasIsTest = element.hasIsTest;
  item.hasIsTestGroup = element.hasIsTestGroup;
  item.hasJS = element.hasJS;
  item.hasOverride = element.hasOverride;
  item.hasProtected = element.hasProtected;
  item.hasRequired = element.hasRequired;
  item.hasSealed = element.hasSealed;
  item.hasVisibleForTemplate = element.hasVisibleForTemplate;
  item.hasVisibleForTesting = element.hasVisibleForTesting;
  item.id = element.id;
  item.name = element.name;
  item.isPrivate = element.isPrivate;
  item.isPublic = element.isPublic;
  item.isSynthetic = element.isSynthetic;
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
