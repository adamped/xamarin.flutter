// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ColorFilterLayer : ContainerLayer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  ColorFilterLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_color(uint color)
        {
            color_ = color;
        }

        public void set_blend_mode(SkBlendMode blend_mode)
        {
            blend_mode_ = blend_mode;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            //TRACE_EVENT0("flutter", "ColorFilterLayer::Paint");
            //FML_DCHECK(needs_painting());

            var color_filter = SkiaSharp.SKColorFilter.CreateBlendMode(color_, blend_mode_);
            SkiaSharp.SKPaint paint = new SkiaSharp.SKPaint();
            paint.setColorFilter(std::move(color_filter));

            Layer.AutoSaveLayer save = Layer.AutoSaveLayer.Create(context, paint_bounds(), paint);
            PaintChildren(context);
        }

        private uint color_;
        private SkBlendMode blend_mode_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ColorFilterLayer(const ColorFilterLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ColorFilterLayer& operator =(const ColorFilterLayer&) = delete;
    }

}