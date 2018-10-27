using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2018 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class PlatformViewLayer : Layer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  PlatformViewLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_offset(SKPoint offset)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = offset;
            offset_ = offset;
        }
        public void set_size(SKSize size)
        {
            size_ = size;
        }
        public void set_view_id(ulong view_id)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: view_id_ = view_id;
            view_id_ = view_id;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            set_paint_bounds(new SKRect(offset_.X, offset_.Y, size_.Width, size_.Height));
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            if (context.view_embedder == null)
            {
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the following C++ macro:
                //!((global::fml.ShouldCreateLogMessage(global::fml.LOG_ERROR))) ? ()0 : new global::fml.LogMessageVoidify() & (new global::fml.LogMessage(global::fml.LOG_ERROR, __FILE__, __LINE__, null).stream()) << "Trying to embed a platform view but the PaintContext " + "does not support embedding";
                return;
            }
            EmbeddedViewParams @params = new EmbeddedViewParams();
            SKMatrix transform = context.canvas.TotalMatrix;
            @params.offsetPixels = new SKPoint(transform.TransX, transform.TransY);
            @params.sizePoints = size_;

            context.view_embedder.CompositeEmbeddedView(view_id_, @params);
        }

        private SKPoint offset_ = new SKPoint();
        private SKSize size_ = new SKSize();
        private ulong view_id_ = new ulong();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PlatformViewLayer(const PlatformViewLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PlatformViewLayer& operator =(const PlatformViewLayer&) = delete;
    }

}