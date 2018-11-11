# Transpiler

This document details how to run the transpiler. It is currently a work in progress.

## Dart Analyzer

In the AST folder is `analyzer.dart`, which uses the Dart Analyzer to create a Semantic Model of the Flutter SDK.

### Running the Dart Analyzer

1) The Flutter SDK isn't included in this project. Download the latest directly from [Flutter GitHub Repo](https://github.com/flutter/flutter/tree/master/packages/flutter/lib)
and place this file directly in `flutter/lib` in this solution.

2) Install [Dart SDK](https://www.dartlang.org/tools/sdk#install) on your computer.

3) Open `analyzer.dart` and change `D:\\Dart\\dart-sdk` to the location of your Dart SDK.

4) In a terminal window or command prompt, run `dart analyzer`.

This will run through the Flutter SDK and create an equivalent *.cs files.

## Semantic Model

To generate the *.cs files, the Dart Analyzer creates a Semantic Model that is used by the writer.

## CSharp Writer

The writer, will take the Semantic Model and output the transpiled C# files into the FlutterSDK project.