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

class RenderingFlutterBinding with SchedulerBinding, RendererBinding {

}