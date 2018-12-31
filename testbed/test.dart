class HitTestTarget {}

mixin DiagnosticableTreeMixin {}

class AbstractNode {}

abstract class RenderObject extends AbstractNode with DiagnosticableTreeMixin implements HitTestTarget
{
  void debugResetSize() { }
}

abstract class RenderSliver extends RenderObject { 
  @override
  void debugResetSize() { }
}

abstract class RenderSliverHelpers implements RenderSliver { }