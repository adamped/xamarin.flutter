class Widget {}
class Row {
  Row(<Widget> children) {}
 }
}

class OutlineButton {
  OutlineButton({Row child});
}

class _OutlineButtonWithIcon extends OutlineButton {
  _OutlineButtonWithIcon({@required Widget icon,
    @required Widget label,
  }) : super(
         child: Row(
           children: <Widget>[
             icon,
             const SizedBox(width: 8.0),
             label,
           ],
         ),
       );
}