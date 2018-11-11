using SkiaSharp;
using static FlutterBinding.Flow.Helper;
using static FlutterBinding.Flow.RasterCache;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    public class RasterCacheKey<ID> where ID : Entry
    {
        public RasterCacheKey(ID id, SKMatrix ctm)
        {
            this.id_ = id;
            this.matrix_ = ctm;
#if !SUPPORT_FRACTIONAL_TRANSLATION
            FML_DCHECK(ctm.TransX == 0F && ctm.TransY == 0F);
#endif
        }

        public ID id()
        {
            return id_;
        }

        public SKMatrix matrix()
        {
            return matrix_;
        }

        public class Hash
        {
            public static uint functorMethod(RasterCacheKey<ID> key)
            {
               return (uint)key.GetHashCode();
            }
        }

        public class Equal
        {
            public static bool functorMethod(RasterCacheKey<ID> lhs, RasterCacheKey<ID> rhs)
            {
                return lhs.id_.Equals(rhs.id_) && lhs.matrix_.Equals(rhs.matrix_);
            }
        }

        private ID id_;
        private SKMatrix matrix_ = new SKMatrix();
    }
}