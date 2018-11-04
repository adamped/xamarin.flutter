using System.Collections.Generic;

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






namespace minikin
{

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class MinikinFont;

    // FontStyle represents all style information needed to select an actual font
    // from a collection. The implementation is packed into two 32-bit words
    // so it can be efficiently copied, embedded in other objects, etc.
    public class FontStyle
    {
        public FontStyle() : this(0, 4, false)
        {
        }
        public FontStyle(int weight, bool italic) : this(0, weight, italic)
        {
        }
        public FontStyle(uint langListId) : this(new uint(langListId), 0, 4, false)
        {
        }

        public FontStyle(int variant, int weight, bool italic)
        {
             FontStyle(FontLanguageListCache.kEmptyListId, variant, weight, italic);
        }
        public FontStyle(uint languageListId, int variant, int weight, bool italic)
        {
            this.bits = pack(variant, weight, italic);
            this.mLanguageListId = languageListId;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int getWeight() const
        public int getWeight()
        {
            return bits & kWeightMask;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool getItalic() const
        public bool getItalic()
        {
            return (bits & kItalicMask) != 0;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int getVariant() const
        public int getVariant()
        {
            return (bits >> kVariantShift) & kVariantMask;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint getLanguageListId() const
        public uint getLanguageListId()
        {
            return mLanguageListId;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator ==(const FontStyle other) const
        public static bool operator ==(FontStyle ImpliedObject, FontStyle other)
        {
            return ImpliedObject.bits == other.bits && ImpliedObject.mLanguageListId == other.mLanguageListId;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: android::hash_t hash() const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  android::hash_t hash();

        // Looks up a language list from an internal cache and returns its ID.
        // If the passed language list is not in the cache, registers it and returns
        // newly assigned ID.
        public uint registerLanguageList(string languages)
        {
            std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
            return FontLanguageListCache.getId(languages);
        }

        private uint kWeightMask = (1 << 4) - 1;
        private uint kItalicMask = 1 << 4;
        private const int kVariantShift = 5;
        private uint kVariantMask = (1 << 2) - 1;

        public uint pack(int variant, int weight, bool italic)
        {
            return (weight & kWeightMask) | (italic ? kItalicMask : 0) | (variant << kVariantShift);
        }

        private uint bits = new uint();
        private uint mLanguageListId = new uint();
    }

    public enum FontVariant
    {
        VARIANT_DEFAULT = 0,
        VARIANT_COMPACT = 1,
        VARIANT_ELEGANT = 2,
    }

    // attributes representing transforms (fake bold, fake italic) to match styles
    public class FontFakery
    {
        public FontFakery()
        {
            this.mFakeBold = false;
            this.mFakeItalic = false;
        }
        public FontFakery(bool fakeBold, bool fakeItalic)
        {
            this.mFakeBold = fakeBold;
            this.mFakeItalic = fakeItalic;
        }
        // TODO: want to support graded fake bolding
        public bool isFakeBold()
        {
            return mFakeBold;
        }
        public bool isFakeItalic()
        {
            return mFakeItalic;
        }

        private bool mFakeBold;
        private bool mFakeItalic;
    }

    public class FakedFont
    {
        // ownership is the enclosing FontCollection
        public MinikinFont font;
        public FontFakery fakery = new FontFakery();
    }


    public class Font
    {
        public Font(MinikinFont typeface, FontStyle style)
        {
            this.typeface = typeface;
            this.style = style;
        }
        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public Font(MinikinFont typeface, FontStyle style)
        {
            this.typeface = typeface;
            this.style = style;
        }
        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public Font(Font o)
        {
            typeface = std::move(o.typeface);
            style = o.style;
            o.typeface = null;
        }
        public Font(Font o)
        {
            typeface = o.typeface;
            style = o.style;
        }

        public MinikinFont typeface;
        public FontStyle style = new FontStyle();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: ClassicUnorderedSet<AxisTag> getSupportedAxesLocked() const;
        public HashSet<AxisTag> getSupportedAxesLocked()
        {
            uint fvarTag = MinikinFont.MakeTag('f', 'v', 'a', 'r');
            HbBlob fvarTable = new HbBlob(getFontTable(typeface.get(), new uint(fvarTag)));
            if (fvarTable.size() == 0)
            {
                return new HashSet<AxisTag>();
            }

            HashSet<AxisTag> supportedAxes = new HashSet<AxisTag>();
            analyzeAxes(fvarTable.get(), fvarTable.size(), supportedAxes);
            return supportedAxes;
        }
    }

    public class FontVariation
    {
        public FontVariation(AxisTag axisTag, float value)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.axisTag = axisTag;
            this.axisTag.CopyFrom(axisTag);
            this.value = value;
        }
        public AxisTag axisTag = new AxisTag();
        public float value;
    }

    public class FontFamily
    {
        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public FontFamily(List<Font> fonts)
        {
            FontFamily(0, std::move(fonts));
        }
        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public FontFamily(int variant, List<Font> fonts)
        {
            FontFamily(FontLanguageListCache.kEmptyListId, variant, std::move(fonts));
        }
        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public FontFamily(uint langId, int variant, List<Font> fonts)
        {
            this.mLangId = langId;
            this.mVariant = variant;
            this.mFonts = std::move(fonts);
            this.mHasVSTable = false;
            computeCoverage();
        }

        // TODO: Good to expose FontUtil.h.
        public bool analyzeStyle(MinikinFont typeface, ref int weight, ref bool italic)
        {
            std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
            uint os2Tag = MinikinFont.MakeTag('O', 'S', '/', '2');
            HbBlob os2Table = new HbBlob(getFontTable(typeface.get(), new uint(os2Tag)));
            if (os2Table.get() == null)
            {
                return false;
            }
            return global::minikin.analyzeStyle(os2Table.get(), os2Table.size(), weight, italic);
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: FakedFont getClosestMatch(FontStyle style) const;
        public FakedFont getClosestMatch(FontStyle style)
        {
            Font bestFont = null;
            int bestMatch = 0;
            for (int i = 0; i < mFonts.size(); i++)
            {
                Font font = mFonts[i];
                int match = computeMatch(font.style, new FontStyle(style));
                if (i == 0 || match < bestMatch)
                {
                    bestFont = font;
                    bestMatch = match;
                }
            }
            if (bestFont != null)
            {
                return new FakedFont() { bestFont.typeface.get(), computeFakery(new FontStyle(style), bestFont.style)};
            }
            return new FakedFont() { null, FontFakery()};
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint langId() const
        public uint langId()
        {
            return mLangId;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int variant() const
        public int variant()
        {
            return mVariant;
        }

        // API's for enumerating the fonts in a family. These don't guarantee any
        // particular order
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int getNumFonts() const
        public int getNumFonts()
        {
            return mFonts.Count;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const MinikinFont*& getFont(int index) const
        public MinikinFont* getFont(int index)
        {
            return mFonts[index].typeface;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: FontStyle getStyle(int index) const
        public FontStyle getStyle(int index)
        {
            return mFonts[index].style;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool isColorEmojiFamily() const;
        public bool isColorEmojiFamily()
        {
            FontLanguages languageList = FontLanguageListCache.getById(mLangId);
            for (int i = 0; i < languageList.size(); ++i)
            {
                if (languageList[i].getEmojiStyle() == FontLanguage.EMSTYLE_EMOJI)
                {
                    return true;
                }
            }
            return false;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const ClassicUnorderedSet<AxisTag>& supportedAxes() const
        public HashSet<AxisTag> supportedAxes()
        {
            return mSupportedAxes;
        }

        // Get Unicode coverage.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const SparseBitSet& getCoverage() const
        public SparseBitSet getCoverage()
        {
            return mCoverage;
        }

        // Returns true if the font has a glyph for the code point and variation
        // selector pair. Caller should acquire a lock before calling the method.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool hasGlyph(uint codepoint, uint variationSelector) const;
        public bool hasGlyph(uint codepoint, uint variationSelector)
        {
            assertMinikinLocked();
            if (variationSelector != 0 && !mHasVSTable)
            {
                // Early exit if the variation selector is specified but the font doesn't
                // have a cmap format 14 subtable.
                return false;
            }

            FontStyle defaultStyle = new FontStyle();
            HarfBuzzSharp.Font font = getHbFontLocked(getClosestMatch(defaultStyle).font);
            uint unusedGlyph = new uint();
            bool result = hb_font_get_glyph(font, codepoint, variationSelector, unusedGlyph);
            hb_font_destroy(font);
            return result;
        }

        // Returns true if this font family has a variaion sequence table (cmap format
        // 14 subtable).
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool hasVSTable() const
        public bool hasVSTable()
        {
            return mHasVSTable;
        }

        // Creates new FontFamily based on this family while applying font variations.
        // Returns nullptr if none of variations apply to this family.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: FontFamily createFamilyWithVariation(const ClassicVector<FontVariation>& variations) const;
        public FontFamily createFamilyWithVariation(List<FontVariation> variations)
        {
            if (variations.Count == 0 || mSupportedAxes.empty())
            {
                return null;
            }

            bool hasSupportedAxis = false;
            foreach (FontVariation variation in variations)
            {
                if (mSupportedAxes.find(variation.axisTag) != mSupportedAxes.end())
                {
                    hasSupportedAxis = true;
                    break;
                }
            }
            if (!hasSupportedAxis)
            {
                // None of variation axes are suppored by this family.
                return null;
            }

            List<Font> fonts = new List<Font>();
            foreach (Font font in mFonts)
            {
                bool supportedVariations = false;
                std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
                HashSet<AxisTag> supportedAxes = font.getSupportedAxesLocked();
                if (supportedAxes.Count > 0)
                {
                    foreach (FontVariation variation in variations)
                    {
                        if (supportedAxes.find(variation.axisTag) != supportedAxes.end())
                        {
                            supportedVariations = true;
                            break;
                        }
                    }
                }
                MinikinFont minikinFont;
                if (supportedVariations)
                {
                    minikinFont = font.typeface.createFontWithVariation(variations);
                }
                if (minikinFont == null)
                {
                    minikinFont = font.typeface;
                }
                fonts.Add(Font(std::move(minikinFont), font.style));
            }

            return new FontFamily(mLangId, mVariant, std::move(fonts));
        }

        public void computeCoverage()
        {
            std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
            FontStyle defaultStyle = new FontStyle();
            MinikinFont typeface = getClosestMatch(defaultStyle).font;
            uint cmapTag = MinikinFont.MakeTag('c', 'm', 'a', 'p');
            HbBlob cmapTable = new HbBlob(getFontTable(typeface, new uint(cmapTag)));
            if (cmapTable.get() == null)
            {
                ALOGE("Could not get cmap table size!\n");
                return;
            }
            mCoverage = CmapCoverage.getCoverage(cmapTable.get(), cmapTable.size(), mHasVSTable);

            for (int i = 0; i < mFonts.size(); ++i)
            {
                HashSet<AxisTag> supportedAxes = mFonts[i].getSupportedAxesLocked();
                mSupportedAxes.insert(supportedAxes.GetEnumerator(), supportedAxes.end());
            }
        }

        private uint mLangId = new uint();
        private int mVariant;
        private List<Font> mFonts = new List<Font>();
        private HashSet<AxisTag> mSupportedAxes = new HashSet<AxisTag>();

        private SparseBitSet mCoverage = new SparseBitSet();
        private bool mHasVSTable;

        // Forbid copying and assignment.
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  FontFamily(const FontFamily&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  void operator =(const FontFamily&) = delete;
    }

} // namespace minikin

