using SkiaSharp;
using System;
using static FlutterBinding.Flow.Helper;

namespace FlutterBinding.Flow
{
    public static class GlobalMembers
    {
        public static readonly int kDisplayRasterizerStatistics = 1 << 0;
        public static readonly int kVisualizeRasterizerStatistics = 1 << 1;
        public static readonly int kDisplayEngineStatistics = 1 << 2;
        public static readonly int kVisualizeEngineStatistics = 1 << 3;

        public static void DrawStatisticsText(SKCanvas canvas, string @string, int x, int y)
        {
            SKPaint paint = new SKPaint();
            paint.SetTextSize(15F);
            paint.SetLinearText(false);
            paint.SetColor(GlobalMembers.SK_ColorGRAY);
            canvas.DrawText(@string, @string.Length, x, y, paint);
        }

        public static void VisualizeStopWatch(SKCanvas canvas, Stopwatch stopwatch, float x, float y, float width, float height, bool show_graph, bool show_labels, string label_prefix)
        {
            const int label_x = 8; // distance from x
            const int label_y = -10; // distance from y+height

            if (show_graph)
            {
                SKRect visualization_rect = SKRect.MakeXYWH(x, y, width, height);
                stopwatch.Visualize(canvas, visualization_rect);
            }

            if (show_labels)
            {
                double ms_per_frame = stopwatch.MaxDelta().ToMillisecondsF();
                double fps;
                if (ms_per_frame < kOneFrameMS)
                {
                    fps = 1e3 / kOneFrameMS;
                }
                else
                {
                    fps = 1e3 / ms_per_frame;
                }

                std::stringstream stream = new std::stringstream();
                stream.setf(std::ios.@fixed | std::ios.showpoint);
                stream << std::setprecision(1);
                stream << label_prefix << "  " << fps << " fps  " << ms_per_frame << "ms/frame";
                GlobalMembers.DrawStatisticsText(canvas, stream.str(), x + label_x, y + height + label_y);
            }
        }

        internal const double kOneFrameMS = 1e3 / 60.0;

        internal const int kMaxSamples = 120;
        internal const int kMaxFrameMarkers = 8;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
        //Stopwatch::~Stopwatch() = default;

        internal static double UnitFrameInterval(double frame_time_ms)
        {
            return frame_time_ms * 60.0 * 1e-3;
        }

        internal static double UnitHeight(double frame_time_ms, double max_unit_interval)
        {
            double unitHeight = UnitFrameInterval(frame_time_ms) / max_unit_interval;
            if (unitHeight > 1.0)
            {
                unitHeight = 1.0;
            }
            return unitHeight;
        }

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
        //CounterValues::~CounterValues() = default;

        public static void DrawCheckerboard(SKCanvas canvas, uint c1, uint c2, int size)
        {
            SKPaint paint = new SKPaint();
            paint.setShader(GlobalMembers.CreateCheckerboardShader(c1, c2, size));
            canvas.DrawPaint(paint);
        }

        public static void DrawCheckerboard(SKCanvas canvas, SKRect rect)
        {
            // Draw a checkerboard
            canvas.Save();
            canvas.ClipRect(rect);

            var checkerboard_color = GlobalMembers.SkColorSetARGB(64, RandomNumbers.NextNumber() % 256, RandomNumbers.NextNumber() % 256, RandomNumbers.NextNumber() % 256);

            DrawCheckerboard(canvas, checkerboard_color, 0x00000000, 12);
            canvas.Restore();

            // Stroke the drawn area
            SKPaint debugPaint = new SKPaint();
            debugPaint.setStrokeWidth(8F);
            debugPaint.setColor(GlobalMembers.SkColorSetA(checkerboard_color, 255));
            debugPaint.setStyle(SKPaint.Style.kStroke_Style);
            canvas.DrawRect(rect, debugPaint);
        }

        public static SKShader CreateCheckerboardShader(uint c1, uint c2, int size)
        {
            SKBitmap bm = new SKBitmap();
            bm.allocN32Pixels(2 * size, 2 * size);
            bm.eraseColor(c1);
            bm.eraseArea(SKRectI.MakeLTRB(0, 0, size, size), c2);
            bm.eraseArea(SKRectI.MakeLTRB(size, size, 2 * size, 2 * size), c2);
            return SKShader.MakeBitmapShader(bm, SKShader.TileMode.kRepeat_TileMode, SKShader.TileMode.kRepeat_TileMode);
        }

        internal static bool CanRasterizePicture(SKPicture picture)
        {
            if (picture == null)
            {
                return false;
            }

            SKRect cull_rect = picture.cullRect();

            if (cull_rect.isEmpty())
            {
                // No point in ever rasterizing an empty picture.
                return false;
            }

            if (!cull_rect.isFinite())
            {
                // Cannot attempt to rasterize into an infinitely large surface.
                return false;
            }

            return true;
        }

        internal static bool IsPictureWorthRasterizing(SKPicture picture, bool will_change, bool is_complex)
        {
            if (will_change)
            {
                // If the picture is going to change in the future, there is no point in
                // doing to extra work to rasterize.
                return false;
            }

            if (!CanRasterizePicture(picture))
            {
                // No point in deciding whether the picture is worth rasterizing if it
                // cannot be rasterized at all.
                return false;
            }

            if (is_complex)
            {
                // The caller seems to have extra information about the picture and thinks
                // the picture is always worth rasterizing.
                return true;
            }

            // TODO(abarth): We should find a better heuristic here that lets us avoid
            // wasting memory on trivial layers that are easy to re-rasterize every frame.
            return picture.approximateOpCount() > 10;
        }

        internal static RasterCacheResult Rasterize(GRContext context, SKMatrix ctm, SKColorSpace dst_color_space, bool checkerboard, SKRect logical_rect, Action<SKCanvas> draw_function)
        {
            SKRectI cache_rect = RasterCache.GetDeviceBounds(logical_rect, ctm);

            SKImageInfo image_info = SKImageInfo.MakeN32Premul(cache_rect.width(), cache_rect.height());

            SKSurface surface = context != null ? SKSurface.MakeRenderTarget(context, SkBudgeted.kYes, image_info) : SKSurface.MakeRaster(image_info);

            if (surface == null)
            {
                return new RasterCacheResult();
            }

            SKCanvas canvas = surface.Dereference().getCanvas();
            SKCanvas xformCanvas = new SKCanvas();
            if (dst_color_space != null)
            {
                xformCanvas = SkCreateColorSpaceXformCanvas(surface.Dereference().getCanvas(), GlobalMembers.sk_ref_sp(dst_color_space));
                if (xformCanvas != null)
                {
                    canvas = xformCanvas.get();
                }
            }

            canvas.Clear(GlobalMembers.SK_ColorTRANSPARENT);
            canvas.Translate(-cache_rect.left(), -cache_rect.top());
            canvas.Concat(ctm);
            draw_function(canvas);

            if (checkerboard)
            {
                DrawCheckerboard(canvas, logical_rect);
            }

            return new RasterCacheResult(surface.Dereference().makeImageSnapshot(), logical_rect);
        }

        public static RasterCacheResult RasterizePicture(SKPicture picture, GRContext context, SKMatrix ctm, SKColorSpace dst_color_space, bool checkerboard)
        {
            TRACE_EVENT0("flutter", "RasterCachePopulate");

            //C++ TO C# CONVERTER TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
            //ORIGINAL LINE: return Rasterize(context, ctm, dst_color_space, checkerboard, picture->cullRect(), [=](SKCanvas* canvas)
            return Rasterize(context, ctm, dst_color_space, checkerboard, picture.cullRect(), (SKCanvas canvas) =>
            {
                canvas.DrawPicture(picture);
            });
        }

        internal static int ClampSize(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }

            if (value < min)
            {
                return min;
            }

            return value;
        }
    }
}

public static class GlobalMembers
{
    
    //public static std::ostream operator <<(std::ostream os, MatrixDecomposition m)
    //{
    //    if (!m.IsValid())
    //    {
    //        os << "Invalid Matrix!" << std::endl;
    //        return os;
    //    }

    //    os << "Translation (x, y, z): " << m.translation() << std::endl;
    //    os << "Scale (z, y, z): " << m.scale() << std::endl;
    //    os << "Shear (zy, yz, zx): " << m.shear() << std::endl;
    //    os << "Perspective (x, y, z, w): " << m.perspective() << std::endl;
    //    os << "Rotation (x, y, z, w): " << m.rotation() << std::endl;

    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, RasterCacheKey<uint> k)
    //{
    //    os << "Picture: " << k.id() << " matrix: " << k.matrix();
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKSizeI size)
    //{
    //    os << size.width() << ", " << size.height();
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKMatrix m)
    //{
    //    SkString string = new SkString();
    //    string.printf("[%8.4f %8.4f %8.4f][%8.4f %8.4f %8.4f][%8.4f %8.4f %8.4f]", m[0], m[1], m[2], m[3], m[4], m[5], m[6], m[7], m[8]);
    //    os << string.c_str();
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKMatrix44 m)
    //{
    //    os << m.get(0, 0) << ", " << m.get(0, 1) << ", " << m.get(0, 2) << ", " << m.get(0, 3) << std::endl;
    //    os << m.get(1, 0) << ", " << m.get(1, 1) << ", " << m.get(1, 2) << ", " << m.get(1, 3) << std::endl;
    //    os << m.get(2, 0) << ", " << m.get(2, 1) << ", " << m.get(2, 2) << ", " << m.get(2, 3) << std::endl;
    //    os << m.get(3, 0) << ", " << m.get(3, 1) << ", " << m.get(3, 2) << ", " << m.get(3, 3);
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKPoint r)
    //{
    //    os << "XY: " << r.fX << ", " << r.fY;
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKRect r)
    //{
    //    os << "LTRB: " << r.fLeft << ", " << r.fTop << ", " << r.fRight << ", " << r.fBottom;
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKRRect r)
    //{
    //    os << "LTRB: " << r.rect().fLeft << ", " << r.rect().fTop << ", " << r.rect().fRight << ", " << r.rect().fBottom;
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SKPoint3 v)
    //{
    //    os << v.x() << ", " << v.y() << ", " << v.z();
    //    return os;
    //}
    //public static std::ostream operator <<(std::ostream os, SkVector4 v)
    //{
    //    os << v.fData[0] << ", " << v.fData[1] << ", " << v.fData[2] << ", " << v.fData[3];
    //    return os;
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(MatrixDecomposition UnnamedParameter, Rotation UnnamedParameter2)
    //{
    //    SKMatrix44 matrix = SKMatrix44.I();

    //    var angle = M_PI_4;
    //    matrix.setRotateAbout(0.0, 0.0, 1.0, angle);

    //    MatrixDecomposition decomposition = new MatrixDecomposition(matrix);
    //    ASSERT_TRUE(decomposition.IsValid());

    //    var sine = Math.Sin(angle * 0.5);

    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[0]);
    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[1]);
    //    ASSERT_FLOAT_EQ(sine, decomposition.rotation().fData[2]);
    //    ASSERT_FLOAT_EQ(Math.Cos(angle * 0.5), decomposition.rotation().fData[3]);
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(MatrixDecomposition UnnamedParameter, Scale UnnamedParameter2)
    //{
    //    SKMatrix44 matrix = SKMatrix44.I();

    //    const var scale = 5.0;
    //    matrix.setScale(scale + 0, scale + 1, scale + 2);

    //    MatrixDecomposition decomposition = new MatrixDecomposition(matrix);
    //    ASSERT_TRUE(decomposition.IsValid());

    //    ASSERT_FLOAT_EQ(scale + 0, decomposition.scale().fX);
    //    ASSERT_FLOAT_EQ(scale + 1, decomposition.scale().fY);
    //    ASSERT_FLOAT_EQ(scale + 2, decomposition.scale().fZ);
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(MatrixDecomposition UnnamedParameter, Translate UnnamedParameter2)
    //{
    //    SKMatrix44 matrix = SKMatrix44.I();

    //    const var translate = 125.0;
    //    matrix.setTranslate(translate + 0, translate + 1, translate + 2);

    //    MatrixDecomposition decomposition = new MatrixDecomposition(matrix);
    //    ASSERT_TRUE(decomposition.IsValid());

    //    ASSERT_FLOAT_EQ(translate + 0, decomposition.translation().fX);
    //    ASSERT_FLOAT_EQ(translate + 1, decomposition.translation().fY);
    //    ASSERT_FLOAT_EQ(translate + 2, decomposition.translation().fZ);
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(MatrixDecomposition UnnamedParameter, Combination UnnamedParameter2)
    //{
    //    SKMatrix44 matrix = SKMatrix44.I();

    //    var rotation = M_PI_4;
    //    const var scale = 5;
    //    const var translate = 125.0;

    //    SKMatrix44 m1 = SKMatrix44.I();
    //    m1.setRotateAbout(0, 0, 1, rotation);

    //    SKMatrix44 m2 = SKMatrix44.I();
    //    m2.setScale(scale);

    //    SKMatrix44 m3 = SKMatrix44.I();
    //    m3.setTranslate(translate, translate, translate);

    //    SKMatrix44 combined = m3 * m2 * m1;

    //    MatrixDecomposition decomposition = new MatrixDecomposition(combined);
    //    ASSERT_TRUE(decomposition.IsValid());

    //    ASSERT_FLOAT_EQ(translate, decomposition.translation().fX);
    //    ASSERT_FLOAT_EQ(translate, decomposition.translation().fY);
    //    ASSERT_FLOAT_EQ(translate, decomposition.translation().fZ);

    //    ASSERT_FLOAT_EQ(scale, decomposition.scale().fX);
    //    ASSERT_FLOAT_EQ(scale, decomposition.scale().fY);
    //    ASSERT_FLOAT_EQ(scale, decomposition.scale().fZ);

    //    var sine = Math.Sin(rotation * 0.5);

    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[0]);
    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[1]);
    //    ASSERT_FLOAT_EQ(sine, decomposition.rotation().fData[2]);
    //    ASSERT_FLOAT_EQ(Math.Cos(rotation * 0.5), decomposition.rotation().fData[3]);
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(MatrixDecomposition UnnamedParameter, ScaleFloatError UnnamedParameter2)
    //{
    //    for (float scale = 0.0001f; scale < 2.0f; scale += 0.000001f)
    //    {
    //        SKMatrix44 matrix = SKMatrix44.I();
    //        matrix.setScale(scale, scale, 1.0f);

    //        MatrixDecomposition decomposition3 = new MatrixDecomposition(matrix);
    //        ASSERT_TRUE(decomposition3.IsValid());

    //        ASSERT_FLOAT_EQ(scale, decomposition3.scale().fX);
    //        ASSERT_FLOAT_EQ(scale, decomposition3.scale().fY);
    //        ASSERT_FLOAT_EQ(1.0f, decomposition3.scale().fZ);
    //        ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[0]);
    //        ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[1]);
    //        ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[2]);
    //    }

    //    SKMatrix44 matrix = SKMatrix44.I();
    //    const var scale = 1.7734375f;
    //    matrix.setScale(scale, scale, 1.0f);

    //    // Bug upper bound (empirical)
    //    const var scale2 = 1.773437559603f;
    //    SKMatrix44 matrix2 = SKMatrix44.I();
    //    matrix2.setScale(scale2, scale2, 1.0f);

    //    // Bug lower bound (empirical)
    //    const var scale3 = 1.7734374403954f;
    //    SKMatrix44 matrix3 = SKMatrix44.I();
    //    matrix3.setScale(scale3, scale3, 1.0f);

    //    MatrixDecomposition decomposition = new MatrixDecomposition(matrix);
    //    ASSERT_TRUE(decomposition.IsValid());

    //    MatrixDecomposition decomposition2 = new MatrixDecomposition(matrix2);
    //    ASSERT_TRUE(decomposition2.IsValid());

    //    MatrixDecomposition decomposition3 = new MatrixDecomposition(matrix3);
    //    ASSERT_TRUE(decomposition3.IsValid());

    //    ASSERT_FLOAT_EQ(scale, decomposition.scale().fX);
    //    ASSERT_FLOAT_EQ(scale, decomposition.scale().fY);
    //    ASSERT_FLOAT_EQ(1.0f, decomposition.scale().fZ);
    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[0]);
    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[1]);
    //    ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[2]);

    //    ASSERT_FLOAT_EQ(scale2, decomposition2.scale().fX);
    //    ASSERT_FLOAT_EQ(scale2, decomposition2.scale().fY);
    //    ASSERT_FLOAT_EQ(1.0f, decomposition2.scale().fZ);
    //    ASSERT_FLOAT_EQ(0, decomposition2.rotation().fData[0]);
    //    ASSERT_FLOAT_EQ(0, decomposition2.rotation().fData[1]);
    //    ASSERT_FLOAT_EQ(0, decomposition2.rotation().fData[2]);

    //    ASSERT_FLOAT_EQ(scale3, decomposition3.scale().fX);
    //    ASSERT_FLOAT_EQ(scale3, decomposition3.scale().fY);
    //    ASSERT_FLOAT_EQ(1.0f, decomposition3.scale().fZ);
    //    ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[0]);
    //    ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[1]);
    //    ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[2]);
    //}

    //public static SKPicture GetSamplePicture()
    //{
    //    SKPictureRecorder recorder = new SKPictureRecorder();
    //    recorder.beginRecording(SKRect.MakeWH(150, 100));
    //    SKPaint paint = new SKPaint();
    //    paint.SetColor(SK_ColorRED);
    //    recorder.getRecordingCanvas().drawRect(SKRect.MakeXYWH(10, 10, 80, 80), paint);
    //    return recorder.finishRecordingAsPicture();
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(RasterCache UnnamedParameter, SimpleInitialization UnnamedParameter2)
    //{
    //    RasterCache cache = new RasterCache();
    //    ASSERT_TRUE(true);
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(RasterCache UnnamedParameter, ThresholdIsRespected UnnamedParameter2)
    //{
    //    int threshold = 3;
    //    RasterCache cache = new RasterCache(new int(threshold));

    //    SKMatrix matrix = SKMatrix.I();

    //    var picture = GetSamplePicture();

    //    sk_sp<SKImage> image = new sk_sp<SKImage>();

    //    sk_sp<SKColorSpace> srgb = SKColorSpace.MakeSRGB();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 1
    //    cache.SweepAfterFrame();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 2
    //    cache.SweepAfterFrame();
    //    ASSERT_TRUE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 3
    //    cache.SweepAfterFrame();
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(RasterCache UnnamedParameter, ThresholdIsRespectedWhenZero UnnamedParameter2)
    //{
    //    int threshold = 0;
    //    RasterCache cache = new RasterCache(new int(threshold));

    //    SKMatrix matrix = SKMatrix.I();

    //    var picture = GetSamplePicture();

    //    sk_sp<SKImage> image = new sk_sp<SKImage>();

    //    sk_sp<SKColorSpace> srgb = SKColorSpace.MakeSRGB();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 1
    //    cache.SweepAfterFrame();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 2
    //    cache.SweepAfterFrame();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 3
    //    cache.SweepAfterFrame();
    //}

    ////C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
    //public static TEST(RasterCache UnnamedParameter, SweepsRemoveUnusedFrames UnnamedParameter2)
    //{
    //    int threshold = 3;
    //    RasterCache cache = new RasterCache(new int(threshold));

    //    SKMatrix matrix = SKMatrix.I();

    //    var picture = GetSamplePicture();

    //    sk_sp<SKImage> image = new sk_sp<SKImage>();

    //    sk_sp<SKColorSpace> srgb = SKColorSpace.MakeSRGB();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 1
    //    cache.SweepAfterFrame();
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 2
    //    cache.SweepAfterFrame();
    //    ASSERT_TRUE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 3
    //    cache.SweepAfterFrame();
    //    ASSERT_TRUE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 4
    //    cache.SweepAfterFrame();
    //    cache.SweepAfterFrame(); // Extra frame without a preroll image access.
    //    ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 5
    //}

}