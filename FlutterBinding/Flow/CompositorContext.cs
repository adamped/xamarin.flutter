using FlutterBinding.Flow.Layers;
using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{
    public class CompositorContext
    {
        public class ScopedFrame : System.IDisposable
        {
            public ScopedFrame(CompositorContext context, GRContext gr_context, SKCanvas canvas, ExternalViewEmbedder view_embedder, SKMatrix root_surface_transformation, bool instrumentation_enabled)
            {
                this.context_ = context;
                this.gr_context_ = gr_context;
                this.canvas_ = canvas;
                this.view_embedder_ = view_embedder;
                this.root_surface_transformation_ = root_surface_transformation;
                this.instrumentation_enabled_ = instrumentation_enabled;
                context_.BeginFrame(this, instrumentation_enabled_);
            }

            public virtual void Dispose()
            {
                context_.EndFrame(this, instrumentation_enabled_);
            }

            public SKCanvas canvas()
            {
                return canvas_;
            }

            public ExternalViewEmbedder view_embedder()
            {
                return view_embedder_;
            }

            public CompositorContext context()
            {
                return context_;
            }

            public SKMatrix root_surface_transformation()
            {
                return root_surface_transformation_;
            }

            public GRContext gr_context()
            {
                return gr_context_;
            }

            public virtual bool Raster(LayerTree layer_tree, bool ignore_raster_cache)
            {
                layer_tree.Preroll(this, ignore_raster_cache);
                layer_tree.Paint(this, ignore_raster_cache);
                return true;
            }

            private CompositorContext context_;
            private GRContext gr_context_;
            private SKCanvas canvas_;
            private ExternalViewEmbedder view_embedder_;
            private readonly SKMatrix root_surface_transformation_;
            private readonly bool instrumentation_enabled_;
        }

        public virtual ScopedFrame AcquireFrame(GRContext gr_context, SKCanvas canvas, ExternalViewEmbedder view_embedder, SKMatrix root_surface_transformation, bool instrumentation_enabled)
        {
            return new ScopedFrame(this, gr_context, canvas, view_embedder, root_surface_transformation, instrumentation_enabled);
        }

        public void OnGRContextCreated()
        {
            texture_registry_.OnGRContextCreated();
            raster_cache_.Clear();
        }

        public void OnGRContextDestroyed()
        {
            texture_registry_.OnGRContextDestroyed();
            raster_cache_.Clear();
        }

        public RasterCache raster_cache()
        {
            return raster_cache_;
        }

        public TextureRegistry texture_registry()
        {
            return texture_registry_;
        }

        private RasterCache raster_cache_ = new RasterCache();
        private TextureRegistry texture_registry_ = new TextureRegistry();

        private void BeginFrame(ScopedFrame frame, bool enable_instrumentation)
        {
        }

        private void EndFrame(ScopedFrame frame, bool enable_instrumentation)
        {
            raster_cache_.SweepAfterFrame();
        }
    }
}