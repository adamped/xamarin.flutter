using System.Collections.Generic;

// Copyright 2017 The Flutter Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

// Copyright 2017 The Flutter Authors. All rights reserved.
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
//ORIGINAL LINE: #define FML_DECLARE_THREAD_CHECKER(c) fml::ThreadChecker c
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_DCHECK_CREATION_THREAD_IS_CURRENT(c) FML_DCHECK((c).IsCreationThreadCurrent())
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_DECLARE_THREAD_CHECKER(c)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FML_DCHECK_CREATION_THREAD_IS_CURRENT(c) ((void)0)
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

namespace flow
{

// A queue that holds Skia objects that must be destructed on the the given task
// runner.
public class SkiaUnrefQueue : fml.RefCountedThreadSafe<SkiaUnrefQueue>
{
  public void Unref(SkRefCnt @object)
  {
	lock (mutex_)
	{
		objects_.AddLast(@object);
	}
	if (!drain_pending_)
	{
	  drain_pending_ = true;
//C++ TO C# CONVERTER TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: task_runner_->PostDelayedTask([strong = fml::Ref(this)]()
	  task_runner_.Dereference().PostDelayedTask(() =>
	  {
		  strong.Drain();
	  }, drain_delay_);
	}
  }

  // Usually, the drain is called automatically. However, during IO manager
  // shutdown (when the platform side reference to the OpenGL context is about
  // to go away), we may need to pre-emptively drain the unref queue. It is the
  // responsibility of the caller to ensure that no further unrefs are queued
  // after this call.
  public void Drain()
  {
	LinkedList<SkRefCnt> skia_objects = new LinkedList<SkRefCnt>();
	lock (mutex_)
	{
	  objects_.swap(skia_objects);
	  drain_pending_ = false;
	}

	foreach (SkRefCnt skia_object in skia_objects)
	{
	  skia_object.unref();
	}
  }

  private readonly fml.RefPtr<fml.TaskRunner> task_runner_ = new fml.RefPtr<fml.TaskRunner>();
  private readonly fml.TimeDelta drain_delay_ = new fml.TimeDelta();
  private object mutex_ = new object();
  private LinkedList<SkRefCnt> objects_ = new LinkedList<SkRefCnt>();
  private bool drain_pending_;

  private SkiaUnrefQueue(fml.RefPtr<fml.TaskRunner> task_runner, fml.TimeDelta delay)
  {
	  this.task_runner_ = new fml.RefPtr<fml.TaskRunner>(std::move(task_runner));
	  this.drain_delay_ = new fml.TimeDelta(delay);
	  this.drain_pending_ = false;
  }

  public new void Dispose()
  {
	Drain();
	  base.Dispose();
  }

//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
//  friend class::fml::RefCountedThreadSafe<SkiaUnrefQueue>;
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
//  friend class::fml::internal::MakeRefCountedHelper<SkiaUnrefQueue>;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SkiaUnrefQueue(const SkiaUnrefQueue&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SkiaUnrefQueue& operator =(const SkiaUnrefQueue&) = delete;
}

/// An object whose deallocation needs to be performed on an specific unref
/// queue. The template argument U need to have a call operator that returns
/// that unref queue.
//C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
public class SkiaGPUObject <T> : System.IDisposable
{

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  SkiaGPUObject() = default;

  public SkiaGPUObject(sk_sp<T> @object, fml.RefPtr<SkiaUnrefQueue> queue)
  {
	  this.object_ = new sk_sp<T>(std::move(@object));
	  this.queue_ = new fml.RefPtr<SkiaUnrefQueue>(std::move(queue));
	FML_DCHECK(queue_ != null && object_ != null);
  }

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  SkiaGPUObject(SkiaGPUObject&&) = default;

  public void Dispose()
  {
	  reset();
  }

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  SkiaGPUObject& operator =(SkiaGPUObject&&) = default;

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: sk_sp<T> get() const
  public sk_sp<T> get()
  {
	  return object_;
  }

  public void reset()
  {
	if (object_ != null)
	{
	  queue_.Dereference().Unref(object_.release());
	}
	queue_ = null;
	FML_DCHECK(object_ == null);
  }

  private sk_sp<T> object_ = new sk_sp<T>();
  private fml.RefPtr<SkiaUnrefQueue> queue_ = new fml.RefPtr<SkiaUnrefQueue>();

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SkiaGPUObject(const SkiaGPUObject&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SkiaGPUObject& operator =(const SkiaGPUObject&) = delete;
}

} // namespace flow




