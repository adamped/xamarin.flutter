using static FlutterBinding.Flow.Helper;
using SkiaSharp;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class PictureLayer : Layer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  PictureLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_offset(SKPoint offset)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = offset;
            offset_ = offset;
        }
        public void set_picture(SkiaGPUObject<SKPicture> picture)
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

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: SKPicture* picture() const
        public SKPicture picture()
        {
            return picture_.get();
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKPicture sk_picture = picture();

            var cache = context.raster_cache;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: SKMatrix ctm = matrix;
            SKMatrix ctm = matrix;
            
            ctm.SetScaleTranslate(ctm.ScaleX, ctm.ScaleY, offset_.X, offset_.Y);
#if !SUPPORT_FRACTIONAL_TRANSLATION
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: ctm = RasterCache::GetIntegralTransCTM(ctm);
            ctm = RasterCache.GetIntegralTransCTM(ctm);
#endif
            cache.Prepare(context.gr_context, sk_picture, ctm, context.dst_color_space, is_complex_, will_change_);


            SKRect bounds = sk_picture.CullRect;
            bounds.Offset(offset_.X, offset_.Y);
            set_paint_bounds(bounds);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "PictureLayer::Paint");
            //FML_DCHECK(picture_);
            FML_DCHECK(needs_painting());

            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);
            context.canvas.Translate(offset_.X, offset_.Y);
#if !SUPPORT_FRACTIONAL_TRANSLATION
            context.canvas.SetMatrix(RasterCache.GetIntegralTransCTM(context.canvas.TotalMatrix));
#endif

            if (context.raster_cache != null)
            {
                SKMatrix ctm = context.canvas.TotalMatrix;
                RasterCacheResult result = context.raster_cache.Get(picture(), ctm);
                if (result.is_valid())
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
        private SkiaGPUObject<SKPicture> picture_;
        private bool is_complex_ = false;
        private bool will_change_ = false;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PictureLayer(const PictureLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PictureLayer& operator =(const PictureLayer&) = delete;
    }

}