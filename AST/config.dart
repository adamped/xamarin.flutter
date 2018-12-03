import 'dart:io';

class Config {

  static bool includeImplementations = false;

  // Path to the flutter src directory
  static String flutterSourcePath = Directory('..\\flutter\\lib\\src')
      .absolute
      .path
      .replaceAll('\\AST\\..', '');

  // Absolute path to the dart-sdk directory
  static String DartSdkPath = "C:\\Program Files\\Dart\\dart-sdk";

  // Root namespace the transpiled namespaces will start with
  static String rootNamespace = "FlutterSDK";

  // Imports that are replaced with .net system or mapping libraries
  static List<String> ignoredImports = new List<String>()
    ..add("package:typed_data/typed_buffers.dart")
    ..add("package:collection/collection.dart")
    ..add("dart:ui")
    ..add("dart:async")
    ..add("dart:math")
    ..add("dart:collection")
    ..add("dart:developer")
    ..add("dart:io")
    ..add("dart:core")
    ..add("dart:typed_data")
    ..add("dart:_http")
    ..add("package:meta/meta.dart")
    ..add("dart:convert")
    ..add("dart:isolate")
    ..add("package:vector_math/vector_math_64.dart")
    ..add("package:typed_data/typed_buffers.dart;");

  // Imports that will get added to every class
  static List<String> defaultImports = new List<String>()
    ..add("System")
    ..add("FlutterSDK")
    ..add("FlutterSDK.Widgets.Framework")
    ..add("System.Net.Http")
    ..add("FlutterBinding.UI")
    ..add("System.Collections.Generic")
    ..add("System.Linq")
    ..add("FlutterBinding.Engine.Painting")
    ..add("static FlutterSDK.Global")
    ..add("FlutterBinding.Mapping");
}
