mixin SchedulerBinding {
  @override
  void initInstances() {
  }

  /// The current [SchedulerBinding], if one has been created.
  static SchedulerBinding get instance => _instance;
  static SchedulerBinding _instance;
}


/// The glue between the render tree and the Flutter engine.
mixin RendererBinding on SchedulerBinding {
  @override
  void initInstances() {
   
  }

  /// The current [RendererBinding], if one has been created.
  static RendererBinding get instance => _instance;
  static RendererBinding _instance;
}

class RenderingFlutterBinding with SchedulerBinding, RendererBinding { //
  /// Creates a binding for the rendering layer.
  ///
  /// The `root` render box is attached directly to the [renderView] and is
  /// given constraints that require it to fill the window.
  RenderingFlutterBinding() {
    assert(renderView != null);
  }
}