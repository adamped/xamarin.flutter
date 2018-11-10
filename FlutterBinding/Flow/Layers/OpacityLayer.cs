using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class OpacityLayer : ContainerLayer
    {

        public void set_alpha(int alpha)
        {
            alpha_ = alpha;
        }
        public void set_offset(SKPoint offset)
        {
            offset_ = offset;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKMatrix child_matrix = matrix;
            child_matrix.SetScaleTranslate(child_matrix.ScaleX, child_matrix.ScaleY, offset_.X, offset_.Y);
            base.Preroll(context, child_matrix);
            if (context.raster_cache != null && layers().Count == 1)
            {
                Layer child = layers()[0];//.get();
                SKMatrix ctm = child_matrix;

#if !SUPPORT_FRACTIONAL_TRANSLATION
                ctm = RasterCache.GetIntegralTransCTM(ctm);
#endif
                context.raster_cache.Prepare(context, child, ctm);
            }
        }

        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "OpacityLayer::Paint");
            FML_DCHECK(needs_painting());

            SKPaint paint = new SKPaint();
            paint.Color = paint.Color.WithAlpha((byte)alpha_);

            context.canvas.Translate(offset_.X, offset_.Y);

#if !SUPPORT_FRACTIONAL_TRANSLATION
            context.canvas.SetMatrix(RasterCache.GetIntegralTransCTM(context.canvas.TotalMatrix));
#endif

            if (layers().Count == 1 && context.raster_cache != null)
            {
                SKMatrix ctm = context.canvas.TotalMatrix;
                RasterCacheResult child_cache = context.raster_cache.Get(layers()[0], ctm);
                if (child_cache.is_valid)
                {
                    child_cache.draw(context.canvas, paint);
                    return;
                }
            }

            Layer.AutoSaveLayer save_layer = Layer.AutoSaveLayer.Create(context, paint_bounds(), paint);
            PaintChildren(context);
        }

        // TODO(chinmaygarde): Once MZ-139 is addressed, introduce a new node in the
        // session scene hierarchy.

        private int alpha_;
        private SKPoint offset_ = new SKPoint();
    }
}