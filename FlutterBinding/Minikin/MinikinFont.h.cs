using System.Collections.Generic;
using System;

/*
 * Copyright (C) 2013 The Android Open Source Project
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




// An abstraction for platform fonts, allowing Minikin to be used with
// multiple actual implementations of fonts.

namespace minikin
{

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class MinikinFont;

    // Possibly move into own .h file?
    // Note: if you add a field here, either add it to LayoutCacheKey or to
    // skipCache()
    public class MinikinPaint
    {
        public MinikinPaint()
        {
            this.font = null;
            this.size = 0F;
            this.scaleX = 0F;
            this.skewX = 0F;
            this.letterSpacing = 0F;
            this.wordSpacing = 0F;
            this.paintFlags = 0;
            this.fakery = new FontFakery();
            this.hyphenEdit = new HyphenEdit();
            this.fontFeatureSettings = string.Empty;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool skipCache() const
        public bool skipCache()
        {
            return !string.IsNullOrEmpty(fontFeatureSettings);
        }

        public MinikinFont font;
        public float size;
        public float scaleX;
        public float skewX;
        public float letterSpacing;
        public float wordSpacing;
        public uint paintFlags = new uint();
        public FontFakery fakery = new FontFakery();
        public HyphenEdit hyphenEdit = new HyphenEdit();
        public string fontFeatureSettings;
    }

    // Only a few flags affect layout, but those that do should have values
    // consistent with Android's paint flags.
    public enum MinikinPaintFlags
    {
        LinearTextFlag = 0x40,
    }

    public class MinikinRect
    {
        public float mLeft;
        public float mTop;
        public float mRight;
        public float mBottom;
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool isEmpty() const
        public bool isEmpty()
        {
            return mLeft == mRight || mTop == mBottom;
        }
        public void set(MinikinRect r)
        {
            mLeft = r.mLeft;
            mTop = r.mTop;
            mRight = r.mRight;
            mBottom = r.mBottom;
        }
        public void offset(float dx, float dy)
        {
            mLeft += dx;
            mTop += dy;
            mRight += dx;
            mBottom += dy;
        }
        public void setEmpty()
        {
            mLeft = mTop = mRight = mBottom = 0F;
        }
        public void join(MinikinRect r)
        {
            if (isEmpty())
            {
                set(r);
            }
            else if (!r.isEmpty())
            {
                mLeft = Math.Min(mLeft, r.mLeft);
                mTop = Math.Min(mTop, r.mTop);
                mRight = Math.Max(mRight, r.mRight);
                mBottom = Math.Max(mBottom, r.mBottom);
            }
        }
    }

    // Callback for freeing data
    public delegate void MinikinDestroyFunc(object data);

    public abstract class MinikinFont : System.IDisposable
    {
        public MinikinFont(int uniqueId)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.mUniqueId = uniqueId;
            this.mUniqueId = uniqueId;
        }

        public void Dispose()
        {
            //var _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
            //purgeHbFontLocked(this);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual float GetHorizontalAdvance(uint glyph_id, const MinikinPaint& paint) const = 0;
        public abstract float GetHorizontalAdvance(uint glyph_id, MinikinPaint paint);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual void GetBounds(MinikinRect* bounds, uint glyph_id, const MinikinPaint& paint) const = 0;
        public abstract void GetBounds(MinikinRect bounds, uint glyph_id, MinikinPaint paint);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual HarfBuzzSharp.Face* CreateHarfBuzzFace() const
        public virtual HarfBuzzSharp.Face CreateHarfBuzzFace()
        {
            return null;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual const ClassicVector<minikin::FontVariation>& GetAxes() const = 0;
        public abstract List<minikin.FontVariation> GetAxes();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual MinikinFont createFontWithVariation(const ClassicVector<FontVariation>&) const
        public virtual MinikinFont createFontWithVariation(List<FontVariation> UnnamedParameter)
        {
            return null;
        }

        public static uint MakeTag(char c1, char c2, char c3, char c4)
        {
            return ((uint)c1 << 24) | ((uint)c2 << 16) | ((uint)c3 << 8) | (uint)c4;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int GetUniqueId() const
        public int GetUniqueId()
        {
            return mUniqueId;
        }

        private readonly int mUniqueId = new int();
    }

} // namespace minikin

