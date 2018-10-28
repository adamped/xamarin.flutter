using FlutterBinding.Flow.Layers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;

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

        public RasterCacheResult(SKImage image, SKRect logical_rect)
        {
            this.image_ = image;
            this.logical_rect_ = new SKRect(logical_rect.Left, logical_rect.Top, logical_rect.Right, logical_rect.Bottom);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: operator bool() const
        //public static implicit operator bool(RasterCacheResult ImpliedObject)
        //{
        //    return (bool)ImpliedObject.image_;
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool is_valid() const
        public bool is_valid;
        //{
        //    return (bool)image_;
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(SKCanvas& canvas, const SKPaint* paint = null) const
        public void draw(SKCanvas canvas, SKPaint paint = null)
        {
            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") auto_restore(&canvas, true);
            var bounds = RasterCache.GetDeviceBounds(logical_rect_, canvas.TotalMatrix);
            //FML_DCHECK(bounds.Size == image_..dimensions());
            canvas.ResetMatrix();
            canvas.DrawImage(image_, bounds.Left, bounds.Top, paint);
        }

        private SKImage image_;
        private SKRect logical_rect_;
    }

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //struct PrerollContext;

    public class RasterCache //: System.IDisposable
    {
        public RasterCache(int threshold = 3)
        {
            this.threshold_ = threshold;
            this.checkerboard_images_ = false;
            this.weak_factory_ = new fml.WeakPtrFactory<RasterCache>(this);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public static SKRectI GetDeviceBounds(SKRect rect, SKMatrix ctm)
        {
            SKRect device_rect = new SKRect();
            ctm.MapRect(device_rect, rect);
            SKRectI bounds = new SKRectI();
            device_rect.roundOut(bounds);
            return bounds;
        }

        public static SKMatrix GetIntegralTransCTM(SKMatrix ctm)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
            //ORIGINAL LINE: SKMatrix result = ctm;
            SKMatrix result = ctm;
            result.TransX = (float)Math.Floor((ctm.TransX) + 0.5f);
            result.TransY = (float)Math.Floor((ctm.TransY) + 0.5f);
            return result;
        }

        // Return true if the cache is generated.
        //
        // We may return false and not generate the cache if
        // 1. The picture is not worth rasterizing
        // 2. The matrix is singular
        // 3. The picture is accessed too few times
        public bool Prepare(GRContext context, SKPicture picture, SKMatrix transformation_matrix, SKColorSpace dst_color_space, bool is_complex, bool will_change)
        {
            if (!GlobalMembers.IsPictureWorthRasterizing(picture, will_change, is_complex))
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
            entry.access_count = GlobalMembers.ClampSize(entry.access_count + 1, 0, threshold_);
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
                entry.image = GlobalMembers.RasterizePicture(picture, context, transformation_matrix, dst_color_space, checkerboard_images_);
            }
            return true;
        }

        public void Prepare(PrerollContext context, Layer layer, SKMatrix ctm)
        {
            RasterCacheKey<Layer> cache_key = new RasterCacheKey<Layer>(layer, ctm);
            Entry entry = layer_cache_[cache_key];
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: entry.access_count = ClampSize(entry.access_count + 1, 0, threshold_);
            entry.access_count = GlobalMembers.ClampSize(entry.access_count + 1, 0, threshold_);
            entry.used_this_frame = true;
            if (!entry.image.is_valid())
            {
                //C++ TO C# CONVERTER TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
                //ORIGINAL LINE: entry.image = Rasterize(context->gr_context, ctm, context->dst_color_space, checkerboard_images_, layer->paint_bounds(), [layer, context](SKCanvas* canvas)
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                entry.image = GlobalMembers.Rasterize(context.gr_context, ctm, context.dst_color_space, checkerboard_images_, layer.paint_bounds(), (SKCanvas canvas) =>
                {
                    Layer.PaintContext paintContext = new Layer.PaintContext(canvas, null, context.texture_registry, context.raster_cache, context.checkerboard_offscreen_layers);
                    layer.Paint(paintContext);
                });
            }
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: RasterCacheResult Get(const SKPicture& picture, const SKMatrix& ctm) const
        public RasterCacheResult Get(SKPicture picture, SKMatrix ctm)
        {
            RasterCacheKey<uint> cache_key = new RasterCacheKey<uint>(picture.uniqueID(), ctm);
            var it = picture_cache_.find(cache_key);
            return it == picture_cache_.end() ? new RasterCacheResult() : it.second.image;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: RasterCacheResult Get(Layer* layer, const SKMatrix& ctm) const
        public RasterCacheResult Get(Layer layer, SKMatrix ctm)
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
            public int access_count = 0;
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

        private readonly int threshold_ = new int();
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