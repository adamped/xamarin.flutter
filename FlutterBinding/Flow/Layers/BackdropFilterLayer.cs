﻿using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class BackdropFilterLayer : ContainerLayer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  BackdropFilterLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_filter(SKImageFilter filter)
        {
            filter_ = filter;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "BackdropFilterLayer::Paint");
            FML_DCHECK(needs_painting());

            Layer.AutoSaveLayer save = Layer.AutoSaveLayer.Create(context, SKCanvas.SaveLayerRec(paint_bounds(), null, filter_.get(), 0 ));
            PaintChildren(context);
        }

        private SKImageFilter filter_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  BackdropFilterLayer(const BackdropFilterLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  BackdropFilterLayer& operator =(const BackdropFilterLayer&) = delete;
    }

}
