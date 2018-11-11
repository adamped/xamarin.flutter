using SkiaSharp;
using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    public abstract class Texture
    {
        protected Texture(ulong id)
        {
            this.id_ = id;
        }

        // Called from GPU thread.
        public abstract void Paint(SKCanvas canvas, SKRect bounds, bool freeze);

        // Called from GPU thread.
        public abstract void OnGRContextCreated();

        // Called from GPU thread.
        public abstract void OnGRContextDestroyed();

        // Called on GPU thread.
        public abstract void MarkNewFrameAvailable();

        public ulong Id()
        {
            return id_;
        }

        private ulong id_ = new ulong();

    }

    public class TextureRegistry
    {

        // Called from GPU thread.
        public void RegisterTexture(Texture texture)
        {
            mapping_[texture.Id()] = texture;
        }

        // Called from GPU thread.
        public void UnregisterTexture(ulong id)
        {
            mapping_.Remove(id);
        }

        // Called from GPU thread.
        public Texture GetTexture(ulong id)
        {
            var it = mapping_[id];
            return it; //: null; // This isn't right either
        }

        // Called from GPU thread.
        public void OnGRContextCreated()
        {
            foreach (var it in mapping_)
            {
                it.Value.OnGRContextCreated();
            }
        }

        // Called from GPU thread.
        public void OnGRContextDestroyed()
        {
            foreach (var it in mapping_)
            {
                it.Value.OnGRContextDestroyed();
            }
        }

        private SortedDictionary<ulong, Texture> mapping_ = new SortedDictionary<ulong, Texture>();
    }

}