using System;
using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;

// Copyright 2015 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    public class Stopwatch : System.IDisposable
    {
        public Stopwatch()
        {
            this.start_ = new fml.TimePoint(fml.TimePoint.Now());
            this.current_sample_ = 0;
            fml.TimeDelta delta = fml.TimeDelta.Zero();
            laps_.Resize(GlobalMembers.kMaxSamples, delta);
            cache_dirty_ = true;
            prev_drawn_sample_index_ = 0;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const fml::TimeDelta& LastLap() const
        public fml.TimeDelta LastLap()
        {
            return laps_[(current_sample_ - 1) % GlobalMembers.kMaxSamples];
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: fml::TimeDelta CurrentLap() const
        public fml.TimeDelta CurrentLap()
        {
            return fml.TimePoint.Now() - start_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: fml::TimeDelta MaxDelta() const
        public fml.TimeDelta MaxDelta()
        {
            fml.TimeDelta max_delta = new fml.TimeDelta();
            for (size_t i = 0; i < GlobalMembers.kMaxSamples; i++)
            {
                if (laps_[i] > max_delta)
                {
                    max_delta = laps_[i];
                }
            }
            return max_delta;
        }


        // Initialize the SkSurface for drawing into. Draws the base background and any
        // timing data from before the initial Visualize() call.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void InitVisualizeSurface(const SKRect& rect) const
        public void InitVisualizeSurface(SKRect rect)
        {
            if (!cache_dirty_)
            {
                return;
            }
            cache_dirty_ = false;

            // TODO(garyq): Use a GPU surface instead of a CPU surface.
            visualize_cache_surface_.CopyFrom(SkSurface.MakeRasterN32Premul(rect.width(), rect.height()));

            SKCanvas cache_canvas = visualize_cache_surface_.Dereference().getCanvas();

            // Establish the graph position.
            const float x = 0F;
            const float y = 0F;
            float width = rect.width();
            float height = rect.height();

            SKPaint paint = new SKPaint();
            paint.setColor(0x99FFFFFF);
            cache_canvas.drawRect(SKRect.MakeXYWH(x, y, width, height), paint);

            // Scale the graph to show frame times up to those that are 3 times the frame
            // time.
            double max_interval = GlobalMembers.kOneFrameMS * 3.0;
            double max_unit_interval = GlobalMembers.UnitFrameInterval(max_interval);

            // Draw the old data to initially populate the graph.
            // Prepare a path for the data. We start at the height of the last point, so
            // it looks like we wrap around
            SKPath path = new SKPath();
            path.setIsVolatile(true);
            path.moveTo(x, height);
            path.lineTo(x, y + height * (1.0 - GlobalMembers.UnitHeight(laps_[0].ToMillisecondsF(), max_unit_interval)));
            double unit_x;
            double unit_next_x = 0.0;
            for (size_t i = 0; i < GlobalMembers.kMaxSamples; i += 1)
            {
                unit_x = unit_next_x;
                unit_next_x = ((double)(i + 1) / GlobalMembers.kMaxSamples);
                double sample_y = y + height * (1.0 - GlobalMembers.UnitHeight(laps_[i].ToMillisecondsF(), max_unit_interval));
                path.lineTo(x + width * unit_x, sample_y);
                path.lineTo(x + width * unit_next_x, sample_y);
            }
            path.lineTo(width, y + height * (1.0 - GlobalMembers.UnitHeight(laps_[GlobalMembers.kMaxSamples - 1].ToMillisecondsF(), max_unit_interval)));
            path.lineTo(width, height);
            path.close();

            // Draw the graph.
            paint.setColor(0xAA0000FF);
            cache_canvas.drawPath(path, paint);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Visualize(SKCanvas& canvas, const SKRect& rect) const
        public void Visualize(SKCanvas canvas, SKRect rect)
        {
            // Initialize visualize cache if it has not yet been initialized.
            InitVisualizeSurface(rect);

            SKCanvas cache_canvas = visualize_cache_surface_.Dereference().getCanvas();
            SKPaint paint = new SKPaint();

            // Establish the graph position.
            const float x = 0F;
            const float y = 0F;
            float width = rect.width();
            float height = rect.height();

            // Scale the graph to show frame times up to those that are 3 times the frame
            // time.
            double max_interval = GlobalMembers.kOneFrameMS * 3.0;
            double max_unit_interval = GlobalMembers.UnitFrameInterval(max_interval);

            double sample_unit_width = (1.0 / GlobalMembers.kMaxSamples);

            // Draw vertical replacement bar to erase old/stale pixels.
            paint.setColor(0x99FFFFFF);
            paint.setStyle(SKPaint.Style.kFill_Style);
            paint.setBlendMode(SkBlendMode.kSrc);
            double sample_x = x + width * ((double)prev_drawn_sample_index_ / GlobalMembers.kMaxSamples);
            var eraser_rect = SKRect.MakeLTRB(sample_x, y, sample_x + width * sample_unit_width, height);
            cache_canvas.drawRect(eraser_rect, paint);

            // Draws blue timing bar for new data.
            paint.setColor(0xAA0000FF);
            paint.setBlendMode(SkBlendMode.kSrcOver);
            var bar_rect = SKRect.MakeLTRB(sample_x, y + height * (1.0 - GlobalMembers.UnitHeight(laps_[current_sample_ == 0 ? GlobalMembers.kMaxSamples - 1 : current_sample_ - 1].ToMillisecondsF(), max_unit_interval)), sample_x + width * sample_unit_width, height);
            cache_canvas.drawRect(bar_rect, paint);

            // Draw horizontal frame markers.
            paint.setStrokeWidth(0F); // hairline
            paint.setStyle(SKPaint.Style.kStroke_Style);
            paint.setColor(0xCC000000);

            if (max_interval > GlobalMembers.kOneFrameMS)
            {
                // Paint the horizontal markers
                size_t frame_marker_count = (size_t)(max_interval / GlobalMembers.kOneFrameMS);

                // Limit the number of markers displayed. After a certain point, the graph
                // becomes crowded
                if (frame_marker_count > GlobalMembers.kMaxFrameMarkers)
                {
                    frame_marker_count = 1;
                }

                for (size_t frame_index = 0; frame_index < frame_marker_count; frame_index++)
                {
                    double frame_height = height * (1.0 - (GlobalMembers.UnitFrameInterval((frame_index + 1) * GlobalMembers.kOneFrameMS) / max_unit_interval));
                    cache_canvas.drawLine(x, y + frame_height, width, y + frame_height, paint);
                }
            }

            // Paint the vertical marker for the current frame.
            // We paint it over the current frame, not after it, because when we
            // paint this we don't yet have all the times for the current frame.
            paint.setStyle(SKPaint.Style.kFill_Style);
            paint.setBlendMode(SkBlendMode.kSrcOver);
            if (GlobalMembers.UnitFrameInterval(LastLap().ToMillisecondsF()) > 1.0)
            {
                // budget exceeded
                paint.setColor(new uint32_t(GlobalMembers.SK_ColorRED));
            }
            else
            {
                // within budget
                paint.setColor(new uint32_t(GlobalMembers.SK_ColorGREEN));
            }
            sample_x = x + width * ((double)current_sample_ / GlobalMembers.kMaxSamples);
            var marker_rect = SKRect.MakeLTRB(sample_x, y, sample_x + width * sample_unit_width, height);
            cache_canvas.drawRect(marker_rect, paint);
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: prev_drawn_sample_index_ = current_sample_;
            prev_drawn_sample_index_.CopyFrom(current_sample_);

            // Draw the cached surface onto the output canvas.
            paint.reset();
            visualize_cache_surface_.Dereference().draw(canvas, rect.x(), rect.y(), paint);
        }

        public void Start()
        {
            start_ = fml.TimePoint.Now();
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: current_sample_ = (current_sample_ + 1) % kMaxSamples;
            current_sample_.CopyFrom((current_sample_ + 1) % GlobalMembers.kMaxSamples);
        }

        public void Stop()
        {
            laps_[current_sample_] = fml.TimePoint.Now() - start_;
        }

        public void SetLapTime(fml.TimeDelta delta)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: current_sample_ = (current_sample_ + 1) % kMaxSamples;
            current_sample_.CopyFrom((current_sample_ + 1) % GlobalMembers.kMaxSamples);
            laps_[current_sample_] = delta;
        }

        private fml.TimePoint start_ = new fml.TimePoint();
        private List<fml.TimeDelta> laps_ = new List<fml.TimeDelta>();
        private size_t current_sample_ = new size_t();
        // Mutable data cache for performance optimization of the graphs. Prevents
        // expensive redrawing of old data.
        private bool cache_dirty_;
        private sk_sp<SkSurface> visualize_cache_surface_ = new sk_sp<SkSurface>();
        private size_t prev_drawn_sample_index_ = new size_t();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Stopwatch(const Stopwatch&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Stopwatch& operator =(const Stopwatch&) = delete;
    }

    public class Counter
    {
        public Counter()
        {
            this.count_ = 0;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: size_t count() const
        public size_t count()
        {
            return count_;
        }

        public void Reset(size_t count = 0)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: count_ = count;
            count_.CopyFrom(count);
        }

        public void Increment(size_t count = 1)
        {
            count_ += count;
        }

        private size_t count_ = new size_t();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Counter(const Counter&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Counter& operator =(const Counter&) = delete;
    }

    public class CounterValues : System.IDisposable
    {
        public CounterValues()
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.current_sample_ = kMaxSamples - 1;
            this.current_sample_.CopyFrom(GlobalMembers.kMaxSamples - 1);
            values_.Resize(GlobalMembers.kMaxSamples, 0);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  void Add(int64_t value);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Visualize(SKCanvas& canvas, const SKRect& rect) const
        public void Visualize(SKCanvas canvas, SKRect rect)
        {
            size_t max_bytes = GetMaxValue();

            if (max_bytes == 0)
            {
                // The backend for this counter probably did not fill in any values.
                return;
            }

            size_t min_bytes = GetMinValue();

            SKPaint paint = new SKPaint();

            // Paint the background.
            paint.setColor(0x99FFFFFF);
            canvas.drawRect(rect, paint);

            // Establish the graph position.
            float x = rect.x();
            float y = rect.y();
            float width = rect.width();
            float height = rect.height();
            float bottom = y + height;
            float right = x + width;

            // Prepare a path for the data.
            SKPath path = new SKPath();
            path.moveTo(x, bottom);

            for (size_t i = 0; i < GlobalMembers.kMaxSamples; ++i)
            {
                long current_bytes = values_[i];
                double ratio = (double)(current_bytes - min_bytes) / (max_bytes - min_bytes);
                path.lineTo(x + (((double)(i) / (double)GlobalMembers.kMaxSamples) * width), y + ((1.0 - ratio) * height));
            }

            path.rLineTo(100F, 0F);
            path.lineTo(right, bottom);
            path.close();

            // Draw the graph.
            paint.setColor(0xAA0000FF);
            canvas.drawPath(path, paint);

            // Paint the vertical marker for the current frame.
            double sample_unit_width = (1.0 / GlobalMembers.kMaxSamples);
            double sample_margin_unit_width = sample_unit_width / 6.0;
            double sample_margin_width = width * sample_margin_unit_width;
            paint.setStyle(SKPaint.Style.kFill_Style);
            paint.setColor(new uint32_t(GlobalMembers.SK_ColorGRAY));
            double sample_x = x + width * ((double)current_sample_ / GlobalMembers.kMaxSamples) - sample_margin_width;
            var marker_rect = SKRect.MakeLTRB(sample_x, y, sample_x + width * sample_unit_width + sample_margin_width * 2, bottom);
            canvas.drawRect(marker_rect, paint);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: long GetCurrentValue() const
        public long GetCurrentValue()
        {
            return values_[current_sample_];
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: long GetMaxValue() const
        public long GetMaxValue()
        {
            var max = long.MinValue;
            for (size_t i = 0; i < GlobalMembers.kMaxSamples; ++i)
            {
                max = Math.Max<long>(max, values_[i]);
            }
            return max;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: long GetMinValue() const
        public long GetMinValue()
        {
            var min = long.MaxValue;
            for (size_t i = 0; i < GlobalMembers.kMaxSamples; ++i)
            {
                min = Math.Min<long>(min, values_[i]);
            }
            return min;
        }

        private List<int64_t> values_ = new List<int64_t>();
        private size_t current_sample_ = new size_t();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  CounterValues(const CounterValues&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  CounterValues& operator =(const CounterValues&) = delete;
    }

}