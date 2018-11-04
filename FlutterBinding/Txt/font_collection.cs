//using System.Collections.Generic;

///*
// * Copyright 2017 Google Inc.
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *      http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

///*
// * Copyright 2017 Google Inc.
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *      http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */


////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "flutter/fml/macros.h"
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/googletest/googletest/include/gtest/gtest_prod.h" // nogncheck
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/skia/include/core/SkFontMgr.h"
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/skia/include/core/SkRefCnt.h"
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_NOTHING_ARG1(arg1)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_NOTHING_ARG2(arg1, arg2)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_NOTHING_ARG3(arg1, arg2, arg3)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_RESTRICT __restrict
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_RESTRICT __restrict__
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX512
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE42
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE41
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSSE3
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE3
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX2
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_AVX
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE2
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_CPU_SSE_LEVEL SK_CPU_SSE_LEVEL_SSE1
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_API __declspec(dllexport)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_API __declspec(dllimport)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) __has_feature(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_HAS_COMPILER_FEATURE(x) 0
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ATTRIBUTE(attr)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkNO_RETURN_HINT() do {} while (false)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK() DumpStackTrace(0, SkDebugfForDumpStackTrace, nullptr)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_DUMP_GOOGLE3_STACK()
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s(%d): fatal error: \"%s\"\n", __FILE__, __LINE__, message)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_DUMP_LINE_FORMAT(message) SkDebugf("%s:%d: fatal error: \"%s\"\n", __FILE__, __LINE__, message)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ABORT(message) do { SkNO_RETURN_HINT(); SK_DUMP_LINE_FORMAT(message); SK_DUMP_GOOGLE3_STACK(); sk_abort_no_print(); } while (false)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_COLOR_MATCHES_PMCOLOR_BYTE_ORDER (SK_A32_SHIFT == 24 && SK_R32_SHIFT == 16 && SK_G32_SHIFT == 8 && SK_B32_SHIFT == 0)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C3 ## 32_SHIFT == 0 && SK_ ## C2 ## 32_SHIFT == 8 && SK_ ## C1 ## 32_SHIFT == 16 && SK_ ## C0 ## 32_SHIFT == 24)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PMCOLOR_BYTE_ORDER(C0, C1, C2, C3) (SK_ ## C0 ## 32_SHIFT == 0 && SK_ ## C1 ## 32_SHIFT == 8 && SK_ ## C2 ## 32_SHIFT == 16 && SK_ ## C3 ## 32_SHIFT == 24)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_UNUSED __pragma(warning(suppress:4189))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_UNUSED SK_ATTRIBUTE(unused)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ALWAYS_INLINE __forceinline
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ALWAYS_INLINE SK_ATTRIBUTE(always_inline) inline
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_NEVER_INLINE __declspec(noinline)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_NEVER_INLINE SK_ATTRIBUTE(noinline)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) _mm_prefetch(reinterpret_cast<const char*>(ptr), _MM_HINT_T0)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PREFETCH(ptr) __builtin_prefetch(ptr)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr) __builtin_prefetch(ptr, 1)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PREFETCH(ptr)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_WRITE_PREFETCH(ptr)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_PRINTF_LIKE(A, B)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_GAMMA_EXPONENT (0.0f)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_HISTOGRAM_BOOLEAN(name, value)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_HISTOGRAM_ENUMERATION(name, value, boundary_value)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkASSERT_RELEASE(cond) static_cast<void>( (cond) ? (void)0 : []{ SK_ABORT("assert(" #cond ")"); }() )
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkASSERT(cond) SkASSERT_RELEASE(cond)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>( (cond) ? (void)0 : [&]{ SkDebugf(fmt"\n", __VA_ARGS__); SK_ABORT("assert(" #cond ")"); }() )
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGFAIL(message) SK_ABORT(message)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...) SkASSERTF(false, fmt, ##__VA_ARGS__)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGCODE(...) __VA_ARGS__
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGF(...) SkDebugf(__VA_ARGS__)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkAssertResult(cond) SkASSERT(cond)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkASSERT(cond) static_cast<void>(0)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkASSERTF(cond, fmt, ...) static_cast<void>(0)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGFAIL(message)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGFAILF(fmt, ...)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGCODE(...)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDEBUGF(...)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkAssertResult(cond) if (cond) {} do {} while(false)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ARRAY_COUNT(array) (sizeof(SkArrayCountHelper(array)))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_MACRO_CONCAT(X, Y) SK_MACRO_CONCAT_IMPL_PRIV(X, Y)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_MACRO_CONCAT_IMPL_PRIV(X, Y) X ## Y
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_MACRO_APPEND_LINE(name) SK_MACRO_CONCAT(name, __LINE__)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_REQUIRE_LOCAL_VAR(classname) static_assert(false, "missing name for " #classname)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_BEGIN_REQUIRE_DENSE _Pragma("GCC diagnostic push") _Pragma("GCC diagnostic error \"-Wpadded\"")
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_END_REQUIRE_DENSE _Pragma("GCC diagnostic pop")
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_INIT_TO_AVOID_WARNING = 0
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define __inline static __inline
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarAs2sCompliment(x) SkFloatAs2sCompliment(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_sqrt(x) sqrtf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_sin(x) sinf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_cos(x) cosf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_tan(x) tanf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_floor(x) floorf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_ceil(x) ceilf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_trunc(x) truncf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_acos(x) static_cast<float>(acos(x))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_asin(x) static_cast<float>(asin(x))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_acos(x) acosf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_asin(x) asinf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_atan2(y,x) atan2f(y,x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_abs(x) fabsf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_copysign(x, y) copysignf(x, y)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_mod(x,y) fmodf(x,y)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_exp(x) expf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_log(x) logf(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_round(x) sk_float_floor((x) + 0.5f)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_log2(x) log2f(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_isnan(a) sk_float_isnan(a)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_MinS32FitsInFloat -SK_MaxS32FitsInFloat
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_MaxS64FitsInFloat (SK_MaxS64 >> (63-24) << (63-24))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_MinS64FitsInFloat -SK_MaxS64FitsInFloat
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_floor2int(x) sk_float_saturate2int(sk_float_floor(x))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_round2int(x) sk_float_saturate2int(sk_float_floor((x) + 0.5f))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_ceil2int(x) sk_float_saturate2int(sk_float_ceil(x))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_floor2int_no_saturate(x) (int)sk_float_floor(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_round2int_no_saturate(x) (int)sk_float_floor((x) + 0.5f)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_float_ceil2int_no_saturate(x) (int)sk_float_ceil(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_floor(x) floor(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_round(x) floor((x) + 0.5)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_ceil(x) ceil(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_floor2int(x) (int)floor(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_round2int(x) (int)floor((x) + 0.5)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define sk_double_ceil2int(x) (int)ceil(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_FloatNaN NAN
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_FloatInfinity (+INFINITY)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_FloatNegativeInfinity (-INFINITY)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_FLT_DECIMAL_DIG FLT_DECIMAL_DIG
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ScalarMax 3.402823466e+38f
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ScalarInfinity SK_FloatInfinity
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ScalarNegativeInfinity SK_FloatNegativeInfinity
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ScalarNaN SK_FloatNaN
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarFloorToScalar(x) sk_float_floor(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarCeilToScalar(x) sk_float_ceil(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarRoundToScalar(x) sk_float_floor((x) + 0.5f)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarTruncToScalar(x) sk_float_trunc(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarFloorToInt(x) sk_float_floor2int(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarCeilToInt(x) sk_float_ceil2int(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarRoundToInt(x) sk_float_round2int(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarAbs(x) sk_float_abs(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarCopySign(x, y) sk_float_copysign(x, y)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarMod(x, y) sk_float_mod(x,y)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarSqrt(x) sk_float_sqrt(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarPow(b, e) sk_float_pow(b, e)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarSin(radians) (float)sk_float_sin(radians)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarCos(radians) (float)sk_float_cos(radians)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarTan(radians) (float)sk_float_tan(radians)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarASin(val) (float)sk_float_asin(val)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarACos(val) (float)sk_float_acos(val)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarATan2(y, x) (float)sk_float_atan2(y,x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarExp(x) (float)sk_float_exp(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarLog(x) (float)sk_float_log(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarLog2(x) (float)sk_float_log2(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkIntToScalar(x) static_cast<SkScalar>(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkIntToFloat(x) static_cast<float>(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarTruncToInt(x) sk_float_saturate2int(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarToFloat(x) static_cast<float>(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkFloatToScalar(x) static_cast<SkScalar>(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarToDouble(x) static_cast<double>(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDoubleToScalar(x) sk_double_to_float(x)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ScalarMin (-SK_ScalarMax)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarDiv(numer, denom) sk_ieee_float_divide(numer, denom)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarFastInvert(x) sk_ieee_float_divide(SK_Scalar1, (x))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarAve(a, b) (((a) + (b)) * SK_ScalarHalf)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkScalarHalf(a) ((a) * SK_ScalarHalf)
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkDegreesToRadians(degrees) ((degrees) * (SK_ScalarPI / 180))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SkRadiansToDegrees(radians) ((radians) * (180 / SK_ScalarPI))
////C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
////ORIGINAL LINE: #define SK_ScalarNearlyZero (SK_Scalar1 / (1 << 12))

namespace FlutterBinding.Txt
{

    public class FontCollection : System.IDisposable
    {
        public FontCollection()
        {
            this.enable_font_fallback_ = true;
        }

        ////C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public void Dispose() { }

        ////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        ////ORIGINAL LINE: int GetFontManagersCount() const
        //  public int GetFontManagersCount()
        //  {
        //	return GetFontManagerOrder().Count;
        //  }

        //  public void SetDefaultFontManager(sk_sp<SkFontMgr> font_manager)
        //  {
        ////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
        ////ORIGINAL LINE: default_font_manager_ = font_manager;
        //	default_font_manager_.CopyFrom(font_manager);
        //  }
        //  public void SetAssetFontManager(sk_sp<SkFontMgr> font_manager)
        //  {
        ////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
        ////ORIGINAL LINE: asset_font_manager_ = font_manager;
        //	asset_font_manager_.CopyFrom(font_manager);
        //  }
        //  public void SetDynamicFontManager(sk_sp<SkFontMgr> font_manager)
        //  {
        ////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
        ////ORIGINAL LINE: dynamic_font_manager_ = font_manager;
        //	dynamic_font_manager_.CopyFrom(font_manager);
        //  }
        //  public void SetTestFontManager(sk_sp<SkFontMgr> font_manager)
        //  {
        ////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
        ////ORIGINAL LINE: test_font_manager_ = font_manager;
        //	test_font_manager_.CopyFrom(font_manager);
        //  }

        //  public minikin.FontCollection GetMinikinFontCollectionForFamily(string font_family, string locale)
        //  {
        //	// Look inside the font collections cache first.
        //	FamilyKey family_key = new FamilyKey(font_family, locale);
        //	var cached = font_collections_cache_.find(family_key);
        ////C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
        //	if (cached != font_collections_cache_.end())
        //	{
        ////C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
        //	  return cached.second;
        //	}

        //	foreach (sk_sp<SkFontMgr> manager in GetFontManagerOrder())
        //	{
        //	  minikin.FontFamily minikin_family = CreateMinikinFontFamily(manager, font_family);
        //	  if (minikin_family == null)
        //	  {
        //		continue;
        //	  }

        //	  // Create a vector of font families for the Minikin font collection.
        //	  List<minikin.FontFamily> minikin_families = new List<minikin.FontFamily>() {minikin_family};
        //	  if (enable_font_fallback_)
        //	  {
        //		foreach (string fallback_family in fallback_fonts_for_locale_[locale])
        //		{
        //		  minikin_families.Add(fallback_fonts_[fallback_family]);
        //		}
        //	  }

        //	  // Create the minikin font collection.
        //	  var font_collection = new minikin.FontCollection(minikin_families);
        //	  if (enable_font_fallback_)
        //	  {
        //		font_collection.set_fallback_font_provider(std::make_unique<TxtFallbackFontProvider>(this));
        //	  }

        //	  // Cache the font collection for future queries.
        //	  font_collections_cache_[family_key] = font_collection;

        //	  return font_collection;
        //	}

        //	var default_font_family = GetDefaultFontFamily();
        //	if (font_family != default_font_family)
        //	{
        //	  minikin.FontCollection default_collection = GetMinikinFontCollectionForFamily(default_font_family, "");
        //	  font_collections_cache_[family_key] = default_collection;
        //	  return default_collection;
        //	}

        //	// No match found in any of our font managers.
        //	return null;
        //  }

        //  public minikin.FontFamily * MatchFallbackFont(uint ch, string locale)
        //  {
        //	foreach (sk_sp<SkFontMgr> manager in GetFontManagerOrder())
        //	{
        //	  List<char> bcp47 = new List<char>();
        //	  if (!string.IsNullOrEmpty(locale))
        //	  {
        //		bcp47.Add(locale);
        //	  }
        //	  SKTypeface typeface = new SKTypeface(manager.matchFamilyStyleCharacter(0, SkFontStyle(), bcp47.data(), bcp47.Count, ch));
        //	  if (typeface == null)
        //	  {
        //		continue;
        //	  }

        //	  SkString sk_family_name = new SkString();
        //	  typeface.getFamilyName(sk_family_name);
        //	  string family_name = sk_family_name.c_str();

        //	  fallback_fonts_for_locale_[locale].Add(family_name);

        //	  return GetFallbackFontFamily(manager, family_name);
        //	}

        //	return GlobalMembers.g_null_family;
        //  }

        //  // Do not provide alternative fonts that can match characters which are
        //  // missing from the requested font family.
        //  public void DisableFontFallback()
        //  {
        //	enable_font_fallback_ = false;
        //  }

        //  private class FamilyKey
        //  {
        //	public FamilyKey(string family, string loc)
        //	{
        //		this.font_family = family;
        //		this.locale = loc;
        //	}

        //	public string font_family;
        //	public string locale;

        ////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        ////ORIGINAL LINE: bool operator ==(const FontCollection::FamilyKey& other) const
        //	public static bool operator == (FamilyKey ImpliedObject, FontCollection.FamilyKey other)
        //	{
        //	  return ImpliedObject.font_family == other.font_family && ImpliedObject.locale == other.locale;
        //	}

        //	public class Hasher
        //	{
        ////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        ////ORIGINAL LINE: int operator ()(const FontCollection::FamilyKey& key) const
        //	  public static int functorMethod(FontCollection.FamilyKey key)
        //	  {
        //		return std::hash<string>()(key.font_family) ^ std::hash<string>()(key.locale);
        //	  }
        //	}
        //  }

        //  private sk_sp<SkFontMgr> default_font_manager_ = new sk_sp<SkFontMgr>();
        //  private sk_sp<SkFontMgr> asset_font_manager_ = new sk_sp<SkFontMgr>();
        //  private sk_sp<SkFontMgr> dynamic_font_manager_ = new sk_sp<SkFontMgr>();
        //  private sk_sp<SkFontMgr> test_font_manager_ = new sk_sp<SkFontMgr>();
        //  private Dictionary<FamilyKey, minikin.FontCollection, FamilyKey.Hasher> font_collections_cache_ = new Dictionary<FamilyKey, minikin.FontCollection, FamilyKey.Hasher>();
        //  private Dictionary<string, minikin.FontFamily> fallback_fonts_ = new Dictionary<string, minikin.FontFamily>();
        //  private Dictionary<string, SortedSet<string>> fallback_fonts_for_locale_ = new Dictionary<string, SortedSet<string>>();
        private bool enable_font_fallback_;


        //  // Return the available font managers in the order they should be queried.
        ////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        ////ORIGINAL LINE: ClassicVector<sk_sp<SkFontMgr>> GetFontManagerOrder() const
        //  private List<sk_sp<SkFontMgr>> GetFontManagerOrder()
        //  {
        //	List<sk_sp<SkFontMgr>> order = new List<sk_sp<SkFontMgr>>();
        //	if (test_font_manager_ != null)
        //	{
        //	  order.Add(test_font_manager_);
        //	}
        //	if (dynamic_font_manager_ != null)
        //	{
        //	  order.Add(dynamic_font_manager_);
        //	}
        //	if (asset_font_manager_ != null)
        //	{
        //	  order.Add(asset_font_manager_);
        //	}
        //	if (default_font_manager_ != null)
        //	{
        //	  order.Add(default_font_manager_);
        //	}
        //	return order;
        //  }

        //  private minikin.FontFamily CreateMinikinFontFamily(sk_sp<SkFontMgr> manager, string family_name)
        //  {
        //	sk_sp<SkFontStyleSet> font_style_set = new sk_sp<SkFontStyleSet>(manager.matchFamily(family_name));
        //	if (font_style_set == null || font_style_set.count() == 0)
        //	{
        //	  return null;
        //	}

        //	List<minikin.Font> minikin_fonts = new List<minikin.Font>();

        //	// Add fonts to the Minikin font family.
        //	for (int i = 0; i < font_style_set.count(); ++i)
        //	{
        //	  // Create the skia typeface.
        //	  SKTypeface skia_typeface = new SKTypeface(SKTypeface(font_style_set.createTypeface(i)));
        //	  if (skia_typeface == null)
        //	  {
        //		continue;
        //	  }

        //	  // Create the minikin font from the skia typeface.
        //	  // Divide by 100 because the weights are given as "100", "200", etc.
        //	  minikin.Font minikin_font = new minikin.Font(new FontSkia(new SKTypeface(skia_typeface)), new minikin.FontStyle(new uint(skia_typeface.fontStyle().weight() / 100, skia_typeface.isItalic())));

        //	  minikin_fonts.Add(minikin_font);
        //	}

        //	return new minikin.FontFamily(minikin_fonts);
        //  }

        //  private minikin.FontFamily * GetFallbackFontFamily(sk_sp<SkFontMgr> manager, string family_name)
        //  {
        //	var fallback_it = fallback_fonts_.find(family_name);
        ////C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
        //	if (fallback_it != fallback_fonts_.end())
        //	{
        ////C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
        //	  return fallback_it.second;
        //	}

        //	minikin.FontFamily minikin_family = CreateMinikinFontFamily(manager, family_name);
        //	if (minikin_family == null)
        //	{
        //	  return GlobalMembers.g_null_family;
        //	}

        //	var insert_it = fallback_fonts_.Add(family_name, minikin_family);

        //	// Clear the cache to force creation of new font collections that will include
        //	// this fallback font.
        //	font_collections_cache_.Clear();

        //	return insert_it.first.second;
        //  }

        ////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //  FML_DISALLOW_COPY_AND_ASSIGN(FontCollection);
        //}

        //} // namespace FlutterBinding.Txt



        ////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
        ////#include "flutter/fml/logging.h"

        //namespace FlutterBinding.Txt
        //{

        ////C++ TO C# CONVERTER NOTE: C# does not allow anonymous namespaces:
        ////namespace


        //public class TxtFallbackFontProvider : minikin.FontCollection.FallbackFontProvider
        //{
        //  public TxtFallbackFontProvider(FontCollection font_collection)
        //  {
        //	  this.font_collection_ = font_collection;
        //  }

        //  public override minikin.FontFamily * matchFallbackFont(uint ch, string locale)
        //  {
        //	FontCollection fc = font_collection_.@lock();
        //	if (fc != null)
        //	{
        //	  return fc.MatchFallbackFont(ch, locale);
        //	}
        //	else
        //	{
        //	  return GlobalMembers.g_null_family;
        //	}
        //  }

        //  private std::weak_ptr<FontCollection> font_collection_ = new std::weak_ptr<FontCollection>();
        //}

        ////C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
        ////FontCollection::~FontCollection() = default;
    }
} // namespace FlutterBinding.Txt
