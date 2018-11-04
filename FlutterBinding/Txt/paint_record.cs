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
//#include "flutter/fml/logging.h"
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
//ORIGINAL LINE: #define SK_WHEN(condition, T) skstd::enable_if_t<!!(condition), T>
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendS32_MaxSize (SkStrAppendU32_MaxSize + 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendS64_MaxSize (SkStrAppendU64_MaxSize + 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendScalar SkStrAppendFloat

using SkiaSharp;

namespace FlutterBinding.Txt
{

    // PaintRecord holds the layout data after Paragraph::Layout() is called. This
    // stores all nessecary offsets, blobs, metrics, and more for Skia to draw the
    // text.
    public class PaintRecord //: System.IDisposable
    {
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  PaintRecord() = delete;

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public PaintRecord(TextStyle style, SKPoint offset, SkTextBlob text, SKFontMetrics metrics, int line, double run_width)
        {
            this.style_ = style;
            this.offset_ = offset;
            this.text_ = text;
            this.metrics_ = metrics;
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.line_ = line;
            this.line_ = line;
            this.run_width_ = run_width;
        }

        public PaintRecord(TextStyle style, SkTextBlob text, SKFontMetrics metrics, int line, double run_width)
        {
            this.style_ = style;
            this.text_ = text;
            this.metrics_ = metrics;
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.line_ = line;
            this.line_ = line;
            this.run_width_ = run_width;
        }

        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public PaintRecord(PaintRecord other)
        {
            style_ = other.style_;
            offset_ = other.offset_;
            text_ = other.text_;
            metrics_ = other.metrics_;
            line_ = other.line_;
            run_width_ = other.run_width_;
        }

        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        //C++ TO C# CONVERTER TODO TASK: The = operator cannot be overloaded in C#:
        //public static PaintRecord operator = (PaintRecord && other)
        //{
        //    style_ = other.style_;
        //    offset_ = other.offset_;
        //    text_ = other.text_;
        //    metrics_ = other.metrics_;
        //    line_ = other.line_;
        //    run_width_ = other.run_width_;
        //    return this;
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: SKPoint offset() const
        public SKPoint offset()
        {
            return offset_;
        }

        public void SetOffset(SKPoint pt)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = pt;
            offset_ = pt;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: SkTextBlob* text() const
        public SkTextBlob text()
        {
            return text_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const SKPaint::FontMetrics& metrics() const
        public SKFontMetrics metrics()
        {
            return metrics_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const TextStyle& style() const
        public TextStyle style()
        {
            return style_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int line() const
        public int line()
        {
            return line_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: double GetRunWidth() const
        public double GetRunWidth()
        {
            return run_width_;
        }

        private TextStyle style_ = new TextStyle();
        // offset_ is the overall offset of the origin of the SkTextBlob.
        private SKPoint offset_ = new SKPoint();
        // SkTextBlob stores the glyphs and coordinates to draw them.
        private SkTextBlob text_ = new SkTextBlob();
        // FontMetrics stores the measurements of the font used.
        private SKFontMetrics metrics_ = new SKFontMetrics();
        private int line_ = new int();
        private double run_width_ = 0.0f;

        //C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FML_DISALLOW_COPY_AND_ASSIGN(PaintRecord);
    }

} // namespace FlutterBinding.Txt


//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "flutter/fml/logging.h"

namespace FlutterBinding.Txt
{

    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
    //PaintRecord::~PaintRecord() = default;

} // namespace FlutterBinding.Txt
