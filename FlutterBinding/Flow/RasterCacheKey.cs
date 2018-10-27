using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;
using SkiaSharp;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
    //ORIGINAL LINE: template <typename ID>
    public class RasterCacheKey<ID>
    {
        public RasterCacheKey(ID id, SKMatrix ctm)
        {
            this.id_ = id;
            this.matrix_ = new SKMatrix(ctm);
            matrix_[SKMatrix.kMTransX] = GlobalMembers.SkScalarFraction(ctm.getTranslateX());
            matrix_[SKMatrix.kMTransY] = GlobalMembers.SkScalarFraction(ctm.getTranslateY());
#if !SUPPORT_FRACTIONAL_TRANSLATION
            FML_DCHECK(matrix_.getTranslateX() == 0F && matrix_.getTranslateY() == 0F);
#endif
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: ID id() const
        public ID id()
        {
            return id_;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const SKMatrix& matrix() const
        public SKMatrix matrix()
        {
            return matrix_;
        }

        public class Hash
        {
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: uint operator ()(RasterCacheKey const& key) const
            public static uint functorMethod(RasterCacheKey key)
            {
                return std::hash<ID>()(key.id_);
            }
        }

        public class Equal
        {
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: constexpr bool operator ()(const RasterCacheKey& lhs, const RasterCacheKey& rhs) const
            public static bool functorMethod(RasterCacheKey lhs, RasterCacheKey rhs)
            {
                return lhs.id_ == rhs.id_ && lhs.matrix_ == rhs.matrix_;
            }
        }

        //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
        //ORIGINAL LINE: template <class Value>
        //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to C++11 template aliases:
        // using Map = Dictionary<RasterCacheKey, Value, Hash, Equal>;

        private ID id_;

        // ctm where only fractional (0-1) translations are preserved:
        //   matrix_ = ctm;
        //   matrix_[SKMatrix::kMTransX] = SkScalarFraction(ctm.getTranslateX());
        //   matrix_[SKMatrix::kMTransY] = SkScalarFraction(ctm.getTranslateY());
        private SKMatrix matrix_ = new SKMatrix();
    }
}