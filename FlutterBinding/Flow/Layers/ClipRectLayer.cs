using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ClipRectLayer : ContainerLayer
    {
        public ClipRectLayer(Clip clip_behavior)
        {
            this.clip_behavior_ = clip_behavior;
        }

        public void set_clip_rect(SKRect clip_rect)
        {
            clip_rect_=clip_rect;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, matrix, ref child_paint_bounds);

            if (child_paint_bounds.IntersectsWith(clip_rect_))
            {
                set_paint_bounds(child_paint_bounds);
            }
        }
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "ClipRectLayer::Paint");
            FML_DCHECK(needs_painting());

            context.canvas.ClipRect(paint_bounds(), antialias: clip_behavior_ != Clip.hardEdge);
            if (clip_behavior_ == Clip.antiAliasWithSaveLayer)
            {
                context.canvas.SaveLayer(paint_bounds(), null);
            }
            PaintChildren(context);
            if (clip_behavior_ == Clip.antiAliasWithSaveLayer)
            {
                context.canvas.Restore();
            }
        }

        private SKRect clip_rect_ = new SKRect();
        private Clip clip_behavior_;
    }

}