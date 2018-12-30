import 'package:analyzer/dart/element/element.dart';

import '../naming.dart';
import 'methods.dart';

class Delegates {
  static String printDelegate(FunctionTypeAliasElement element) { 
    var returnType = Naming.getReturnType(element);
    var methodName = Naming.nameWithTypeParameters(element, false);
    var parameter = Methods.printParameter(element, element, null); 
    return "public delegate ${returnType} ${methodName}(${parameter});";
  }
}
