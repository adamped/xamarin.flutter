import 'dart:io';

class Config {
  static String directoryPath = Directory('..\\flutter\\lib\\src')
      .absolute
      .path
      .replaceAll('\\AST\\..', '');
  static String rootNamespace = "FlutterSDK";
}
