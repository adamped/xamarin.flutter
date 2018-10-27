// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class PhysicalShapeLayer : ContainerLayer
    {
        public PhysicalShapeLayer(Clip clip_behavior)
        {
            this.isRect_ = false;
            this.clip_behavior_ = new flow.Clip(clip_behavior);
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_path(SkPath path)
        {
            path_.CopyFrom(path);
            isRect_ = false;
            SkiaSharp.SKRect rect = new SkiaSharp.SKRect();
            if (path.isRect(rect))
            {
                isRect_ = true;
                frameRRect_.CopyFrom(SkRRect.MakeRect(rect));
            }
            else if (path.isRRect(frameRRect_))
            {
                isRect_ = frameRRect_.isRect();
            }
            else if (path.isOval(rect))
            {
                // isRRect returns false for ovals, so we need to explicitly check isOval
                // as well.
                frameRRect_.CopyFrom(SkRRect.MakeOval(rect));
            }
            else
            {
                // Scenic currently doesn't provide an easy way to create shapes from
                // arbitrary paths.
                // For shapes that cannot be represented as a rounded rectangle we
                // default to use the bounding rectangle.
                // TODO(amirh): fix this once we have a way to create a Scenic shape from
                // an SkPath.
                frameRRect_.CopyFrom(SkRRect.MakeRect(path.getBounds()));
            }
        }

        public void set_elevation(float elevation)
        {
            elevation_ = elevation;
        }
        public void set_color(uint color)
        {
            color_ = color;
        }
        public void set_shadow_color(uint shadow_color)
        {
            shadow_color_ = shadow_color;
        }
        public void set_device_pixel_ratio(float dpr)
        {
            device_pixel_ratio_ = dpr;
        }

        public static void DrawShadow(SkiaSharp.SKCanvas canvas, SkPath path, uint color, float elevation, bool transparentOccluder, float dpr)
        {
            const float kAmbientAlpha = 0.039f;
            const float kSpotAlpha = 0.25f;
            const float kLightHeight = 600F;
            const float kLightRadius = 800F;

            SkShadowFlags flags = transparentOccluder ? SkShadowFlags.kTransparentOccluder_ShadowFlag : SkShadowFlags.kNone_ShadowFlag;
            SkiaSharp.SKRect bounds = path.getBounds();
            float shadow_x = (bounds.left() + bounds.right()) / 2;
            float shadow_y = bounds.top() - 600.0f;
            uint inAmbient = GlobalMembers.SkColorSetA(color, kAmbientAlpha * (((color) >> 24) & 0xFF));
            uint inSpot = GlobalMembers.SkColorSetA(color, kSpotAlpha * (((color) >> 24) & 0xFF));
            uint ambientColor;
            uint spotColor;
            SkShadowUtils.ComputeTonalColors(inAmbient, inSpot, ref ambientColor, ref spotColor);
            SkShadowUtils.DrawShadow(canvas, path, SkPoint3.Make(0, 0, dpr * elevation), SkPoint3.Make(shadow_x, shadow_y, dpr * kLightHeight), dpr * kLightRadius, ambientColor, spotColor, flags);
        }

        public override void Preroll(PrerollContext context, SkMatrix matrix)
        {
            SkiaSharp.SKRect child_paint_bounds = new SkiaSharp.SKRect();
            PrerollChildren(context, matrix, child_paint_bounds);

            if (elevation_ == 0F)
            {
                set_paint_bounds(path_.getBounds());
            }
            else
            {
#if OS_FUCHSIA
	  // Let the system compositor draw all shadows for us.
	  set_needs_system_composite(true);
#else
                // Add some margin to the paint bounds to leave space for the shadow.
                // The margin is hardcoded to an arbitrary maximum for now because Skia
                // doesn't provide a way to calculate it.  We fill this whole region
                // and clip children to it so we don't need to join the child paint bounds.
                SkiaSharp.SKRect bounds = new SkiaSharp.SKRect(path_.getBounds());
                bounds.outset(20.0, 20.0);
                set_paint_bounds(bounds);
#endif
            }
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "PhysicalShapeLayer::Paint");
            FML_DCHECK(needs_painting());

            if (elevation_ != 0F)
            {
                DrawShadow(context.canvas, path_, shadow_color_, elevation_, (((color_) >> 24) & 0xFF) != 0xff, device_pixel_ratio_);
            }

            // Call drawPath without clip if possible for better performance.
            SkiaSharp.SKPaint paint = new SkiaSharp.SKPaint();
            paint.setColor(color_);
            if (clip_behavior_ != Clip.antiAliasWithSaveLayer)
            {
                context.canvas.drawPath(path_, paint);
            }

            int saveCount = context.canvas.save();
            switch (clip_behavior_)
            {
                case Clip.hardEdge:
                    context.canvas.clipPath(path_, false);
                    break;
                case Clip.antiAlias:
                    context.canvas.clipPath(path_, true);
                    break;
                case Clip.antiAliasWithSaveLayer:
                    context.canvas.clipPath(path_, true);
                    context.canvas.saveLayer(paint_bounds(), null);
                    break;
                case Clip.none:
                    break;
            }

            if (clip_behavior_ == Clip.antiAliasWithSaveLayer)
            {
                // If we want to avoid the bleeding edge artifact
                // (https://github.com/flutter/flutter/issues/18057#issue-328003931)
                // using saveLayer, we have to call drawPaint instead of drawPath as
                // anti-aliased drawPath will always have such artifacts.
                context.canvas.drawPaint(paint);
            }

            PaintChildren(context);

            context.canvas.restoreToCount(saveCount);
        }


        private float elevation_;
        private uint color_;
        private uint shadow_color_;
        private float device_pixel_ratio_;
        private SkPath path_ = new SkPath();
        private bool isRect_;
        private SkRRect frameRRect_ = new SkRRect();
        private Clip clip_behavior_;
    }

}