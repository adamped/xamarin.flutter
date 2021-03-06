﻿using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class LayerTree
    {
        public LayerTree()
        {
            this.frame_size_ = new SKSizeI();
            this.rasterizer_tracing_threshold_ = 0;
            this.checkerboard_raster_cache_images_ = false;
            this.checkerboard_offscreen_layers_ = false;
        }

        public void Preroll(CompositorContext.ScopedFrame frame, bool ignore_raster_cache = false)
        {
            TRACE_EVENT0("flutter", "LayerTree::Preroll");
            
            //TODO: frame.canvas().imageInfo().colorSpace()
            SKColorSpace color_space = frame.canvas() != null ? SKImageInfo.Empty.ColorSpace : null;
            frame.context().raster_cache().SetCheckboardCacheImages(checkerboard_raster_cache_images_);
            PrerollContext context = ignore_raster_cache ? null :new PrerollContext(frame.context().raster_cache(), frame.gr_context(), color_space, SKRect.Empty, frame.context().texture_registry(), checkerboard_offscreen_layers_);

            root_layer_.Preroll(context, frame.root_surface_transformation());
        }

        public void Paint(CompositorContext.ScopedFrame frame, bool ignore_raster_cache = false)
        {
            TRACE_EVENT0("flutter", "LayerTree::Paint");
            Layer.PaintContext context = new Layer.PaintContext(frame.canvas(), frame.view_embedder(), frame.context().texture_registry(), ignore_raster_cache ? null : frame.context().raster_cache(), checkerboard_offscreen_layers_);

            if (root_layer_.needs_painting())
            {
                root_layer_.Paint(context);
            }
        }

        public SKPicture Flatten(SKRect bounds)
        {
            TRACE_EVENT0("flutter", "LayerTree::Flatten");

            SKPictureRecorder recorder = new SKPictureRecorder();
            var canvas = recorder.BeginRecording(bounds);

            if (canvas == null)
            {
                return null;
            }

            TextureRegistry unused_texture_registry = new TextureRegistry();
            SKMatrix root_surface_transformation = new SKMatrix();
            // No root surface transformation. So assume identity.
            canvas.ResetMatrix();

            PrerollContext preroll_context = new PrerollContext(null, null, null, SKRect.Empty, unused_texture_registry, false);

            Layer.PaintContext paint_context = new Layer.PaintContext(canvas, null, unused_texture_registry, null, false);

            // Even if we don't have a root layer, we still need to create an empty
            // picture.
            if (root_layer_ != null)
            {
                root_layer_.Preroll(preroll_context, root_surface_transformation);
                // The needs painting flag may be set after the preroll. So check it after.
                if (root_layer_.needs_painting())
                {
                    root_layer_.Paint(paint_context);
                }
            }

            return recorder.EndRecording();
        }

        public void set_root_layer(Layer root_layer)
        {
            root_layer_ = root_layer;
        }

        public SKSizeI frame_size()
        {
            return frame_size_;
        }

        public void set_frame_size(SKSizeI frame_size)
        {
            frame_size_ = frame_size;
        }

        // The number of frame intervals missed after which the compositor must
        // trace the rasterized picture to a trace file. Specify 0 to disable all
        // tracing
        public void set_rasterizer_tracing_threshold(uint interval)
        {
            rasterizer_tracing_threshold_ = interval;
        }
        public uint rasterizer_tracing_threshold()
        {
            return rasterizer_tracing_threshold_;
        }

        public void set_checkerboard_raster_cache_images(bool checkerboard)
        {
            checkerboard_raster_cache_images_ = checkerboard;
        }

        public void set_checkerboard_offscreen_layers(bool checkerboard)
        {
            checkerboard_offscreen_layers_ = checkerboard;
        }

        private SKSizeI frame_size_ = new SKSizeI(); // Physical pixels.
        public Layer root_layer_;
        private uint rasterizer_tracing_threshold_;
        private bool checkerboard_raster_cache_images_;
        private bool checkerboard_offscreen_layers_;
    }
}