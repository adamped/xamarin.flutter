using FlutterBinding.Txt;
using HarfBuzzSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using static FlutterBinding.Txt.Paragraph;

/*
 * Copyright 2017 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*
 * Copyright 2017 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */



//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "flutter/fml/compiler_specific.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "flutter/fml/macros.h"
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_NOTHING_ARG1(arg1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_NOTHING_ARG2(arg1, arg2)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_NOTHING_ARG3(arg1, arg2, arg3)
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
//ORIGINAL LINE: #define DISABLE_TEST_WINDOWS(TEST_NAME) DISABLED_##TEST_NAME
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FRIEND_TEST_WINDOWS_DISABLED_EXPANDED(SUITE, TEST_NAME) FRIEND_TEST(SUITE, TEST_NAME)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FRIEND_TEST_WINDOWS_DISABLED(SUITE, TEST_NAME) FRIEND_TEST_WINDOWS_DISABLED_EXPANDED(SUITE, DISABLE_TEST_WINDOWS(TEST_NAME))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define DISABLE_TEST_WINDOWS(TEST_NAME) TEST_NAME
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FRIEND_TEST_WINDOWS_DISABLED(SUITE, TEST_NAME) FRIEND_TEST(SUITE, TEST_NAME)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_WHEN(condition, T) skstd::enable_if_t<!!(condition), T>
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendS32_MaxSize (SkStrAppendU32_MaxSize + 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendS64_MaxSize (SkStrAppendU64_MaxSize + 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendScalar SkStrAppendFloat
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/googletest/googletest/include/gtest/gtest_prod.h" // nogncheck

//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class SKCanvas;

namespace FlutterBinding.Txt
{


    // Paragraph provides Layout, metrics, and painting capabilites for text. Once a
    // Paragraph is constructed with ParagraphBuilder::Build(), an example basic
    // workflow can be this:
    //
    //   std::unique_ptr<Paragraph> paragraph = paragraph_builder.Build();
    //   paragraph->Layout(<somewidthgoeshere>);
    //   paragraph->Paint(<someSKCanvas>, <xpos>, <ypos>);
    public class Paragraph //: System.IDisposable
    {
        // Constructor. It is highly recommended to construct a paragraph with a
        // ParagraphBuilder.
        public Paragraph()
        {
            breaker_.setLocale(icu.Locale(), null);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public enum Affinity
        {
            UPSTREAM,
            DOWNSTREAM
        }

        // Struct that holds calculated metrics for each line.
        //C++ TO C# CONVERTER TODO TASK: C# does not allow declaring types within methods:
        public class LineBoxMetrics
        {
            public List<TextBox> boxes;
            // Per-line metrics for max and min coordinates for left and right boxes.
            // These metrics cannot be calculated in layout generically because of
            // selections that do not cover the whole line.
            public float max_right;// = FLT_MIN;
            public float min_left; //= FLT_MAX;
        };


        // TODO(garyq): Implement kIncludeLineSpacing and kExtendEndOfLine

        // Options for various types of bounding boxes provided by
        // GetRectsForRange(...).
        public enum RectHeightStyle
        {
            // Provide tight bounding boxes that fit heights per run.
            kTight,

            // The height of the boxes will be the maximum height of all runs in the
            // line. All rects in the same line will be the same height.
            kMax,

            // Extends the top and/or bottom edge of the bounds to fully cover any line
            // spacing. The top edge of each line should be the same as the bottom edge
            // of the line above. There should be no gaps in vertical coverage given any
            // ParagraphStyle line_height.
            //
            // The top and bottom of each rect will cover half of the
            // space above and half of the space below the line.
            kIncludeLineSpacingMiddle,
            // The line spacing will be added to the top of the rect.
            kIncludeLineSpacingTop,
            // The line spacing will be added to the bottom of the rect.
            kIncludeLineSpacingBottom
        }

        public enum RectWidthStyle
        {
            // Provide tight bounding boxes that fit widths to the runs of each line
            // independently.
            kTight,

            // Extends the width of the last rect of each line to match the position of
            // the widest rect over all the lines.
            kMax
        }

        public class PositionWithAffinity
        {
            public readonly int position = new int();
            public readonly Affinity affinity;

            public PositionWithAffinity(int p, Affinity a)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.position = p;
                this.position = p;
                this.affinity = a;
            }
        }

        public class TextBox
        {
            public SKRect rect = new SKRect();
            public TextDirection direction;

            public TextBox(SKRect r, TextDirection d)
            {
                this.rect = r;
                this.direction = d;
            }
        }

        //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
        //ORIGINAL LINE: template <typename T>
        public class Range<T> where T : struct
        {
            public Range()
            {
                this.start = default(T);
                this.end = default(T);
            }
            public Range(T s, T e)
            {
                this.start = s;
                this.end = e;
            }

            public T start = new T();
            public T end = new T();

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: bool operator ==(const Range<T>& other) const
            //public static bool operator ==(Range ImpliedObject, Range<T> other)
            //{
            //    return ImpliedObject.start == other.start && ImpliedObject.end == other.end;
            //}

            public T width()
            {
                return end - start;
            }

            public void Shift(T delta)
            {
                start += delta;
                end += delta;
            }
        }

        // Minikin Layout doLayout() and LineBreaker addStyleRun() has an
        // O(N^2) (according to benchmarks) time complexity where N is the total
        // number of characters. However, this is not significant for reasonably sized
        // paragraphs. It is currently recommended to break up very long paragraphs
        // (10k+ characters) to ensure speedy layout.
        //
        // Layout calculates the positioning of all the glyphs. Must call this method
        // before Painting and getting any statistics from this class.
        public void Layout(double width, bool force = false)
        {
            // Do not allow calling layout multiple times without changing anything.
            if (!needs_layout_ && width == width_ && !force)
            {
                return;
            }
            needs_layout_ = false;

            width_ = Math.Floor(width);

            if (!ComputeLineBreaks())
            {
                return;
            }

            List<BidiRun> bidi_runs = new List<BidiRun>();
            if (!ComputeBidiRuns(bidi_runs))
            {
                return;
            }

            SKPaint paint = new SKPaint();
            paint.setAntiAlias(true);
            paint.setTextEncoding(SKPaint.TextEncoding.kGlyphID_TextEncoding);
            paint.setSubpixelText(true);
            paint.HintingLevel = SKPaintHinting.Slight; // SetHinting(SKPaint.Hinting.kSlight_Hinting);

            records_.Clear();
            line_heights_.Clear();
            line_baselines_.Clear();
            glyph_lines_.Clear();
            code_unit_runs_.Clear();
            line_max_spacings_.Clear();
            line_max_descent_.Clear();
            line_max_ascent_.Clear();
            max_right_ = double.MinValue; // FLT_MIN;
            min_left_ = double.MaxValue; // FLT_MAX;

            minikin.Layout layout = new minikin.Layout();
            SkTextBlobBuilder builder = new SkTextBlobBuilder();
            double y_offset = 0;
            double prev_max_descent = 0;
            double max_word_width = 0;

            int line_limit = Math.Min(paragraph_style_.max_lines, line_ranges_.Count);
            did_exceed_max_lines_ = (line_ranges_.Count > paragraph_style_.max_lines);

            for (int line_number = 0; line_number < line_limit; ++line_number)
            {
                LineRange line_range = line_ranges_[line_number];

                // Break the line into words if justification should be applied.
                List<Range<int>> words = new List<Range<int>>();
                double word_gap_width = 0;
                int word_index = 0;
                bool justify_line = (paragraph_style_.text_align == TextAlign.justify && line_number != line_limit - 1 && !line_range.hard_break);
                GlobalMembers.FindWords(text_, line_range.start, line_range.end, words);
                if (justify_line)
                {
                    if (words.Count > 1)
                    {
                        word_gap_width = (width_ - line_widths_[line_number]) / (words.Count - 1);
                    }
                }

                // Exclude trailing whitespace from right-justified lines so the last
                // visible character in the line will be flush with the right margin.
                int line_end_index = (paragraph_style_.effective_align() == TextAlign.right || paragraph_style_.effective_align() == TextAlign.center) ? line_range.end_excluding_whitespace : line_range.end;

                // Find the runs comprising this line.
                List<BidiRun> line_runs = new List<BidiRun>();
                foreach (BidiRun bidi_run in bidi_runs)
                {
                    if (bidi_run.start() < line_end_index && bidi_run.end() > line_range.start)
                    {
                        line_runs.Add(Math.Max(bidi_run.start(), line_range.start), Math.Min(bidi_run.end(), line_end_index), bidi_run.direction(), bidi_run.style());
                    }
                }

                List<GlyphPosition> line_glyph_positions = new List<GlyphPosition>();
                List<CodeUnitRun> line_code_unit_runs = new List<CodeUnitRun>();
                double run_x_offset = 0;
                double justify_x_offset = 0;
                List<PaintRecord> paint_records = new List<PaintRecord>();

                foreach (BidiRun line_run_it in line_runs)
                {
                    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to references to variables:
                    //ORIGINAL LINE: const BidiRun& run = line_run_it;
                    BidiRun run = line_run_it;
                    minikin.FontStyle font = new minikin.FontStyle();
                    minikin.MinikinPaint minikin_paint = new minikin.MinikinPaint();
                    GlobalMembers.GetFontAndMinikinPaint(run.style(), font, minikin_paint);
                    paint.setTextSize(run.style().font_size);

                    minikin.FontCollection minikin_font_collection = GetMinikinFontCollectionForStyle(run.style());

                    // Lay out this run.
                    UInt16 text_ptr = text_.data();
                    int text_start = run.start();
                    int text_count = run.end() - run.start();
                    int text_size = text_.Count;

                    // Apply ellipsizing if the run was not completely laid out and this
                    // is the last line (or lines are unlimited).
                    var ellipsis = paragraph_style_.ellipsis;
                    List<UInt16> ellipsized_text = new List<UInt16>();
                    if (ellipsis.length() && !double.IsInfinity(width_) && !line_range.hard_break && line_run_it == line_runs.end() - 1 && (line_number == line_limit - 1 || paragraph_style_.unlimited_lines()))
                    {
                        //C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
                        float ellipsis_width = layout.measureText(ellipsis.data(), 0, ellipsis.length(), ellipsis.length(), run.is_rtl(), font, minikin_paint, minikin_font_collection, null);

                        List<float> text_advances = new List<float>(text_count);
                        float text_width = layout.measureText(text_ptr, text_start, text_count, text_.Count, run.is_rtl(), font, minikin_paint, minikin_font_collection, text_advances.data());

                        // Truncate characters from the text until the ellipsis fits.
                        int truncate_count = 0;
                        while (truncate_count < text_count && run_x_offset + text_width + ellipsis_width > width_)
                        {
                            text_width -= text_advances[text_count - truncate_count - 1];
                            truncate_count++;
                        }

                        ellipsized_text.Capacity = text_count - truncate_count + ellipsis.length();
                        //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
                        ellipsized_text.insert(ellipsized_text.GetEnumerator(), text_.GetEnumerator() + run.start(), text_.GetEnumerator() + run.end() - truncate_count);
                        ellipsized_text.AddRange(ellipsis);
                        text_ptr = ellipsized_text.data();
                        text_start = 0;
                        text_count = ellipsized_text.Count;
                        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                        //ORIGINAL LINE: text_size = text_count;
                        text_size = text_count;

                        // If there is no line limit, then skip all lines after the ellipsized
                        // line.
                        if (paragraph_style_.unlimited_lines())
                        {
                            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                            //ORIGINAL LINE: line_limit = line_number + 1;
                            line_limit = line_number + 1;
                            did_exceed_max_lines_ = true;
                        }
                    }

                    layout.doLayout(text_ptr, text_start, text_count, text_size, run.is_rtl(), font, minikin_paint, minikin_font_collection);

                    if (layout.nGlyphs() == 0)
                    {
                        continue;
                    }

                    List<float> layout_advances = new List<float>(text_count);
                    layout.getAdvances(layout_advances.data());

                    // Break the layout into blobs that share the same SKPaint parameters.
                    List<Range<int>> glyph_blobs = GlobalMembers.GetLayoutTypefaceRuns(layout);

                    double word_start_position = numeric_limits<double>.quiet_NaN();

                    // Build a Skia text blob from each group of glyphs.
                    foreach (Range<int> glyph_blob in glyph_blobs)
                    {
                        List<GlyphPosition> glyph_positions = new List<GlyphPosition>();

                        GlobalMembers.GetGlyphTypeface(layout, glyph_blob.start).apply(paint);
                        SkTextBlobBuilder.RunBuffer blob_buffer = builder.allocRunPos(paint, glyph_blob.end - glyph_blob.start);

                        for (int glyph_index = glyph_blob.start; glyph_index < glyph_blob.end;)
                        {
                            int cluster_start_glyph_index = glyph_index;
                            uint cluster = layout.getGlyphCluster(cluster_start_glyph_index);
                            double glyph_x_offset;

                            // Add all the glyphs in this cluster to the text blob.
                            do
                            {
                                int blob_index = glyph_index - glyph_blob.start;
                                blob_buffer.glyphs[blob_index] = layout.getGlyphId(glyph_index);

                                int pos_index = blob_index * 2;
                                blob_buffer.pos[pos_index] = layout.getX(glyph_index) + justify_x_offset;
                                blob_buffer.pos[pos_index + 1] = layout.getY(glyph_index);

                                if (glyph_index == cluster_start_glyph_index)
                                {
                                    glyph_x_offset = blob_buffer.pos[pos_index];
                                }

                                glyph_index++;
                            } while (glyph_index < glyph_blob.end && layout.getGlyphCluster(glyph_index) == cluster);

                            Range<int> glyph_code_units = new Range<int>((int)cluster, 0);
                            List<int> grapheme_code_unit_counts = new List<int>();
                            if (run.is_rtl())
                            {
                                if (cluster_start_glyph_index > 0)
                                {
                                    glyph_code_units.end = layout.getGlyphCluster(cluster_start_glyph_index - 1);
                                }
                                else
                                {
                                    glyph_code_units.end = text_count;
                                }
                                grapheme_code_unit_counts.Add(glyph_code_units.width());
                            }
                            else
                            {
                                if (glyph_index < layout.nGlyphs())
                                {
                                    glyph_code_units.end = layout.getGlyphCluster(glyph_index);
                                }
                                else
                                {
                                    glyph_code_units.end = text_count;
                                }

                                // The glyph may be a ligature.  Determine how many graphemes are
                                // joined into this glyph and how many input code units map to
                                // each grapheme.
                                int code_unit_count = 1;
                                for (int offset = glyph_code_units.start + 1; offset < glyph_code_units.end; ++offset)
                                {
                                    if (minikin.GraphemeBreak.isGraphemeBreak(layout_advances.data(), text_ptr, text_start, text_count, offset))
                                    {
                                        grapheme_code_unit_counts.Add(code_unit_count);
                                        code_unit_count = 1;
                                    }
                                    else
                                    {
                                        code_unit_count++;
                                    }
                                }
                                grapheme_code_unit_counts.Add(code_unit_count);
                            }
                            float glyph_advance = layout.getCharAdvance(glyph_code_units.start);
                            float grapheme_advance = glyph_advance / grapheme_code_unit_counts.Count;

                            glyph_positions.Add(run_x_offset + glyph_x_offset, grapheme_advance, run.start() + glyph_code_units.start, grapheme_code_unit_counts[0]);

                            // Compute positions for the additional graphemes in the ligature.
                            for (int i = 1; i < grapheme_code_unit_counts.Count; ++i)
                            {
                                glyph_positions.Add(glyph_positions[glyph_positions.Count - 1].x_pos.end, grapheme_advance, glyph_positions[glyph_positions.Count - 1].code_units.start + grapheme_code_unit_counts[i - 1], grapheme_code_unit_counts[i]);
                            }

                            if (word_index < words.Count && words[word_index].start == run.start() + glyph_code_units.start)
                            {
                                word_start_position = run_x_offset + glyph_x_offset;
                            }

                            if (word_index < words.Count && words[word_index].end == run.start() + glyph_code_units.end)
                            {
                                if (justify_line)
                                {
                                    justify_x_offset += word_gap_width;
                                }
                                word_index++;

                                if (!double.IsNaN(word_start_position))
                                {
                                    double word_width = glyph_positions[glyph_positions.Count - 1].x_pos.end - word_start_position;
                                    max_word_width = Math.Max(word_width, max_word_width);
                                    word_start_position = numeric_limits<double>.quiet_NaN();
                                }
                            }
                        } // for each in glyph_blob

                        if (glyph_positions.Count == 0)
                        {
                            continue;
                        }
                        SKFontMetrics metrics = new SKFontMetrics();
                        paint.getFontMetrics(metrics);
                        paint_records.Add(run.style(), SKPoint.Make(run_x_offset, 0), builder.make(), metrics, line_number, layout.getAdvance());

                        line_glyph_positions.AddRange(glyph_positions);

                        // Add a record of glyph positions sorted by code unit index.
                        List<GlyphPosition> code_unit_positions = new List<GlyphPosition>(glyph_positions);
                        //C++ TO C# CONVERTER TODO TASK: The 'Compare' parameter of std::sort produces a boolean value, while the .NET Comparison parameter produces a tri-state result:
                        //ORIGINAL LINE: std::sort(code_unit_positions.begin(), code_unit_positions.end(), [](const GlyphPosition& a, const GlyphPosition& b)
                        code_unit_positions.Sort((GlyphPosition a, GlyphPosition b) =>
                                  {
                                      return a.code_units.start < b.code_units.start;
                                  });
                        line_code_unit_runs.Add(new CodeUnitRun(code_unit_positions, new Range<int>(run.start(), run.end()), new Range<double>(glyph_positions[0].x_pos.start, glyph_positions[glyph_positions.Count - 1].x_pos.end), line_number, metrics, run.direction()));

                        min_left_ = Math.Min(min_left_, glyph_positions[0].x_pos.start);
                        max_right_ = Math.Max(max_right_, glyph_positions[glyph_positions.Count - 1].x_pos.end);
                    } // for each in glyph_blobs

                    run_x_offset += layout.getAdvance();
                } // for each in line_runs

                // Adjust the glyph positions based on the alignment of the line.
                double line_x_offset = GetLineXOffset(run_x_offset);
                if ((int)line_x_offset != 0)
                {
                    foreach (CodeUnitRun code_unit_run in line_code_unit_runs)
                    {
                        code_unit_run.Shift(line_x_offset);
                    }
                    foreach (GlyphPosition position in line_glyph_positions)
                    {
                        position.Shift(line_x_offset);
                    }
                }

                int next_line_start = (line_number < line_ranges_.Count - 1) ? line_ranges_[line_number + 1].start : text_.Count;
                glyph_lines_.Add(new GlyphLine(line_glyph_positions, next_line_start - line_range.start));
                code_unit_runs_.AddRange(line_code_unit_runs);

                double max_line_spacing = 0;
                double max_descent = 0;
                float max_unscaled_ascent = 0F;
                //C++ TO C# CONVERTER TODO TASK: Lambda expressions cannot be assigned to 'var':
                Action<SKFontMetrics, TextStyle> update_line_metrics = (SKFontMetrics metrics, TextStyle style) =>
                {
                    // TODO(garyq): Multipling in the style.height on the first line is
                    // probably wrong. Figure out how paragraph and line heights are supposed
                    // to work and fix it.
                    double line_spacing = (line_number == 0) ? -metrics.Ascent * style.height : (-metrics.Ascent + metrics.Leading) * style.height;
                    if (line_spacing > max_line_spacing)
                    {
                        max_line_spacing = line_spacing;
                        if (line_number == 0)
                        {
                            alphabetic_baseline_ = line_spacing;
                            ideographic_baseline_ = (metrics.Descent - metrics.Ascent) * style.height;
                        }
                    }
                    max_line_spacing = Math.Max(line_spacing, max_line_spacing);

                    double descent = metrics.Descent * style.height;
                    max_descent = Math.Max(descent, max_descent);

                    max_unscaled_ascent = Math.Max(-metrics.Ascent, max_unscaled_ascent);
                };

                foreach (PaintRecord paint_record in paint_records)
                {
                    update_line_metrics(paint_record.metrics(), paint_record.style());
                }
                // If no fonts were actually rendered, then compute a baseline based on the
                // font of the paragraph style.
                if (paint_records.Count == 0)
                {
                    SKFontMetrics metrics = new SKFontMetrics();
                    TextStyle style = paragraph_style_.GetTextStyle();
                    paint.setTypeface(GetDefaultSkiaTypeface(style));
                    paint.setTextSize(style.font_size);
                    paint.getFontMetrics(metrics);
                    update_line_metrics(metrics, style);
                }

                // TODO(garyq): Remove rounding of line heights because it is irrelevant in
                // a world of high DPI devices.
                line_heights_.Add((line_heights_.Count == 0 ? 0 : line_heights_[line_heights_.Count - 1]) + Math.Round(max_line_spacing + max_descent));
                line_baselines_.Add(line_heights_[line_heights_.Count - 1] - max_descent);
                y_offset += Math.Round(max_line_spacing + prev_max_descent);
                prev_max_descent = max_descent;

                // The max line spacing and ascent have been multiplied by -1 to make math
                // in GetRectsForRange more logical/readable.
                line_max_spacings_.Add(max_line_spacing);
                line_max_descent_.Add(max_descent);
                line_max_ascent_.Add(max_unscaled_ascent);

                foreach (PaintRecord paint_record in paint_records)
                {
                    paint_record.SetOffset(SKPoint.Make(paint_record.Offset().x() + line_x_offset, y_offset));
                    records_.Add(paint_record);
                }
            } // for each line_number

            if (paragraph_style_.max_lines == 1 || (paragraph_style_.unlimited_lines() && paragraph_style_.ellipsized()))
            {
                min_intrinsic_width_ = max_intrinsic_width_;
            }
            else
            {
                min_intrinsic_width_ = Math.Min(max_word_width, max_intrinsic_width_);
            }

            //C++ TO C# CONVERTER TODO TASK: The 'Compare' parameter of std::sort produces a boolean value, while the .NET Comparison parameter produces a tri-state result:
            //ORIGINAL LINE: std::sort(code_unit_runs_.begin(), code_unit_runs_.end(), [](const CodeUnitRun& a, const CodeUnitRun& b)
            code_unit_runs_.Sort((CodeUnitRun a, CodeUnitRun b) =>
            {
                return a.code_units.start < b.code_units.start;
            });
        }

        // Paints the Laid out text onto the supplied SKCanvas at (x, y) offset from
        // the origin. Only valid after Layout() is called.

        // The x,y coordinates will be the very top left corner of the rendered
        // paragraph.
        public void Paint(SKCanvas canvas, double x, double y)
        {
            SKPoint base_offset = new SKPoint((float)x, (float)y);
            SKPaint paint = new SKPaint();
            foreach (PaintRecord record in records_)
            {
                if (record.style().has_foreground)
                {
                    paint = record.style().foreground;
                }
                else
                {
                    paint.reset();
                    paint.setColor(record.style().color);
                }
                SKPoint offset = base_offset + record.offset();
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: PaintBackground(canvas, record, base_offset);
                PaintBackground(canvas, record, base_offset);
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: PaintShadow(canvas, record, offset);
                PaintShadow(canvas, record, offset);
                canvas.DrawText(record.text(), offset.X, offset.Y, paint);
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: PaintDecorations(canvas, record, base_offset);
                PaintDecorations(canvas, record, base_offset);
            }
        }

        // Getter for paragraph_style_.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const ParagraphStyle& GetParagraphStyle() const
        public ParagraphStyle GetParagraphStyle()
        {
            return paragraph_style_;
        }

        // Returns the number of characters/unicode characters. AKA text_.size()
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int TextSize() const
        public int TextSize()
        {
            return text_.Count;
        }

        // Returns the height of the laid out paragraph. NOTE this is not a tight
        // bounding height of the glyphs, as some glyphs do not reach as low as they
        // can.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetHeight() const
        public double GetHeight()
        {
            return line_heights_.Count > 0 ? line_heights_[line_heights_.Count - 1] : 0;
        }

        // Returns the width provided in the Layout() method. This is the maximum
        // width any line in the laid out paragraph can occupy. We expect that
        // GetMaxWidth() >= GetLayoutWidth().
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetMaxWidth() const
        public double GetMaxWidth()
        {
            return width_;
        }

        // Distance from top of paragraph to the Alphabetic baseline of the first
        // line. Used for alphabetic fonts (A-Z, a-z, greek, etc.)
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetAlphabeticBaseline() const
        public double GetAlphabeticBaseline()
        {
            // Currently -fAscent
            return alphabetic_baseline_;
        }

        // Distance from top of paragraph to the Ideographic baseline of the first
        // line. Used for ideographic fonts (Chinese, Japanese, Korean, etc.)
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetIdeographicBaseline() const
        public double GetIdeographicBaseline()
        {
            // TODO(garyq): Currently -fAscent + fUnderlinePosition. Verify this.
            return ideographic_baseline_;
        }

        // Returns the total width covered by the paragraph without linebreaking.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetMaxIntrinsicWidth() const
        public double GetMaxIntrinsicWidth()
        {
            return max_intrinsic_width_;
        }

        // Currently, calculated similarly to as GetLayoutWidth(), however this is not
        // nessecarily 100% correct in all cases.
        //
        // Returns the actual max width of the longest line after Layout().
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetMinIntrinsicWidth() const
        public double GetMinIntrinsicWidth()
        {
            return min_intrinsic_width_;
        }

        // Returns a vector of bounding boxes that enclose all text between start and
        // end glyph indexes, including start and excluding end.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: ClassicVector<Paragraph::TextBox> GetRectsForRange(int start, int end, RectHeightStyle rect_height_style, RectWidthStyle rect_width_style) const
        public List<Paragraph.TextBox> GetRectsForRange(int start, int end, RectHeightStyle rect_height_style, RectWidthStyle rect_width_style)
        {


            SortedDictionary<int, LineBoxMetrics> line_metrics = new SortedDictionary<int, LineBoxMetrics>();
            // Text direction of the first line so we can extend the correct side for
            // RectWidthStyle::kMax.
            TextDirection first_line_dir = TextDirection.ltr;

            // Lines that are actually in the requested range.
            int max_line = 0;
            int min_line = int.MaxValue;// INT_MAX;

            // Generate initial boxes and calculate metrics.
            foreach (CodeUnitRun run in code_unit_runs_)
            {
                // Check to see if we are finished.
                if (run.code_units.start >= end)
                {
                    break;
                }
                if (run.code_units.end <= start)
                {
                    continue;
                }

                double baseline = line_baselines_[run.line_number];
                float top = (float)baseline + run.font_metrics.Ascent;
                float bottom = (float)baseline + run.font_metrics.Descent;

                max_line = Math.Max(run.line_number, max_line);
                min_line = Math.Min(run.line_number, min_line);

                // Calculate left and right.
                float left;
                float right;
                if (run.code_units.start >= start && run.code_units.end <= end)
                {
                    left = (float)run.x_pos.start;
                    right = (float)run.x_pos.end;
                }
                else
                {
                    left = 3.402823466e+38f;
                    right = (-3.402823466e+38f);
                    foreach (GlyphPosition gp in run.positions)
                    {
                        if (gp.code_units.start >= start && gp.code_units.end <= end)
                        {
                            left = Math.Min(left, (float)gp.x_pos.start);
                            right = Math.Max(right, (float)gp.x_pos.end);
                        }
                    }
                    if (left == 3.402823466e+38f || right == (-SK_ScalarMax))
                    {
                        continue;
                    }
                }
                // Keep track of the min and max horizontal coordinates over all lines. Not
                // needed for kTight.
                if (rect_width_style == RectWidthStyle.kMax)
                {
                    line_metrics[run.line_number].max_right = Math.Max(line_metrics[run.line_number].max_right, right);
                    line_metrics[run.line_number].min_left = Math.Min(line_metrics[run.line_number].min_left, left);
                    if (min_line == run.line_number)
                    {
                        first_line_dir = run.direction;
                    }
                }
                line_metrics[run.line_number].boxes.Add(new TextBox(new SKRect(left, top, right, bottom), run.direction));
            }

            // Add empty rectangles representing any newline characters within the
            // range.
            for (int line_number = 0; line_number < line_ranges_.Count; ++line_number)
            {
                LineRange line = line_ranges_[line_number];
                if (line.start >= end)
                {
                    break;
                }
                if (line.end_including_newline <= start)
                {
                    continue;
                }
                if (!line_metrics.ContainsKey(line_number))
                {
                    if (line.end != line.end_including_newline && line.end >= start && line.end_including_newline <= end)
                    {
                        float x = (float)line_widths_[line_number];
                        float top = (line_number > 0) ? line_heights_[line_number - 1] : 0F;
                        float bottom = (float)line_heights_[line_number];
                        line_metrics[line_number].boxes.Add(new TextBox(new SKRect(x, top, x, bottom), TextDirection.ltr));
                    }
                }
            }

            // "Post-process" metrics and aggregate final rects to return.
            List<Paragraph.TextBox> boxes = new List<Paragraph.TextBox>();
            foreach (var kv in line_metrics)
            {
                // Handle rect_width_styles. We skip the last line because not everything is
                // selected.
                if (rect_width_style == RectWidthStyle.kMax && kv.Key != max_line)
                {
                    if (line_metrics[kv.Key].min_left > min_left_ && (kv.Key != min_line || first_line_dir == TextDirection.rtl))
                    {
                        line_metrics[kv.Key].boxes.Add(new TextBox(new SKRect((float)min_left_, (float)(line_baselines_[kv.Key] - line_max_ascent_[kv.Key]), line_metrics[kv.Key].min_left, (float)line_baselines_[kv.Key] + line_max_descent_[kv.Key]), TextDirection.rtl));
                    }
                    if (line_metrics[kv.Key].max_right < max_right_ && (kv.Key != min_line || first_line_dir == TextDirection.ltr))
                    {
                        line_metrics[kv.Key].boxes.Add(new TextBox(new SKRect(line_metrics[kv.Key].max_right, (float)(line_baselines_[kv.Key] - line_max_ascent_[kv.Key]), (float)max_right_, (float)line_baselines_[kv.Key] + line_max_descent_[kv.Key]), TextDirection.ltr));
                    }
                }

                // Handle rect_height_styles. The height metrics used are all positive to
                // make the signage clear here.
                if (rect_height_style == RectHeightStyle.kTight)
                {
                    // Ignore line max height and width and generate tight bounds.
                    boxes.AddRange(kv.Value.boxes);
                }
                else if (rect_height_style == RectHeightStyle.kMax)
                {
                    foreach (var box in kv.Value.boxes)
                    {
                        boxes.Add(new TextBox(new SKRect(box.rect.Left, (float)line_baselines_[kv.Key] - line_max_ascent_[kv.Key], box.rect.Right, (float)(line_baselines_[kv.Key] + line_max_descent_[kv.Key])), box.direction));
                    }
                }
                else if (rect_height_style == RectHeightStyle.kIncludeLineSpacingMiddle)
                {
                    float adjusted_bottom = line_baselines_[kv.Key] + line_max_descent_[kv.Key];
                    if (kv.Key < line_ranges_.Count - 1)
                    {
                        adjusted_bottom += (line_max_spacings_[kv.Key + 1] - line_max_ascent_[kv.Key + 1]) / 2;
                    }
                    float adjusted_top = line_baselines_[kv.Key] - line_max_ascent_[kv.Key];
                    if (kv.Key != 0)
                    {
                        adjusted_top -= (line_max_spacings_[kv.Key] - line_max_ascent_[kv.Key]) / 2;
                    }
                    foreach (var box in kv.Value.boxes)
                    {
                        boxes.Add(new TextBox(new SKRect(box.rect.Left, adjusted_top, box.rect.Right, adjusted_bottom), box.direction));
                    }
                }
                else if (rect_height_style == RectHeightStyle.kIncludeLineSpacingTop)
                {
                    foreach (var box in kv.Value.boxes)
                    {
                        float adjusted_top = kv.Key == 0 ? line_baselines_[kv.Key] - line_max_ascent_[kv.Key] : line_baselines_[kv.Key] - line_max_spacings_[kv.Key];
                        boxes.Add(new TextBox(new SKRect(box.rect.Left, adjusted_top, box.rect.Right, line_baselines_[kv.Key] + line_max_descent_[kv.Key]), box.direction));
                    }
                }
                else
                { // kIncludeLineSpacingBottom
                    foreach (var box in kv.Value.boxes)
                    {
                        float adjusted_bottom = (float)line_baselines_[kv.Key] + line_max_descent_[kv.Key];
                        if (kv.Key < line_ranges_.Count - 1)
                        {
                            adjusted_bottom += -line_max_ascent_[kv.Key] + line_max_spacings_[kv.Key];
                        }
                        boxes.Add(new TextBox(new SKRect(box.rect.Left, (float)(line_baselines_[kv.Key] - line_max_ascent_[kv.Key]), box.rect.Right, adjusted_bottom), box.direction));
                    }
                }
            }
            return boxes;
        }

        // Returns the index of the glyph that corresponds to the provided coordinate,
        // with the top left corner as the origin, and +y direction as down.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Paragraph::PositionWithAffinity GetGlyphPositionAtCoordinate(double dx, double dy) const
        public Paragraph.PositionWithAffinity GetGlyphPositionAtCoordinate(double dx, double dy)
        {
            if (line_heights_.Count == 0)
            {
                return new PositionWithAffinity(0, Affinity.DOWNSTREAM);
            }

            int y_index = new int();
            for (y_index = 0; y_index < line_heights_.Count - 1; ++y_index)
            {
                if (dy < line_heights_[y_index])
                {
                    break;
                }
            }

            List<GlyphPosition> line_glyph_position = glyph_lines_[y_index].positions;
            if (line_glyph_position.Count == 0)
            {
                int line_start_index = std::accumulate(glyph_lines_.GetEnumerator(), glyph_lines_.GetEnumerator() + y_index, 0, (int a, GlyphLine b) =>
                {
                    return a + (int)b.total_code_units;
                });
                return new PositionWithAffinity(line_start_index, Affinity.DOWNSTREAM);
            }

            int x_index = new int();
            GlyphPosition gp = null;
            for (x_index = 0; x_index < line_glyph_position.Count; ++x_index)
            {
                double glyph_end = (x_index < line_glyph_position.Count - 1) ? line_glyph_position[x_index + 1].x_pos.start : line_glyph_position[x_index].x_pos.end;
                if (dx < glyph_end)
                {
                    gp = line_glyph_position[x_index];
                    break;
                }
            }

            if (gp == null)
            {
                GlyphPosition last_glyph = line_glyph_position[line_glyph_position.Count - 1];
                return new PositionWithAffinity(last_glyph.code_units.end, Affinity.UPSTREAM);
            }

            // Find the direction of the run that contains this glyph.
            TextDirection direction = TextDirection.ltr;
            foreach (CodeUnitRun run in code_unit_runs_)
            {
                if (gp.code_units.start >= run.code_units.start && gp.code_units.end <= run.code_units.end)
                {
                    direction = run.direction;
                    break;
                }
            }

            double glyph_center = (gp.x_pos.start + gp.x_pos.end) / 2;
            if ((direction == TextDirection.ltr && dx < glyph_center) || (direction == TextDirection.rtl && dx >= glyph_center))
            {
                return new PositionWithAffinity(gp.code_units.start, Affinity.DOWNSTREAM);
            }
            else
            {
                return new PositionWithAffinity(gp.code_units.end, Affinity.UPSTREAM);
            }
        }

        // Finds the first and last glyphs that define a word containing the glyph at
        // index offset.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Paragraph::Range<int> GetWordBoundary(int offset) const
        public Paragraph.Range<int> GetWordBoundary(int offset)
        {
            if (text_.Count == 0)
            {
                return new Range<int>(0, 0);
            }

            if (word_breaker_ == null)
            {
                UErrorCode status = U_ZERO_ERROR;
                word_breaker_.reset(icu.BreakIterator.createWordInstance(icu.Locale(), status));
                if (!U_SUCCESS(status))
                {
                    return new Range<int>(0, 0);
                }
            }

            word_breaker_.setText(icu.UnicodeString(false, text_.data(), text_.Count));

            int prev_boundary = word_breaker_.preceding(offset + 1);
            int next_boundary = word_breaker_.next();
            if (prev_boundary == icu.BreakIterator.DONE)
            {
                prev_boundary = offset;
            }
            if (next_boundary == icu.BreakIterator.DONE)
            {
                next_boundary = offset;
            }
            return new Range<int>(prev_boundary, next_boundary);
        }

        // Returns the number of lines the paragraph takes up. If the text exceeds the
        // amount width and maxlines provides, Layout() truncates the extra text from
        // the layout and this will return the max lines allowed.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int GetLineCount() const
        public int GetLineCount()
        {
            return line_heights_.Count;
        }

        // Checks if the layout extends past the maximum lines and had to be
        // truncated.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool DidExceedMaxLines() const
        public bool DidExceedMaxLines()
        {
            return did_exceed_max_lines_;
        }

        // Sets the needs_layout_ to dirty. When Layout() is called, a new Layout will
        // be performed when this is set to true. Can also be used to prevent a new
        // Layout from being calculated by setting to false.
        public void SetDirty(bool dirty = true)
        {
            needs_layout_ = dirty;
        }

        //C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
        //  friend class ParagraphBuilder;
        //C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, SimpleParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, SimpleRedParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, RainbowParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, DefaultStyleParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, BoldParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST_WINDOWS_DISABLED(ParagraphTest, LeftAlignParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST_WINDOWS_DISABLED(ParagraphTest, RightAlignParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST_WINDOWS_DISABLED(ParagraphTest, CenterAlignParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST_WINDOWS_DISABLED(ParagraphTest, JustifyAlignParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, DecorationsParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, ItalicsParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, ChineseParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, DISABLED_ArabicParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, SpacingParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, LongWordParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, KernScaleParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST_WINDOWS_DISABLED(ParagraphTest, NewlineParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST_WINDOWS_DISABLED(ParagraphTest, EmojiParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, HyphenBreakParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, RepeatLayoutParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, Ellipsize);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, UnderlineShiftParagraph);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, SimpleShadow);
        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FRIEND_TEST(ParagraphTest, ComplexShadow);

        // Starting data to layout.
        private List<UInt16> text_ = new List<UInt16>();
        private StyledRuns runs_ = new StyledRuns();
        private ParagraphStyle paragraph_style_ = new ParagraphStyle();
        private FontCollection font_collection_;

        private minikin.LineBreaker breaker_ = new minikin.LineBreaker();
        private icu.BreakIterator word_breaker_ = new icu.BreakIterator();

        private class LineRange
        {
            public LineRange(int s, int e, int eew, int ein, bool h)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.start = s;
                this.start = s;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.end = e;
                this.end = e;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.end_excluding_whitespace = eew;
                this.end_excluding_whitespace = eew;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.end_including_newline = ein;
                this.end_including_newline = ein;
                this.hard_break = h;
            }
            public int start = new int();
            public int end = new int();
            public int end_excluding_whitespace = new int();
            public int end_including_newline = new int();
            public bool hard_break;
        }
        private List<LineRange> line_ranges_ = new List<LineRange>();
        private List<double> line_widths_ = new List<double>();

        // Stores the result of Layout().
        private List<PaintRecord> records_ = new List<PaintRecord>();

        private List<double> line_heights_ = new List<double>();
        private List<double> line_baselines_ = new List<double>();
        private bool did_exceed_max_lines_;

        // Metrics for use in GetRectsForRange(...);
        // Per-line max metrics over all runs in a given line.
        private List<float> line_max_spacings_ = new List<float>();
        private List<float> line_max_descent_ = new List<float>();
        private List<float> line_max_ascent_ = new List<float>();
        // Overall left and right extremes over all lines.
        private double max_right_;
        private double min_left_;

        internal class BidiRun
        {
            public BidiRun(int s, int e, TextDirection d, TextStyle st)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.start_ = s;
                this.start_ = s;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.end_ = e;
                this.end_ = e;
                this.direction_ = d;
                this.style_ = st;
            }

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: int start() const
            public int start()
            {
                return start_;
            }
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: int end() const
            public int end()
            {
                return end_;
            }
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: TextDirection direction() const
            public TextDirection direction()
            {
                return direction_;
            }
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: const TextStyle& style() const
            public TextStyle style()
            {
                return style_;
            }
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: bool is_rtl() const
            public bool is_rtl()
            {
                return direction_ == TextDirection.rtl;
            }

            private int start_ = new int();
            private int end_ = new int();
            private TextDirection direction_;
            private readonly TextStyle style_;
        }

        internal class GlyphPosition
        {
            public Range<int> code_units = new Range<int>();
            public Range<double> x_pos = new Range<double>();

            public GlyphPosition(double x_start, double x_advance, int code_unit_index, int code_unit_width)
            {
                this.code_units = new Range<int>(code_unit_index, code_unit_index + code_unit_width);
                this.x_pos = new Paragraph.Range<double>(x_start, x_start + x_advance);
            }

            public void Shift(double delta)
            {
                x_pos.Shift(delta);
            }
        }

        internal class GlyphLine
        {
            // Glyph positions sorted by x coordinate.
            public readonly List<GlyphPosition> positions = new List<GlyphPosition>();
            public readonly int total_code_units = new int();

            //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
            public GlyphLine(List<GlyphPosition> p, int tcu)
            {
                this.positions = p;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.total_code_units = tcu;
                this.total_code_units = tcu;
            }
        }

        internal class CodeUnitRun
        {
            // Glyph positions sorted by code unit index.
            public List<GlyphPosition> positions = new List<GlyphPosition>();
            public Range<int> code_units = new Range<int>();
            public Range<double> x_pos = new Range<double>();
            public int line_number = new int();
            public SKFontMetrics font_metrics = new SKFontMetrics();
            public TextDirection direction;

            //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
            public CodeUnitRun(List<GlyphPosition> p, Range<int> cu, Range<double> x, int line, SKFontMetrics metrics, TextDirection dir)
            {
                this.positions = p;
                this.code_units = cu;
                this.x_pos = x;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: this.line_number = line;
                this.line_number = line;
                this.font_metrics = metrics;
                this.direction = dir;
            }

            public void Shift(double delta)
            {
                x_pos.Shift(delta);
                foreach (GlyphPosition position in positions)
                {
                    position.Shift(delta);
                }
            }
        }

        // Holds the laid out x positions of each glyph.
        private List<GlyphLine> glyph_lines_ = new List<GlyphLine>();

        // Holds the positions of each range of code units in the text.
        // Sorted in code unit index order.
        private List<CodeUnitRun> code_unit_runs_ = new List<CodeUnitRun>();

        // The max width of the paragraph as provided in the most recent Layout()
        // call.
        private double width_ = -1.0f;
        private double max_intrinsic_width_ = 0;
        private double min_intrinsic_width_ = 0;
        private double alphabetic_baseline_ = double.MaxValue; // FLT_MAX;
        private double ideographic_baseline_ = double.MaxValue; // FLT_MAX;

        private bool needs_layout_ = true;

        private class WaveCoordinates
        {
            public double x_start;
            public double y_start;
            public double x_end;
            public double y_end;

            public WaveCoordinates(double x_s, double y_s, double x_e, double y_e)
            {
                this.x_start = x_s;
                this.y_start = y_s;
                this.x_end = x_e;
                this.y_end = y_e;
            }
        }

        // Passes in the text and Styled Runs. text_ and runs_ will later be passed
        // into breaker_ in InitBreaker(), which is called in Layout().
        private void SetText(List<UInt16> text, StyledRuns runs)
        {
            needs_layout_ = true;
            if (text.Count == 0)
            {
                return;
            }
            text_ = text;
            runs_ = runs;
        }

        private void SetParagraphStyle(ParagraphStyle style)
        {
            needs_layout_ = true;
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: paragraph_style_ = style;
            paragraph_style_ = style;
        }

        private void SetFontCollection(FontCollection font_collection)
        {
            font_collection_ = font_collection;
        }

        // Break the text into lines.
        private bool ComputeLineBreaks()
        {
            line_ranges_.Clear();
            line_widths_.Clear();
            max_intrinsic_width_ = 0;

            List<int> newline_positions = new List<int>();
            for (int i = 0; i < text_.Count; ++i)
            {
                ULineBreak ulb = (ULineBreak)u_getIntPropertyValue(text_[i], UCHAR_LINE_BREAK);
                if (ulb == U_LB_LINE_FEED || ulb == U_LB_MANDATORY_BREAK)
                {
                    newline_positions.Add(i);
                }
            }
            newline_positions.Add(text_.Count);

            int run_index = 0;
            for (int newline_index = 0; newline_index < newline_positions.Count; ++newline_index)
            {
                int block_start = (newline_index > 0) ? newline_positions[newline_index - 1] + 1 : 0;
                int block_end = newline_positions[newline_index];
                int block_size = block_end - block_start;

                if (block_size == 0)
                {
                    line_ranges_.Add(new LineRange(block_start, block_end, block_end, block_end + 1, true));
                    line_widths_.Add(0);
                    continue;
                }

                breaker_.setLineWidths(0.0f, 0, width_);
                breaker_.setJustified(paragraph_style_.text_align == TextAlign.justify);
                breaker_.setStrategy(paragraph_style_.break_strategy);
                breaker_.resize(block_size);
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                breaker_.buffer =  text_.GetRange(block_start, block_size);
                breaker_.setText();

                // Add the runs that include this line to the LineBreaker.
                double block_total_width = 0;
                while (run_index < runs_.size())
                {
                    StyledRuns.Run run = runs_.GetRun(run_index);
                    if (run.start >= block_end)
                    {
                        break;
                    }
                    if (run.end < block_start)
                    {
                        run_index++;
                        continue;
                    }

                    minikin.FontStyle font = new minikin.FontStyle();
                    minikin.MinikinPaint paint = new minikin.MinikinPaint();
                    GlobalMembers.GetFontAndMinikinPaint(run.style, font, paint);
                    minikin.FontCollection collection = GetMinikinFontCollectionForStyle(run.style);
                    if (collection == null)
                    {
                        //FML_LOG(INFO) << "Could not find font collection for family \"" << run.style.font_family << "\".";
                        return false;
                    }
                    int run_start = Math.Max(run.start, block_start) - block_start;
                    int run_end = Math.Min(run.end, block_end) - block_start;
                    bool isRtl = (paragraph_style_.text_direction == TextDirection.rtl);
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                    //ORIGINAL LINE: double run_width = breaker_.addStyleRun(&paint, collection, font, run_start, run_end, isRtl);
                    double run_width = breaker_.addStyleRun(paint, collection, new minikin.FontStyle(font), run_start, run_end, isRtl);
                    block_total_width += run_width;

                    if (run.end > block_end)
                    {
                        break;
                    }
                    run_index++;
                }

                max_intrinsic_width_ = Math.Max(max_intrinsic_width_, block_total_width);

                int breaks_count = breaker_.computeBreaks();
                int[] breaks = breaker_.getBreaks();
                for (int i = 0; i < breaks_count; ++i)
                {
                    int break_start = (i > 0) ? breaks[i - 1] : 0;
                    int line_start = break_start + block_start;
                    int line_end = breaks[i] + block_start;
                    bool hard_break = i == breaks_count - 1;
                    int line_end_including_newline = (hard_break && line_end < text_.Count) ? line_end + 1 : line_end;
                    int line_end_excluding_whitespace = line_end;
                    while (line_end_excluding_whitespace > line_start && minikin.isLineEndSpace(text_[line_end_excluding_whitespace - 1]))
                    {
                        line_end_excluding_whitespace--;
                    }
                    line_ranges_.Add(new LineRange(line_start, line_end, line_end_excluding_whitespace, line_end_including_newline, hard_break));
                    line_widths_.Add(breaker_.getWidths()[i]);
                }

                breaker_.finish();
            }

            return true;
        }

        // Break the text into runs based on LTR/RTL text direction.
        private bool ComputeBidiRuns(List<BidiRun> result)
        {
            if (text_.Count == 0)
            {
                return true;
            }

            //C++ TO C# CONVERTER TODO TASK: Lambda expressions cannot be assigned to 'var':
            var ubidi_closer = (UBiDi b) =>
            {
                ubidi_close(b);
            };
            var bidi = new std::unique_ptr<UBiDi, decltype(ubidi_closer) > (ubidi_open(), ubidi_closer);
            if (bidi == null)
            {
                return false;
            }

            UBiDiLevel paraLevel = (paragraph_style_.text_direction == TextDirection.rtl) ? UBIDI_RTL : UBIDI_LTR;
            UErrorCode status = U_ZERO_ERROR;
            //C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
            ubidi_setPara(bidi.get(), text_.data(), text_.Count, paraLevel, null, status);
            if (!U_SUCCESS(status))
            {
                return false;
            }

            int bidi_run_count = ubidi_countRuns(bidi.get(), status);
            if (!U_SUCCESS(status))
            {
                return false;
            }

            // Build a map of styled runs indexed by start position.
            SortedDictionary<int, StyledRuns.Run> styled_run_map = new SortedDictionary<int, StyledRuns.Run>();
            for (int i = 0; i < runs_.size(); ++i)
            {
                StyledRuns.Run run = runs_.GetRun(i);
                styled_run_map.Add(run.start, run);
            }

            for (int bidi_run_index = 0; bidi_run_index < bidi_run_count; ++bidi_run_index)
            {
                int bidi_run_start = new int();
                int bidi_run_length = new int();
                UBiDiDirection direction = ubidi_getVisualRun(bidi.get(), bidi_run_index, bidi_run_start, bidi_run_length);
                if (!U_SUCCESS(status))
                {
                    return false;
                }

                // Exclude the leading bidi control character if present.
                UChar32 first_char = new UChar32();
                U16_GET(text_.data(), 0, bidi_run_start, (int)text_.Count, first_char);
                if (u_hasBinaryProperty(first_char, UCHAR_BIDI_CONTROL))
                {
                    bidi_run_start++;
                    bidi_run_length--;
                }
                if (bidi_run_length == 0)
                {
                    continue;
                }

                // Exclude the trailing bidi control character if present.
                UChar32 last_char = new UChar32();
                U16_GET(text_.data(), 0, bidi_run_start + bidi_run_length - 1, (int)text_.Count, last_char);
                if (u_hasBinaryProperty(last_char, UCHAR_BIDI_CONTROL))
                {
                    bidi_run_length--;
                }
                if (bidi_run_length == 0)
                {
                    continue;
                }

                int bidi_run_end = bidi_run_start + bidi_run_length;
                TextDirection text_direction = direction == UBIDI_RTL ? TextDirection.rtl : TextDirection.ltr;

                // Break this bidi run into chunks based on text style.
                List<BidiRun> chunks = new List<BidiRun>();
                int chunk_start = bidi_run_start;
                while (chunk_start < bidi_run_end)
                {
                    var styled_run_iter = styled_run_map.upper_bound(chunk_start);
                    styled_run_iter--;
                    StyledRuns.Run styled_run = styled_run_iter.Value;
                    int chunk_end = Math.Min(bidi_run_end, styled_run.end);
                    chunks.Add(chunk_start, chunk_end, text_direction, styled_run.style);
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                    //ORIGINAL LINE: chunk_start = chunk_end;
                    chunk_start = chunk_end
                }

                if (text_direction == TextDirection.ltr)
                {
                    result.AddRange(chunks);
                }
                else
                {
                    //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
                    result.insert(result.end(), chunks.rbegin(), chunks.rend());
                }
            }

            return true;
        }

        // Calculate the starting X offset of a line based on the line's width and
        // alignment.
        private double GetLineXOffset(double line_total_advance)
        {
            if (double.IsInfinity(width_))
            {
                return 0;
            }

            TextAlign align = paragraph_style_.effective_align();

            if (align == TextAlign.right)
            {
                return width_ - line_total_advance;
            }
            else if (align == TextAlign.center)
            {
                return (width_ - line_total_advance) / 2;
            }
            else
            {
                return 0;
            }
        }

        // Creates and draws the decorations onto the canvas.
        private void PaintDecorations(SKCanvas canvas, PaintRecord record, SKPoint base_offset)
        {
            if (record.style().decoration == (int)TextDecoration.kNone)
            {
                return;
            }

            SKFontMetrics metrics = record.metrics();
            SKPaint paint = new SKPaint();
            paint.setStyle(SKPaint.Style.kStroke_Style);
            if (record.style().decoration_color == GlobalMembers.SK_ColorTRANSPARENT)
            {
                paint.setColor(record.style().color);
            }
            else
            {
                paint.Color = record.style().decoration_color;
            }
            paint.setAntiAlias(true);

            // This is set to 2 for the double line style
            int decoration_count = 1;

            // Filled when drawing wavy decorations.
            SKPath path = new SKPath();

            double width = 0;
            if (paragraph_style_.text_align == TextAlign.justify && record.line() != GetLineCount() - 1)
            {
                width = width_;
            }
            else
            {
                width = record.GetRunWidth();
            }

            float underline_thickness;
            if ((metrics.fFlags & (int)SKFontMetrics.FontMetricsFlags.kUnderlineThicknessIsValid_Flag) && metrics.fUnderlineThickness > 0F)
            {
                underline_thickness = metrics.fUnderlineThickness;
            }
            else
            {
                // Backup value if the fUnderlineThickness metric is not available:
                // Divide by 14pt as it is the default size.
                underline_thickness = record.style().font_size / 14.0f;
            }
            paint.setStrokeWidth(underline_thickness * record.style().decoration_thickness_multiplier);

            SKPoint record_offset = base_offset + record.offset();
            float x = record_offset.X;
            float y = record_offset.Y;

            // Setup the decorations.
            switch (record.style().decoration_style)
            {
                case TextDecorationStyle.kSolid:
                    {
                        break;
                    }
                case TextDecorationStyle.kDouble:
                    {
                        decoration_count = 2;
                        break;
                    }
                // Note: the intervals are scaled by the thickness of the line, so it is
                // possible to change spacing by changing the decoration_thickness
                // property of TextStyle.
                case TextDecorationStyle.kDotted:
                    {
                        // Divide by 14pt as it is the default size.
                        float scale = record.style().font_size / 14.0f;
                        float[] intervals = { 1.0f * scale, 1.5f * scale, 1.0f * scale, 1.5f * scale };
                        //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
                        //ORIGINAL LINE: int count = sizeof(intervals) / sizeof(intervals[0]);
                        int count = intervals.Length;
                        paint.SetPathEffect(SKPathEffect.MakeCompose(SkDashPathEffect.Make(intervals, count, 0.0f), SkDiscretePathEffect.Make(0F, 0F)));
                        break;
                    }
                // Note: the intervals are scaled by the thickness of the line, so it is
                // possible to change spacing by changing the decoration_thickness
                // property of TextStyle.
                case TextDecorationStyle.kDashed:
                    {
                        // Divide by 14pt as it is the default size.
                        float scale = record.style().font_size / 14.0f;
                        float[] intervals = { 4.0f * scale, 2.0f * scale, 4.0f * scale, 2.0f * scale };
                        //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
                        //ORIGINAL LINE: int count = sizeof(intervals) / sizeof(intervals[0]);
                        int count = intervals.Length;
                        paint.setPathEffect(SKPathEffect.MakeCompose(SkDashPathEffect.Make(intervals, count, 0.0f), SkDiscretePathEffect.Make(0F, 0F)));
                        break;
                    }
                case TextDecorationStyle.kWavy:
                    {
                        int wave_count = 0;
                        double x_start = 0;
                        double wavelength = underline_thickness * record.style().decoration_thickness_multiplier;
                        path.MoveTo(x, y);
                        while (x_start + wavelength * 2 < width)
                        {
                            path.RQuadTo(wavelength, wave_count % 2 != 0 ? wavelength : -wavelength, wavelength * 2, 0F);
                            x_start += wavelength * 2;
                            ++wave_count;
                        }
                        break;
                    }
            }

            // Draw the decorations.
            // Use a for loop for "kDouble" decoration style
            for (int i = 0; i < decoration_count; i++)
            {
                double y_offset = i * underline_thickness * GlobalMembers.kDoubleDecorationSpacing;
                double y_offset_original = y_offset;
                // Underline
                if ((record.style().decoration & (int)TextDecoration.kUnderline) != 0)
                {
                    y_offset += ((metrics.fFlags & (int)SKFontMetrics.FontMetricsFlags.kUnderlinePositionIsValid_Flag) != null) ? metrics.fUnderlinePosition : underline_thickness;
                    if (record.style().decoration_style != TextDecorationStyle.kWavy)
                    {
                        canvas.DrawLine(x, (float)(y + y_offset), (float)(x + width), (float)(y + y_offset), paint);
                    }
                    else
                    {
                        SKPath offsetPath = new SKPath(path);
                        offsetPath.Offset(0F, (float)y_offset);
                        canvas.DrawPath(offsetPath, paint);
                    }
                    y_offset = y_offset_original;
                }
                // Overline
                if ((record.style().decoration & (int)TextDecoration.kOverline) != 0)
                {
                    // We subtract fAscent here because for double overlines, we want the
                    // second line to be above, not below the first.
                    y_offset -= metrics.fAscent;
                    if (record.style().decoration_style != TextDecorationStyle.kWavy)
                    {
                        canvas.DrawLine(x, y - y_offset, x + width, y - y_offset, paint);
                    }
                    else
                    {
                        SKPath offsetPath = new SKPath(path);
                        offsetPath.Offset(0F, (float)-y_offset);
                        canvas.DrawPath(offsetPath, paint);
                    }
                    y_offset = y_offset_original;
                }
                // Strikethrough
                if ((record.style().decoration & (int)TextDecoration.kLineThrough) != 0)
                {
                    if (metrics.fFlags & (int)SKFontMetrics.FontMetricsFlags.kStrikeoutThicknessIsValid_Flag != null)
                    {
                        paint.setStrokeWidth(metrics.fStrikeoutThickness * record.style().decoration_thickness_multiplier);
                    }
                    // Make sure the double line is "centered" vertically.
                    y_offset += (decoration_count - 1.0) * underline_thickness * GlobalMembers.kDoubleDecorationSpacing / -2.0;
                    y_offset += ((metrics.fFlags & (int)SKFontMetrics.FontMetricsFlags.kStrikeoutThicknessIsValid_Flag) != null) ? metrics.fStrikeoutPosition : metrics.fXHeight / -2.0;
                    if (record.style().decoration_style != TextDecorationStyle.kWavy)
                    {
                        canvas.DrawLine(x, y + y_offset, x + width, y + y_offset, paint);
                    }
                    else
                    {
                        SKPath offsetPath = new SKPath(path);
                        offsetPath.Offset(0F, (float)y_offset);
                        canvas.DrawPath(offsetPath, paint);
                    }
                    y_offset = y_offset_original;
                }
            }
        }

        // Draws the background onto the canvas.
        private void PaintBackground(SKCanvas canvas, PaintRecord record, SKPoint base_offset)
        {
            if (!record.style().has_background)
            {
                return;
            }

            SKFontMetrics metrics = record.metrics();
            SKRect rect = new SKRect(0, metrics.fAscent, record.GetRunWidth(), metrics.fDescent);
            rect.Offset(base_offset + record.offset());
            canvas.DrawRect(rect, record.style().background);
        }

        // Draws the shadows onto the canvas.
        private void PaintShadow(SKCanvas canvas, PaintRecord record, SKPoint offset)
        {
            if (record.style().text_shadows.Count == 0)
            {
                return;
            }
            foreach (TextShadow text_shadow in record.style().text_shadows)
            {
                if (!text_shadow.hasShadow())
                {
                    continue;
                }

                SKPaint paint = new SKPaint();
                paint.Color = text_shadow.color;
                if (text_shadow.blur_radius != 0.0)
                {
                    paint.setMaskFilter(SkMaskFilter.MakeBlur(SkBlurStyle.kNormal_SkBlurStyle, text_shadow.blur_radius, false));
                }
                canvas.DrawText(record.text(), offset.X + text_shadow.offset.X, offset.Y + text_shadow.offset.Y, paint);
            }
        }

        // Obtain a Minikin font collection matching this text style.
        private minikin.FontCollection GetMinikinFontCollectionForStyle(TextStyle style)
        {
            string locale;
            if (!string.IsNullOrEmpty(style.locale))
            {
                uint language_list_id = minikin.FontStyle.registerLanguageList(style.locale);
                minikin.FontLanguages langs = minikin.FontLanguageListCache.getById(language_list_id);
                if (langs != null)
                {
                    locale = langs[0].getString();
                }
            }

            return font_collection_.GetMinikinFontCollectionForFamily(style.font_family, locale);
        }

        // Get a default SkTypeface for a text style.
        private SKTypeface GetDefaultSkiaTypeface(TextStyle style)
        {
            minikin.FontCollection collection = GetMinikinFontCollectionForStyle(style);
            if (collection == null)
            {
                return null;
            }
            minikin.FakedFont faked_font = collection.baseFontFaked(GlobalMembers.GetMinikinFontStyle(style));
            return ((FontSkia)faked_font.font).GetSkTypeface();
        }

        //C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FML_DISALLOW_COPY_AND_ASSIGN(Paragraph);
    }

} // namespace FlutterBinding.Txt




//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "flutter/fml/logging.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/ubidi.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/utf16.h"

//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkAutoCanvasRestore(...) SK_REQUIRE_LOCAL_VAR(SkAutoCanvasRestore)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_REGISTER_FLATTENABLE(type) SkFlattenable::Register(#type, type::CreateProc);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_FLATTENABLE_HOOKS(type) static sk_sp<SkFlattenable> CreateProc(SkReadBuffer&); friend class SkFlattenable::PrivateInitializer; Factory getFactory() const override { return type::CreateProc; } const char* getTypeName() const override { return #type; }
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SkTypeface.h"
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_DECLARE_STATIC_MUTEX(name) static SkBaseMutex name;
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkAutoMutexAcquire(...) SK_REQUIRE_LOCAL_VAR(SkAutoMutexAcquire)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkAutoExclusive(...) SK_REQUIRE_LOCAL_VAR(SkAutoExclusive)

namespace FlutterBinding.Txt
{
    //C++ TO C# CONVERTER NOTE: C# does not allow anonymous namespaces:
    //namespace

    //public class GlyphTypeface
    //{
    //    public GlyphTypeface(SKTypeface typeface, minikin.FontFakery fakery)
    //    {
    //        this.typeface_ = typeface;
    //        this.fake_bold_ = fakery.isFakeBold();
    //        this.fake_italic_ = fakery.isFakeItalic();
    //    }

    //    //public static bool operator ==(GlyphTypeface ImpliedObject, GlyphTypeface other)
    //    //{
    //    //    return other.typeface_.get() == ImpliedObject.typeface_.get() && other.fake_bold_ == ImpliedObject.fake_bold_ && other.fake_italic_ == ImpliedObject.fake_italic_;
    //    //}

    //    //public static bool operator !=(GlyphTypeface ImpliedObject, GlyphTypeface other)
    //    //{
    //    //    return !(*ImpliedObject == other);
    //    //}

    //    public void apply(SKPaint paint)
    //    {
    //        paint.setTypeface(new SKTypeface(typeface_));
    //        paint.setFakeBoldText(fake_bold_);
    //        paint.setTextSkewX(fake_italic_ ? -DefineConstants.SK_Scalar1 / 4 : 0F);
    //    }

    //    private SKTypeface typeface_;
    //    private bool fake_bold_;
    //    private bool fake_italic_;
    //}

} // namespace FlutterBinding.Txt
