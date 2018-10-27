﻿// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ClipRectLayer : ContainerLayer
    {
        public ClipRectLayer(Clip clip_behavior)
        {
            this.clip_behavior_ = new flow.Clip(clip_behavior);
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_clip_rect(SkiaSharp.SKRect clip_rect)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: clip_rect_ = clip_rect;
            clip_rect_.CopyFrom(clip_rect);
        }

        public override void Preroll(PrerollContext context, SkMatrix matrix)
        {
            SkiaSharp.SKRect child_paint_bounds = SkiaSharp.SKRect.MakeEmpty();
            PrerollChildren(context, matrix, child_paint_bounds);

            if (child_paint_bounds.intersect(clip_rect_))
            {
                set_paint_bounds(child_paint_bounds);
            }
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "ClipRectLayer::Paint");
            FML_DCHECK(needs_painting());

            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);
            context.canvas.clipRect(paint_bounds(), clip_behavior_ != Clip.hardEdge);
            if (clip_behavior_ == Clip.antiAliasWithSaveLayer)
            {
                context.canvas.saveLayer(paint_bounds(), null);
            }
            PaintChildren(context);
            if (clip_behavior_ == Clip.antiAliasWithSaveLayer)
            {
                context.canvas.restore();
            }
        }

        private SkiaSharp.SKRect clip_rect_ = new SkiaSharp.SKRect();
        private Clip clip_behavior_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ClipRectLayer(const ClipRectLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ClipRectLayer& operator =(const ClipRectLayer&) = delete;
    }

}