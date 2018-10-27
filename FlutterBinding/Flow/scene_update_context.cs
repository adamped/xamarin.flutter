﻿using System;
using System.Collections.Generic;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

// Copyright 2016 The Chromium Authors. All rights reserved.
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
//ORIGINAL LINE: #define FML_FRIEND_REF_COUNTED_THREAD_SAFE(T) friend class ::fml::RefCountedThreadSafe<T>
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_FRIEND_MAKE_REF_COUNTED(T) friend class ::fml::internal::MakeRefCountedHelper<T>
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
//ORIGINAL LINE: #define FML_DECLARE_THREAD_CHECKER(c) fml::ThreadChecker c
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_DCHECK_CREATION_THREAD_IS_CURRENT(c) FML_DCHECK((c).IsCreationThreadCurrent())
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_DECLARE_THREAD_CHECKER(c)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_DCHECK_CREATION_THREAD_IS_CURRENT(c) ((void)0)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_THREAD_ANNOTATION_ATTRIBUTE__(x)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_THREAD_ANNOTATION_ATTRIBUTE__(x)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_GUARDED_BY(x) FML_THREAD_ANNOTATION_ATTRIBUTE__(guarded_by(x))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_PT_GUARDED_BY(x) FML_THREAD_ANNOTATION_ATTRIBUTE__(pt_guarded_by(x))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ACQUIRE(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(acquire_capability(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_RELEASE(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(release_capability(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ACQUIRED_AFTER(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(acquired_after(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ACQUIRED_BEFORE(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(acquired_before(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_EXCLUSIVE_LOCKS_REQUIRED(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(exclusive_locks_required(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_SHARED_LOCKS_REQUIRED(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(shared_locks_required(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_LOCKS_EXCLUDED(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(locks_excluded(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_LOCK_RETURNED(x) FML_THREAD_ANNOTATION_ATTRIBUTE__(lock_returned(x))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_LOCKABLE FML_THREAD_ANNOTATION_ATTRIBUTE__(lockable)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_SCOPED_LOCKABLE FML_THREAD_ANNOTATION_ATTRIBUTE__(scoped_lockable)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_EXCLUSIVE_LOCK_FUNCTION(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(exclusive_lock_function(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_SHARED_LOCK_FUNCTION(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(shared_lock_function(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ASSERT_EXCLUSIVE_LOCK(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(assert_exclusive_lock(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ASSERT_SHARED_LOCK(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(assert_shared_lock(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_EXCLUSIVE_TRYLOCK_FUNCTION(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(exclusive_trylock_function(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_SHARED_TRYLOCK_FUNCTION(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(shared_trylock_function(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_UNLOCK_FUNCTION(...) FML_THREAD_ANNOTATION_ATTRIBUTE__(unlock_function(__VA_ARGS__))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_NO_THREAD_SAFETY_ANALYSIS FML_THREAD_ANNOTATION_ATTRIBUTE__(no_thread_safety_analysis)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_NOT_THREAD_SAFE FML_NO_THREAD_SAFETY_ANALYSIS
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ALLOW_UNUSED_LOCAL(x) false ? (void)x : (void)0
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_NOINLINE __declspec(noinline)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ALIGNAS(byte_alignment)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ALIGNAS(byte_alignment) __declspec(align(byte_alignment))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ALIGNOF(type) __alignof__(type)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_ALIGNOF(type) __alignof(type)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_PRINTF_FORMAT(format_param, dots_param)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_PRINTF_FORMAT(format_param, dots_param)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_WHEN(condition, T) skstd::enable_if_t<!!(condition), T>
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendS32_MaxSize (SkStrAppendU32_MaxSize + 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendS64_MaxSize (SkStrAppendU64_MaxSize + 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkStrAppendScalar SkStrAppendFloat

namespace flow
{

//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class Layer;
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class ExportNodeHolder;
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class ExportNode;

public class SceneUpdateContext : System.IDisposable
{
  public abstract class SurfaceProducerSurface : System.IDisposable
  {
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//	virtual ~SurfaceProducerSurface() = default;

	public abstract size_t AdvanceAndGetAge();

	public abstract bool FlushSessionAcquireAndReleaseEvents();

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool IsValid() const = 0;
	public abstract bool IsValid();

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual SkISize GetSize() const = 0;
	public abstract SkISize GetSize();

	public abstract void SignalWritesFinished(Action on_writes_committed);

	public abstract scenic.Image GetImage();

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual sk_sp<SkSurface> GetSkiaSurface() const = 0;
	public abstract sk_sp<SkSurface> GetSkiaSurface();
  }

  public abstract class SurfaceProducer : System.IDisposable
  {
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//	virtual ~SurfaceProducer() = default;

	public abstract std::unique_ptr<SurfaceProducerSurface> ProduceSurface(SkISize size);

	public abstract void SubmitSurface(std::unique_ptr<SurfaceProducerSurface> surface);
  }

  public class Entity : System.IDisposable
  {
	public Entity(SceneUpdateContext context)
	{
		this.context_ = new flow.SceneUpdateContext(context);
		this.previous_entity_ = context.top_entity_;
		this.entity_node_ = context.session();
	  if (previous_entity_ != null)
	  {
		previous_entity_.entity_node_.AddChild(entity_node_);
	  }
	  context.top_entity_ = this;
	}
	public void Dispose()
	{
	  FML_DCHECK(context_.top_entity_ == this);
	  context_.top_entity_ = previous_entity_;
	}

	public SceneUpdateContext context()
	{
		return context_;
	}
	public scenic.EntityNode entity_node()
	{
		return entity_node_;
	}

	private SceneUpdateContext context_;
	private readonly Entity previous_entity_;

	private scenic.EntityNode entity_node_ = new scenic.EntityNode();
  }

  public class Clip : Entity
  {
	public Clip(SceneUpdateContext context, scenic.Shape shape, SkRect shape_bounds) : base(context)
	{
	  scenic.ShapeNode shape_node = new scenic.ShapeNode(context.session());
	  shape_node.SetShape(shape);
	  shape_node.SetTranslation(shape_bounds.width() * 0.5f + shape_bounds.left(), shape_bounds.height() * 0.5f + shape_bounds.top(), 0.0f);

	  entity_node().AddPart(shape_node);
	  entity_node().SetClip(0u, true);
	}
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	public void Dispose();
  }

  public class Transform : Entity
  {
	public Transform(SceneUpdateContext context, SkMatrix transform) : base(context)
	{
		this.previous_scale_x_ = context.top_scale_x_;
		this.previous_scale_y_ = context.top_scale_y_;
	  if (!transform.isIdentity())
	  {
		// TODO(MZ-192): The perspective and shear components in the matrix
		// are not handled correctly.
		MatrixDecomposition decomposition = new MatrixDecomposition(transform);
		if (decomposition.IsValid())
		{
		  entity_node().SetTranslation(decomposition.translation().x(), decomposition.translation().y(), decomposition.translation().z());

		  entity_node().SetScale(decomposition.scale().x(), decomposition.scale().y(), decomposition.scale().z());
		  context.top_scale_x_ *= decomposition.scale().x();
		  context.top_scale_y_ *= decomposition.scale().y();

		  entity_node().SetRotation(decomposition.rotation().fData[0], decomposition.rotation().fData[1], decomposition.rotation().fData[2], decomposition.rotation().fData[3]);
		}
	  }
	}
	public Transform(SceneUpdateContext context, float scale_x, float scale_y, float scale_z) : base(context)
	{
		this.previous_scale_x_ = context.top_scale_x_;
		this.previous_scale_y_ = context.top_scale_y_;
	  if (scale_x != 1.0f || scale_y != 1.0f || scale_z != 1.0f)
	  {
		entity_node().SetScale(scale_x, scale_y, scale_z);
		context.top_scale_x_ *= scale_x;
		context.top_scale_y_ *= scale_y;
	  }
	}
	public new void Dispose()
	{
	  context().top_scale_x_ = previous_scale_x_;
	  context().top_scale_y_ = previous_scale_y_;
		base.Dispose();
	}

	private readonly float previous_scale_x_;
	private readonly float previous_scale_y_;
  }

  public class Frame : Entity
  {
	public Frame(SceneUpdateContext context, SkRRect rrect, uint color, float elevation) : base(context)
	{
		this.rrect_ = new SkRRect(rrect);
		this.color_ = color;
		this.paint_bounds_ = new SkRect(SkRect.MakeEmpty());
	  if (elevation != 0.0F)
	  {
		entity_node().SetTranslation(0.0f, 0.0f, elevation);
	  }
	}
	public new void Dispose()
	{
	  context().CreateFrame(entity_node(), rrect_, color_, paint_bounds_, std::move(paint_layers_));
		base.Dispose();
	}

	public void AddPaintedLayer(Layer layer)
	{
	  FML_DCHECK(layer.needs_painting());
	  paint_layers_.Add(layer);
	  paint_bounds_.join(layer.paint_bounds());
	}

	private readonly SkRRect rrect_;
	private readonly uint color_;

	private List<Layer> paint_layers_ = new List<Layer>();
	private SkRect paint_bounds_ = new SkRect();
  }

  public SceneUpdateContext(scenic.Session session, SurfaceProducer surface_producer)
  {
	  this.session_ = session;
	  this.surface_producer_ = surface_producer;
	FML_DCHECK(surface_producer_ != null);
  }

  public void Dispose()
  {
	// Release Mozart session resources for all ExportNodes.
	foreach (var export_node in export_nodes_)
	{
	  export_node.Dispose(false);
	}
  }

  public scenic.Session session()
  {
	  return session_;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool has_metrics() const
  public bool has_metrics()
  {
	  return !metrics_ == null;
  }
  public void set_metrics(fuchsia.ui.gfx.MetricsPtr metrics)
  {
	metrics_ = std::move(metrics);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const fuchsia::ui::gfx::MetricsPtr& metrics() const
  public fuchsia.ui.gfx.MetricsPtr metrics()
  {
	  return metrics_;
  }

  public void AddChildScene(ExportNode export_node, SkPoint offset, bool hit_testable)
  {
	FML_DCHECK(top_entity_);

	export_node.Bind(this, top_entity_.entity_node(), offset, hit_testable);
  }

  // Adds reference to |export_node| so we can call export_node->Dispose() in
  // our destructor. Caller is responsible for calling RemoveExportNode() before
  // |export_node| is destroyed.
  public void AddExportNode(ExportNode export_node)
  {
	export_nodes_.Add(export_node); // Might already have been added.
  }

  // Removes reference to |export_node|.
  public void RemoveExportNode(ExportNode export_node)
  {
	export_nodes_.erase(export_node);
  }

  // TODO(chinmaygarde): This method must submit the surfaces as soon as paint
  // tasks are done. However, given that there is no support currently for
  // Vulkan semaphores, we need to submit all the surfaces after an explicit
  // CPU wait. Once Vulkan semaphores are available, this method must return
  // void and the implementation must submit surfaces on its own as soon as the
  // specific canvas operations are done.
  public List<std::unique_ptr<flow.SceneUpdateContext.SurfaceProducerSurface>> ExecutePaintTasks(CompositorContext.ScopedFrame frame)
  {
	TRACE_EVENT0("flutter", "SceneUpdateContext::ExecutePaintTasks");
	List<std::unique_ptr<SurfaceProducerSurface>> surfaces_to_submit = new List<std::unique_ptr<SurfaceProducerSurface>>();
	foreach (var task in paint_tasks_)
	{
	  FML_DCHECK(task.surface);
	  SkiaSharp.SKCanvas canvas = task.surface.GetSkiaSurface().getCanvas();
	  Layer.PaintContext context = new flow.Layer.PaintContext(canvas, frame.context().frame_time(), frame.context().engine_time(), frame.context().texture_registry(), frame.context().raster_cache(), false);
	  canvas.restoreToCount(1);
	  canvas.save();
	  canvas.clear(task.background_color);
	  canvas.scale(task.scale_x, task.scale_y);
	  canvas.translate(-task.left, -task.top);
	  foreach (Layer layer in task.layers)
	  {
		layer.Paint(context);
	  }
	  surfaces_to_submit.emplace_back(std::move(task.surface));
	}
	paint_tasks_.Clear();
	return surfaces_to_submit;
  }

  private class PaintTask
  {
	public std::unique_ptr<SurfaceProducerSurface> surface = new std::unique_ptr<SurfaceProducerSurface>();
	public float left;
	public float top;
	public float scale_x;
	public float scale_y;
	public uint background_color;
	public List<Layer> layers = new List<Layer>();
  }

  private void CreateFrame(scenic.EntityNode entity_node, SkRRect rrect, uint color, SkRect paint_bounds, List<Layer> paint_layers)
  {
	// Frames always clip their children.
	entity_node.SetClip(0u, true);

	// We don't need a shape if the frame is zero size.
	if (rrect.isEmpty())
	{
	  return;
	}

	// Add a part which represents the frame's geometry for clipping purposes
	// and possibly for its texture.
	// TODO(MZ-137): Need to be able to express the radii as vectors.
	SkRect shape_bounds = rrect.getBounds();
	scenic.RoundedRectangle shape = new scenic.RoundedRectangle(session_, rrect.width(), rrect.height(), rrect.radii(SkRRect.Corner.kUpperLeft_Corner).x(), rrect.radii(SkRRect.Corner.kUpperRight_Corner).x(), rrect.radii(SkRRect.Corner.kLowerRight_Corner).x(), rrect.radii(SkRRect.Corner.kLowerLeft_Corner).x());
	scenic.ShapeNode shape_node = new scenic.ShapeNode(session_);
	shape_node.SetShape(shape);
	shape_node.SetTranslation(shape_bounds.width() * 0.5f + shape_bounds.left(), shape_bounds.height() * 0.5f + shape_bounds.top(), 0.0f);
	entity_node.AddPart(shape_node);

	// Check whether the painted layers will be visible.
	if (paint_bounds.isEmpty() || !paint_bounds.intersects(shape_bounds))
	{
	  paint_layers.Clear();
	}

	// Check whether a solid color will suffice.
	if (paint_layers.Count == 0)
	{
	  SetShapeColor(shape_node, color);
	  return;
	}

	// Apply current metrics and transformation scale factors.
	float scale_x = metrics_.scale_x * top_scale_x_;
	float scale_y = metrics_.scale_y * top_scale_y_;

	// If the painted area only covers a portion of the frame then we can
	// reduce the texture size by drawing just that smaller area.
//C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
//ORIGINAL LINE: SkRect inner_bounds = shape_bounds;
	SkRect inner_bounds = new SkRect(shape_bounds);
	inner_bounds.intersect(paint_bounds);
	if (inner_bounds != shape_bounds && rrect.contains(inner_bounds))
	{
	  SetShapeColor(shape_node, color);

	  scenic.Rectangle inner_shape = new scenic.Rectangle(session_, inner_bounds.width(), inner_bounds.height());
	  scenic.ShapeNode inner_node = new scenic.ShapeNode(session_);
	  inner_node.SetShape(inner_shape);
	  inner_node.SetTranslation(inner_bounds.width() * 0.5f + inner_bounds.left(), inner_bounds.height() * 0.5f + inner_bounds.top(), 0.0f);
	  entity_node.AddPart(inner_node);
	  SetShapeTextureOrColor(inner_node, color, scale_x, scale_y, inner_bounds, std::move(paint_layers));
	  return;
	}

	// Apply a texture to the whole shape.
	SetShapeTextureOrColor(shape_node, color, scale_x, scale_y, shape_bounds, std::move(paint_layers));
  }
  private void SetShapeTextureOrColor(scenic.ShapeNode shape_node, uint color, float scale_x, float scale_y, SkRect paint_bounds, List<Layer> paint_layers)
  {
	scenic.Image image = GenerateImageIfNeeded(color, scale_x, scale_y, paint_bounds, std::move(paint_layers));
	if (image != null)
	{
	  scenic.Material material = new scenic.Material(session_);
	  material.SetTexture(image);
	  shape_node.SetMaterial(material);
	  return;
	}

	SetShapeColor(shape_node, color);
  }
  private void SetShapeColor(scenic.ShapeNode shape_node, uint color)
  {
	if ((((color) >> 24) & 0xFF) == 0)
	{
	  return;
	}

	scenic.Material material = new scenic.Material(session_);
	material.SetColor((((color) >> 16) & 0xFF), (((color) >> 8) & 0xFF), (((color) >> 0) & 0xFF), (((color) >> 24) & 0xFF));
	shape_node.SetMaterial(material);
  }
  private scenic.Image GenerateImageIfNeeded(uint color, float scale_x, float scale_y, SkRect paint_bounds, List<Layer> paint_layers)
  {
	// Bail if there's nothing to paint.
	if (paint_layers.Count == 0)
	{
	  return null;
	}

	// Bail if the physical bounds are empty after rounding.
	SkISize physical_size = SkISize.Make(paint_bounds.width() * scale_x, paint_bounds.height() * scale_y);
	if (physical_size.isEmpty())
	{
	  return null;
	}

	// Acquire a surface from the surface producer and register the paint tasks.
	var surface = surface_producer_.ProduceSurface(physical_size);

	if (surface == null)
	{
  //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the following C++ macro:
	  !((global::fml.ShouldCreateLogMessage(global::fml.LOG_ERROR))) ? ()0 : new global::fml.LogMessageVoidify() & (new global::fml.LogMessage(global::fml.LOG_ERROR, __FILE__, __LINE__, null).stream()) << "Could not acquire a surface from the surface producer " + "of size: " << physical_size.width() << "x" << physical_size.height();
	  return null;
	}

	var image = surface.GetImage();

	// Enqueue the paint task.
	paint_tasks_.Add({.surface = std::move(surface), .left = paint_bounds.left(), .top = paint_bounds.top(), .scale_x = scale_x, .scale_y = scale_y, .background_color = color, .layers = std::move(paint_layers)});
	return image;
  }

  private Entity top_entity_ = null;
  private float top_scale_x_ = 1.0f;
  private float top_scale_y_ = 1.0f;

  private readonly scenic.Session session_;
  private readonly SurfaceProducer surface_producer_;

  private fuchsia.ui.gfx.MetricsPtr metrics_ = new fuchsia.ui.gfx.MetricsPtr();

  private List<PaintTask> paint_tasks_ = new List<PaintTask>();

  // Save ExportNodes so we can dispose them in our destructor.
  private SortedSet<ExportNode> export_nodes_ = new SortedSet<ExportNode>();

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SceneUpdateContext(const SceneUpdateContext&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SceneUpdateContext& operator =(const SceneUpdateContext&) = delete;
}

} // namespace flow



//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPE_HELPER(t) skc_##t
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPE(t) SKC_TYPE_HELPER(t)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPE(t) t
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_AS_HELPER(t) as_##t
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_AS(t) SKC_AS_HELPER(t)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_CONVERT_HELPER(t) convert_##t
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_CONVERT(t) SKC_CONVERT_HELPER(t)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_CONVERT_MODE_HELPER(t,m) convert_##t##_##m
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_CONVERT_MODE(t,m) SKC_CONVERT_HELPER(t)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_SHORT_MIN (-SKC_SHORT_MAX - 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_INT_MIN (-SKC_INT_MAX - 1)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPED_HANDLE_MASK_TYPE (SKC_TYPED_HANDLE_TYPE_IS_PATH | SKC_TYPED_HANDLE_TYPE_IS_RASTER)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPED_HANDLE_TO_HANDLE(h) ((h) & ~SKC_TYPED_HANDLE_MASK_TYPE)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPED_HANDLE_IS_TYPE(h,t) ((h) & (t))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPED_HANDLE_IS_PATH(h) ((h) & SKC_TYPED_HANDLE_TYPE_IS_PATH)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SKC_TYPED_HANDLE_IS_RASTER(h) ((h) & SKC_TYPED_HANDLE_TYPE_IS_RASTER)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_TRACE_COUNTER(category_group, name, count) TRACE_COUNTER(category_group, name, 0u, name, count)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT0(a, b) TRACE_DURATION(a, b)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT1(a, b, c, d) TRACE_DURATION(a, b, c, d)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT2(a, b, c, d, e, f) TRACE_DURATION(a, b, c, d, e, f)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_BEGIN0(a, b, c) TRACE_ASYNC_BEGIN(a, b, c)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_END0(a, b, c) TRACE_ASYNC_END(a, b, c)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_BEGIN1(a, b, c, d, e) TRACE_ASYNC_BEGIN(a, b, c, d, e)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_END1(a, b, c, d, e) TRACE_ASYNC_END(a, b, c, d, e)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define __FML__TOKEN_CAT__(x, y) x##y
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define __FML__TOKEN_CAT__2(x, y) __FML__TOKEN_CAT__(x, y)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define __FML__AUTO_TRACE_END(name) ::fml::tracing::ScopedInstantEnd __FML__TOKEN_CAT__2(__trace_end_, __LINE__)(name);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_TRACE_COUNTER(category_group, name, count) ::fml::tracing::TraceCounter(category_group, name, count);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT0(category_group, name) ::fml::tracing::TraceEvent0(category_group, name); __FML__AUTO_TRACE_END(name)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT1(category_group, name, arg1_name, arg1_val) ::fml::tracing::TraceEvent1(category_group, name, arg1_name, arg1_val); __FML__AUTO_TRACE_END(name)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT2(category_group, name, arg1_name, arg1_val, arg2_name, arg2_val) ::fml::tracing::TraceEvent2(category_group, name, arg1_name, arg1_val, arg2_name, arg2_val); __FML__AUTO_TRACE_END(name)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_BEGIN0(category_group, name, id) ::fml::tracing::TraceEventAsyncBegin0(category_group, name, id);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_END0(category_group, name, id) ::fml::tracing::TraceEventAsyncEnd0(category_group, name, id);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_BEGIN1(category_group, name, id, arg1_name, arg1_val) ::fml::tracing::TraceEventAsyncBegin1(category_group, name, id, arg1_name, arg1_val);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_ASYNC_END1(category_group, name, id, arg1_name, arg1_val) ::fml::tracing::TraceEventAsyncEnd1(category_group, name, id, arg1_name, arg1_val);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_EVENT_INSTANT0(category_group, name) ::fml::tracing::TraceEventInstant0(category_group, name);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_FLOW_BEGIN(category, name, id) ::fml::tracing::TraceEventFlowBegin0(category, name, id);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_FLOW_STEP(category, name, id) ::fml::tracing::TraceEventFlowStep0(category, name, id);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define TRACE_FLOW_END(category, name, id) ::fml::tracing::TraceEventFlowEnd0(category, name, id);
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SK_DECLARE_STATIC_MUTEX(name) static SkBaseMutex name;
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkAutoMutexAcquire(...) SK_REQUIRE_LOCAL_VAR(SkAutoMutexAcquire)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SkAutoExclusive(...) SK_REQUIRE_LOCAL_VAR(SkAutoExclusive)

namespace flow
{

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//SceneUpdateContext::Clip::~Clip() = default;

} // namespace flow
