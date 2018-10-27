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

        public void set_offset(SkPoint offset)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = offset;
            offset_.CopyFrom(offset);
        }
        public void set_picture(SkiaGPUObject<SkPicture> picture)
        {
            picture_ = std::move(picture);
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
        //ORIGINAL LINE: SkPicture* picture() const
        public SkPicture picture()
        {
            return picture_.get().get();
        }

        public override void Preroll(PrerollContext context, SkMatrix matrix)
        {
            SkPicture sk_picture = picture();

            if (auto cache = context.raster_cache)
	{
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: SkMatrix ctm = matrix;
                SkMatrix ctm = new SkMatrix(matrix);
                ctm.postTranslate(offset_.x(), offset_.y());
#if !SUPPORT_FRACTIONAL_TRANSLATION
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: ctm = RasterCache::GetIntegralTransCTM(ctm);
                ctm.CopyFrom(RasterCache.GetIntegralTransCTM(ctm));
#endif
                cache.Prepare(context.gr_context, sk_picture, ctm, context.dst_color_space, is_complex_, will_change_);
            }

            SkiaSharp.SKRect bounds = sk_picture.cullRect().makeOffset(offset_.x(), offset_.y());
            set_paint_bounds(bounds);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "PictureLayer::Paint");
            FML_DCHECK(picture_.get());
            FML_DCHECK(needs_painting());

            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);
            context.canvas.translate(offset_.x(), offset_.y());
#if !SUPPORT_FRACTIONAL_TRANSLATION
            context.canvas.setMatrix(RasterCache.GetIntegralTransCTM(context.canvas.getTotalMatrix()));
#endif

            if (context.raster_cache != null)
            {
                SkMatrix ctm = context.canvas.getTotalMatrix();
                RasterCacheResult result = context.raster_cache.Get(picture(), ctm);
                if (result.is_valid())
                {
                    result.draw(context.canvas);
                    return;
                }
            }
            context.canvas.drawPicture(picture());
        }

        private SkPoint offset_ = new SkPoint();
        // Even though pictures themselves are not GPU resources, they may reference
        // images that have a reference to a GPU resource.
        private SkiaGPUObject<SkPicture> picture_ = new SkiaGPUObject<SkPicture>();
        private bool is_complex_ = false;
        private bool will_change_ = false;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PictureLayer(const PictureLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PictureLayer& operator =(const PictureLayer&) = delete;
    }

}