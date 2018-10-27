// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class LayerTree : System.IDisposable
    {
        public LayerTree()
        {
            this.frame_size_ = new SkISize();
            this.rasterizer_tracing_threshold_ = 0;
            this.checkerboard_raster_cache_images_ = false;
            this.checkerboard_offscreen_layers_ = false;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void Preroll(CompositorContext.ScopedFrame frame, bool ignore_raster_cache = false)
        {
            TRACE_EVENT0("flutter", "LayerTree::Preroll");
            SkColorSpace color_space = frame.canvas() != null ? frame.canvas().imageInfo().colorSpace() : null;
            frame.context().raster_cache().SetCheckboardCacheImages(checkerboard_raster_cache_images_);
            PrerollContext context = new PrerollContext(ignore_raster_cache ? null : frame.context().raster_cache(), frame.gr_context(), color_space, SkiaSharp.SKRect.MakeEmpty(), frame.context().frame_time(), frame.context().engine_time(), frame.context().texture_registry(), checkerboard_offscreen_layers_);

            root_layer_.Preroll(context, frame.root_surface_transformation());
        }



        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(CompositorContext::ScopedFrame& frame, bool ignore_raster_cache = false) const
        public void Paint(CompositorContext.ScopedFrame frame, bool ignore_raster_cache = false)
        {
            TRACE_EVENT0("flutter", "LayerTree::Paint");
            Layer.PaintContext context = new flow.Layer.PaintContext(frame.canvas(), frame.view_embedder(), frame.context().frame_time(), frame.context().engine_time(), frame.context().texture_registry(), ignore_raster_cache ? null : frame.context().raster_cache(), checkerboard_offscreen_layers_);

            if (root_layer_.needs_painting())
            {
                root_layer_.Paint(context);
            }
        }

        public sk_sp<SkPicture> Flatten(SkiaSharp.SKRect bounds)
        {
            TRACE_EVENT0("flutter", "LayerTree::Flatten");

            SkiaSharp.SKPictureRecorder recorder = new SkiaSharp.SKPictureRecorder();
            var canvas = recorder.BeginRecording(bounds);

            if (canvas == null)
            {
                return null;
            }

            Stopwatch unused_stopwatch = new Stopwatch();
            TextureRegistry unused_texture_registry = new TextureRegistry();
            SkMatrix root_surface_transformation = new SkMatrix();
            // No root surface transformation. So assume identity.
            root_surface_transformation.reset();

            PrerollContext preroll_context = new PrerollContext(null, null, null, SkiaSharp.SKRect.MakeEmpty(), unused_stopwatch, unused_stopwatch, unused_texture_registry, false);

            Layer.PaintContext paint_context = new flow.Layer.PaintContext(*canvas, null, unused_stopwatch, unused_stopwatch, unused_texture_registry, null, false);

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

            return recorder.finishRecordingAsPicture();
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Layer* root_layer() const
        public Layer root_layer()
        {
            return root_layer_.get();
        }

        public void set_root_layer(Layer root_layer)
        {
            root_layer_ = std::move(root_layer);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const SkISize& frame_size() const
        public SkISize frame_size()
        {
            return frame_size_;
        }

        public void set_frame_size(SkISize frame_size)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: frame_size_ = frame_size;
            frame_size_.CopyFrom(frame_size);
        }

        public void set_construction_time(fml.TimeDelta delta)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: construction_time_ = delta;
            construction_time_.CopyFrom(delta);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const fml::TimeDelta& construction_time() const
        public fml.TimeDelta construction_time()
        {
            return construction_time_;
        }

        // The number of frame intervals missed after which the compositor must
        // trace the rasterized picture to a trace file. Specify 0 to disable all
        // tracing
        public void set_rasterizer_tracing_threshold(uint interval)
        {
            rasterizer_tracing_threshold_ = interval;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint rasterizer_tracing_threshold() const
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

        private SkISize frame_size_ = new SkISize(); // Physical pixels.
        private Layer root_layer_;
        private fml.TimeDelta construction_time_ = new fml.TimeDelta();
        private uint rasterizer_tracing_threshold_;
        private bool checkerboard_raster_cache_images_;
        private bool checkerboard_offscreen_layers_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  LayerTree(const LayerTree&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  LayerTree& operator =(const LayerTree&) = delete;
    }

}