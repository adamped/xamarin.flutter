using System.Collections.Generic;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    public class RasterCacheResult
    {
        public RasterCacheResult()
        {
        }

        public RasterCacheResult(sk_sp<SkImage> image, SkiaSharp.SKRect logical_rect)
        {
            this.image_ = new sk_sp<SkImage>(std::move(image));
            this.logical_rect_ = new SkiaSharp.SKRect(logical_rect);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: operator bool() const
        public static implicit operator bool(RasterCacheResult ImpliedObject)
        {
            return (bool)ImpliedObject.image_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool is_valid() const
        public bool is_valid()
        {
            return (bool)image_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(SkiaSharp.SKCanvas& canvas, const SkiaSharp.SKPaint* paint = null) const
        public void draw(SkiaSharp.SKCanvas canvas, SkiaSharp.SKPaint paint = null)
        {
            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") auto_restore(&canvas, true);
            SkIRect bounds = RasterCache.GetDeviceBounds(logical_rect_, canvas.getTotalMatrix());
            FML_DCHECK(bounds.size() == image_.Dereference().dimensions());
            canvas.resetMatrix();
            canvas.drawImage(new sk_sp<SkImage>(image_), bounds.fLeft, bounds.fTop, paint);
        }

        private sk_sp<SkImage> image_ = new sk_sp<SkImage>();
        private SkiaSharp.SKRect logical_rect_ = new SkiaSharp.SKRect();
    }

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //struct PrerollContext;

    public class RasterCache : System.IDisposable
    {
        public RasterCache(size_t threshold = 3)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.threshold_ = threshold;
            this.threshold_.CopyFrom(threshold);
            this.checkerboard_images_ = false;
            this.weak_factory_ = new fml.WeakPtrFactory<RasterCache>(this);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public static SkIRect GetDeviceBounds(SkiaSharp.SKRect rect, SkMatrix ctm)
        {
            SkiaSharp.SKRect device_rect = new SkiaSharp.SKRect();
            ctm.mapRect(device_rect, rect);
            SkIRect bounds = new SkIRect();
            device_rect.roundOut(bounds);
            return bounds;
        }

        public static SkMatrix GetIntegralTransCTM(SkMatrix ctm)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
            //ORIGINAL LINE: SkMatrix result = ctm;
            SkMatrix result = new SkMatrix(ctm);
            result[SkMatrix.kMTransX] = floorf((ctm.getTranslateX()) + 0.5f);
            result[SkMatrix.kMTransY] = floorf((ctm.getTranslateY()) + 0.5f);
            return result;
        }

        // Return true if the cache is generated.
        //
        // We may return false and not generate the cache if
        // 1. The picture is not worth rasterizing
        // 2. The matrix is singular
        // 3. The picture is accessed too few times
        public bool Prepare(GrContext context, SkPicture picture, SkMatrix transformation_matrix, SkColorSpace dst_color_space, bool is_complex, bool will_change)
        {
            if (!flow.GlobalMembers.IsPictureWorthRasterizing(picture, will_change, is_complex))
            {
                // We only deal with pictures that are worthy of rasterization.
                return false;
            }

            // Decompose the matrix (once) for all subsequent operations. We want to make
            // sure to avoid volumetric distortions while accounting for scaling.
            MatrixDecomposition matrix = new MatrixDecomposition(transformation_matrix);

            if (!matrix.IsValid())
            {
                // The matrix was singular. No point in going further.
                return false;
            }

            RasterCacheKey<uint> cache_key = new RasterCacheKey<uint>(picture.uniqueID(), transformation_matrix);

            Entry entry = picture_cache_[cache_key];
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: entry.access_count = ClampSize(entry.access_count + 1, 0, threshold_);
            entry.access_count.CopyFrom(flow.GlobalMembers.ClampSize(entry.access_count + 1, 0, new size_t(threshold_)));
            entry.used_this_frame = true;

            if (entry.access_count < threshold_ || threshold_ == 0)
            {
                // Frame threshold has not yet been reached.
                return false;
            }

            if (!entry.image.is_valid())
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: entry.image = RasterizePicture(picture, context, transformation_matrix, dst_color_space, checkerboard_images_);
                entry.image.CopyFrom(flow.GlobalMembers.RasterizePicture(picture, context, transformation_matrix, dst_color_space, checkerboard_images_));
            }
            return true;
        }

        public void Prepare(PrerollContext context, Layer layer, SkMatrix ctm)
        {
            RasterCacheKey<Layer> cache_key = new RasterCacheKey<Layer>(layer, ctm);
            Entry entry = layer_cache_[cache_key];
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: entry.access_count = ClampSize(entry.access_count + 1, 0, threshold_);
            entry.access_count.CopyFrom(flow.GlobalMembers.ClampSize(entry.access_count + 1, 0, new size_t(threshold_)));
            entry.used_this_frame = true;
            if (!entry.image.is_valid())
            {
                //C++ TO C# CONVERTER TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
                //ORIGINAL LINE: entry.image = Rasterize(context->gr_context, ctm, context->dst_color_space, checkerboard_images_, layer->paint_bounds(), [layer, context](SkiaSharp.SKCanvas* canvas)
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                entry.image.CopyFrom(flow.GlobalMembers.Rasterize(context.gr_context, ctm, context.dst_color_space, checkerboard_images_, layer.paint_bounds(), (SkiaSharp.SKCanvas canvas) =>
                {
                    Layer.PaintContext paintContext = new flow.Layer.PaintContext(canvas, null, context.frame_time, context.engine_time, context.texture_registry, context.raster_cache, context.checkerboard_offscreen_layers);
                    layer.Paint(paintContext);
                }));
            }
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: RasterCacheResult Get(const SkPicture& picture, const SkMatrix& ctm) const
        public RasterCacheResult Get(SkPicture picture, SkMatrix ctm)
        {
            RasterCacheKey<uint> cache_key = new RasterCacheKey<uint>(picture.uniqueID(), ctm);
            var it = picture_cache_.find(cache_key);
            return it == picture_cache_.end() ? new RasterCacheResult() : it.second.image;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: RasterCacheResult Get(Layer* layer, const SkMatrix& ctm) const
        public RasterCacheResult Get(Layer layer, SkMatrix ctm)
        {
            RasterCacheKey<Layer> cache_key = new RasterCacheKey<Layer>(layer, ctm);
            var it = layer_cache_.find(cache_key);
            return it == layer_cache_.end() ? new RasterCacheResult() : it.second.image;
        }

        public void SweepAfterFrame()
        {
            SweepOneCacheAfterFrame<RasterCacheKey<uint>.Map<Entry>, RasterCacheKey<uint>.Map<Entry>.iterator>(picture_cache_);
            SweepOneCacheAfterFrame<RasterCacheKey<Layer>.Map<Entry>, RasterCacheKey<Layer>.Map<Entry>.iterator>(layer_cache_);
        }

        public void Clear()
        {
            picture_cache_.clear();
        }

        public void SetCheckboardCacheImages(bool checkerboard)
        {
            if (checkerboard_images_ == checkerboard)
            {
                return;
            }

            checkerboard_images_ = checkerboard;

            // Clear all existing entries so previously rasterized items (with or without
            // a checkerboard) will be refreshed in subsequent passes.
            Clear();
        }

        private class Entry
        {
            public bool used_this_frame = false;
            public size_t access_count = 0;
            public RasterCacheResult image = new RasterCacheResult();
        }

        //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
        //ORIGINAL LINE: template <class Cache, class Iterator>
        private static void SweepOneCacheAfterFrame<Cache, Iterator>(Cache cache)
        {
            List<Iterator> dead = new List<Iterator>();

            for (var it = cache.begin(); it != cache.end(); ++it)
            {
                Entry entry = it.second;
                if (!entry.used_this_frame)
                {
                    dead.Add(it);
                }
                entry.used_this_frame = false;
            }

            foreach (var it in dead)
            {
                cache.erase(it);
            }
        }

        private readonly size_t threshold_ = new size_t();
        private PictureRasterCacheKey.Map<Entry> picture_cache_ = new PictureRasterCacheKey.Map<Entry>();
        private LayerRasterCacheKey.Map<Entry> layer_cache_ = new LayerRasterCacheKey.Map<Entry>();
        private bool checkerboard_images_;
        private fml.WeakPtrFactory<RasterCache> weak_factory_ = new fml.WeakPtrFactory<RasterCache>();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  RasterCache(const RasterCache&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  RasterCache& operator =(const RasterCache&) = delete;
    }

}