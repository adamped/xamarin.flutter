import 'package:analyzer/dart/element/element.dart';

import '../implementation/implementation.dart';
import '../naming.dart';
import 'methods.dart';

class Functions {
  static String printFunction(FunctionElement element) {
    var code = new StringBuffer();
    code.write(methodSignature(element));

    code.writeln("{");
    code.writeln(Implementation.FunctionBody(element));
    code.writeln("}");

    return code.toString();
  }

  static String methodSignature(ExecutableElement element) {
    var methodName = Naming.nameWithTypeParameters(element, false);
    methodName = Naming.getFormattedName(
        methodName,
        element.isPrivate
            ? NameStyle.LeadingUnderscoreLowerCamelCase
            : NameStyle.UpperCamelCase);

    var parameter = Methods.printParameter(element);
    var returnType = Naming.getReturnType(element); 

    // TODO This is a workaround
    if(returnType == "() → dynamic"){
      returnType = "object";
    }
    return "${returnType} ${methodName}(${parameter})";
  }
}
