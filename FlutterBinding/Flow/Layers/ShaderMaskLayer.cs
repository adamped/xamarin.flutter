// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ShaderMaskLayer : ContainerLayer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  ShaderMaskLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_shader(sk_sp<SkShader> shader)
        {
            shader_.CopyFrom(shader);
        }

        public void set_mask_rect(SkiaSharp.SKRect mask_rect)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: mask_rect_ = mask_rect;
            mask_rect_.CopyFrom(mask_rect);
        }

        public void set_blend_mode(SkBlendMode blend_mode)
        {
            blend_mode_ = blend_mode;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "ShaderMaskLayer::Paint");
            FML_DCHECK(needs_painting());

            Layer.AutoSaveLayer save = Layer.AutoSaveLayer.Create(context, paint_bounds(), null);
            PaintChildren(context);

            SkiaSharp.SKPaint paint = new SkiaSharp.SKPaint();
            paint.setBlendMode(blend_mode_);
            paint.setShader(new sk_sp<SkShader>(shader_));
            context.canvas.translate(mask_rect_.left(), mask_rect_.top());
            context.canvas.drawRect(SkiaSharp.SKRect.MakeWH(mask_rect_.width(), mask_rect_.height()), paint);
        }

        private sk_sp<SkShader> shader_ = new sk_sp<SkShader>();
        private SkiaSharp.SKRect mask_rect_ = new SkiaSharp.SKRect();
        private SkBlendMode blend_mode_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ShaderMaskLayer(const ShaderMaskLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ShaderMaskLayer& operator =(const ShaderMaskLayer&) = delete;
    }

}