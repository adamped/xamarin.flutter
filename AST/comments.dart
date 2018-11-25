import 'package:analyzer/dart/element/element.dart';

class Comments {
  static void appendComment(StringBuffer buffer, Element element) {
    return;
    var dartComment = element.documentationComment;
    if (dartComment == null || dartComment == "") return;

    buffer.writeln("/// <Summary>");
    buffer.writeln(dartComment);
    buffer.writeln("/// </Summary>");
  }
}