using static FlutterBinding.Flow.Helper;
using SkiaSharp;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class TextureLayer : Layer
    {

        public void set_offset(SKPoint offset)
        {
            offset_ = offset;
        }
        public void set_size(SKSize size)
        {
            size_ = size;
        }
        public void set_texture_id(ulong texture_id)
        {
            texture_id_ = texture_id;
        }
        public void set_freeze(bool freeze)
        {
            freeze_ = freeze;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            set_paint_bounds(new SKRect(offset_.X, offset_.Y, size_.Width, size_.Height));
        }
        public override void Paint(PaintContext context)
        {
            Texture texture = context.texture_registry.GetTexture(texture_id_);
            if (texture == null)
            {
                return;
            }
            texture.Paint(context.canvas, paint_bounds(), freeze_);
        }

        private SKPoint offset_ = new SKPoint();
        private SKSize size_ = new SKSize();
        private ulong texture_id_ = new ulong();
        private bool freeze_;
    }

}