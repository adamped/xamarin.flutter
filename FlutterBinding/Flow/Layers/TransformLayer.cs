// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class TransformLayer : ContainerLayer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  TransformLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_transform(SkMatrix transform)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: transform_ = transform;
            transform_.CopyFrom(transform);
        }

        public override void Preroll(PrerollContext context, SkMatrix matrix)
        {
            SkMatrix child_matrix = new SkMatrix();
            child_matrix.setConcat(matrix, transform_);

            SkiaSharp.SKRect child_paint_bounds = SkiaSharp.SKRect.MakeEmpty();
            PrerollChildren(context, child_matrix, child_paint_bounds);

            transform_.mapRect(child_paint_bounds);
            set_paint_bounds(child_paint_bounds);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "TransformLayer::Paint");
            FML_DCHECK(needs_painting());

            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);
            context.canvas.concat(transform_);
            PaintChildren(context);
        }

        private SkMatrix transform_ = new SkMatrix();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TransformLayer(const TransformLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TransformLayer& operator =(const TransformLayer&) = delete;
    }

}