abstract class Listenable { }

class ChangeNotifier {}

abstract class CustomPainter extends Listenable {

bool shouldRepaint(covariant CustomPainter oldDelegate);


}



class ScrollbarPainter extends ChangeNotifier implements CustomPainter {

@override
  bool shouldRepaint(CustomPainter old) { return true; }
}