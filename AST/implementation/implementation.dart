import 'package:analyzer/dart/element/element.dart';

import '../naming.dart';

/// Provides methods to transpile the body of elements
class Implementation {
  static String MethodBody(MethodElement element) {
    var body = element.computeNode().body;
    var bodyLines = Naming.tokenToText(body.beginToken, false).split("\n");
    var rawBody = bodyLines.map((l) => "// ${l}\n").join();

    // Transpile logic comes here
    var transpiledBody = rawBody + "throw new NotImplementedException();";  
    return transpiledBody;
  }

  static String FieldBody(PropertyAccessorElement element){
    var body = element.computeNode();
    var bodyLines = Naming.tokenToText(body.beginToken, false).split("\n");
    var rawBody = bodyLines.map((l) => "// ${l}\n").join();
        
    // Transpile logic comes here
    var transpiledBody = rawBody + "throw new NotImplementedException();";  
    return transpiledBody;
  }

   static String FunctionBody(FunctionElement element) {
    // TODO
    return "throw new NotImplementedException();";  
  }
}
