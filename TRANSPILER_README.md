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

This will run through the Flutter SDK and create an equivalent *.sm.json file. This is the generated semantic model.

## Semantic Model

This is just a JSON file that contains the customized semantic model from Dart. It is output by the Dart Analyzer, and read by the CSharpWriter.

## CSharp Writer

Open the Dart2CSharpTranspiler and run the Console app, assuming all previous steps completed successfully.

This will output the Flutter SDK in C# to the Flutter/FlutterSDK.csproj project.

The FlutterBinding project is a manually created and edited project and is not part of the transpiling.