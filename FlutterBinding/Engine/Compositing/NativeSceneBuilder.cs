using FlutterBinding.Engine.Painting;
using FlutterBinding.Flow.Layers;
using FlutterBinding.UI;
using SkiaSharp;
using System.Collections.Generic;

namespace FlutterBinding.Engine.Compositing
{
    //https://github.com/flutter/engine/blob/master/lib/ui/compositing/scene_builder.h
    //https://github.com/flutter/engine/blob/master/lib/ui/compositing/scene_builder.cc

    public class NativeSceneBuilder
    {
        ContainerLayer root_layer_;
        ContainerLayer current_layer_;

        protected void Constructor() { }

        protected void PushTransform(List<double> matrix4)
        {
            var sk_matrix = Matrix.ToSkMatrix(matrix4);
            var layer = new TransformLayer();
            layer.set_transform(sk_matrix);
            PushLayer(layer);
        }

        protected NativeEngineLayer PushOffset(double dx, double dy)
        {
            SKMatrix sk_matrix = SKMatrix.MakeTranslation((float)dx, (float)dy);
            var layer = new TransformLayer();
            layer.set_transform(sk_matrix);
            PushLayer(layer);
            return NativeEngineLayer.MakeRetained(layer);
        }

        protected void PushClipRect(double left,
                                double right,
                                double top,
                                double bottom,
                                int clipBehavior)
        {
            var clipRect = new SKRect((float)left, (float)top, (float)right, (float)bottom);
            var layer = new ClipRectLayer((Flow.Layers.Clip)clipBehavior);
            layer.set_clip_rect(clipRect);
            PushLayer(layer);
        }

        protected void PushClipPath(SKPath path, int clipBehavior)
        {
            // FML_DCHECK(clip_behavior != flow::Clip::none);
            var layer = new ClipPathLayer((Flow.Layers.Clip)clipBehavior);
            layer.set_clip_path(path);
            PushLayer(layer);
        }

        protected void PushLayer(ContainerLayer layer)
        {
            if (root_layer_ == null)
            {
                root_layer_ = layer;
                current_layer_ = root_layer_;
                return;
            }

            if (current_layer_ == null)
            {
                return;
            }

            ContainerLayer newLayer = layer;
            current_layer_.Add(layer);
            current_layer_ = newLayer;
        }

    }
}
