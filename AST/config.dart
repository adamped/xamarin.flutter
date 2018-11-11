import 'dart:io';

class Config {

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
    ..add("dart:typed_data")
    ..add("package:meta/meta.dart")
    ..add("dart:convert")
    ..add("dart:isolate")
    ..add("package:vector_math/vector_math_64.dart")
    ..add("package:typed_data/typed_buffers.dart;");
}
