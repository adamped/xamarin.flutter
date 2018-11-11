using SkiaSharp;
using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public abstract class ContainerLayer : Layer
    {

        public void Add(Layer layer)
        {
            layer.set_parent(this);
            layers_.Add(layer);
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            TRACE_EVENT0("flutter", "ContainerLayer::Preroll");

            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, matrix, ref child_paint_bounds);
            set_paint_bounds(child_paint_bounds);
        }
        public List<Layer> layers()
        {
            return layers_;
        }

        protected void PrerollChildren(PrerollContext context, SKMatrix child_matrix, ref SKRect child_paint_bounds)
        {
            foreach (var layer in layers_)
            {
                PrerollContext child_context = context;
                layer.Preroll(child_context, child_matrix);

                if (layer.needs_system_composite())
                {
                    set_needs_system_composite(true);
                }
                child_paint_bounds.Union(layer.paint_bounds());
            }
        }
        protected void PaintChildren(PaintContext context)
        {
            FML_DCHECK(needs_painting());

            // Intentionally not tracing here as there should be no self-time
            // and the trace event on this common function has a small overhead.
            foreach (var layer in layers_)
            {
                if (layer.needs_painting())
                {
                    layer.Paint(context);
                }
            }
        }

        public List<Layer> layers_ = new List<Layer>();
    }

}