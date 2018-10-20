import 'package:analyzer/analyzer.dart';
import 'package:analyzer/dart/ast/token.dart';
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
        ..accessors = convertAccessors(element.accessors)
        ..functions = convertFunctions(element.functions);

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

List<ResolvedFunctionElement> convertFunctions(List<FunctionElement> elements) {
  List<ResolvedFunctionElement> list = [];

  for (var item in elements) {
    if (item != null) {
      var accessor = new ResolvedFunctionElement()
        ..isEntryPoint = item.isEntryPoint;

      convertExecutableElement(accessor, item);

      list.add(accessor);
    }
  }

  return list;
}

List<ResolvedPropertyAccessorElement> convertAccessors(
    List<PropertyAccessorElement> elements) {
  List<ResolvedPropertyAccessorElement> list = [];

  for (var item in elements) {
    if (item != null) list.add(convertAccessor(item));
  }

  return list;
}

ResolvedPropertyAccessorElement convertAccessor(
    PropertyAccessorElement accessor) {
  if (accessor == null) return null;

  var item = new ResolvedPropertyAccessorElement()
    ..isGetter = accessor.isGetter
    ..isSetter = accessor.isSetter;
  convertExecutableElement(item, accessor);
  return item;
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
      ..supertype = convertInterfaceType(item.supertype)
      ..mixins = convertInterfaceTypes(item.mixins)
      ..methods = methodList(item.methods)
      ..accessors = convertAccessors(item.accessors)
      ..allSupertypes = convertInterfaceTypes(item.allSupertypes)
      ..superclassConstraints =
          convertInterfaceTypes(item.superclassConstraints)
      ..fields = convertFields(item.fields);

    convertElement(resolved, item);

    list.add(resolved);
  }

  return list;
}

List<ResolvedFieldElement> convertFields(List<FieldElement> elements) {
  List<ResolvedFieldElement> list = [];

  for (var item in elements) {
    var field = new ResolvedFieldElement()
      ..isEnumConstant = item.isEnumConstant;

    convertPropertyInducingElement(field, item);

    list.add(field);
  }

  return list;
}

ResolvedPropertyInducingElement convertPropertyInducingElement(
    ResolvedFieldElement item, PropertyInducingElement element) {
  if (element == null) return null;

  item
    ..getter = convertAccessor(element.getter)
    ..setter = convertAccessor(element.setter);

  convertResolvedVariableElement(item, element);

  return item;
}

ResolvedVariableElement convertResolvedVariableElement(
    ResolvedPropertyInducingElement item, VariableElement element) {
  if (element == null) return item;
  item
    //..constantValue = element.constantValue
    ..hasImplicitType = element.hasImplicitType
    ..isConst = element.isConst
    ..isFinal = element.isFinal
    ..isStatic = element.isStatic
    ..type = convertDartType(new ResolvedDartType(), element.type);

  return item;
}

ResolvedInterfaceType convertInterfaceType(InterfaceType element) {
  if (element == null) return null;

  return new ResolvedInterfaceType()
    ..accessors = convertAccessors(element.accessors)
    ..constructors = convertConstructors(element.constructors)
    ..interfaces = convertInterfaceTypes(element.interfaces)
    ..methods = convertMethodElements(element.methods)
    ..mixins = convertInterfaceTypes(element.mixins)
    ..superclassConstraints =
        convertInterfaceTypes(element.superclassConstraints)
    ..superclass = convertInterfaceType(element.superclass);
}

List<ResolvedMethodElement> convertMethodElements(
    List<MethodElement> elements) {
  List<ResolvedMethodElement> list = [];

  for (var item in elements) {
    var method = new ResolvedMethodElement();
    convertExecutableElement(method, item);
    list.add(method);
  }

  return list;
}

List<ResolvedInterfaceType> convertInterfaceTypes(
    List<InterfaceType> interfaces) {
  List<ResolvedInterfaceType> list = [];
  for (var item in interfaces) {
    list.add(convertInterfaceType(item));
  }

  return list;
}

List<ResolvedConstructorElement> convertConstructors(
    List<ConstructorElement> constructors) {
  List<ResolvedConstructorElement> list = [];
  for (var item in constructors) {
    var constructor = new ResolvedConstructorElement()
      ..isConst = item.isConst
      ..isFactory = item.isFactory
      ..isDefaultConstructor = item.isDefaultConstructor
      ..isStatic = item.isStatic;

    convertExecutableElement(constructor, item);

    list.add(constructor);
  }

  return list;
}

List<ResolvedMethodElement> methodList(List<MethodElement> methods) {
  List<ResolvedMethodElement> list = [];
  for (var item in methods) {
    var element = new ResolvedMethodElement()
      ..hasImplicitReturnType = item.hasImplicitReturnType
      ..isAbstract = item.isAbstract
      ..isAsynchronous = item.isAsynchronous
      ..isExternal = item.isExternal
      ..isGenerator = item.isGenerator
      ..isOperator = item.isOperator
      ..isStatic = item.isStatic
      ..isSynchronous = item.isSynchronous
      ..methodDeclaration = getMethodDeclaration(item);

    convertFunctionTypedElement(element, item);

    list.add(element);
  }

  return list;
}

ResolvedMethodDeclaration getMethodDeclaration(MethodElement item) {
  var method = new ResolvedMethodDeclaration();

  var resolved = item.computeNode();

  method.body = getFunctionBody(resolved);

  return method;
}

ResolvedFunctionBody getFunctionBody(MethodDeclaration node) {
  if (node == null) return null;

  var body = new ResolvedFunctionBody();

  body.beginToken = getToken(node.beginToken);

  return body;
}

ResolvedToken getToken(Token token) {
  var t = new ResolvedToken()
    ..kind = token.kind
    ..lexeme = token.lexeme
    ..isKeyword = token.isKeyword
    ..isKeywordOrIdentifier = token.isKeywordOrIdentifier
    ..isOperator = token.isOperator;

  if (token.next != null && !token.isEof) t.next = getToken(token.next);

  return t;
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

  convertFunctionTypedElement(item, element);
}

convertFunctionTypedElement(
    ResolvedFunctionTypeElement item, FunctionTypedElement element) {
  item.parameters = parameterList(element.parameters);
  item.returnType = convertDartType(new ResolvedDartType(), element.returnType);
  item.type = convertFunctionType(new ResolvedFunctionType(), element.type);
}

ResolvedDartType convertDartType(ResolvedDartType item, DartType type) {
  item.displayName = type.displayName;
  item.isBottom = type.isBottom;
  item.isDartAsyncFuture = type.isDartAsyncFuture;
  item.isDartAsyncFutureOr = type.isDartAsyncFutureOr;
  item.isDartCoreFunction = type.isDartCoreFunction;
  item.isDartCoreNull = type.isDartCoreNull;
  item.isDynamic = type.isDynamic;
  item.isObject = type.isObject;
  item.isUndefined = type.isUndefined;
  item.isVoid = type.isVoid;

  return item;
}

ResolvedFunctionType convertFunctionType(
    ResolvedFunctionType item, FunctionType type) {
  convertDartType(item, type);
  return item;
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
