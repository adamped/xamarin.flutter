// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    public class PerformanceOverlayLayer : Layer
    {
        public PerformanceOverlayLayer(ulong options)
        {
            this.options_ = options;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            const int padding = 8;

            if (options_ == 0)
            {
                return;
            }

            TRACE_EVENT0("flutter", "PerformanceOverlayLayer::Paint");
            float x = paint_bounds().x() + padding;
            float y = paint_bounds().y() + padding;
            float width = paint_bounds().width() - (padding * 2);
            float height = paint_bounds().height() / 2;
            //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
            //  (...) static_assert(false, "missing name for " "SkAutoCanvasRestore") save(&context.canvas, true);

            GlobalMembers.VisualizeStopWatch(context.canvas, context.frame_time, x, y, width, height - padding, options_ & GlobalMembers.kVisualizeRasterizerStatistics, options_ & GlobalMembers.kDisplayRasterizerStatistics, "GPU");

            GlobalMembers.VisualizeStopWatch(context.canvas, context.engine_time, x, y + height, width, height - padding, options_ & GlobalMembers.kVisualizeEngineStatistics, options_ & GlobalMembers.kDisplayEngineStatistics, "UI");
        }

        private int options_;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PerformanceOverlayLayer(const PerformanceOverlayLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PerformanceOverlayLayer& operator =(const PerformanceOverlayLayer&) = delete;
    }

}