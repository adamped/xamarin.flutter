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

    public class FontCollection
    {
        public FontCollection(FontFamily typeface)
        {
            this.mMaxChar = 0;
            List<FontFamily> typefaces = new List<FontFamily>();
            typefaces.Add(typeface);
            init(typefaces);
        }
        //C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
        public FontCollection(vector<FontFamily> typefaces)
        {
            this.mMaxChar = 0;
            init(typefaces);
        }

        // libtxt extension: an interface for looking up fallback fonts for characters
        // that do not match this collection's font families.
        public abstract class FallbackFontProvider //: System.IDisposable
        {
            //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
            //	virtual ~FallbackFontProvider() = default;
            public abstract FontFamily* matchFallbackFont(uint ch, string locale);
        }

        public class Run
        {
            public FakedFont fakedFont = new FakedFont();
            public int start;
            public int end;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void itemize(const UInt16* string, int string_length, FontStyle style, ClassicVector<Run>* result) const;
        public void itemize(UInt16 @string, int string_size, FontStyle style, vector<Run> result)
        {
            uint langListId = style.getLanguageListId();
            int variant = style.getVariant();
            FontFamily lastFamily = null;
            Run run = null;

            if (string_size == 0)
            {
                return;
            }

            const uint kEndOfString = 0xFFFFFFFF;

            uint nextCh = 0;
            uint prevCh = 0;
            int nextUtf16Pos = 0;
            int readLength = 0;
            U16_NEXT(@string, readLength, string_size, nextCh);

            do
            {
                uint ch = new uint(nextCh);
                int utf16Pos = nextUtf16Pos;
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: nextUtf16Pos = readLength;
                nextUtf16Pos.CopyFrom(readLength);
                if (readLength < string_size)
                {
                    U16_NEXT(@string, readLength, string_size, nextCh);
                }
                else
                {
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                    //ORIGINAL LINE: nextCh = kEndOfString;
                    nextCh = kEndOfString;
                }

                bool shouldContinueRun = false;
                if (lastFamily != null)
                {
                    if (isStickyWhitelisted(ch))
                    {
                        // Continue using existing font as long as it has coverage and is
                        // whitelisted
                        shouldContinueRun = lastFamily.getCoverage().get(ch);
                    }
                    else if (ch == SOFT_HYPHEN || isVariationSelector(ch))
                    {
                        // Always continue if the character is the soft hyphen or a variation
                        // selector.
                        shouldContinueRun = true;
                    }
                }

                if (!shouldContinueRun)
                {
                    FontFamily* family = getFamilyForChar(ch, isVariationSelector(new uint(nextCh)) ? nextCh : 0, langListId, variant);
                    if (utf16Pos == 0 || family.get() != lastFamily)
                    {
                        int start = utf16Pos;
                        // Workaround for combining marks and emoji modifiers until we implement
                        // per-cluster font selection: if a combining mark or an emoji modifier
                        // is found in a different font that also supports the previous
                        // character, attach previous character to the new run. U+20E3 COMBINING
                        // ENCLOSING KEYCAP, used in emoji, is handled properly by this since
                        // it's a combining mark too.
                        if (utf16Pos != 0 && ((U_GET_GC_MASK(ch) & U_GC_M_MASK) != 0 || (isEmojiModifier(ch) && isEmojiBase(prevCh))) && family != null && family.getCoverage().get(prevCh))
                        {
                            int prevChLength = U16_LENGTH(prevCh);
                            run.end -= prevChLength;
                            if (run.start == run.end)
                            {
                                result.pop_back();
                            }
                            start -= prevChLength;
                        }
                        result.push_back({ family.getClosestMatch(style), (int)start, 0});
                        run = result.back();
                        lastFamily = family.get();
                    }
                }
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: prevCh = ch;
                prevCh.CopyFrom(ch);
                run.end = nextUtf16Pos; // exclusive
            } while (nextCh != kEndOfString);
        }

        // Returns true if there is a glyph for the code point and variation selector
        // pair. Returns false if no fonts have a glyph for the code point and
        // variation selector pair, or invalid variation selector is passed.
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool hasVariationSelector(uint baseCodepoint, uint variationSelector) const;
        public bool hasVariationSelector(uint baseCodepoint, uint variationSelector)
        {
            if (!isVariationSelector(new uint(variationSelector)))
            {
                return false;
            }
            if (baseCodepoint >= mMaxChar)
            {
                return false;
            }

            std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);

            // Currently mRanges can not be used here since it isn't aware of the
            // variation sequence.
            for (int i = 0; i < mVSFamilyVec.size(); i++)
            {
                if (mVSFamilyVec[i].hasGlyph(baseCodepoint, variationSelector))
                {
                    return true;
                }
            }

            // Even if there is no cmap format 14 subtable entry for the given sequence,
            // should return true for <char, text presentation selector> case since we
            // have special fallback rule for the sequence. Note that we don't need to
            // restrict this to already standardized variation sequences, since Unicode is
            // adding variation sequences more frequently now and may even move towards
            // allowing text and emoji variation selectors on any character.
            if (variationSelector == TEXT_STYLE_VS)
            {
                for (int i = 0; i < mFamilies.size(); ++i)
                {
                    if (!mFamilies[i].isColorEmojiFamily() && mFamilies[i].hasGlyph(baseCodepoint, 0))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Get base font with fakery information (fake bold could affect metrics)
        public FakedFont baseFontFaked(FontStyle style)
        {
            return mFamilies[0].getClosestMatch(style);
        }

        // Creates new FontCollection based on this collection while applying font
        // variations. Returns nullptr if none of variations apply to this collection.
        public FontCollection createCollectionWithVariation(List<FontVariation> variations)
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
                // None of variation axes are supported by this font collection.
                return null;
            }

            List<FontFamily> families = new List<FontFamily>();
            foreach (FontFamily family in mFamilies)
            {
                FontFamily newFamily = family.createFamilyWithVariation(variations);
                if (newFamily != null)
                {
                    families.Add(newFamily);
                }
                else
                {
                    families.Add(family);
                }
            }

            return new FontCollection(families);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const ClassicUnorderedSet<AxisTag>& getSupportedTags() const
        public HashSet<AxisTag> getSupportedTags()
        {
            return mSupportedAxes;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint getId() const;
        public uint getId()
        {
            return mId;
        }

        public void set_fallback_font_provider(std::unique_ptr<FallbackFontProvider> ffp)
        {
            mFallbackFontProvider = std::move(ffp);
        }

        private const int kLogCharsPerPage = 8;
        private int kPageMask = (1 << kLogCharsPerPage) - 1;

        // mFamilyVec holds the indices of the mFamilies and mRanges holds the range
        // of indices of mFamilyVec. The maximum number of pages is 0x10FF (U+10FFFF
        // >> 8). The maximum number of the fonts is 0xFF. Thus, technically the
        // maximum length of mFamilyVec is 0x10EE01 (0x10FF * 0xFF). However, in
        // practice, 16-bit integers are enough since most fonts supports only limited
        // range of code points.
        private class Range
        {
            public UInt16 start = new UInt16();
            public UInt16 end = new UInt16();
        }

        // Initialize the FontCollection.
        public void init(vector<FontFamily> typefaces)
        {
            std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
            mId = GlobalMembers.sNextId++;
            vector<uint> lastChar = new vector<uint>();
            int nTypefaces = typefaces.size();
#if VERBOSE_DEBUG
	  ALOGD("nTypefaces = %zd\n", nTypefaces);
#endif
            FontStyle defaultStyle = new FontStyle();
            for (int i = 0; i < nTypefaces; i++)
            {
                FontFamily* family = typefaces[i];
                if (family.getClosestMatch(defaultStyle).font == null)
                {
                    continue;
                }
                SparseBitSet coverage = family.getCoverage();
                mFamilies.push_back(family); // emplace_back would be better
                if (family.hasVSTable())
                {
                    mVSFamilyVec.push_back(family);
                }
                mMaxChar = Math.Max(mMaxChar, coverage.length());
                lastChar.push_back(coverage.nextSetBit(0));

                HashSet<AxisTag> supportedAxes = family.supportedAxes();
                mSupportedAxes.insert(supportedAxes.GetEnumerator(), supportedAxes.end());
            }
            nTypefaces = mFamilies.size();
            LOG_ALWAYS_FATAL_IF(nTypefaces == 0, "Font collection must have at least one valid typeface");
            LOG_ALWAYS_FATAL_IF(nTypefaces > 254, "Font collection may only have up to 254 font families.");
            int nPages = (mMaxChar + kPageMask) >> kLogCharsPerPage;
            // TODO: Use variation selector map for mRanges construction.
            // A font can have a glyph for a base code point and variation selector pair
            // but no glyph for the base code point without variation selector. The family
            // won't be listed in the range in this case.
            for (int i = 0; i < nPages; i++)
            {
                Range dummy = new Range();
                mRanges.push_back(dummy);
                Range range = mRanges.back();
#if VERBOSE_DEBUG
		ALOGD("i=%zd: range start = %zd\n", i, offset);
#endif
                range.start = mFamilyVec.size();
                for (int j = 0; j < nTypefaces; j++)
                {
                    if (lastChar[j] < (i + 1) << kLogCharsPerPage)
                    {
                        FontFamily* family = mFamilies[j];
                        mFamilyVec.push_back((byte)j);
                        uint nextChar = family.getCoverage().nextSetBit((i + 1) << kLogCharsPerPage);
#if VERBOSE_DEBUG
			ALOGD("nextChar = %d (j = %zd)\n", nextChar, j);
#endif
                        lastChar[j] = nextChar;
                    }
                }
                range.end = mFamilyVec.size();
            }
            // See the comment in Range for more details.
            LOG_ALWAYS_FATAL_IF(mFamilyVec.size() >= 0xFFFF, "Exceeded the maximum indexable cmap coverage.");
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const FontFamily*& getFamilyForChar(uint ch, uint vs, uint langListId, int variant) const;
        public FontFamily* getFamilyForChar(uint ch, uint vs, uint langListId, int variant)
        {
            if (ch >= mMaxChar)
            {
                // libtxt: check if the fallback font provider can match this character
                if (mFallbackFontProvider)
                {
                    FontFamily* fallback = mFallbackFontProvider.matchFallbackFont(ch, GetFontLocale(new uint(langListId)));
                    if (fallback != null)
                    {
                        return fallback;
                    }
                }
                return mFamilies[0];
            }

            Range range = mRanges[ch >> kLogCharsPerPage];

            if (vs != 0)
            {
                range = new Range(0, (UInt16)mFamilies.size());
            }

#if VERBOSE_DEBUG
	  ALOGD("querying range %zd:%zd\n", range.start, range.end);
#endif
            int bestFamilyIndex = -1;
            uint bestScore = kUnsupportedFontScore;
            for (int i = range.start; i < range.end; i++)
            {
                FontFamily* family = vs == 0 ? mFamilies[mFamilyVec[i]] : mFamilies[i];
                uint score = calcFamilyScore(ch, vs, variant, langListId, family);
                if (score == kFirstFontScore)
                {
                    // If the first font family supports the given character or variation
                    // sequence, always use it.
                    return family;
                }
                if (score > bestScore)
                {
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                    //ORIGINAL LINE: bestScore = score;
                    bestScore.CopyFrom(score);
                    bestFamilyIndex = i;
                }
            }
            if (bestFamilyIndex == -1)
            {
                // libtxt: check if the fallback font provider can match this character
                if (mFallbackFontProvider)
                {
                    FontFamily* fallback = mFallbackFontProvider.matchFallbackFont(ch, GetFontLocale(new uint(langListId)));
                    if (fallback != null)
                    {
                        return fallback;
                    }
                }

                UErrorCode errorCode = U_ZERO_ERROR;
                UNormalizer2 normalizer = unorm2_getNFDInstance(errorCode);
                if (U_SUCCESS(errorCode))
                {
                    UChar[] decomposed = Arrays.InitializeWithDefaultInstances<UChar>(4);
                    int len = unorm2_getRawDecomposition(normalizer, ch, decomposed, 4, errorCode);
                    if (U_SUCCESS(errorCode) && len > 0)
                    {
                        int off = 0;
                        U16_NEXT_UNSAFE(decomposed, off, ch);
                        return getFamilyForChar(ch, vs, langListId, variant);
                    }
                }
                return mFamilies[0];
            }
            return vs == 0 ? mFamilies[mFamilyVec[bestFamilyIndex]] : mFamilies[bestFamilyIndex];
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint calcFamilyScore(uint ch, uint vs, int variant, uint langListId, const FontFamily*& fontFamily) const;
        public uint calcFamilyScore(uint ch, uint vs, int variant, uint langListId, FontFamily fontFamily)
        {
            uint coverageScore = calcCoverageScore(ch, vs, fontFamily);
            if (coverageScore == kFirstFontScore || coverageScore == kUnsupportedFontScore)
            {
                // No need to calculate other scores.
                return coverageScore;
            }

            uint languageScore = calcLanguageMatchingScore(langListId, fontFamily);
            uint variantScore = calcVariantMatchingScore(variant, fontFamily);

            // Subscores are encoded into 31 bits representation to meet the subscore
            // priority. The highest 2 bits are for coverage score, then following 28 bits
            // are for language score, then the last 1 bit is for variant score.
            return coverageScore << 29 | languageScore << 1 | variantScore;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint calcCoverageScore(uint ch, uint vs, const FontFamily*& fontFamily) const;
        public uint calcCoverageScore(uint ch, uint vs, FontFamily fontFamily)
        {
            bool hasVSGlyph = (vs != 0) && fontFamily.hasGlyph(ch, vs);
            if (!hasVSGlyph && !fontFamily.getCoverage().get(ch))
            {
                // The font doesn't support either variation sequence or even the base
                // character.
                return kUnsupportedFontScore;
            }

            if ((vs == 0 || hasVSGlyph) && mFamilies[0] == fontFamily)
            {
                // If the first font family supports the given character or variation
                // sequence, always use it.
                return kFirstFontScore;
            }

            if (vs == 0)
            {
                return 1;
            }

            if (hasVSGlyph)
            {
                return 3;
            }

            if (vs == EMOJI_STYLE_VS || vs == TEXT_STYLE_VS)
            {
                FontLanguages langs = FontLanguageListCache.getById(fontFamily.langId());
                bool hasEmojiFlag = false;
                for (int i = 0; i < langs.size(); ++i)
                {
                    if (langs[i].getEmojiStyle() == FontLanguage.EMSTYLE_EMOJI)
                    {
                        hasEmojiFlag = true;
                        break;
                    }
                }

                if (vs == EMOJI_STYLE_VS)
                {
                    return hasEmojiFlag ? 2 : 1;
                }
                else
                { // vs == TEXT_STYLE_VS
                    return hasEmojiFlag ? 1 : 2;
                }
            }
            return 1;
        }

        public uint calcLanguageMatchingScore(uint userLangListId, FontFamily fontFamily)
        {
            FontLanguages langList = FontLanguageListCache.getById(new uint(userLangListId));
            FontLanguages fontLanguages = FontLanguageListCache.getById(fontFamily.langId());

            int maxCompareNum = Math.Min(langList.size(), FONT_LANGUAGES_LIMIT);
            uint score = 0;
            for (int i = 0; i < maxCompareNum; ++i)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: score = score * 5u + langList[i].calcScoreFor(fontLanguages);
                score.CopyFrom(score * 5u + langList[i].calcScoreFor(fontLanguages));
            }
            return score;
        }

        public uint calcVariantMatchingScore(int variant, FontFamily fontFamily)
        {
            return (fontFamily.variant() == 0 || fontFamily.variant() == variant) ? 1 : 0;
        }

        // static for allocating unique id's
        private static uint sNextId = new uint();

        // unique id for this font collection (suitable for cache key)
        private uint mId = new uint();

        // Highest UTF-32 code point that can be mapped
        private uint mMaxChar = new uint();

        // This vector has pointers to the all font family instances in this
        // collection. This vector can't be empty.
        private List<FontFamily> mFamilies = new List<FontFamily>();

        // Following two vectors are pre-calculated tables for resolving coverage
        // faster. For example, to iterate over all fonts which support Unicode code
        // point U+XXYYZZ, iterate font families index from
        // mFamilyVec[mRanges[0xXXYY].start] to mFamilyVec[mRange[0xXXYY].end] instead
        // of whole mFamilies. This vector contains indices into mFamilies. This
        // vector can't be empty.
        private List<Range> mRanges = new List<Range>();
        private List<byte> mFamilyVec = new List<byte>();

        // This vector has pointers to the font family instances which have cmap 14
        // subtables.
        private List<FontFamily> mVSFamilyVec = new List<FontFamily>();

        // Set of supported axes in this collection.
        private HashSet<AxisTag> mSupportedAxes = new HashSet<AxisTag>();

        // libtxt extension: Fallback font provider.
        private std::unique_ptr<FallbackFontProvider> mFallbackFontProvider = new std::unique_ptr<FallbackFontProvider>();
    }

} // namespace minikin

