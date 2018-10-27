using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class LayerTree;

    public class CompositorContext : System.IDisposable
    {
        public class ScopedFrame : System.IDisposable
        {
            public ScopedFrame(CompositorContext context, GRContext gr_context, SKCanvas canvas, ExternalViewEmbedder view_embedder, SKMatrix root_surface_transformation, bool instrumentation_enabled)
            {
                this.context_ = new CompositorContext(context);
                this.gr_context_ = gr_context;
                this.canvas_ = canvas;
                this.view_embedder_ = view_embedder;
                this.root_surface_transformation_ = new SKMatrix(root_surface_transformation);
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

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: CompositorContext& context() const
            public CompositorContext context()
            {
                return context_;
            }

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: const SKMatrix& root_surface_transformation() const
            public SKMatrix root_surface_transformation()
            {
                return root_surface_transformation_;
            }

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: GRContext* gr_context() const
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

            //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
            //	ScopedFrame(const ScopedFrame&) = delete;
            //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
            //	ScopedFrame& operator =(const ScopedFrame&) = delete;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  CompositorContext();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public virtual std::unique_ptr<CompositorContext.ScopedFrame> AcquireFrame(GRContext gr_context, SKCanvas canvas, ExternalViewEmbedder view_embedder, SKMatrix root_surface_transformation, bool instrumentation_enabled)
        {
            return std::make_unique<ScopedFrame>(this, gr_context, canvas, view_embedder, root_surface_transformation, instrumentation_enabled);
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

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const Counter& frame_count() const
        public Counter frame_count()
        {
            return frame_count_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const Stopwatch& frame_time() const
        public Stopwatch frame_time()
        {
            return frame_time_;
        }

        public Stopwatch engine_time()
        {
            return engine_time_;
        }

        private RasterCache raster_cache_ = new RasterCache();
        private TextureRegistry texture_registry_ = new TextureRegistry();
        private Counter frame_count_ = new Counter();
        private Stopwatch frame_time_ = new Stopwatch();
        private Stopwatch engine_time_ = new Stopwatch();

        private void BeginFrame(ScopedFrame frame, bool enable_instrumentation)
        {
            if (enable_instrumentation)
            {
                frame_count_.Increment();
                frame_time_.Start();
            }
        }

        private void EndFrame(ScopedFrame frame, bool enable_instrumentation)
        {
            raster_cache_.SweepAfterFrame();
            if (enable_instrumentation)
            {
                frame_time_.Stop();
            }
        }

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  CompositorContext(const CompositorContext&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  CompositorContext& operator =(const CompositorContext&) = delete;
    }

}