using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    // This should be an exact copy of the Clip enum in painting.dart.
    public enum Clip
    {
        none,
        hardEdge,
        antiAlias,
        antiAliasWithSaveLayer
    }

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class ContainerLayer;

    public class PrerollContext
    {
        public RasterCache raster_cache;
        public GRContext gr_context;
        public SKColorSpace dst_color_space;
        public SKRect child_paint_bounds = new SKRect();

        // The following allows us to paint in the end of subtree preroll
        public readonly Stopwatch frame_time;
        public readonly Stopwatch engine_time;
        public TextureRegistry texture_registry;
        public readonly bool checkerboard_offscreen_layers;
    }

    // Represents a single composited layer. Created on the UI thread but then
    // subquently used on the Rasterizer thread.
    public abstract class Layer : System.IDisposable
    {
        public Layer()
        {
            this.parent_ = null;
            this.needs_system_composite_ = false;
            this.paint_bounds_ = new SKRect(SKRect.MakeEmpty());
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public virtual void Preroll(PrerollContext context, SKMatrix matrix)
        {
        }

        public class PaintContext
        {
            public SKCanvas canvas;
            public ExternalViewEmbedder view_embedder;
            public readonly Stopwatch frame_time;
            public readonly Stopwatch engine_time;
            public TextureRegistry texture_registry;
            public readonly RasterCache raster_cache;
            public readonly bool checkerboard_offscreen_layers;
        }

        // Calls SKCanvas::saveLayer and restores the layer upon destruction. Also
        // draws a checkerboard over the layer if that is enabled in the PaintContext.
        public class AutoSaveLayer : System.IDisposable
        {
            public static Layer.AutoSaveLayer Create(PaintContext paint_context, SKRect bounds, SKPaint paint)
            {
                return new Layer.AutoSaveLayer(paint_context, bounds, paint);
            }

            public static Layer.AutoSaveLayer Create(PaintContext paint_context, SKCanvas.SaveLayerRec layer_rec)
            {
                return new Layer.AutoSaveLayer(paint_context, layer_rec);
            }

            public void Dispose()
            {
                if (paint_context_.checkerboard_offscreen_layers)
                {
                    DrawCheckerboard(paint_context_.canvas, bounds_);
                }
                paint_context_.canvas.restore();
            }

            private AutoSaveLayer(PaintContext paint_context, SKRect bounds, SKPaint paint)
            {
                this.paint_context_ = new Layer.PaintContext(paint_context);
                this.bounds_ = new SKRect(bounds);
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: paint_context_.canvas.saveLayer(bounds_, paint);
                paint_context_.canvas.saveLayer(new SKRect(bounds_), paint);
            }

            private AutoSaveLayer(PaintContext paint_context, SKCanvas.SaveLayerRec layer_rec)
            {
                this.paint_context_ = new Layer.PaintContext(paint_context);
                this.bounds_ = new SKRect(layer_rec.fBounds);
                paint_context_.canvas.saveLayer(layer_rec);
            }

            private readonly PaintContext paint_context_;
            private readonly SKRect bounds_ = new SKRect();
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual void Paint(PaintContext& context) const = 0;
        public abstract void Paint(PaintContext context);

#if OS_FUCHSIA
  // Updates the system composited scene.
#if OS_FUCHSIA
  public virtual void UpdateScene(SceneUpdateContext context)
  {
  }
#endif
#endif

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: ContainerLayer* parent() const
        public ContainerLayer parent()
        {
            return parent_;
        }

        public void set_parent(ContainerLayer parent)
        {
            parent_ = parent;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool needs_system_composite() const
        public bool needs_system_composite()
        {
            return needs_system_composite_;
        }
        public void set_needs_system_composite(bool value)
        {
            needs_system_composite_ = value;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const SKRect& paint_bounds() const
        public SKRect paint_bounds()
        {
            return paint_bounds_;
        }

        // This must be set by the time Preroll() returns otherwise the layer will
        // be assumed to have empty paint bounds (paints no content).
        public void set_paint_bounds(SKRect paint_bounds)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: paint_bounds_ = paint_bounds;
            paint_bounds_.CopyFrom(paint_bounds);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool needs_painting() const
        public bool needs_painting()
        {
            return !paint_bounds_.isEmpty();
        }

        private ContainerLayer parent_;
        private bool needs_system_composite_;
        private SKRect paint_bounds_ = new SKRect();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Layer(const Layer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Layer& operator =(const Layer&) = delete;
    }

}