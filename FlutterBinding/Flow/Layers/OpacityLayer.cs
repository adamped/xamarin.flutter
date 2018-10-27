using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class OpacityLayer : ContainerLayer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  OpacityLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_alpha(int alpha)
        {
            alpha_ = alpha;
        }
        public void set_offset(SKPoint offset)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = offset;
            offset_.CopyFrom(offset);
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
            //ORIGINAL LINE: SKMatrix child_matrix = matrix;
            SKMatrix child_matrix = new SKMatrix(matrix);
            child_matrix.postTranslate(offset_.fX, offset_.fY);
            base.Preroll(context, child_matrix);
            if (context.raster_cache != null && layers().Count == 1)
            {
                Layer child = layers()[0].get();
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: SKMatrix ctm = child_matrix;
                SKMatrix ctm = new SKMatrix(child_matrix);
#if !SUPPORT_FRACTIONAL_TRANSLATION
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: ctm = RasterCache::GetIntegralTransCTM(ctm);
                ctm.CopyFrom(RasterCache.GetIntegralTransCTM(ctm));
#endif
                context.raster_cache.Prepare(context, child, ctm);
            }
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "OpacityLayer::Paint");
            FML_DCHECK(needs_painting());

            SKPaint paint = new SKPaint();
            paint.setAlpha(alpha_);

            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);
            context.canvas.translate(offset_.fX, offset_.fY);

#if !SUPPORT_FRACTIONAL_TRANSLATION
            context.canvas.setMatrix(RasterCache.GetIntegralTransCTM(context.canvas.getTotalMatrix()));
#endif

            if (layers().Count == 1 && context.raster_cache)
            {
                SKMatrix ctm = context.canvas.getTotalMatrix();
                RasterCacheResult child_cache = context.raster_cache.Get(layers()[0].get(), ctm);
                if (child_cache.is_valid())
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

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  OpacityLayer(const OpacityLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  OpacityLayer& operator =(const OpacityLayer&) = delete;
    }

}