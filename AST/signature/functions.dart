import 'package:analyzer/dart/element/element.dart';
import 'package:analyzer/src/dart/element/type.dart';

import '../implementation/implementation.dart';
import '../naming.dart';
import 'methods.dart';

class Functions {
  static String printFunction(FunctionElement element) {
    var code = new StringBuffer();
    code.write("internal static ");
    code.write(methodSignature(element));

    code.writeln("{");
    code.writeln(Implementation.functionBody(element));
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

    var parameter = Methods.printParameter(element, element, null);
    var returnType = Naming.getReturnType(element);

    if (returnType.contains("() → dynamic")) {
      var type = element.returnType;
      if (type is FunctionTypeImpl)
        returnType = type.newPrune[0].name;
      else
        throw new AssertionError('Unaccounted for returnType, please fix.');
    }
    return "${returnType} ${methodName}(${parameter})";
  }
}
