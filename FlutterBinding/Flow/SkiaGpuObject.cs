using System.Collections.Generic;

// Copyright 2017 The Flutter Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
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
    public class SkiaGPUObject<T> : System.IDisposable
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

}