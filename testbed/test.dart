
abstract class RenderAbstractViewport extends RenderObject {
 
  static RenderAbstractViewport of(RenderObject object) {  
    return null;
  }

  final double offset;

}

abstract class RenderViewportBase<ParentDataClass extends ContainerParentDataMixin<RenderSliver>>
    extends RenderBox with ContainerRenderObjectMixin<RenderSliver, ParentDataClass>
    implements RenderAbstractViewport {

}

class DiagnosticableTreeMixin {}

class HitTestTarget {}

abstract class AbstractNode<T> {}

abstract class RenderObject extends AbstractNode with DiagnosticableTreeMixin implements HitTestTarget {}

mixin ContainerParentDataMixin<ChildType extends RenderObject> on ParentData {}


class RenderBox {}
class RenderSilver {}
class ParentDataClass {}
class ParentDataType {}
mixin ContainerRenderObjectMixin<ChildType extends RenderObject, ParentDataType extends ContainerParentDataMixin<ChildType>> on RenderObject {}

