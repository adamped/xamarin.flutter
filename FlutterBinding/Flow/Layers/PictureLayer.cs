using static FlutterBinding.Flow.Helper;
using SkiaSharp;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class PictureLayer : Layer
    {

        public void set_offset(SKPoint offset)
        {
            offset_ = offset;
        }
        public void set_picture(SKPicture picture)
        {
            picture_ = picture;
        }

        public void set_is_complex(bool value)
        {
            is_complex_ = value;
        }
        public void set_will_change(bool value)
        {
            will_change_ = value;
        }

        public SKPicture picture()
        {
            return picture_;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKPicture sk_picture = picture();

            var cache = context.raster_cache;
            SKMatrix ctm = matrix;
            
            ctm.SetScaleTranslate(ctm.ScaleX, ctm.ScaleY, offset_.X, offset_.Y);
#if !SUPPORT_FRACTIONAL_TRANSLATION
            ctm = RasterCache.GetIntegralTransCTM(ctm);
#endif
            cache.Prepare(context.gr_context, sk_picture, ctm, context.dst_color_space, is_complex_, will_change_);


            SKRect bounds = sk_picture.CullRect;
            bounds.Offset(offset_.X, offset_.Y);
            set_paint_bounds(bounds);
        }

        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "PictureLayer::Paint");
            //FML_DCHECK(picture_);
            FML_DCHECK(needs_painting());

            context.canvas.Translate(offset_.X, offset_.Y);
#if !SUPPORT_FRACTIONAL_TRANSLATION
            context.canvas.SetMatrix(RasterCache.GetIntegralTransCTM(context.canvas.TotalMatrix));
#endif

            if (context.raster_cache != null)
            {
                SKMatrix ctm = context.canvas.TotalMatrix;
                RasterCacheResult result = context.raster_cache.Get(picture(), ctm);
                if (result.is_valid)
                {
                    result.draw(context.canvas);
                    return;
                }
            }
            context.canvas.DrawPicture(picture());
        }

        private SKPoint offset_ = new SKPoint();
        // Even though pictures themselves are not GPU resources, they may reference
        // images that have a reference to a GPU resource.
        private SKPicture picture_;
        private bool is_complex_ = false;
        private bool will_change_ = false;
    }

}