using System;
using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;
using SkiaSharp;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    //public class Stopwatch //: System.IDisposable
    //{
    //    public Stopwatch()
    //    {
    //        this.start_ = new fml.TimePoint(fml.TimePoint.Now());
    //        this.current_sample_ = 0;
    //        fml.TimeDelta delta = fml.TimeDelta.Zero();
    //        laps_.Resize(GlobalMembers.kMaxSamples, delta);
    //        cache_dirty_ = true;
    //        prev_drawn_sample_index_ = 0;
    //    }

    //    //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
    //    //  public void Dispose();

    //    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //    //ORIGINAL LINE: const fml::TimeDelta& LastLap() const
    //    public fml.TimeDelta LastLap()
    //    {
    //        return laps_[(current_sample_ - 1) % GlobalMembers.kMaxSamples];
    //    }

    //    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //    //ORIGINAL LINE: fml::TimeDelta CurrentLap() const
    //    public fml.TimeDelta CurrentLap()
    //    {
    //        return fml.TimePoint.Now() - start_;
    //    }

    //    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //    //ORIGINAL LINE: fml::TimeDelta MaxDelta() const
    //    public fml.TimeDelta MaxDelta()
    //    {
    //        fml.TimeDelta max_delta = new fml.TimeDelta();
    //        for (int i = 0; i < GlobalMembers.kMaxSamples; i++)
    //        {
    //            if (laps_[i] > max_delta)
    //            {
    //                max_delta = laps_[i];
    //            }
    //        }
    //        return max_delta;
    //    }


    //    // Initialize the SKSurface for drawing into. Draws the base background and any
    //    // timing data from before the initial Visualize() call.
    //    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //    //ORIGINAL LINE: void InitVisualizeSurface(const SKRect& rect) const
    //    public void InitVisualizeSurface(SKRect rect)
    //    {
    //        if (!cache_dirty_)
    //        {
    //            return;
    //        }
    //        cache_dirty_ = false;

    //        // TODO(garyq): Use a GPU surface instead of a CPU surface.
    //        visualize_cache_surface_.CopyFrom(SKSurface.MakeRasterN32Premul(rect.width(), rect.height()));

    //        SKCanvas cache_canvas = visualize_cache_surface_.Dereference().getCanvas();

    //        // Establish the graph position.
    //        const float x = 0F;
    //        const float y = 0F;
    //        float width = rect.Width;
    //        float height = rect.Height;

    //        SKPaint paint = new SKPaint();
    //        paint.Color = 0x99FFFFFF;
    //        cache_canvas.DrawRect(new SKRect(x, y, x + width, y + height), paint);

    //        // Scale the graph to show frame times up to those that are 3 times the frame
    //        // time.
    //        double max_interval = GlobalMembers.kOneFrameMS * 3.0;
    //        double max_unit_interval = GlobalMembers.UnitFrameInterval(max_interval);

    //        // Draw the old data to initially populate the graph.
    //        // Prepare a path for the data. We start at the height of the last point, so
    //        // it looks like we wrap around
    //        SKPath path = new SKPath();
    //        path.setIsVolatile(true);
    //        path.MoveTo(x, height);
    //        path.LineTo(x, y + height * (1.0 - GlobalMembers.UnitHeight(laps_[0].ToMillisecondsF(), max_unit_interval)));
    //        double unit_x;
    //        double unit_next_x = 0.0;
    //        for (int i = 0; i < GlobalMembers.kMaxSamples; i += 1)
    //        {
    //            unit_x = unit_next_x;
    //            unit_next_x = ((double)(i + 1) / GlobalMembers.kMaxSamples);
    //            double sample_y = y + height * (1.0 - GlobalMembers.UnitHeight(laps_[i].ToMillisecondsF(), max_unit_interval));
    //            path.LineTo((float)(x + width * unit_x), (float)sample_y);
    //            path.LineTo((float)(x + width * unit_next_x), (float)sample_y);
    //        }
    //        path.LineTo(width, y + height * (1.0 - GlobalMembers.UnitHeight(laps_[GlobalMembers.kMaxSamples - 1].ToMillisecondsF(), max_unit_interval)));
    //        path.LineTo(width, height);
    //        path.Close();

    //        // Draw the graph.
    //        paint.Color = 0xAA0000FF;
    //        cache_canvas.DrawPath(path, paint);
    //    }

    //    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //    //ORIGINAL LINE: void Visualize(SKCanvas& canvas, const SKRect& rect) const
    //    public void Visualize(SKCanvas canvas, SKRect rect)
    //    {
    //        // Initialize visualize cache if it has not yet been initialized.
    //        InitVisualizeSurface(rect);

    //        SKCanvas cache_canvas = visualize_cache_surface_.Dereference().getCanvas();
    //        SKPaint paint = new SKPaint();

    //        // Establish the graph position.
    //        const float x = 0F;
    //        const float y = 0F;
    //        float width = rect.Width;
    //        float height = rect.Height;

    //        // Scale the graph to show frame times up to those that are 3 times the frame
    //        // time.
    //        double max_interval = GlobalMembers.kOneFrameMS * 3.0;
    //        double max_unit_interval = GlobalMembers.UnitFrameInterval(max_interval);

    //        double sample_unit_width = (1.0 / GlobalMembers.kMaxSamples);

    //        // Draw vertical replacement bar to erase old/stale pixels.
    //        paint.Color = 0x99FFFFFF;
    //        paint.Style = SKPaintStyle.Fill;
    //        paint.BlendMode = SKBlendMode.Src;
    //        double sample_x = x + width * ((double)prev_drawn_sample_index_ / GlobalMembers.kMaxSamples);
    //        var eraser_rect = new SKRect((float)sample_x, y, (float)(sample_x + width * sample_unit_width), height);
    //        cache_canvas.DrawRect(eraser_rect, paint);

    //        // Draws blue timing bar for new data.
    //        paint.Color = 0xAA0000FF;
    //        paint.BlendMode = SKBlendMode.SrcOver;
    //        var bar_rect = new SKRect((float)sample_x, y + height * (1.0 - GlobalMembers.UnitHeight(laps_[current_sample_ == 0 ? GlobalMembers.kMaxSamples - 1 : current_sample_ - 1].ToMillisecondsF(), max_unit_interval)), (float)(sample_x + width * sample_unit_width), height);
    //        cache_canvas.DrawRect(bar_rect, paint);

    //        // Draw horizontal frame markers.
    //        paint.StrokeWidth = 0F; // hairline
    //        paint.Style = SKPaintStyle.Stroke;
    //        paint.Color = 0xCC000000;

    //        if (max_interval > GlobalMembers.kOneFrameMS)
    //        {
    //            // Paint the horizontal markers
    //            int frame_marker_count = (int)(max_interval / GlobalMembers.kOneFrameMS);

    //            // Limit the number of markers displayed. After a certain point, the graph
    //            // becomes crowded
    //            if (frame_marker_count > GlobalMembers.kMaxFrameMarkers)
    //            {
    //                frame_marker_count = 1;
    //            }

    //            for (int frame_index = 0; frame_index < frame_marker_count; frame_index++)
    //            {
    //                double frame_height = height * (1.0 - (GlobalMembers.UnitFrameInterval((frame_index + 1) * GlobalMembers.kOneFrameMS) / max_unit_interval));
    //                cache_canvas.DrawLine(x, (float)(y + frame_height), width, (float)(y + frame_height), paint);
    //            }
    //        }

    //        // Paint the vertical marker for the current frame.
    //        // We paint it over the current frame, not after it, because when we
    //        // paint this we don't yet have all the times for the current frame.
    //        paint.Style = SKPaintStyle.Fill;
    //        paint.BlendMode = SKBlendMode.SrcOver;
    //        if (GlobalMembers.UnitFrameInterval(LastLap().ToMillisecondsF()) > 1.0)
    //        {
    //            // budget exceeded
    //            paint.Color = GlobalMembers.SK_ColorRED;
    //        }
    //        else
    //        {
    //            // within budget
    //            paint.Color = GlobalMembers.SK_ColorGREEN;
    //        }
    //        sample_x = x + width * ((double)current_sample_ / GlobalMembers.kMaxSamples);
    //        var marker_rect = new SKRect((float)sample_x, y, (float)(sample_x + width * sample_unit_width), height);
    //        cache_canvas.DrawRect(marker_rect, paint);
    //        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
    //        //ORIGINAL LINE: prev_drawn_sample_index_ = current_sample_;
    //        prev_drawn_sample_index_ = current_sample_;

    //        // Draw the cached surface onto the output canvas.
    //        paint.Reset();
    //        visualize_cache_surface_.Dereference().draw(canvas, rect.Left, rect.Top, paint);
    //    }

    //    public void Start()
    //    {
    //        start_ = fml.TimePoint.Now();
    //        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
    //        //ORIGINAL LINE: current_sample_ = (current_sample_ + 1) % kMaxSamples;
    //        current_sample_ = (current_sample_ + 1) % GlobalMembers.kMaxSamples;
    //    }

    //    public void Stop()
    //    {
    //        laps_[current_sample_] = fml.TimePoint.Now() - start_;
    //    }

    //    public void SetLapTime(fml.TimeDelta delta)
    //    {
    //        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
    //        //ORIGINAL LINE: current_sample_ = (current_sample_ + 1) % kMaxSamples;
    //        current_sample_.CopyFrom((current_sample_ + 1) % GlobalMembers.kMaxSamples);
    //        laps_[current_sample_] = delta;
    //    }

    //    private fml.TimePoint start_ = new fml.TimePoint();
    //    private List<fml.TimeDelta> laps_ = new List<fml.TimeDelta>();
    //    private int current_sample_ = new int();
    //    // Mutable data cache for performance optimization of the graphs. Prevents
    //    // expensive redrawing of old data.
    //    private bool cache_dirty_;
    //    private SKSurface visualize_cache_surface_;
    //    private int prev_drawn_sample_index_ = new int();

    //    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
    //    //  Stopwatch(const Stopwatch&) = delete;
    //    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
    //    //  Stopwatch& operator =(const Stopwatch&) = delete;
    //}

    public class Counter
    {
        public Counter()
        {
            this.count_ = 0;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int count() const
        public int count()
        {
            return count_;
        }

        public void Reset(int count = 0)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: count_ = count;
            count_ = count;
        }

        public void Increment(int count = 1)
        {
            count_ += count;
        }

        private int count_ = new int();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Counter(const Counter&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Counter& operator =(const Counter&) = delete;
    }

    public partial class CounterValues //: System.IDisposable
    {
        public CounterValues()
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.current_sample_ = kMaxSamples - 1;
            this.current_sample_ = GlobalMembers.kMaxSamples - 1;
            values_.Resize(GlobalMembers.kMaxSamples, (ulong)0);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  void Add(ulong value);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Visualize(SKCanvas& canvas, const SKRect& rect) const
        //public void Visualize(SKCanvas canvas, SKRect rect)
        //{
        //    var max_bytes = GetMaxValue();

        //    if (max_bytes == 0)
        //    {
        //        // The backend for this counter probably did not fill in any values.
        //        return;
        //    }

        //    var min_bytes = GetMinValue();

        //    SKPaint paint = new SKPaint();

        //    // Paint the background.
        //    paint.Color = 0x99FFFFFF;
        //    canvas.DrawRect(rect, paint);

        //    // Establish the graph position.
        //    float x = rect.Left;
        //    float y = rect.Top;
        //    float width = rect.Width;
        //    float height = rect.Height;
        //    float bottom = y + height;
        //    float right = x + width;

        //    // Prepare a path for the data.
        //    SKPath path = new SKPath();
        //    path.MoveTo(x, bottom);

        //    for (int i = 0; i < GlobalMembers.kMaxSamples; ++i)
        //    {
        //        ulong current_bytes = values_[i];
        //        double ratio = (double)(current_bytes - min_bytes) / (max_bytes - min_bytes);
        //        path.LineTo((float)(x + (((double)(i) / (double)GlobalMembers.kMaxSamples) * width)), (float)(y + ((1.0 - ratio) * height)));
        //    }

        //    path.RLineTo(100F, 0F);
        //    path.LineTo(right, bottom);
        //    path.Close();

        //    // Draw the graph.
        //    paint.Color = 0xAA0000FF;
        //    canvas.DrawPath(path, paint);

        //    // Paint the vertical marker for the current frame.
        //    double sample_unit_width = (1.0 / GlobalMembers.kMaxSamples);
        //    double sample_margin_unit_width = sample_unit_width / 6.0;
        //    double sample_margin_width = width * sample_margin_unit_width;
        //    paint.Style = SKPaintStyle.Fill;
        //    paint.Color = GlobalMembers.SK_ColorGRAY;
        //    double sample_x = x + width * ((double)current_sample_ / GlobalMembers.kMaxSamples) - sample_margin_width;
        //    var marker_rect = new SKRect((float)sample_x, y, (float)(sample_x + width * sample_unit_width + sample_margin_width * 2), bottom);
        //    canvas.DrawRect(marker_rect, paint);
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: long GetCurrentValue() const
        public ulong GetCurrentValue()
        {
            return values_[current_sample_];
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: long GetMaxValue() const
        public ulong GetMaxValue()
        {
            var max = ulong.MinValue;
            for (int i = 0; i < GlobalMembers.kMaxSamples; ++i)
            {
                max = Math.Max(max, values_[i]);
            }
            return max;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: long GetMinValue() const
        public ulong GetMinValue()
        {
            var min = ulong.MaxValue;
            for (int i = 0; i < GlobalMembers.kMaxSamples; ++i)
            {
                min = Math.Min(min, values_[i]);
            }
            return min;
        }

        private List<ulong> values_ = new List<ulong>();
        private int current_sample_ = new int();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  CounterValues(const CounterValues&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  CounterValues& operator =(const CounterValues&) = delete;
    }

}