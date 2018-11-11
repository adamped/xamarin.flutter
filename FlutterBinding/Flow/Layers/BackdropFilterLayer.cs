using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class BackdropFilterLayer : ContainerLayer
    {

        public void set_filter(SKImageFilter filter)
        {
            filter_ = filter;
        }
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "BackdropFilterLayer::Paint");
            FML_DCHECK(needs_painting());
          
            Layer.AutoSaveLayer save = Layer.AutoSaveLayer.Create(context, paint_bounds(), new SKPaint() { ImageFilter = filter_ }); //, null, filter_, 0);
            PaintChildren(context);
        }

        private SKImageFilter filter_;
    }

}
