using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class TransformLayer : ContainerLayer
    {

        public void set_transform(SKMatrix transform)
        {
            transform_ = transform;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKMatrix child_matrix = new SKMatrix();
            SKMatrix.Concat(ref child_matrix, matrix, transform_);
            
            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, child_matrix, ref child_paint_bounds);

            transform_.MapRect(child_paint_bounds);
            set_paint_bounds(child_paint_bounds);
        }

        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "TransformLayer::Paint");
            FML_DCHECK(needs_painting());

            context.canvas.Concat(ref transform_);
            PaintChildren(context);
        }

        private SKMatrix transform_ = new SKMatrix();
    }

}