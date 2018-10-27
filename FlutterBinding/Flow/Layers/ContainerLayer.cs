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
        public ContainerLayer()
        {
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void Add(Layer layer)
        {
            layer.set_parent(this);
            layers_.Add(layer);
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            TRACE_EVENT0("flutter", "ContainerLayer::Preroll");

            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, matrix, child_paint_bounds);
            set_paint_bounds(child_paint_bounds);
        }



        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const ClassicVector<Layer*>& layers() const
        public List<Layer> layers()
        {
            return layers_;
        }

        protected void PrerollChildren(PrerollContext context, SKMatrix child_matrix, SKRect child_paint_bounds)
        {
            foreach (var layer in layers_)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: PrerollContext child_context = *context;
                PrerollContext child_context = context;
                layer.Preroll(child_context, child_matrix);

                if (layer.needs_system_composite())
                {
                    set_needs_system_composite(true);
                }
                child_paint_bounds.Union(layer.paint_bounds());
            }
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void PaintChildren(PaintContext& context) const
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

        private List<Layer> layers_ = new List<Layer>();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ContainerLayer(const ContainerLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ContainerLayer& operator =(const ContainerLayer&) = delete;
    }

}