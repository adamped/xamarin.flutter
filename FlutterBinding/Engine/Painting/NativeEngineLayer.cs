using FlutterBinding.Flow.Layers;

namespace FlutterBinding.Engine.Painting
{
    //https://github.com/flutter/engine/blob/master/lib/ui/painting/engine_layer.h
    //https://github.com/flutter/engine/blob/master/lib/ui/painting/engine_layer.cc
       
    public class NativeEngineLayer
    {

        public static NativeEngineLayer MakeRetained(ContainerLayer layer)
        {
            return new NativeEngineLayer(layer);
        }
        
        ContainerLayer _layer;
        NativeEngineLayer(ContainerLayer layer)
        {
            _layer = layer;
        }

    }
}
