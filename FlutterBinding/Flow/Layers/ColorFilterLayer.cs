using SkiaSharp;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ColorFilterLayer : ContainerLayer
    {

        public void set_color(uint color)
        {
            color_ = color;
        }

        public void set_blend_mode(SKBlendMode blend_mode)
        {
            blend_mode_ = blend_mode;
        }
        
        public override void Paint(PaintContext context)
        {

            var color_filter = SKColorFilter.CreateBlendMode(color_, blend_mode_);
            SKPaint paint = new SKPaint();
            paint.ColorFilter = color_filter;

            Layer.AutoSaveLayer save = Layer.AutoSaveLayer.Create(context, paint_bounds(), paint);
            PaintChildren(context);
        }

        private uint color_;
        private SKBlendMode blend_mode_;
    }

}