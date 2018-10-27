using System.Collections.Generic;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    public abstract class Texture : System.IDisposable
    {
        protected Texture(int64_t id)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.id_ = id;
            this.id_.CopyFrom(id);
        }

        // Called from GPU thread.
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        // Called from GPU thread.
        public abstract void Paint(SkiaSharp.SKCanvas canvas, SkiaSharp.SKRect bounds, bool freeze);

        // Called from GPU thread.
        public abstract void OnGrContextCreated();

        // Called from GPU thread.
        public abstract void OnGrContextDestroyed();

        // Called on GPU thread.
        public abstract void MarkNewFrameAvailable();

        public int64_t Id()
        {
            return id_;
        }

        private int64_t id_ = new int64_t();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Texture(const Texture&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  Texture& operator =(const Texture&) = delete;
    }

    public class TextureRegistry : System.IDisposable
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  TextureRegistry();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        // Called from GPU thread.
        public void RegisterTexture(Texture texture)
        {
            mapping_[texture.Id()] = texture;
        }

        // Called from GPU thread.
        public void UnregisterTexture(int64_t id)
        {
            mapping_.Remove(id);
        }

        // Called from GPU thread.
        public Texture GetTexture(int64_t id)
        {
            var it = mapping_.find(id);
            //C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
            return it != mapping_.end() ? it.second : null;
        }

        // Called from GPU thread.
        public void OnGrContextCreated()
        {
            foreach (var it in mapping_)
            {
                it.second.OnGrContextCreated();
            }
        }

        // Called from GPU thread.
        public void OnGrContextDestroyed()
        {
            foreach (var it in mapping_)
            {
                it.second.OnGrContextDestroyed();
            }
        }

        private SortedDictionary<int64_t, Texture> mapping_ = new SortedDictionary<int64_t, Texture>();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TextureRegistry(const TextureRegistry&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  TextureRegistry& operator =(const TextureRegistry&) = delete;
    }

}