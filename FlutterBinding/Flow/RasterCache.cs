using FlutterBinding.Flow.Layers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using static FlutterBinding.Flow.RasterCache;

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

        public bool is_valid => image_ != null; //?? I'm not sure if this is right
       

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(SKCanvas& canvas, const SKPaint* paint = null) const
        public void draw(SKCanvas canvas, SKPaint paint = null)
        {
            var bounds = RasterCache.GetDeviceBounds(logical_rect_, canvas.TotalMatrix);
            canvas.ResetMatrix();
            canvas.DrawImage(image_, bounds.Left, bounds.Top, paint);
        }

        private SKImage image_;
        private SKRect logical_rect_;
    }
    
    public class UniqueEntry : Entry
    {
        public UniqueEntry(uint value) => Value = value;
        public uint Value { get; private set; }
    }


    public class RasterCache //: System.IDisposable
    {
        public RasterCache(int threshold = 3)
        {
            this.threshold_ = threshold;
            this.checkerboard_images_ = false;
            this.weak_factory_ = this;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public static SKRectI GetDeviceBounds(SKRect rect, SKMatrix ctm)
        {
            SKRect device_rect = new SKRect();
            SKMatrix.MapRect(ref ctm, out device_rect, ref rect);
            var bounds = SKRectI.Round(device_rect);      
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
            //MatrixDecomposition matrix = new MatrixDecomposition(transformation_matrix);

            //if (!matrix.IsValid())
            //{
            //    // The matrix was singular. No point in going further.
            //    return false;
            //}

            RasterCacheKey<UniqueEntry> cache_key = new RasterCacheKey<UniqueEntry>(new UniqueEntry(picture.UniqueId), transformation_matrix);

            Entry entry = picture_cache_.First(x => x.Equals(cache_key)).id(); // I used Linq, that aint going to be good for performance
            entry.access_count = GlobalMembers.ClampSize(entry.access_count + 1, 0, threshold_);
            entry.used_this_frame = true;

            if (entry.access_count < threshold_ || threshold_ == 0)
            {
                // Frame threshold has not yet been reached.
                return false;
            }

            if (!entry.image.is_valid)
            {
                entry.image = GlobalMembers.RasterizePicture(picture, context, transformation_matrix, dst_color_space, checkerboard_images_);
            }
            return true;
        }

        public void Prepare(PrerollContext context, Layer layer, SKMatrix ctm)
        {
            RasterCacheKey<Layer> cache_key = new RasterCacheKey<Layer>(layer, ctm);
            Entry entry = layer_cache_.First(x=>x == cache_key).id(); // I used Linq, that aint going to be good for performance
         
            entry.access_count = GlobalMembers.ClampSize(entry.access_count + 1, 0, threshold_);
            entry.used_this_frame = true;
            if (!entry.image.is_valid)
            {
                entry.image = GlobalMembers.Rasterize(context.gr_context, ctm, context.dst_color_space, checkerboard_images_, layer.paint_bounds(), (SKCanvas canvas) =>
                {
                    Layer.PaintContext paintContext = new Layer.PaintContext(canvas, null, context.texture_registry, context.raster_cache, context.checkerboard_offscreen_layers);
                    layer.Paint(paintContext);
                });
            }
        }

        public RasterCacheResult Get(SKPicture picture, SKMatrix ctm)
        {
            var cache_key = new RasterCacheKey<UniqueEntry>(new UniqueEntry(picture.UniqueId), ctm);
            var it = picture_cache_.First(x => x.Equals(cache_key));
            return it == picture_cache_.Last() ? new RasterCacheResult() : new RasterCacheResult(); // This aint right;
        }
        
        public RasterCacheResult Get(Layer layer, SKMatrix ctm)
        {
            //RasterCacheKey<Layer> cache_key = new RasterCacheKey<Layer>(layer, ctm);
            //var it = layer_cache_.find(cache_key);
            return new RasterCacheResult(); // This aint right;
            //return it == layer_cache_.end() ? new RasterCacheResult() : it.second.image;
        }

        public void SweepAfterFrame()
        {
            SweepOneCacheAfterFrame(picture_cache_);
            SweepOneCacheAfterFrame(layer_cache_);
        }

        public void Clear()
        {
            picture_cache_.Clear();
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

        public class Entry
        {
            public bool used_this_frame = false;
            public int access_count = 0;
            public RasterCacheResult image = new RasterCacheResult();
        }

        private static void SweepOneCacheAfterFrame<Cache>(List<RasterCacheKey<Cache>> cache) where Cache : Entry
        {
            var dead = new List<RasterCacheKey<Cache>>();

            for (int i = 0; i < cache.Count; i++)
            {
                Entry entry = cache[i].id();
                if (!entry.used_this_frame)
                {
                    dead.Add(cache[i]);
                }
                entry.used_this_frame = false;
            }

            foreach (var it in dead)
            {
                cache.Remove(it);
            }
        }

        private readonly int threshold_ = new int();
        private List<RasterCacheKey<Entry>> picture_cache_ = new List<RasterCacheKey<Entry>>(); // = new PictureRasterCacheKey.Map<Entry>();
        private List<RasterCacheKey<Layer>> layer_cache_ = new List<RasterCacheKey<Layer>>(); // new LayerRasterCacheKey.Map<Entry>();
        private bool checkerboard_images_;
        private RasterCache weak_factory_;
    }

}