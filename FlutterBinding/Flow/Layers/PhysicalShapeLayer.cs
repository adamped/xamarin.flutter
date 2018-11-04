using SkiaSharp;
using static FlutterBinding.Flow.Helper;

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
            this.clip_behavior_ = clip_behavior;
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_path(SKPath path)
        {
            
            path_ = path;
            isRect_ = false;
            SKRect rect = new SKRect();
            //if (path.isRect(rect))
            //{
            //    isRect_ = true;
            //    frameRRect_ = new SKRoundRect(rect);
            //}
            //else if (SKPath.path.isRRect(frameRRect_))
            //{
            //    isRect_ = frameRRect_.isRect();
            //}
            //else if (path.isOval(rect))
            //{
            //    // isRRect returns false for ovals, so we need to explicitly check isOval
            //    // as well.
            //    frameRRect_ = new SKRoundRect(rect);
            //}
            //else
            //{
                // Scenic currently doesn't provide an easy way to create shapes from
                // arbitrary paths.
                // For shapes that cannot be represented as a rounded rectangle we
                // default to use the bounding rectangle.
                // TODO(amirh): fix this once we have a way to create a Scenic shape from
                // an SKPath.
                frameRRect_ = new SKRoundRect(path.Bounds);
            //}
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

        public static void DrawShadow(SKCanvas canvas, SKPath path, uint color, float elevation, bool transparentOccluder, float dpr)
        {
            //const float kAmbientAlpha = 0.039f;
            //const float kSpotAlpha = 0.25f;
            //const float kLightHeight = 600F;
            //const float kLightRadius = 800F;

            //SkShadowFlags flags = transparentOccluder ? SkShadowFlags.kTransparentOccluder_ShadowFlag : SkShadowFlags.kNone_ShadowFlag;
            //SKRect bounds = path.Bounds;
            //float shadow_x = (bounds.Left + bounds.Right) / 2;
            //float shadow_y = bounds.Top - 600.0f;
            //uint inAmbient = GlobalMembers.SkColorSetA(color, kAmbientAlpha * (((color) >> 24) & 0xFF));
            //uint inSpot = GlobalMembers.SkColorSetA(color, kSpotAlpha * (((color) >> 24) & 0xFF));
            //uint ambientColor;
            //uint spotColor;
            //SkShadowUtils.ComputeTonalColors(inAmbient, inSpot, ref ambientColor, ref spotColor);
            //SkShadowUtils.DrawShadow(canvas, path, SKPoint3.Make(0, 0, dpr * elevation), SKPoint3.Make(shadow_x, shadow_y, dpr * kLightHeight), dpr * kLightRadius, ambientColor, spotColor, flags);
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKRect child_paint_bounds = new SKRect();
            PrerollChildren(context, matrix, child_paint_bounds);

            if (elevation_ == 0F)
            {
                set_paint_bounds(path_.Bounds);
            }
            else
            {
                // Add some margin to the paint bounds to leave space for the shadow.
                // The margin is hardcoded to an arbitrary maximum for now because Skia
                // doesn't provide a way to calculate it.  We fill this whole region
                // and clip children to it so we don't need to join the child paint bounds.
                SKRect bounds = path_.Bounds;
                bounds.Offset(20.0f, 20.0f);
                set_paint_bounds(bounds);
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
            SKPaint paint = new SKPaint();
            paint.Color = color_;
            if (clip_behavior_ != Clip.antiAliasWithSaveLayer)
            {
                context.canvas.DrawPath(path_, paint);
            }

            int saveCount = context.canvas.Save();
            switch (clip_behavior_)
            {
                case Clip.hardEdge:
                    context.canvas.ClipPath(path_, antialias: false);
                    break;
                case Clip.antiAlias:
                    context.canvas.ClipPath(path_, antialias: true);
                    break;
                case Clip.antiAliasWithSaveLayer:
                    context.canvas.ClipPath(path_, antialias: true);
                    context.canvas.SaveLayer(paint_bounds(), null);
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
                context.canvas.DrawPaint(paint);
            }

            PaintChildren(context);

            context.canvas.RestoreToCount(saveCount);
        }


        private float elevation_;
        private uint color_;
        private uint shadow_color_;
        private float device_pixel_ratio_;
        private SKPath path_ = new SKPath();
        private bool isRect_;
        private SKRoundRect frameRRect_ = new SKRoundRect();
        private Clip clip_behavior_;
    }

}