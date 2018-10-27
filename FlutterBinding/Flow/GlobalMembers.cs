using System;

namespace flow
{
	public static class GlobalMembers
	{
	public static readonly int kDisplayRasterizerStatistics = 1 << 0;
	public static readonly int kVisualizeRasterizerStatistics = 1 << 1;
	public static readonly int kDisplayEngineStatistics = 1 << 2;
	public static readonly int kVisualizeEngineStatistics = 1 << 3;

	public static void DrawStatisticsText(SkiaSharp.SKCanvas canvas, string @string, int x, int y)
	{
	  SkPaint paint = new SkPaint();
	  paint.setTextSize(15F);
	  paint.setLinearText(false);
	  paint.setColor(new uint32_t(GlobalMembers.SK_ColorGRAY));
	  canvas.drawText(@string, @string.Length, x, y, paint);
	}

	public static void VisualizeStopWatch(SkiaSharp.SKCanvas canvas, Stopwatch stopwatch, float x, float y, float width, float height, bool show_graph, bool show_labels, string label_prefix)
	{
	  const int label_x = 8; // distance from x
	  const int label_y = -10; // distance from y+height

	  if (show_graph)
	  {
		SkRect visualization_rect = SkRect.MakeXYWH(x, y, width, height);
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

	internal double kOneFrameMS = 1e3 / 60.0;

	internal const size_t kMaxSamples = 120;
	internal const size_t kMaxFrameMarkers = 8;

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

	public static void DrawCheckerboard(SkiaSharp.SKCanvas canvas, uint32_t c1, uint32_t c2, int size)
	{
	  SkPaint paint = new SkPaint();
	  paint.setShader(GlobalMembers.CreateCheckerboardShader(new uint32_t(c1), new uint32_t(c2), size));
	  canvas.drawPaint(paint);
	}

	public static void DrawCheckerboard(SkiaSharp.SKCanvas canvas, SkRect rect)
	{
	  // Draw a checkerboard
	  canvas.save();
	  canvas.clipRect(rect);

	  var checkerboard_color = GlobalMembers.SkColorSetARGB(64, RandomNumbers.NextNumber() % 256, RandomNumbers.NextNumber() % 256, RandomNumbers.NextNumber() % 256);

	  DrawCheckerboard(canvas, checkerboard_color, 0x00000000, 12);
	  canvas.restore();

	  // Stroke the drawn area
	  SkPaint debugPaint = new SkPaint();
	  debugPaint.setStrokeWidth(8F);
	  debugPaint.setColor(GlobalMembers.SkColorSetA(checkerboard_color, 255));
	  debugPaint.setStyle(SkPaint.Style.kStroke_Style);
	  canvas.drawRect(rect, debugPaint);
	}

	public static sk_sp<SkShader> CreateCheckerboardShader(uint32_t c1, uint32_t c2, int size)
	{
	  SkBitmap bm = new SkBitmap();
	  bm.allocN32Pixels(2 * size, 2 * size);
	  bm.eraseColor(new uint32_t(c1));
	  bm.eraseArea(SkIRect.MakeLTRB(0, 0, size, size), new uint32_t(c2));
	  bm.eraseArea(SkIRect.MakeLTRB(size, size, 2 * size, 2 * size), new uint32_t(c2));
	  return SkShader.MakeBitmapShader(bm, SkShader.TileMode.kRepeat_TileMode, SkShader.TileMode.kRepeat_TileMode);
	}

	internal static bool CanRasterizePicture(SkPicture picture)
	{
	  if (picture == null)
	  {
		return false;
	  }

	  SkRect cull_rect = picture.cullRect();

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

	internal static bool IsPictureWorthRasterizing(SkPicture picture, bool will_change, bool is_complex)
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

	internal static RasterCacheResult Rasterize(GrContext context, SkMatrix ctm, SkColorSpace dst_color_space, bool checkerboard, SkRect logical_rect, Action<SkiaSharp.SKCanvas> draw_function)
	{
	  SkIRect cache_rect = RasterCache.GetDeviceBounds(logical_rect, ctm);

	  SkImageInfo image_info = SkImageInfo.MakeN32Premul(cache_rect.width(), cache_rect.height());

	  sk_sp<SkSurface> surface = context != null ? SkSurface.MakeRenderTarget(context, SkBudgeted.kYes, image_info) : SkSurface.MakeRaster(image_info);

	  if (surface == null)
	  {
		return new RasterCacheResult();
	  }

	  SkiaSharp.SKCanvas canvas = surface.Dereference().getCanvas();
	  std::unique_ptr<SkiaSharp.SKCanvas> xformCanvas = new std::unique_ptr<SkiaSharp.SKCanvas>();
	  if (dst_color_space != null)
	  {
		xformCanvas = SkCreateColorSpaceXformCanvas(surface.Dereference().getCanvas(), GlobalMembers.sk_ref_sp(dst_color_space));
		if (xformCanvas != null)
		{
		  canvas = xformCanvas.get();
		}
	  }

	  canvas.clear(new uint32_t(GlobalMembers.SK_ColorTRANSPARENT));
	  canvas.translate(-cache_rect.left(), -cache_rect.top());
	  canvas.concat(ctm);
	  draw_function(canvas);

	  if (checkerboard)
	  {
		DrawCheckerboard(canvas, logical_rect);
	  }

	  return new RasterCacheResult(surface.Dereference().makeImageSnapshot(), logical_rect);
	}

	public static RasterCacheResult RasterizePicture(SkPicture picture, GrContext context, SkMatrix ctm, SkColorSpace dst_color_space, bool checkerboard)
	{
	  TRACE_EVENT0("flutter", "RasterCachePopulate");

//C++ TO C# CONVERTER TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: return Rasterize(context, ctm, dst_color_space, checkerboard, picture->cullRect(), [=](SkiaSharp.SKCanvas* canvas)
  return Rasterize(context, ctm, dst_color_space, checkerboard, picture.cullRect(), (SkiaSharp.SKCanvas canvas) =>
  {
	  canvas.drawPicture(picture);
  });
	}

	internal static size_t ClampSize(size_t value, size_t min, size_t max)
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
	// Copyright 2017 The Chromium Authors. All rights reserved.
	// Use of this source code is governed by a BSD-style license that can be
	// found in the LICENSE file.

	// Copyright 2017 The Chromium Authors. All rights reserved.
	// Use of this source code is governed by a BSD-style license that can be
	// found in the LICENSE file.


	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_EMBEDDER_ONLY [[deprecated]]
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY(TypeName) TypeName(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_ASSIGN(TypeName) TypeName& operator=(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_MOVE(TypeName) TypeName(TypeName&&) = delete; TypeName& operator=(TypeName&&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY_AND_ASSIGN(TypeName) TypeName(const TypeName&) = delete; TypeName& operator=(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY_ASSIGN_AND_MOVE(TypeName) TypeName(const TypeName&) = delete; TypeName(TypeName&&) = delete; TypeName& operator=(const TypeName&) = delete; TypeName& operator=(TypeName&&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_IMPLICIT_CONSTRUCTORS(TypeName) TypeName() = delete; FML_DISALLOW_COPY_ASSIGN_AND_MOVE(TypeName)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_CONCAT(X, Y) SK_MACRO_CONCAT_IMPL_PRIV(X, Y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_CONCAT_IMPL_PRIV(X, Y) X ## Y
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_APPEND_LINE(name) SK_MACRO_CONCAT(name, __LINE__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_REQUIRE_LOCAL_VAR(classname) static_assert(false, "missing name for " #classname)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_BEGIN_REQUIRE_DENSE _Pragma("GCC diagnostic push") _Pragma("GCC diagnostic error \"-Wpadded\"")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_END_REQUIRE_DENSE _Pragma("GCC diagnostic pop")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_INIT_TO_AVOID_WARNING = 0
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG1(arg1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG2(arg1, arg2)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG3(arg1, arg2, arg3)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_OS_EMBEDDED @CONFIG_EMBEDDED@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_OS_IPHONE @CONFIG_IPHONE@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_IPHONE_SIMULATOR @CONFIG_IPHONE_SIMULATOR@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_RESTRICT __restrict
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_RESTRICT __restrict__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX512
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE42
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE41
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSSE3
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE3
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE1
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_API __declspec(dllexport)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_API __declspec(dllimport)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) __has_feature(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) 0
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkNO_RETURN_HINT() do {} while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK() DumpStackTrace(0, SkDebugfForDumpStackTrace, nullptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s(%d): fatal error: \"%s\"\n", __FILE__, __LINE__, message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s:%d: fatal error: \"%s\"\n", __FILE__, __LINE__, message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ABORT(message) do { SkNO_RETURN_HINT(); SK_DUMP_LINE_FORMAT(message); SK_DUMP_GOOGLE3_STACK(); sk_abort_no_print(); } while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_COLOR_MATCHES_PMCOLOR_BYTE_ORDER (SK_A32_SHIFT == 24 && SK_R32_SHIFT == 16 && SK_G32_SHIFT == 8 && SK_B32_SHIFT == 0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C3 ## 32_SHIFT == 0 && SK_ ## C2 ## 32_SHIFT == 8 && SK_ ## C1 ## 32_SHIFT == 16 && SK_ ## C0 ## 32_SHIFT == 24)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C0 ## 32_SHIFT == 0 && SK_ ## C1 ## 32_SHIFT == 8 && SK_ ## C2 ## 32_SHIFT == 16 && SK_ ## C3 ## 32_SHIFT == 24)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_UNUSED __pragma(warning(suppress:4189))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_UNUSED SK_ATTRIBUTE(unused)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ALWAYS_INLINE __forceinline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ALWAYS_INLINE SK_ATTRIBUTE(always_inline) inline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NEVER_INLINE __declspec(noinline)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NEVER_INLINE SK_ATTRIBUTE(noinline)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr) __builtin_prefetch(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) __builtin_prefetch(ptr, 1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_GAMMA_EXPONENT (0.0f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HISTOGRAM_BOOLEAN(name, value)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HISTOGRAM_ENUMERATION(name, value, boundary_value)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT_RELEASE(cond) static_cast<void>( (cond) ? (void)0 : []{ SK_ABORT("assert(" #cond ")"); }() )
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT(cond) SkASSERT_RELEASE(cond)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>( (cond) ? (void)0 : [&]{ SkDebugf(fmt"\n", __VA_ARGS__); SK_ABORT("assert(" #cond ")"); }() )
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAIL(message) SK_ABORT(message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...) SkASSERTF(false, fmt, ##__VA_ARGS__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGCODE(...) __VA_ARGS__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGF(...) SkDebugf(__VA_ARGS__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAssertResult(cond) SkASSERT(cond)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT(cond) static_cast<void>(0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>(0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAIL(message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGCODE(...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGF(...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAssertResult(cond) if (cond) {} do {} while(false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ARRAY_COUNT(array) (sizeof(SkArrayCountHelper(array)))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define __inline static __inline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAs2sCompliment(x) SkFloatAs2sCompliment(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_sqrt(x) sqrtf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_sin(x) sinf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_cos(x) cosf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_tan(x) tanf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor(x) floorf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil(x) ceilf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_trunc(x) truncf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_acos(x) static_cast<float>(acos(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_asin(x) static_cast<float>(asin(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_acos(x) acosf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_asin(x) asinf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_atan2(y,x) atan2f(y,x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_abs(x) fabsf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_copysign(x, y) copysignf(x, y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_mod(x,y) fmodf(x,y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_exp(x) expf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_log(x) logf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round(x) sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_log2(x) log2f(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_isnan(a) sk_float_isnan(a)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MinS32FitsInFloat -SK_MaxS32FitsInFloat
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MaxS64FitsInFloat (SK_MaxS64 >> (63-24) << (63-24))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MinS64FitsInFloat -SK_MaxS64FitsInFloat
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor2int(x) sk_float_saturate2int(sk_float_floor(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round2int(x) sk_float_saturate2int(sk_float_floor((x) + 0.5f))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil2int(x) sk_float_saturate2int(sk_float_ceil(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor2int_no_saturate(x) (int)sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round2int_no_saturate(x) (int)sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil2int_no_saturate(x) (int)sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_floor(x) floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_round(x) floor((x) + 0.5)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_ceil(x) ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_floor2int(x) (int)floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_round2int(x) (int)floor((x) + 0.5)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_ceil2int(x) (int)ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatNaN NAN
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatInfinity (+INFINITY)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatNegativeInfinity (-INFINITY)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FLT_DECIMAL_DIG FLT_DECIMAL_DIG
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarMax 3.402823466e+38f
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarInfinity SK_FloatInfinity
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNegativeInfinity SK_FloatNegativeInfinity
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNaN SK_FloatNaN
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFloorToScalar(x) sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCeilToScalar(x) sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarRoundToScalar(x) sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTruncToScalar(x) sk_float_trunc(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFloorToInt(x) sk_float_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCeilToInt(x) sk_float_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarRoundToInt(x) sk_float_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAbs(x) sk_float_abs(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCopySign(x, y) sk_float_copysign(x, y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarMod(x, y) sk_float_mod(x,y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarSqrt(x) sk_float_sqrt(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarPow(b, e) sk_float_pow(b, e)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarSin(radians) (float)sk_float_sin(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCos(radians) (float)sk_float_cos(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTan(radians) (float)sk_float_tan(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarASin(val) (float)sk_float_asin(val)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarACos(val) (float)sk_float_acos(val)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarATan2(y, x) (float)sk_float_atan2(y,x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarExp(x) (float)sk_float_exp(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarLog(x) (float)sk_float_log(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarLog2(x) (float)sk_float_log2(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToScalar(x) static_cast<SkScalar>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToFloat(x) static_cast<float>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTruncToInt(x) sk_float_saturate2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToFloat(x) static_cast<float>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkFloatToScalar(x) static_cast<SkScalar>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToDouble(x) static_cast<double>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDoubleToScalar(x) sk_double_to_float(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarMin (-SK_ScalarMax)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarDiv(numer, denom) sk_ieee_float_divide(numer, denom)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFastInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAve(a, b) (((a) + (b)) * SK_ScalarHalf)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarHalf(a) ((a) * SK_ScalarHalf)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDegreesToRadians(degrees) ((degrees) * (SK_ScalarPI / 180))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkRadiansToDegrees(radians) ((radians) * (180 / SK_ScalarPI))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNearlyZero (SK_Scalar1 / (1 << 12))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloor(x) sk_double_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeil(x) sk_double_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRound(x) sk_double_round(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloorToInt(x) sk_double_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeilToInt(x) sk_double_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRoundToInt(x) sk_double_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloor(x) sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeil(x) sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRound(x) sk_float_round(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloorToInt(x) sk_float_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeilToInt(x) sk_float_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRoundToInt(x) sk_float_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToMScalar(n) static_cast<SkMScalar>(n)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarToScalar(x) SkMScalarToFloat(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToMScalar(x) SkFloatToMScalar(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define LOG_0 LOG_ERROR
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LOG_STREAM(severity) ::fml::LogMessage(::fml::LOG_##severity, __FILE__, __LINE__, nullptr).stream()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LAZY_STREAM(stream, condition) !(condition) ? (void)0 : ::fml::LogMessageVoidify() & (stream)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_EAT_STREAM_PARAMETERS(ignored) true || (ignored) ? (void)0 : ::fml::LogMessageVoidify() & ::fml::LogMessage(::fml::LOG_FATAL, 0, 0, nullptr).stream()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LOG_IS_ON(severity) (::fml::ShouldCreateLogMessage(::fml::LOG_##severity))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LOG(severity) FML_LAZY_STREAM(FML_LOG_STREAM(severity), FML_LOG_IS_ON(severity))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_CHECK(condition) FML_LAZY_STREAM( ::fml::LogMessage(::fml::LOG_FATAL, __FILE__, __LINE__, #condition).stream(), !(condition))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_VLOG_IS_ON(verbose_level) ((verbose_level) <= ::fml::GetVlogVerbosity())
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_VLOG_STREAM(verbose_level) ::fml::LogMessage(-verbose_level, __FILE__, __LINE__, nullptr).stream()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_VLOG(verbose_level) FML_LAZY_STREAM(FML_VLOG_STREAM(verbose_level), FML_VLOG_IS_ON(verbose_level))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DLOG(severity) FML_LOG(severity)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DCHECK(condition) FML_CHECK(condition)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DLOG(severity) FML_EAT_STREAM_PARAMETERS(true)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DCHECK(condition) FML_EAT_STREAM_PARAMETERS(condition)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_NOTREACHED() FML_DCHECK(false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_NOTIMPLEMENTED() FML_LOG(ERROR) << "Not implemented in: " << __PRETTY_FUNCTION__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_STRING(X) GR_STRING_IMPL(X)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_STRING_IMPL(X) #X
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CONCAT(X,Y) GR_CONCAT_IMPL(X,Y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CONCAT_IMPL(X,Y) X##Y
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_FILE_AND_LINE_STR __FILE__ "(" GR_STRING(__LINE__) ") : "
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_WARN(MSG) (GR_FILE_AND_LINE_STR "WARNING: " MSG)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_WARN(MSG) ("WARNING: " MSG)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_ALWAYSBREAK SkNO_RETURN_HINT(); __debugbreak()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_ALWAYSBREAK SkNO_RETURN_HINT(); *((int*)(int64_t)(int32_t)0xbeefcafe) = 0;
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DEBUGBREAK GR_ALWAYSBREAK
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_ALWAYSASSERT(COND) do { if (!(COND)) { SkDebugf("%s %s failed\n", GR_FILE_AND_LINE_STR, #COND); GR_ALWAYSBREAK; } } while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DEBUGASSERT(COND) GR_ALWAYSASSERT(COND)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DEBUGASSERT(COND)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GrAlwaysAssert(COND) GR_ALWAYSASSERT(COND)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_STATIC_ASSERT(CONDITION) static_assert(CONDITION, "bug")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_MAKE_BITFIELD_OPS(X) inline X operator |(X a, X b) { return (X) (+a | +b); } inline X& operator |=(X& a, X b) { return (a = a | b); } inline X operator &(X a, X b) { return (X) (+a & +b); } inline X& operator &=(X& a, X b) { return (a = a & b); } template <typename T> inline X operator &(T a, X b) { return (X) (+a & +b); } template <typename T> inline X operator &(X a, T b) { return (X) (+a & +b); }
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DECL_BITFIELD_OPS_FRIENDS(X) friend X operator |(X a, X b); friend X& operator |=(X& a, X b); friend X operator &(X a, X b); friend X& operator &=(X& a, X b); template <typename T> friend X operator &(T a, X b); template <typename T> friend X operator &(X a, T b);
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_MAKE_BITFIELD_CLASS_OPS(X) constexpr GrTFlagsMask<X> operator~(X a) { return GrTFlagsMask<X>(~static_cast<int>(a)); } constexpr X operator|(X a, X b) { return static_cast<X>(static_cast<int>(a) | static_cast<int>(b)); } inline X& operator|=(X& a, X b) { return (a = a | b); } constexpr bool operator&(X a, X b) { return SkToBool(static_cast<int>(a) & static_cast<int>(b)); }
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DECL_BITFIELD_CLASS_OPS_FRIENDS(X) friend constexpr GrTFlagsMask<X> operator ~(X); friend constexpr X operator |(X, X); friend X& operator |=(X&, X); friend constexpr bool operator &(X, X);
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_MAX(a, b) (((b) < (a)) ? (a) : (b))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_MIN(a, b) (((b) < (a)) ? (b) : (a))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_DIV_ROUND_UP(X, Y) (((X) + ((Y)-1)) / (Y))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_ALIGN_UP(X, A) (GR_CT_DIV_ROUND_UP((X),(A)) * (A))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorSetRGB(r, g, b) SkColorSetARGB(0xFF, r, g, b)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetA(color) (((color) >> 24) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetR(color) (((color) >> 16) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetG(color) (((color) >> 8) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetB(color) (((color) >> 0) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_REGISTER_FLATTENABLE(type) SkFlattenable::Register(#type, type::CreateProc);
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FLATTENABLE_HOOKS(type) static sk_sp<SkFlattenable> CreateProc(SkReadBuffer&); friend class SkFlattenable::PrivateInitializer; Factory getFactory() const override { return type::CreateProc; } const char* getTypeName() const override { return #type; }

	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define DEF_PRINTER(x) std::ostream& operator<<(std::ostream&, const x&);

	public static std::ostream operator << (std::ostream os, flow.MatrixDecomposition m)
	{
	  if (!m.IsValid())
	  {
		os << "Invalid Matrix!" << std::endl;
		return os;
	  }

	  os << "Translation (x, y, z): " << m.translation() << std::endl;
	  os << "Scale (z, y, z): " << m.scale() << std::endl;
	  os << "Shear (zy, yz, zx): " << m.shear() << std::endl;
	  os << "Perspective (x, y, z, w): " << m.perspective() << std::endl;
	  os << "Rotation (x, y, z, w): " << m.rotation() << std::endl;

	  return os;
	}
	public static std::ostream operator << (std::ostream os, RasterCacheKey<uint> k)
	{
	  os << "Picture: " << k.id() << " matrix: " << k.matrix();
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkISize size)
	{
	  os << size.width() << ", " << size.height();
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkMatrix m)
	{
	  SkString string = new SkString();
	  string.printf("[%8.4f %8.4f %8.4f][%8.4f %8.4f %8.4f][%8.4f %8.4f %8.4f]", m[0], m[1], m[2], m[3], m[4], m[5], m[6], m[7], m[8]);
	  os << string.c_str();
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkMatrix44 m)
	{
	  os << m.get(0, 0) << ", " << m.get(0, 1) << ", " << m.get(0, 2) << ", " << m.get(0, 3) << std::endl;
	  os << m.get(1, 0) << ", " << m.get(1, 1) << ", " << m.get(1, 2) << ", " << m.get(1, 3) << std::endl;
	  os << m.get(2, 0) << ", " << m.get(2, 1) << ", " << m.get(2, 2) << ", " << m.get(2, 3) << std::endl;
	  os << m.get(3, 0) << ", " << m.get(3, 1) << ", " << m.get(3, 2) << ", " << m.get(3, 3);
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkPoint r)
	{
	  os << "XY: " << r.fX << ", " << r.fY;
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkRect r)
	{
	  os << "LTRB: " << r.fLeft << ", " << r.fTop << ", " << r.fRight << ", " << r.fBottom;
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkRRect r)
	{
	  os << "LTRB: " << r.rect().fLeft << ", " << r.rect().fTop << ", " << r.rect().fRight << ", " << r.rect().fBottom;
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkPoint3 v)
	{
	  os << v.x() << ", " << v.y() << ", " << v.z();
	  return os;
	}
	public static std::ostream operator << (std::ostream os, SkVector4 v)
	{
	  os << v.fData[0] << ", " << v.fData[1] << ", " << v.fData[2] << ", " << v.fData[3];
	  return os;
	}




	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WHEN(condition, T) skstd::enable_if_t<!!(condition), T>
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkStrAppendS32_MaxSize (SkStrAppendU32_MaxSize + 1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkStrAppendS64_MaxSize (SkStrAppendU64_MaxSize + 1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkStrAppendScalar SkStrAppendFloat


	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_EMBEDDER_ONLY [[deprecated]]
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY(TypeName) TypeName(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_ASSIGN(TypeName) TypeName& operator=(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_MOVE(TypeName) TypeName(TypeName&&) = delete; TypeName& operator=(TypeName&&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY_AND_ASSIGN(TypeName) TypeName(const TypeName&) = delete; TypeName& operator=(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY_ASSIGN_AND_MOVE(TypeName) TypeName(const TypeName&) = delete; TypeName(TypeName&&) = delete; TypeName& operator=(const TypeName&) = delete; TypeName& operator=(TypeName&&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_IMPLICIT_CONSTRUCTORS(TypeName) TypeName() = delete; FML_DISALLOW_COPY_ASSIGN_AND_MOVE(TypeName)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_CONCAT(X, Y) SK_MACRO_CONCAT_IMPL_PRIV(X, Y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_CONCAT_IMPL_PRIV(X, Y) X ## Y
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_APPEND_LINE(name) SK_MACRO_CONCAT(name, __LINE__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_REQUIRE_LOCAL_VAR(classname) static_assert(false, "missing name for " #classname)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_BEGIN_REQUIRE_DENSE _Pragma("GCC diagnostic push") _Pragma("GCC diagnostic error \"-Wpadded\"")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_END_REQUIRE_DENSE _Pragma("GCC diagnostic pop")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_INIT_TO_AVOID_WARNING = 0
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG1(arg1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG2(arg1, arg2)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG3(arg1, arg2, arg3)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_OS_EMBEDDED @CONFIG_EMBEDDED@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_OS_IPHONE @CONFIG_IPHONE@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_IPHONE_SIMULATOR @CONFIG_IPHONE_SIMULATOR@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_RESTRICT __restrict
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_RESTRICT __restrict__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX512
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE42
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE41
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSSE3
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE3
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE1
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_API __declspec(dllexport)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_API __declspec(dllimport)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) __has_feature(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) 0
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkNO_RETURN_HINT() do {} while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK() DumpStackTrace(0, SkDebugfForDumpStackTrace, nullptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s(%d): fatal error: \"%s\"\n", __FILE__, __LINE__, message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s:%d: fatal error: \"%s\"\n", __FILE__, __LINE__, message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ABORT(message) do { SkNO_RETURN_HINT(); SK_DUMP_LINE_FORMAT(message); SK_DUMP_GOOGLE3_STACK(); sk_abort_no_print(); } while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_COLOR_MATCHES_PMCOLOR_BYTE_ORDER (SK_A32_SHIFT == 24 && SK_R32_SHIFT == 16 && SK_G32_SHIFT == 8 && SK_B32_SHIFT == 0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C3 ## 32_SHIFT == 0 && SK_ ## C2 ## 32_SHIFT == 8 && SK_ ## C1 ## 32_SHIFT == 16 && SK_ ## C0 ## 32_SHIFT == 24)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C0 ## 32_SHIFT == 0 && SK_ ## C1 ## 32_SHIFT == 8 && SK_ ## C2 ## 32_SHIFT == 16 && SK_ ## C3 ## 32_SHIFT == 24)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_UNUSED __pragma(warning(suppress:4189))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_UNUSED SK_ATTRIBUTE(unused)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ALWAYS_INLINE __forceinline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ALWAYS_INLINE SK_ATTRIBUTE(always_inline) inline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NEVER_INLINE __declspec(noinline)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NEVER_INLINE SK_ATTRIBUTE(noinline)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr) __builtin_prefetch(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) __builtin_prefetch(ptr, 1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_GAMMA_EXPONENT (0.0f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HISTOGRAM_BOOLEAN(name, value)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HISTOGRAM_ENUMERATION(name, value, boundary_value)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT_RELEASE(cond) static_cast<void>( (cond) ? (void)0 : []{ SK_ABORT("assert(" #cond ")"); }() )
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT(cond) SkASSERT_RELEASE(cond)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>( (cond) ? (void)0 : [&]{ SkDebugf(fmt"\n", __VA_ARGS__); SK_ABORT("assert(" #cond ")"); }() )
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAIL(message) SK_ABORT(message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...) SkASSERTF(false, fmt, ##__VA_ARGS__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGCODE(...) __VA_ARGS__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGF(...) SkDebugf(__VA_ARGS__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAssertResult(cond) SkASSERT(cond)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT(cond) static_cast<void>(0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>(0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAIL(message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGCODE(...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGF(...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAssertResult(cond) if (cond) {} do {} while(false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ARRAY_COUNT(array) (sizeof(SkArrayCountHelper(array)))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define __inline static __inline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAs2sCompliment(x) SkFloatAs2sCompliment(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_sqrt(x) sqrtf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_sin(x) sinf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_cos(x) cosf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_tan(x) tanf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor(x) floorf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil(x) ceilf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_trunc(x) truncf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_acos(x) static_cast<float>(acos(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_asin(x) static_cast<float>(asin(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_acos(x) acosf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_asin(x) asinf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_atan2(y,x) atan2f(y,x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_abs(x) fabsf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_copysign(x, y) copysignf(x, y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_mod(x,y) fmodf(x,y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_exp(x) expf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_log(x) logf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round(x) sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_log2(x) log2f(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_isnan(a) sk_float_isnan(a)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MinS32FitsInFloat -SK_MaxS32FitsInFloat
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MaxS64FitsInFloat (SK_MaxS64 >> (63-24) << (63-24))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MinS64FitsInFloat -SK_MaxS64FitsInFloat
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor2int(x) sk_float_saturate2int(sk_float_floor(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round2int(x) sk_float_saturate2int(sk_float_floor((x) + 0.5f))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil2int(x) sk_float_saturate2int(sk_float_ceil(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor2int_no_saturate(x) (int)sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round2int_no_saturate(x) (int)sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil2int_no_saturate(x) (int)sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_floor(x) floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_round(x) floor((x) + 0.5)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_ceil(x) ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_floor2int(x) (int)floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_round2int(x) (int)floor((x) + 0.5)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_ceil2int(x) (int)ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatNaN NAN
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatInfinity (+INFINITY)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatNegativeInfinity (-INFINITY)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FLT_DECIMAL_DIG FLT_DECIMAL_DIG
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarMax 3.402823466e+38f
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarInfinity SK_FloatInfinity
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNegativeInfinity SK_FloatNegativeInfinity
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNaN SK_FloatNaN
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFloorToScalar(x) sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCeilToScalar(x) sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarRoundToScalar(x) sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTruncToScalar(x) sk_float_trunc(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFloorToInt(x) sk_float_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCeilToInt(x) sk_float_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarRoundToInt(x) sk_float_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAbs(x) sk_float_abs(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCopySign(x, y) sk_float_copysign(x, y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarMod(x, y) sk_float_mod(x,y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarSqrt(x) sk_float_sqrt(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarPow(b, e) sk_float_pow(b, e)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarSin(radians) (float)sk_float_sin(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCos(radians) (float)sk_float_cos(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTan(radians) (float)sk_float_tan(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarASin(val) (float)sk_float_asin(val)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarACos(val) (float)sk_float_acos(val)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarATan2(y, x) (float)sk_float_atan2(y,x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarExp(x) (float)sk_float_exp(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarLog(x) (float)sk_float_log(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarLog2(x) (float)sk_float_log2(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToScalar(x) static_cast<SkScalar>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToFloat(x) static_cast<float>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTruncToInt(x) sk_float_saturate2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToFloat(x) static_cast<float>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkFloatToScalar(x) static_cast<SkScalar>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToDouble(x) static_cast<double>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDoubleToScalar(x) sk_double_to_float(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarMin (-SK_ScalarMax)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarDiv(numer, denom) sk_ieee_float_divide(numer, denom)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFastInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAve(a, b) (((a) + (b)) * SK_ScalarHalf)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarHalf(a) ((a) * SK_ScalarHalf)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDegreesToRadians(degrees) ((degrees) * (SK_ScalarPI / 180))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkRadiansToDegrees(radians) ((radians) * (180 / SK_ScalarPI))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNearlyZero (SK_Scalar1 / (1 << 12))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloor(x) sk_double_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeil(x) sk_double_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRound(x) sk_double_round(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloorToInt(x) sk_double_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeilToInt(x) sk_double_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRoundToInt(x) sk_double_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloor(x) sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeil(x) sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRound(x) sk_float_round(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloorToInt(x) sk_float_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeilToInt(x) sk_float_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRoundToInt(x) sk_float_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToMScalar(n) static_cast<SkMScalar>(n)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarToScalar(x) SkMScalarToFloat(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToMScalar(x) SkFloatToMScalar(x)

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(MatrixDecomposition UnnamedParameter, Rotation UnnamedParameter2)
	{
	  SkMatrix44 matrix = SkMatrix44.I();

	  var angle = M_PI_4;
	  matrix.setRotateAbout(0.0, 0.0, 1.0, angle);

	  flow.MatrixDecomposition decomposition = new flow.MatrixDecomposition(matrix);
	  ASSERT_TRUE(decomposition.IsValid());

	  var sine = Math.Sin(angle * 0.5);

	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[0]);
	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[1]);
	  ASSERT_FLOAT_EQ(sine, decomposition.rotation().fData[2]);
	  ASSERT_FLOAT_EQ(Math.Cos(angle * 0.5), decomposition.rotation().fData[3]);
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(MatrixDecomposition UnnamedParameter, Scale UnnamedParameter2)
	{
	  SkMatrix44 matrix = SkMatrix44.I();

	  const var scale = 5.0;
	  matrix.setScale(scale + 0, scale + 1, scale + 2);

	  flow.MatrixDecomposition decomposition = new flow.MatrixDecomposition(matrix);
	  ASSERT_TRUE(decomposition.IsValid());

	  ASSERT_FLOAT_EQ(scale + 0, decomposition.scale().fX);
	  ASSERT_FLOAT_EQ(scale + 1, decomposition.scale().fY);
	  ASSERT_FLOAT_EQ(scale + 2, decomposition.scale().fZ);
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(MatrixDecomposition UnnamedParameter, Translate UnnamedParameter2)
	{
	  SkMatrix44 matrix = SkMatrix44.I();

	  const var translate = 125.0;
	  matrix.setTranslate(translate + 0, translate + 1, translate + 2);

	  flow.MatrixDecomposition decomposition = new flow.MatrixDecomposition(matrix);
	  ASSERT_TRUE(decomposition.IsValid());

	  ASSERT_FLOAT_EQ(translate + 0, decomposition.translation().fX);
	  ASSERT_FLOAT_EQ(translate + 1, decomposition.translation().fY);
	  ASSERT_FLOAT_EQ(translate + 2, decomposition.translation().fZ);
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(MatrixDecomposition UnnamedParameter, Combination UnnamedParameter2)
	{
	  SkMatrix44 matrix = SkMatrix44.I();

	  var rotation = M_PI_4;
	  const var scale = 5;
	  const var translate = 125.0;

	  SkMatrix44 m1 = SkMatrix44.I();
	  m1.setRotateAbout(0, 0, 1, rotation);

	  SkMatrix44 m2 = SkMatrix44.I();
	  m2.setScale(scale);

	  SkMatrix44 m3 = SkMatrix44.I();
	  m3.setTranslate(translate, translate, translate);

	  SkMatrix44 combined = m3 * m2 * m1;

	  flow.MatrixDecomposition decomposition = new flow.MatrixDecomposition(combined);
	  ASSERT_TRUE(decomposition.IsValid());

	  ASSERT_FLOAT_EQ(translate, decomposition.translation().fX);
	  ASSERT_FLOAT_EQ(translate, decomposition.translation().fY);
	  ASSERT_FLOAT_EQ(translate, decomposition.translation().fZ);

	  ASSERT_FLOAT_EQ(scale, decomposition.scale().fX);
	  ASSERT_FLOAT_EQ(scale, decomposition.scale().fY);
	  ASSERT_FLOAT_EQ(scale, decomposition.scale().fZ);

	  var sine = Math.Sin(rotation * 0.5);

	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[0]);
	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[1]);
	  ASSERT_FLOAT_EQ(sine, decomposition.rotation().fData[2]);
	  ASSERT_FLOAT_EQ(Math.Cos(rotation * 0.5), decomposition.rotation().fData[3]);
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(MatrixDecomposition UnnamedParameter, ScaleFloatError UnnamedParameter2)
	{
	  for (float scale = 0.0001f; scale < 2.0f; scale += 0.000001f)
	  {
		SkMatrix44 matrix = SkMatrix44.I();
		matrix.setScale(scale, scale, 1.0f);

		flow.MatrixDecomposition decomposition3 = new flow.MatrixDecomposition(matrix);
		ASSERT_TRUE(decomposition3.IsValid());

		ASSERT_FLOAT_EQ(scale, decomposition3.scale().fX);
		ASSERT_FLOAT_EQ(scale, decomposition3.scale().fY);
		ASSERT_FLOAT_EQ(1.0f, decomposition3.scale().fZ);
		ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[0]);
		ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[1]);
		ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[2]);
	  }

	  SkMatrix44 matrix = SkMatrix44.I();
	  const var scale = 1.7734375f;
	  matrix.setScale(scale, scale, 1.0f);

	  // Bug upper bound (empirical)
	  const var scale2 = 1.773437559603f;
	  SkMatrix44 matrix2 = SkMatrix44.I();
	  matrix2.setScale(scale2, scale2, 1.0f);

	  // Bug lower bound (empirical)
	  const var scale3 = 1.7734374403954f;
	  SkMatrix44 matrix3 = SkMatrix44.I();
	  matrix3.setScale(scale3, scale3, 1.0f);

	  flow.MatrixDecomposition decomposition = new flow.MatrixDecomposition(matrix);
	  ASSERT_TRUE(decomposition.IsValid());

	  flow.MatrixDecomposition decomposition2 = new flow.MatrixDecomposition(matrix2);
	  ASSERT_TRUE(decomposition2.IsValid());

	  flow.MatrixDecomposition decomposition3 = new flow.MatrixDecomposition(matrix3);
	  ASSERT_TRUE(decomposition3.IsValid());

	  ASSERT_FLOAT_EQ(scale, decomposition.scale().fX);
	  ASSERT_FLOAT_EQ(scale, decomposition.scale().fY);
	  ASSERT_FLOAT_EQ(1.0f, decomposition.scale().fZ);
	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[0]);
	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[1]);
	  ASSERT_FLOAT_EQ(0, decomposition.rotation().fData[2]);

	  ASSERT_FLOAT_EQ(scale2, decomposition2.scale().fX);
	  ASSERT_FLOAT_EQ(scale2, decomposition2.scale().fY);
	  ASSERT_FLOAT_EQ(1.0f, decomposition2.scale().fZ);
	  ASSERT_FLOAT_EQ(0, decomposition2.rotation().fData[0]);
	  ASSERT_FLOAT_EQ(0, decomposition2.rotation().fData[1]);
	  ASSERT_FLOAT_EQ(0, decomposition2.rotation().fData[2]);

	  ASSERT_FLOAT_EQ(scale3, decomposition3.scale().fX);
	  ASSERT_FLOAT_EQ(scale3, decomposition3.scale().fY);
	  ASSERT_FLOAT_EQ(1.0f, decomposition3.scale().fZ);
	  ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[0]);
	  ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[1]);
	  ASSERT_FLOAT_EQ(0, decomposition3.rotation().fData[2]);
	}

	// Copyright 2017 The Chromium Authors. All rights reserved.
	// Use of this source code is governed by a BSD-style license that can be
	// found in the LICENSE file.

	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_EMBEDDER_ONLY [[deprecated]]
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY(TypeName) TypeName(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_ASSIGN(TypeName) TypeName& operator=(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_MOVE(TypeName) TypeName(TypeName&&) = delete; TypeName& operator=(TypeName&&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY_AND_ASSIGN(TypeName) TypeName(const TypeName&) = delete; TypeName& operator=(const TypeName&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_COPY_ASSIGN_AND_MOVE(TypeName) TypeName(const TypeName&) = delete; TypeName(TypeName&&) = delete; TypeName& operator=(const TypeName&) = delete; TypeName& operator=(TypeName&&) = delete
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DISALLOW_IMPLICIT_CONSTRUCTORS(TypeName) TypeName() = delete; FML_DISALLOW_COPY_ASSIGN_AND_MOVE(TypeName)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_CONCAT(X, Y) SK_MACRO_CONCAT_IMPL_PRIV(X, Y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_CONCAT_IMPL_PRIV(X, Y) X ## Y
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MACRO_APPEND_LINE(name) SK_MACRO_CONCAT(name, __LINE__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_REQUIRE_LOCAL_VAR(classname) static_assert(false, "missing name for " #classname)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_BEGIN_REQUIRE_DENSE _Pragma("GCC diagnostic push") _Pragma("GCC diagnostic error \"-Wpadded\"")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_END_REQUIRE_DENSE _Pragma("GCC diagnostic pop")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_INIT_TO_AVOID_WARNING = 0
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG1(arg1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG2(arg1, arg2)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NOTHING_ARG3(arg1, arg2, arg3)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_OS_EMBEDDED @CONFIG_EMBEDDED@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_OS_IPHONE @CONFIG_IPHONE@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define TARGET_IPHONE_SIMULATOR @CONFIG_IPHONE_SIMULATOR@
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_RESTRICT __restrict
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_RESTRICT __restrict__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX512
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE42
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE41
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSSE3
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE3
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE1
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_API __declspec(dllexport)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_API __declspec(dllimport)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) __has_feature(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) 0
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkNO_RETURN_HINT() do {} while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK() DumpStackTrace(0, SkDebugfForDumpStackTrace, nullptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s(%d): fatal error: \"%s\"\n", __FILE__, __LINE__, message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s:%d: fatal error: \"%s\"\n", __FILE__, __LINE__, message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ABORT(message) do { SkNO_RETURN_HINT(); SK_DUMP_LINE_FORMAT(message); SK_DUMP_GOOGLE3_STACK(); sk_abort_no_print(); } while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_COLOR_MATCHES_PMCOLOR_BYTE_ORDER (SK_A32_SHIFT == 24 && SK_R32_SHIFT == 16 && SK_G32_SHIFT == 8 && SK_B32_SHIFT == 0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C3 ## 32_SHIFT == 0 && SK_ ## C2 ## 32_SHIFT == 8 && SK_ ## C1 ## 32_SHIFT == 16 && SK_ ## C0 ## 32_SHIFT == 24)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C0 ## 32_SHIFT == 0 && SK_ ## C1 ## 32_SHIFT == 8 && SK_ ## C2 ## 32_SHIFT == 16 && SK_ ## C3 ## 32_SHIFT == 24)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_UNUSED __pragma(warning(suppress:4189))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_UNUSED SK_ATTRIBUTE(unused)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ALWAYS_INLINE __forceinline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ALWAYS_INLINE SK_ATTRIBUTE(always_inline) inline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NEVER_INLINE __declspec(noinline)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_NEVER_INLINE SK_ATTRIBUTE(noinline)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr) __builtin_prefetch(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) __builtin_prefetch(ptr, 1)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PREFETCH(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_GAMMA_EXPONENT (0.0f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HISTOGRAM_BOOLEAN(name, value)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_HISTOGRAM_ENUMERATION(name, value, boundary_value)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT_RELEASE(cond) static_cast<void>( (cond) ? (void)0 : []{ SK_ABORT("assert(" #cond ")"); }() )
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT(cond) SkASSERT_RELEASE(cond)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>( (cond) ? (void)0 : [&]{ SkDebugf(fmt"\n", __VA_ARGS__); SK_ABORT("assert(" #cond ")"); }() )
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAIL(message) SK_ABORT(message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...) SkASSERTF(false, fmt, ##__VA_ARGS__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGCODE(...) __VA_ARGS__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGF(...) SkDebugf(__VA_ARGS__)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAssertResult(cond) SkASSERT(cond)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERT(cond) static_cast<void>(0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>(0)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAIL(message)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGCODE(...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDEBUGF(...)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAssertResult(cond) if (cond) {} do {} while(false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ARRAY_COUNT(array) (sizeof(SkArrayCountHelper(array)))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define __inline static __inline
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAs2sCompliment(x) SkFloatAs2sCompliment(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_sqrt(x) sqrtf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_sin(x) sinf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_cos(x) cosf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_tan(x) tanf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor(x) floorf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil(x) ceilf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_trunc(x) truncf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_acos(x) static_cast<float>(acos(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_asin(x) static_cast<float>(asin(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_acos(x) acosf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_asin(x) asinf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_atan2(y,x) atan2f(y,x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_abs(x) fabsf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_copysign(x, y) copysignf(x, y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_mod(x,y) fmodf(x,y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_exp(x) expf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_log(x) logf(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round(x) sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_log2(x) log2f(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_isnan(a) sk_float_isnan(a)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MinS32FitsInFloat -SK_MaxS32FitsInFloat
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MaxS64FitsInFloat (SK_MaxS64 >> (63-24) << (63-24))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_MinS64FitsInFloat -SK_MaxS64FitsInFloat
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor2int(x) sk_float_saturate2int(sk_float_floor(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round2int(x) sk_float_saturate2int(sk_float_floor((x) + 0.5f))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil2int(x) sk_float_saturate2int(sk_float_ceil(x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_floor2int_no_saturate(x) (int)sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_round2int_no_saturate(x) (int)sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_float_ceil2int_no_saturate(x) (int)sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_floor(x) floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_round(x) floor((x) + 0.5)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_ceil(x) ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_floor2int(x) (int)floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_round2int(x) (int)floor((x) + 0.5)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define sk_double_ceil2int(x) (int)ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatNaN NAN
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatInfinity (+INFINITY)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FloatNegativeInfinity (-INFINITY)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FLT_DECIMAL_DIG FLT_DECIMAL_DIG
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarMax 3.402823466e+38f
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarInfinity SK_FloatInfinity
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNegativeInfinity SK_FloatNegativeInfinity
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNaN SK_FloatNaN
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFloorToScalar(x) sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCeilToScalar(x) sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarRoundToScalar(x) sk_float_floor((x) + 0.5f)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTruncToScalar(x) sk_float_trunc(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFloorToInt(x) sk_float_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCeilToInt(x) sk_float_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarRoundToInt(x) sk_float_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAbs(x) sk_float_abs(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCopySign(x, y) sk_float_copysign(x, y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarMod(x, y) sk_float_mod(x,y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarSqrt(x) sk_float_sqrt(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarPow(b, e) sk_float_pow(b, e)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarSin(radians) (float)sk_float_sin(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarCos(radians) (float)sk_float_cos(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTan(radians) (float)sk_float_tan(radians)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarASin(val) (float)sk_float_asin(val)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarACos(val) (float)sk_float_acos(val)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarATan2(y, x) (float)sk_float_atan2(y,x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarExp(x) (float)sk_float_exp(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarLog(x) (float)sk_float_log(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarLog2(x) (float)sk_float_log2(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToScalar(x) static_cast<SkScalar>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToFloat(x) static_cast<float>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarTruncToInt(x) sk_float_saturate2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToFloat(x) static_cast<float>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkFloatToScalar(x) static_cast<SkScalar>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToDouble(x) static_cast<double>(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDoubleToScalar(x) sk_double_to_float(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarMin (-SK_ScalarMax)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarDiv(numer, denom) sk_ieee_float_divide(numer, denom)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarFastInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarAve(a, b) (((a) + (b)) * SK_ScalarHalf)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarHalf(a) ((a) * SK_ScalarHalf)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkDegreesToRadians(degrees) ((degrees) * (SK_ScalarPI / 180))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkRadiansToDegrees(radians) ((radians) * (180 / SK_ScalarPI))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_ScalarNearlyZero (SK_Scalar1 / (1 << 12))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloor(x) sk_double_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeil(x) sk_double_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRound(x) sk_double_round(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloorToInt(x) sk_double_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeilToInt(x) sk_double_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRoundToInt(x) sk_double_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloor(x) sk_float_floor(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeil(x) sk_float_ceil(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRound(x) sk_float_round(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarFloorToInt(x) sk_float_floor2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarCeilToInt(x) sk_float_ceil2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarRoundToInt(x) sk_float_round2int(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkIntToMScalar(n) static_cast<SkMScalar>(n)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkMScalarToScalar(x) SkMScalarToFloat(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkScalarToMScalar(x) SkFloatToMScalar(x)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorSetRGB(r, g, b) SkColorSetARGB(0xFF, r, g, b)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetA(color) (((color) >> 24) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetR(color) (((color) >> 16) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetG(color) (((color) >> 8) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkColorGetB(color) (((color) >> 0) & 0xFF)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SkAutoCanvasRestore(...) SK_REQUIRE_LOCAL_VAR(SkAutoCanvasRestore)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define LOG_0 LOG_ERROR
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LOG_STREAM(severity) ::fml::LogMessage(::fml::LOG_##severity, __FILE__, __LINE__, nullptr).stream()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LAZY_STREAM(stream, condition) !(condition) ? (void)0 : ::fml::LogMessageVoidify() & (stream)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_EAT_STREAM_PARAMETERS(ignored) true || (ignored) ? (void)0 : ::fml::LogMessageVoidify() & ::fml::LogMessage(::fml::LOG_FATAL, 0, 0, nullptr).stream()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LOG_IS_ON(severity) (::fml::ShouldCreateLogMessage(::fml::LOG_##severity))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_LOG(severity) FML_LAZY_STREAM(FML_LOG_STREAM(severity), FML_LOG_IS_ON(severity))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_CHECK(condition) FML_LAZY_STREAM( ::fml::LogMessage(::fml::LOG_FATAL, __FILE__, __LINE__, #condition).stream(), !(condition))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_VLOG_IS_ON(verbose_level) ((verbose_level) <= ::fml::GetVlogVerbosity())
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_VLOG_STREAM(verbose_level) ::fml::LogMessage(-verbose_level, __FILE__, __LINE__, nullptr).stream()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_VLOG(verbose_level) FML_LAZY_STREAM(FML_VLOG_STREAM(verbose_level), FML_VLOG_IS_ON(verbose_level))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DLOG(severity) FML_LOG(severity)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DCHECK(condition) FML_CHECK(condition)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DLOG(severity) FML_EAT_STREAM_PARAMETERS(true)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DCHECK(condition) FML_EAT_STREAM_PARAMETERS(condition)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_NOTREACHED() FML_DCHECK(false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_NOTIMPLEMENTED() FML_LOG(ERROR) << "Not implemented in: " << __PRETTY_FUNCTION__
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_STRING(X) GR_STRING_IMPL(X)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_STRING_IMPL(X) #X
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CONCAT(X,Y) GR_CONCAT_IMPL(X,Y)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CONCAT_IMPL(X,Y) X##Y
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_FILE_AND_LINE_STR __FILE__ "(" GR_STRING(__LINE__) ") : "
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_WARN(MSG) (GR_FILE_AND_LINE_STR "WARNING: " MSG)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_WARN(MSG) ("WARNING: " MSG)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_ALWAYSBREAK SkNO_RETURN_HINT(); __debugbreak()
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_ALWAYSBREAK SkNO_RETURN_HINT(); *((int*)(int64_t)(int32_t)0xbeefcafe) = 0;
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DEBUGBREAK GR_ALWAYSBREAK
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_ALWAYSASSERT(COND) do { if (!(COND)) { SkDebugf("%s %s failed\n", GR_FILE_AND_LINE_STR, #COND); GR_ALWAYSBREAK; } } while (false)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DEBUGASSERT(COND) GR_ALWAYSASSERT(COND)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DEBUGASSERT(COND)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GrAlwaysAssert(COND) GR_ALWAYSASSERT(COND)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_STATIC_ASSERT(CONDITION) static_assert(CONDITION, "bug")
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_MAKE_BITFIELD_OPS(X) inline X operator |(X a, X b) { return (X) (+a | +b); } inline X& operator |=(X& a, X b) { return (a = a | b); } inline X operator &(X a, X b) { return (X) (+a & +b); } inline X& operator &=(X& a, X b) { return (a = a & b); } template <typename T> inline X operator &(T a, X b) { return (X) (+a & +b); } template <typename T> inline X operator &(X a, T b) { return (X) (+a & +b); }
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DECL_BITFIELD_OPS_FRIENDS(X) friend X operator |(X a, X b); friend X& operator |=(X& a, X b); friend X operator &(X a, X b); friend X& operator &=(X& a, X b); template <typename T> friend X operator &(T a, X b); template <typename T> friend X operator &(X a, T b);
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_MAKE_BITFIELD_CLASS_OPS(X) constexpr GrTFlagsMask<X> operator~(X a) { return GrTFlagsMask<X>(~static_cast<int>(a)); } constexpr X operator|(X a, X b) { return static_cast<X>(static_cast<int>(a) | static_cast<int>(b)); } inline X& operator|=(X& a, X b) { return (a = a | b); } constexpr bool operator&(X a, X b) { return SkToBool(static_cast<int>(a) & static_cast<int>(b)); }
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_DECL_BITFIELD_CLASS_OPS_FRIENDS(X) friend constexpr GrTFlagsMask<X> operator ~(X); friend constexpr X operator |(X, X); friend X& operator |=(X&, X); friend constexpr bool operator &(X, X);
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_MAX(a, b) (((b) < (a)) ? (a) : (b))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_MIN(a, b) (((b) < (a)) ? (b) : (a))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_DIV_ROUND_UP(X, Y) (((X) + ((Y)-1)) / (Y))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define GR_CT_ALIGN_UP(X, A) (GR_CT_DIV_ROUND_UP((X),(A)) * (A))
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_REGISTER_FLATTENABLE(type) SkFlattenable::Register(#type, type::CreateProc);
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define SK_FLATTENABLE_HOOKS(type) static sk_sp<SkFlattenable> CreateProc(SkReadBuffer&); friend class SkFlattenable::PrivateInitializer; Factory getFactory() const override { return type::CreateProc; } const char* getTypeName() const override { return #type; }
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_FRIEND_REF_COUNTED_THREAD_SAFE(T) friend class ::fml::RefCountedThreadSafe<T>
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_FRIEND_MAKE_REF_COUNTED(T) friend class ::fml::internal::MakeRefCountedHelper<T>
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DECLARE_THREAD_CHECKER(c) fml::ThreadChecker c
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DCHECK_CREATION_THREAD_IS_CURRENT(c) FML_DCHECK((c).IsCreationThreadCurrent())
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DECLARE_THREAD_CHECKER(c)
	//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
	//ORIGINAL LINE: #define FML_DCHECK_CREATION_THREAD_IS_CURRENT(c) ((void)0)

	public static sk_sp<SkPicture> GetSamplePicture()
	{
	  SkPictureRecorder recorder = new SkPictureRecorder();
	  recorder.beginRecording(SkRect.MakeWH(150, 100));
	  SkPaint paint = new SkPaint();
	  paint.setColor(new uint32_t(SK_ColorRED));
	  recorder.getRecordingCanvas().drawRect(SkRect.MakeXYWH(10, 10, 80, 80), paint);
	  return recorder.finishRecordingAsPicture();
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(RasterCache UnnamedParameter, SimpleInitialization UnnamedParameter2)
	{
	  flow.RasterCache cache = new flow.RasterCache();
	  ASSERT_TRUE(true);
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(RasterCache UnnamedParameter, ThresholdIsRespected UnnamedParameter2)
	{
	  size_t threshold = 3;
	  flow.RasterCache cache = new flow.RasterCache(new size_t(threshold));

	  SkMatrix matrix = SkMatrix.I();

	  var picture = GetSamplePicture();

	  sk_sp<SkImage> image = new sk_sp<SkImage>();

	  sk_sp<SkColorSpace> srgb = SkColorSpace.MakeSRGB();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 1
	  cache.SweepAfterFrame();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 2
	  cache.SweepAfterFrame();
	  ASSERT_TRUE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 3
	  cache.SweepAfterFrame();
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(RasterCache UnnamedParameter, ThresholdIsRespectedWhenZero UnnamedParameter2)
	{
	  size_t threshold = 0;
	  flow.RasterCache cache = new flow.RasterCache(new size_t(threshold));

	  SkMatrix matrix = SkMatrix.I();

	  var picture = GetSamplePicture();

	  sk_sp<SkImage> image = new sk_sp<SkImage>();

	  sk_sp<SkColorSpace> srgb = SkColorSpace.MakeSRGB();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 1
	  cache.SweepAfterFrame();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 2
	  cache.SweepAfterFrame();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 3
	  cache.SweepAfterFrame();
	}

//C++ TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
	public static TEST(RasterCache UnnamedParameter, SweepsRemoveUnusedFrames UnnamedParameter2)
	{
	  size_t threshold = 3;
	  flow.RasterCache cache = new flow.RasterCache(new size_t(threshold));

	  SkMatrix matrix = SkMatrix.I();

	  var picture = GetSamplePicture();

	  sk_sp<SkImage> image = new sk_sp<SkImage>();

	  sk_sp<SkColorSpace> srgb = SkColorSpace.MakeSRGB();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 1
	  cache.SweepAfterFrame();
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 2
	  cache.SweepAfterFrame();
	  ASSERT_TRUE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 3
	  cache.SweepAfterFrame();
	  ASSERT_TRUE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 4
	  cache.SweepAfterFrame();
	  cache.SweepAfterFrame(); // Extra frame without a preroll image access.
	  ASSERT_FALSE(cache.Prepare(null, picture.get(), matrix, srgb.get(), true, false)); // 5
	}

}