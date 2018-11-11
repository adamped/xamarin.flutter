using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2018 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class PlatformViewLayer : Layer
    {

        public void set_offset(SKPoint offset)
        {
            offset_ = offset;
        }
        public void set_size(SKSize size)
        {
            size_ = size;
        }
        public void set_view_id(ulong view_id)
        {
            view_id_ = view_id;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            set_paint_bounds(new SKRect(offset_.X, offset_.Y, size_.Width, size_.Height));
        }
        public override void Paint(PaintContext context)
        {
            if (context.view_embedder == null)
            {
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
    }

}