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

        public static SKColor SK_ColorTRANSPARENT = SKColor.Parse("#FF000000");

        internal const double kOneFrameMS = 1e3 / 60.0;

        internal const int kMaxSamples = 120;
        internal const int kMaxFrameMarkers = 8;

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

        internal static bool CanRasterizePicture(SKPicture picture)
        {
            if (picture == null)
            {
                return false;
            }

            SKRect cull_rect = picture.CullRect;

            if (cull_rect.IsEmpty)
            {
                // No point in ever rasterizing an empty picture.
                return false;
            }

            if (float.IsInfinity(cull_rect.Width) || float.IsInfinity(cull_rect.Height))
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
            return false; //picture.approximateOpCount() > 10;
        }

        internal static RasterCacheResult Rasterize(GRContext context, SKMatrix ctm, SKColorSpace dst_color_space, bool checkerboard, SKRect logical_rect, Action<SKCanvas> draw_function)
        {
            SKRectI cache_rect = RasterCache.GetDeviceBounds(logical_rect, ctm);

            SKImageInfo image_info = new SKImageInfo(cache_rect.Width, cache_rect.Height);

            SKSurface surface = context != null ? SKSurface.CreateAsRenderTarget(context, new GRGlBackendTextureDesc() { Width = cache_rect.Width, Height = cache_rect.Height } ) : SKSurface.Create(image_info); //{ image_info.

            if (surface == null)
            {
                return new RasterCacheResult();
            }

            SKCanvas canvas = surface.Canvas;
            SKCanvas xformCanvas = canvas;
            //if (dst_color_space != null)
            //{
            //    xformCanvas = SkCreateColorSpaceXformCanvas(canvas, GlobalMembers.sk_ref_sp(dst_color_space));
            //    if (xformCanvas != null)
            //    {
            //        canvas = xformCanvas;
            //    }
            //}

            canvas.Clear(GlobalMembers.SK_ColorTRANSPARENT);
            canvas.Translate(-cache_rect.Left, -cache_rect.Top);
            canvas.Concat(ref ctm);
            draw_function(canvas);

            //if (checkerboard)
            //{
            //    DrawCheckerboard(canvas, logical_rect);
            //}

            return new RasterCacheResult(surface.Snapshot(), logical_rect);
        }

        public static RasterCacheResult RasterizePicture(SKPicture picture, GRContext context, SKMatrix ctm, SKColorSpace dst_color_space, bool checkerboard)
        {
            TRACE_EVENT0("flutter", "RasterCachePopulate");

            return Rasterize(context, ctm, dst_color_space, checkerboard, picture.CullRect, (SKCanvas canvas) =>
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