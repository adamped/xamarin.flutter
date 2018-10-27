using SkiaSharp;
using static FlutterBinding.Flow.Helper;

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

        public void set_transform(SKMatrix transform)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: transform_ = transform;
            transform_ = transform;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKMatrix child_matrix = new SKMatrix();
            child_matrix.SetConcat(matrix, transform_);

            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, child_matrix, child_paint_bounds);

            transform_.MapRect(child_paint_bounds);
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
            context.canvas.Concat(ref transform_);
            PaintChildren(context);
        }

        private SKMatrix transform_ = new SKMatrix();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TransformLayer(const TransformLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TransformLayer& operator =(const TransformLayer&) = delete;
    }

}