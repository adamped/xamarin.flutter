using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ShaderMaskLayer : ContainerLayer
    {

        public void set_shader(SKShader shader)
        {
            shader_ = shader;
        }

        public void set_mask_rect(SKRect mask_rect)
        {
            mask_rect_ = mask_rect;
        }

        public void set_blend_mode(SKBlendMode blend_mode)
        {
            blend_mode_ = blend_mode;
        }
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "ShaderMaskLayer::Paint");
            FML_DCHECK(needs_painting());

            Layer.AutoSaveLayer save = Layer.AutoSaveLayer.Create(context, paint_bounds(), null);
            PaintChildren(context);

            SKPaint paint = new SKPaint();
            paint.BlendMode = blend_mode_;
            paint.Shader = shader_;
            context.canvas.Translate(mask_rect_.Left, mask_rect_.Top);
            context.canvas.DrawRect(new SKRect(0, 0, mask_rect_.Width, mask_rect_.Height), paint);
        }

        private SKShader shader_;
        private SKRect mask_rect_;
        private SKBlendMode blend_mode_;
    }

}