﻿using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class ClipRRectLayer : ContainerLayer
    {
        public ClipRRectLayer(Clip clip_behavior)
        {
            this.clip_behavior_ = clip_behavior;
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_clip_rrect(SKRoundRect clip_rrect)
        {
            clip_rrect_ = clip_rrect;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            SKRect child_paint_bounds = SKRect.Empty;
            PrerollChildren(context, matrix, child_paint_bounds);

            if (child_paint_bounds.IntersectsWith(clip_rrect_.Rect))
            {
                set_paint_bounds(child_paint_bounds);
            }
        }
               
        public override void Paint(PaintContext context)
        {
            TRACE_EVENT0("flutter", "ClipRRectLayer::Paint");
            FML_DCHECK(needs_painting());

            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);
            context.canvas.ClipRoundRect(clip_rrect_, antialias: clip_behavior_ != Clip.hardEdge);
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

        private SKRoundRect clip_rrect_;
        private Clip clip_behavior_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ClipRRectLayer(const ClipRRectLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ClipRRectLayer& operator =(const ClipRRectLayer&) = delete;
    }

}