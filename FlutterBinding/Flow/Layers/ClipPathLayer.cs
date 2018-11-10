using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ClipPathLayer : ContainerLayer
    {
        public ClipPathLayer(Clip clip_behavior = Clip.antiAlias)
        {
            this.clip_behavior_ = clip_behavior;
        }

        public void set_clip_path(SKPath clip_path)
        {
            clip_path_ = clip_path;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, matrix, ref child_paint_bounds);

            if (child_paint_bounds.IntersectsWith(clip_path_.Bounds))
            {
                set_paint_bounds(child_paint_bounds);
            }
        }

        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "ClipPathLayer::Paint");
            FML_DCHECK(needs_painting());
            
            context.canvas.ClipPath(clip_path_, antialias: clip_behavior_ != Clip.hardEdge);
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

        private SKPath clip_path_ = new SKPath();
        private Clip clip_behavior_;
    }

}