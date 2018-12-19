# Transpiler

This document details how to run the transpiler. It is currently a work in progress.

## Dart Analyzer

In the AST folder is `analyzer.dart`, which uses the Dart Analyzer to create a Semantic Model of the Flutter SDK.

### Running the Dart Analyzer

1. The Flutter SDK isn't included in this project. Install it and make sure that its `bin` directory is in PATH.
2.  Run `download-flutter.ps1` script in flutter sub directory (make sure that PATH from previous step is applied, or if it wasn't, run `flutter packages get` manually).

3. Install [Dart SDK](https://www.dartlang.org/tools/sdk#install) on your computer.

4. Make sure there is a DART_SDK environment  variable pointing to `dart_sdk` directory. Optionally add its bin directory to PATH (if not already).

5. In a terminal window or command prompt, run `dart analyzer`.

This will run through the Flutter SDK and create an equivalent *.cs files.

## Semantic Model

To generate the *.cs files, the Dart Analyzer creates a Semantic Model that is used by the writer.

## CSharp Writer

The writer, will take the Semantic Model and output the transpiled C# files into the FlutterSDK project.