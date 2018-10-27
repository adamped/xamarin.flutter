using static FlutterBinding.Flow.Helper;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class TextureLayer : Layer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  TextureLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_offset(SKPoint offset)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = offset;
            offset_.CopyFrom(offset);
        }
        public void set_size(SKSize size)
        {
            size_.CopyFrom(size);
        }
        public void set_texture_id(ulong texture_id)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: texture_id_ = texture_id;
            texture_id_.CopyFrom(texture_id);
        }
        public void set_freeze(bool freeze)
        {
            freeze_ = freeze;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            set_paint_bounds(SKRect.MakeXYWH(offset_.x(), offset_.y(), size_.width(), size_.height()));
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            Texture texture = context.texture_registry.GetTexture(new ulong(texture_id_));
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

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TextureLayer(const TextureLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TextureLayer& operator =(const TextureLayer&) = delete;
    }

}