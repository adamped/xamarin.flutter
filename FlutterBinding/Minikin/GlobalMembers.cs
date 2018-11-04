using System;
using System.Collections.Generic;

namespace minikin
{
    public static class GlobalMembers
    {
        // These could perhaps be optimized to use __builtin_bswap16 and friends.
        internal static uint readU16(byte[] data, int offset)
        {
            return ((uint)data[offset]) << 8 | ((uint)data[offset + 1]);
        }

        internal static uint readU32(byte[] data, int offset)
        {
            return ((uint)data[offset]) << 24 | ((uint)data[offset + 1]) << 16 | ((uint)data[offset + 2]) << 8 | ((uint)data[offset + 3]);
        }

        internal static void addRange(vector<uint> coverage, uint start, uint end)
        {
#if VERBOSE_DEBUG
	  ALOGD("adding range %d-%d\n", start, end);
#endif
            if (coverage.empty() || coverage.back() < start)
            {
                coverage.push_back(start);
                coverage.push_back(end);
            }
            else
            {
                coverage.back() = end;
            }
        }

        // Get the coverage information out of a Format 4 subtable, storing it in the
        // coverage vector
        internal static bool getCoverageFormat4(vector<uint> coverage, byte data, int size)
        {
            const int kSegCountOffset = 6;
            const int kEndCountOffset = 14;
            const int kHeaderSize = 16;
            const int kSegmentSize = 8; // total size of array elements for one segment
            if (kEndCountOffset > size)
            {
                return false;
            }
            int segCount = readU16(data, kSegCountOffset) >> 1;
            if (kHeaderSize + segCount * kSegmentSize > size != null)
            {
                return false;
            }
            for (int i = 0; i < segCount; i++)
            {
                uint end = readU16(data, kEndCountOffset + 2 * i);
                uint start = readU16(data, kHeaderSize + 2 * (segCount + i));
                if (end < start)
                {
                    // invalid segment range: size must be positive
                    android_errorWriteLog(0x534e4554, "26413177");
                    return false;
                }
                uint rangeOffset = readU16(data, kHeaderSize + 2 * (3 * segCount + i));
                if (rangeOffset == 0)
                {
                    uint delta = readU16(data, kHeaderSize + 2 * (2 * segCount + i));
                    if (((end + delta) & 0xffff) > end - start)
                    {
                        addRange(coverage, new uint(start), end + 1);
                    }
                    else
                    {
                        for (uint j = start; j < end + 1; j++)
                        {
                            if (((j + delta) & 0xffff) != 0)
                            {
                                addRange(coverage, new uint(j), j + 1);
                            }
                        }
                    }
                }
                else
                {
                    for (uint j = start; j < end + 1; j++)
                    {
                        uint actualRangeOffset = kHeaderSize + 6 * segCount + rangeOffset + (i + j - start) * 2;
                        if (actualRangeOffset + 2 > size != null)
                        {
                            // invalid rangeOffset is considered a "warning" by OpenType Sanitizer
                            continue;
                        }
                        uint glyphId = readU16(data, new uint(actualRangeOffset));
                        if (glyphId != 0)
                        {
                            addRange(coverage, new uint(j), j + 1);
                        }
                    }
                }
            }
            return true;
        }

        // Get the coverage information out of a Format 12 subtable, storing it in the
        // coverage vector
        internal static bool getCoverageFormat12(vector<uint> coverage, byte data, int size)
        {
            const int kNGroupsOffset = 12;
            const int kFirstGroupOffset = 16;
            const int kGroupSize = 12;
            const int kStartCharCodeOffset = 0;
            const int kEndCharCodeOffset = 4;
            int kMaxNGroups = 0xfffffff0 / kGroupSize; // protection against overflow
                                                       // For all values < kMaxNGroups, kFirstGroupOffset + nGroups * kGroupSize fits
                                                       // in 32 bits.
            if (kFirstGroupOffset > size)
            {
                return false;
            }
            uint nGroups = readU32(data, kNGroupsOffset);
            if (nGroups >= kMaxNGroups != null || kFirstGroupOffset + nGroups * kGroupSize > size)
            {
                android_errorWriteLog(0x534e4554, "25645298");
                return false;
            }
            for (uint i = 0; i < nGroups; i++)
            {
                uint groupOffset = kFirstGroupOffset + i * kGroupSize;
                uint start = readU32(data, groupOffset + kStartCharCodeOffset);
                uint end = readU32(data, groupOffset + kEndCharCodeOffset);
                if (end < start)
                {
                    // invalid group range: size must be positive
                    android_errorWriteLog(0x534e4554, "26413177");
                    return false;
                }

                // No need to read outside of Unicode code point range.
                if (start > MAX_UNICODE_CODE_POINT)
                {
                    return true;
                }
                if (end > MAX_UNICODE_CODE_POINT)
                {
                    // file is inclusive, vector is exclusive
                    addRange(coverage, new uint(start), MAX_UNICODE_CODE_POINT + 1);
                    return true;
                }
                addRange(coverage, new uint(start), end + 1); // file is inclusive, vector is exclusive
            }
            return true;
        }

        // Lower value has higher priority. 0 for the highest priority table.
        // kLowestPriority for unsupported tables.
        // This order comes from HarfBuzz's hb-ot-font.cc and needs to be kept in sync
        // with it.
        public const byte kLowestPriority = 255;
        public static byte getTablePriority(UInt16 platformId, UInt16 encodingId)
        {
            if (platformId == 3 && encodingId == 10)
            {
                return 0;
            }
            if (platformId == 0 && encodingId == 6)
            {
                return 1;
            }
            if (platformId == 0 && encodingId == 4)
            {
                return 2;
            }
            if (platformId == 3 && encodingId == 1)
            {
                return 3;
            }
            if (platformId == 0 && encodingId == 3)
            {
                return 4;
            }
            if (platformId == 0 && encodingId == 2)
            {
                return 5;
            }
            if (platformId == 0 && encodingId == 1)
            {
                return 6;
            }
            if (platformId == 0 && encodingId == 0)
            {
                return 7;
            }
            // Tables other than above are not supported.
            return kLowestPriority;
        }

        public static bool isEmoji(uint c)
        {
            return u_hasBinaryProperty(c, UCHAR_EMOJI);
        }

        public static bool isEmojiModifier(uint c)
        {
            return u_hasBinaryProperty(c, UCHAR_EMOJI_MODIFIER);
        }

        public static bool isEmojiBase(uint c)
        {
            // These two characters were removed from Emoji_Modifier_Base in Emoji 4.0,
            // but we need to keep them as emoji modifier bases since there are fonts and
            // user-generated text out there that treats these as potential emoji bases.
            if (c == 0x1F91D || c == 0x1F93C)
            {
                return true;
            }
            return u_hasBinaryProperty(c, UCHAR_EMOJI_MODIFIER_BASE);
        }

        public static UCharDirection emojiBidiOverride(object UnnamedParameter, UChar32 c)
        {
            return u_charDirection(c);
        }
        internal static T max<T>(T a, T b)
        {
            return a > b != null ? a : b;
        }

        public static readonly uint EMOJI_STYLE_VS = 0xFE0F;
        public static readonly uint TEXT_STYLE_VS = 0xFE0E;

        public static uint FontCollection.sNextId = 0;

        // libtxt: return a locale string for a language list ID
        public static string GetFontLocale(uint langListId)
        {
            FontLanguages langs = FontLanguageListCache.getById(new uint(langListId));
            return langs.size() != null ? langs[0].getString() : "";
        }

        // Special scores for the font fallback.
        public static readonly uint kUnsupportedFontScore = 0;
        public static readonly uint kFirstFontScore = UINT32_MAX;

        public static readonly uint NBSP = 0x00A0;
        public static readonly uint SOFT_HYPHEN = 0x00AD;
        public static readonly uint ZWJ = 0x200C;
        public static readonly uint ZWNJ = 0x200D;
        public static readonly uint HYPHEN = 0x2010;
        public static readonly uint NB_HYPHEN = 0x2011;
        public static readonly uint NNBSP = 0x202F;
        public static readonly uint FEMALE_SIGN = 0x2640;
        public static readonly uint MALE_SIGN = 0x2642;
        public static readonly uint STAFF_OF_AESCULAPIUS = 0x2695;

        // Characters where we want to continue using existing font run instead of
        // recomputing the best match in the fallback list.
        internal uint[] stickyWhitelist = { '!', ',', '-', '.', ':', ';', '?', NBSP, ZWJ, ZWNJ, HYPHEN, NB_HYPHEN, NNBSP, FEMALE_SIGN, MALE_SIGN, STAFF_OF_AESCULAPIUS };

        internal static bool isStickyWhitelisted(uint c)
        {
            //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
            //ORIGINAL LINE: for (int i = 0; i < sizeof(stickyWhitelist) / sizeof(stickyWhitelist[0]); i++)
            for (int i = 0; i < stickyWhitelist.Length; i++)
            {
                if (stickyWhitelist[i] == c)
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool isVariationSelector(uint c)
        {
            return (0xFE00 <= c != null && c <= 0xFE0F) || (0xE0100 <= c != null && c <= 0xE01EF);
        }

        // Compute a matching metric between two styles - 0 is an exact match
        internal static int computeMatch(FontStyle style1, FontStyle style2)
        {
            if (style1 == style2)
            {
                return 0;
            }
            int score = Math.Abs(style1.getWeight() - style2.getWeight());
            if (style1.getItalic() != style2.getItalic())
            {
                score += 2;
            }
            return score;
        }

        internal static FontFakery computeFakery(FontStyle wanted, FontStyle actual)
        {
            // If desired weight is semibold or darker, and 2 or more grades
            // higher than actual (for example, medium 500 -> bold 700), then
            // select fake bold.
            int wantedWeight = wanted.getWeight();
            bool isFakeBold = wantedWeight >= 6 && (wantedWeight - actual.getWeight()) >= 2;
            bool isFakeItalic = wanted.getItalic() && !actual.getItalic();
            return FontFakery(isFakeBold, isFakeItalic);
        }

        // Due to the limits in font fallback score calculation, we can't use anything
        // more than 12 languages.
        public static readonly int FONT_LANGUAGES_LIMIT = 12;

        // The language or region code is encoded to 15 bits.
        public static readonly UInt16 INVALID_CODE = 0x7fff;

        //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
        //class FontLanguages;

        //C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
        //ORIGINAL LINE: #define SCRIPT_TAG(c1, c2, c3, c4) (((uint)(c1)) << 24 | ((uint)(c2)) << 16 | ((uint)(c3)) << 8 | ((uint)(c4)))

        // Check if a language code supports emoji according to its subtag
        internal static bool isEmojiSubtag(string buf, int bufLen, string subtag, int subtagLen)
        {
            if (bufLen < subtagLen)
            {
                return false;
            }
            if (string.Compare(buf, 0, subtag, 0, subtagLen) != 0)
            {
                return false; // no match between two strings
            }
            return (bufLen == subtagLen || buf[subtagLen] == '\0' || buf[subtagLen] == '-' || buf[subtagLen] == '_');
        }

        // Pack the three letter code into 15 bits and stored to 16 bit integer. The
        // highest bit is 0. For the region code, the letters must be all digits in
        // three letter case, so the number of possible values are 10. For the language
        // code, the letters must be all small alphabets, so the number of possible
        // values are 26. Thus, 5 bits are sufficient for each case and we can pack the
        // three letter language code or region code to 15 bits.
        //
        // In case of two letter code, use fullbit(0x1f) for the first letter instead.
        internal static UInt16 packLanguageOrRegion(string c, int length, byte twoLetterBase, byte threeLetterBase)
        {
            if (length == 2)
            {
                return 0x7c00u | (UInt16)(c[0] - twoLetterBase) << 5 | (UInt16)(c[1] - twoLetterBase);
            }
            else
            {
                return ((UInt16)(c[0] - threeLetterBase) << 10) | (UInt16)(c[1] - threeLetterBase) << 5 | (UInt16)(c[2] - threeLetterBase);
            }
        }

        internal static int unpackLanguageOrRegion(UInt16 @in, ref string @out, byte twoLetterBase, byte threeLetterBase)
        {
            byte first = (@in >> 10) &0x1f;
            byte second = (@in >> 5) &0x1f;
            byte third = @in &0x1f;

            if (first == 0x1f)
            {
                @out[0] = second + twoLetterBase;
                @out[1] = third + twoLetterBase;
                return 2;
            }
            else
            {
                @out[0] = first + threeLetterBase;
                @out[1] = second + threeLetterBase;
                @out[2] = third + threeLetterBase;
                return 3;
            }
        }

        // Find the next '-' or '_' index from startOffset position. If not found,
        // returns bufferLength.
        internal static int nextDelimiterIndex(string buffer, int bufferLength, int startOffset)
        {
            for (int i = startOffset; i < bufferLength; ++i)
            {
                if (buffer[i] == '-' || buffer[i] == '_')
                {
                    return i;
                }
            }
            return bufferLength;
        }

        internal static bool isLowercase(char c)
        {
            return 'a' <= c && c <= 'z';
        }

        internal static bool isUppercase(char c)
        {
            return 'A' <= c && c <= 'Z';
        }

        internal static bool isDigit(char c)
        {
            return '0' <= c && c <= '9';
        }

        // Returns true if the buffer is valid for language code.
        internal static bool isValidLanguageCode(string buffer, int length)
        {
            if (length != 2 && length != 3)
            {
                return false;
            }
            if (!isLowercase(buffer[0]))
            {
                return false;
            }
            if (!isLowercase(buffer[1]))
            {
                return false;
            }
            if (length == 3 && !isLowercase(buffer[2]))
            {
                return false;
            }
            return true;
        }

        // Returns true if buffer is valid for script code. The length of buffer must
        // be 4.
        internal static bool isValidScriptCode(string buffer)
        {
            return isUppercase(buffer[0]) && isLowercase(buffer[1]) && isLowercase(buffer[2]) && isLowercase(buffer[3]);
        }

        // Returns true if the buffer is valid for region code.
        internal static bool isValidRegionCode(string buffer, int length)
        {
            return (length == 2 && isUppercase(buffer[0]) && isUppercase(buffer[1])) || (length == 3 && isDigit(buffer[0]) && isDigit(buffer[1]) && isDigit(buffer[2]));
        }


        // Returns the text length of output.
        internal static int toLanguageTag(ref string output, int outSize, string locale)
        {
            output[0] = '\0';
            if (string.IsNullOrEmpty(locale))
            {
                return 0;
            }

            int outLength = 0;
            UErrorCode uErr = U_ZERO_ERROR;
            outLength = uloc_canonicalize(locale, output, outSize, uErr);
            if (U_FAILURE(uErr))
            {
                // unable to build a proper language identifier
                ALOGD("uloc_canonicalize(\"%s\") failed: %s", locale, u_errorName(uErr));
                output[0] = '\0';
                return 0;
            }

            // Preserve "und" and "und-****" since uloc_addLikelySubtags changes "und" to
            // "en-Latn-US".
            if (string.Compare(output, 0, "und", 0, 3) == 0 && (outLength == 3 || (outLength == 8 && output[3] == '_')))
            {
                return outLength;
            }

            string likelyChars = new string(new char[ULOC_FULLNAME_CAPACITY]);
            uErr = U_ZERO_ERROR;
            uloc_addLikelySubtags(output, likelyChars, ULOC_FULLNAME_CAPACITY, uErr);
            if (U_FAILURE(uErr))
            {
                // unable to build a proper language identifier
                ALOGD("uloc_addLikelySubtags(\"%s\") failed: %s", output, u_errorName(uErr));
                output[0] = '\0';
                return 0;
            }

            uErr = U_ZERO_ERROR;
            outLength = uloc_toLanguageTag(likelyChars, output, outSize, 0, uErr);
            if (U_FAILURE(uErr))
            {
                // unable to build a proper language identifier
                ALOGD("uloc_toLanguageTag(\"%s\") failed: %s", likelyChars, u_errorName(uErr));
                output[0] = '\0';
                return 0;
            }
#if VERBOSE_DEBUG
	  ALOGD("ICU normalized '%s' to '%s'", locale, output);
#endif
            return outLength;
        }

        internal static List<FontLanguage> parseLanguageList(string input)
        {
            List<FontLanguage> result = new List<FontLanguage>();
            int currentIdx = 0;
            int commaLoc = 0;
            string langTag = new string(new char[ULOC_FULLNAME_CAPACITY]);
            HashSet<UInt64> seen = new HashSet<UInt64>();
            string locale = new string(input.Length, 0);

            while ((commaLoc = input.IndexOfAny((Convert.ToString(',')).ToCharArray(), currentIdx)) != -1)
            {
                locale.assign(input, currentIdx, commaLoc - currentIdx);
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: currentIdx = commaLoc + 1;
                currentIdx.CopyFrom(commaLoc + 1);
                int length = toLanguageTag(ref langTag, ULOC_FULLNAME_CAPACITY, locale);
                FontLanguage lang = new FontLanguage(langTag, length);
                UInt64 identifier = lang.getIdentifier();
                if (!lang.isUnsupported() && seen.count(identifier) == 0)
                {
                    result.Add(lang);
                    if (result.Count == FONT_LANGUAGES_LIMIT)
                    {
                        break;
                    }
                    seen.Add(identifier);
                }
            }
            if (result.Count < FONT_LANGUAGES_LIMIT)
            {
                locale.assign(input, currentIdx, input.Length - currentIdx);
                int length = toLanguageTag(ref langTag, ULOC_FULLNAME_CAPACITY, locale);
                FontLanguage lang = new FontLanguage(langTag, length);
                UInt64 identifier = lang.getIdentifier();
                if (!lang.isUnsupported() && seen.count(identifier) == 0)
                {
                    result.Add(lang);
                }
            }
            return result;
        }

        public static bool analyzeStyle(byte os2_data, int os2_size, ref int weight, ref bool italic)
        {
            const int kUsWeightClassOffset = 4;
            const int kFsSelectionOffset = 62;
            UInt16 kItalicFlag = (1 << 0);
            if (os2_size < kFsSelectionOffset + 2)
            {
                return false;
            }
            UInt16 weightClass = readU16(os2_data, kUsWeightClassOffset);
            weight = weightClass / 100;
            UInt16 fsSelection = readU16(os2_data, kFsSelectionOffset);
            italic = (fsSelection & kItalicFlag) != 0;
            return true;
        }
        public static void analyzeAxes(byte fvar_data, int fvar_size, HashSet<uint> axes)
        {
            const int kMajorVersionOffset = 0;
            const int kMinorVersionOffset = 2;
            const int kOffsetToAxesArrayOffset = 4;
            const int kAxisCountOffset = 8;
            const int kAxisSizeOffset = 10;

            axes.Clear();

            if (fvar_size < kAxisSizeOffset + 2)
            {
                return;
            }
            UInt16 majorVersion = readU16(fvar_data, kMajorVersionOffset);
            UInt16 minorVersion = readU16(fvar_data, kMinorVersionOffset);
            uint axisOffset = readU16(fvar_data, kOffsetToAxesArrayOffset);
            uint axisCount = readU16(fvar_data, kAxisCountOffset);
            uint axisSize = readU16(fvar_data, kAxisSizeOffset);

            if (majorVersion != 1 || minorVersion != 0 || axisOffset != 0x10 || axisSize != 0x14)
            {
                return; // Unsupported version.
            }
            if (fvar_size < axisOffset + axisOffset * axisCount)
            {
                return; // Invalid table size.
            }
            for (uint i = 0; i < axisCount; ++i)
            {
                int axisRecordOffset = axisOffset + i * axisSize;
                uint tag = readU32(fvar_data, axisRecordOffset);
                axes.Add(tag);
            }
        }

        internal static UInt16 readU16(byte[] data, int offset)
        {
            return data[offset] << 8 | data[offset + 1];
        }

        internal static uint readU32(byte[] data, int offset)
        {
            return ((uint)data[offset]) << 24 | ((uint)data[offset + 1]) << 16 | ((uint)data[offset + 2]) << 8 | ((uint)data[offset + 3]);
        }

        public static int tailoredGraphemeClusterBreak(uint c)
        {
            // Characters defined as Control that we want to treat them as Extend.
            // These are curated manually.
            if (c == 0x00AD || c == 0x061C || c == 0x180E || c == 0x200B || c == 0x200E || c == 0x200F || (0x202A <= c != null && c <= 0x202E) || ((c | 0xF) == 0x206F) || c == 0xFEFF || ((c | 0x7F) == 0xE007F)) // recently undeprecated tag characters in Plane 14
            {
                return U_GCB_EXTEND;
            }
            // THAI CHARACTER SARA AM is treated as a normal letter by most other
            // implementations: they allow a grapheme break before it.
            else if (c == 0x0E33)
            {
                return U_GCB_OTHER;
            }
            else
            {
                return u_getIntPropertyValue(c, UCHAR_GRAPHEME_CLUSTER_BREAK);
            }
        }

        // Returns true for all characters whose IndicSyllabicCategory is Pure_Killer.
        // From http://www.unicode.org/Public/9.0.0/ucd/IndicSyllabicCategory.txt
        public static bool isPureKiller(uint c)
        {
            return (c == 0x0E3A || c == 0x0E4E || c == 0x0F84 || c == 0x103A || c == 0x1714 || c == 0x1734 || c == 0x17D1 || c == 0x1BAA || c == 0x1BF2 || c == 0x1BF3 || c == 0xA806 || c == 0xA953 || c == 0xABED || c == 0x11134 || c == 0x112EA || c == 0x1172B);
        }

        public static void purgeHbFontCacheLocked()
        {
            assertMinikinLocked();
            getFontCacheLocked.functorMethod().clear();
        }
        public static void purgeHbFontLocked(MinikinFont minikinFont)
        {
            assertMinikinLocked();
            int fontId = minikinFont.GetUniqueId();
            getFontCacheLocked.functorMethod().remove(fontId);
        }

        // Returns a new reference to a HarfBuzzSharp.Font object, caller is
        // responsible for calling hb_font_destroy() on it.
        //C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
        private static HarfBuzzSharp.Font getHbFontLocked_nullFaceFont = null;
        public static HarfBuzzSharp.Font getHbFontLocked(MinikinFont minikinFont)
        {
            assertMinikinLocked();
            // TODO: get rid of nullFaceFont
            //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //  static HarfBuzzSharp.Font* nullFaceFont = null;
            if (minikinFont == null)
            {
                if (getHbFontLocked_nullFaceFont == null)
                {
                    getHbFontLocked_nullFaceFont = hb_font_create(null);
                }
                return hb_font_reference(getHbFontLocked_nullFaceFont);
            }

            HbFontCache fontCache = getFontCacheLocked.functorMethod();
            int fontId = minikinFont.GetUniqueId();
            HarfBuzzSharp.Font font = fontCache.get(fontId);
            if (font != null)
            {
                return hb_font_reference(font);
            }

            HarfBuzzSharp.Face face = minikinFont.CreateHarfBuzzFace();

            HarfBuzzSharp.Font parent_font = hb_font_create(face);
            hb_ot_font_set_funcs(parent_font);

            uint upem = hb_face_get_upem(face);
            hb_font_set_scale(parent_font, upem, upem);

            font = hb_font_create_sub_font(parent_font);
            List<hb_variation_t> variations = new List<hb_variation_t>();
            foreach (FontVariation variation in minikinFont.GetAxes())
            {
                variations.Add({ variation.axisTag, variation.value});
            }
            hb_font_set_variations(font, variations.data(), variations.Count);
            hb_font_destroy(parent_font);
            hb_face_destroy(face);
            fontCache.put(fontId, font);
            return hb_font_reference(font);
        }
        //C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
        private static HbFontCache getFontCacheLocked_cache = null;

        public static HbFontCache getFontCacheLocked()
        {
            assertMinikinLocked();
            //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //  static HbFontCache* cache = null;
            if (getFontCacheLocked_cache == null)
            {
                getFontCacheLocked_cache = new HbFontCache();
            }
            return getFontCacheLocked_cache.functorMethod;
        }

        internal const UInt16 CHAR_HYPHEN_MINUS = 0x002D;
        internal const UInt16 CHAR_SOFT_HYPHEN = 0x00AD;
        internal const UInt16 CHAR_MIDDLE_DOT = 0x00B7;
        internal const UInt16 CHAR_HYPHEN = 0x2010;

        internal static uint[] HYPHEN_STR = { 0x2010, 0 };
        internal static uint[] ARMENIAN_HYPHEN_STR = { 0x058A, 0 };
        internal static uint[] MAQAF_STR = { 0x05BE, 0 };
        internal static uint[] UCAS_HYPHEN_STR = { 0x1400, 0 };
        internal static uint[] ZWJ_STR = { 0x200D, 0 };
        internal static uint[] ZWJ_AND_HYPHEN_STR = { 0x200D, 0x2010, 0 };

        internal static UScriptCode getScript(uint codePoint)
        {
            UErrorCode errorCode = U_ZERO_ERROR;
            UScriptCode script = uscript_getScript((UChar32)codePoint, errorCode);
            if (U_SUCCESS(errorCode))
            {
                return script;
            }
            else
            {
                return USCRIPT_INVALID_CODE;
            }
        }

        internal static HyphenationType hyphenationTypeBasedOnScript(uint codePoint)
        {
            // Note: It's not clear what the best hyphen for Hebrew is. While maqaf is the
            // "correct" hyphen for Hebrew, modern practice may have shifted towards
            // Western hyphens. We use normal hyphens for now to be safe.
            // BREAK_AND_INSERT_MAQAF is already implemented, so if we want to switch to
            // maqaf for Hebrew, we can simply add a condition here.
            UScriptCode script = getScript(new uint(codePoint));
            if (script == USCRIPT_KANNADA || script == USCRIPT_MALAYALAM || script == USCRIPT_TAMIL || script == USCRIPT_TELUGU)
            {
                // Grantha is not included, since we don't support non-BMP hyphenation yet.
                return HyphenationType.BREAK_AND_DONT_INSERT_HYPHEN;
            }
            else if (script == USCRIPT_ARMENIAN)
            {
                return HyphenationType.BREAK_AND_INSERT_ARMENIAN_HYPHEN;
            }
            else if (script == USCRIPT_CANADIAN_ABORIGINAL)
            {
                return HyphenationType.BREAK_AND_INSERT_UCAS_HYPHEN;
            }
            else
            {
                return HyphenationType.BREAK_AND_INSERT_HYPHEN;
            }
        }

        internal static int getJoiningType(UChar32 codepoint)
        {
            return u_getIntPropertyValue(codepoint, UCHAR_JOINING_TYPE);
        }

        // Assumption for caller: location must be >= 2 and word[location] ==
        // CHAR_SOFT_HYPHEN. This function decides if the letters before and after the
        // hyphen should appear as joining.
        internal static HyphenationType getHyphTypeForArabic(UInt16[] word, int len, int location)
        {
            IntPtr i = location;
            int type = U_JT_NON_JOINING;
            while ((int)i < len && (type = getJoiningType(word[i])) == U_JT_TRANSPARENT)
            {
                i++;
            }
            if (type == U_JT_DUAL_JOINING || type == U_JT_RIGHT_JOINING || type == U_JT_JOIN_CAUSING)
            {
                // The next character is of the type that may join the last character. See
                // if the last character is also of the right type.
                i = location - 2; // Skip the soft hyphen
                type = U_JT_NON_JOINING;
                while (i >= 0 && (type = getJoiningType(word[i])) == U_JT_TRANSPARENT)
                {
                    i--;
                }
                if (type == U_JT_DUAL_JOINING || type == U_JT_LEFT_JOINING || type == U_JT_JOIN_CAUSING)
                {
                    return HyphenationType.BREAK_AND_INSERT_HYPHEN_AND_ZWJ;
                }
            }
            return HyphenationType.BREAK_AND_INSERT_HYPHEN;
        }

        public static readonly int kDirection_Mask = 0x1;

        internal static uint disabledDecomposeCompatibility(hb_unicode_funcs_t UnnamedParameter, hb_codepoint_t UnnamedParameter2, hb_codepoint_t UnnamedParameter3, object UnnamedParameter4)
        {
            return 0;
        }

        public static android.hash_t hash_type(LayoutCacheKey key)
        {
            return key.hash();
        }

        internal static HarfBuzzSharp.Position harfbuzzGetGlyphHorizontalAdvance(HarfBuzzSharp.Font UnnamedParameter, object fontData, hb_codepoint_t glyph, object UnnamedParameter2)
        {
            //C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
            MinikinPaint paint = reinterpret_cast<MinikinPaint>(fontData);
            float advance = paint.font.GetHorizontalAdvance(glyph, paint);
            return 256 * advance + 0.5;
        }

        internal static hb_bool_t harfbuzzGetGlyphHorizontalOrigin(HarfBuzzSharp.Font UnnamedParameter, object UnnamedParameter2, hb_codepoint_t UnnamedParameter3, HarfBuzzSharp.Position UnnamedParameter4, HarfBuzzSharp.Position UnnamedParameter5, object UnnamedParameter6)
        {
            // Just return true, following the way that Harfbuzz-FreeType
            // implementation does.
            return true;
        }
        //C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
        private static hb_font_funcs_t getHbFontFuncs_hbFuncs = null;
        //C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
        private static hb_font_funcs_t getHbFontFuncs_hbFuncsForColorBitmap = null;

        public static hb_font_funcs_t getHbFontFuncs(bool forColorBitmapFont)
        {
            assertMinikinLocked();

            //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //  static hb_font_funcs_t* hbFuncs = null;
            //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //  static hb_font_funcs_t* hbFuncsForColorBitmap = null;

            hb_font_funcs_t[] funcs = forColorBitmapFont ? getHbFontFuncs_hbFuncs : getHbFontFuncs_hbFuncsForColorBitmap;
            if (funcs[0] == null)
            {
                funcs[0] = hb_font_funcs_create();
                if (forColorBitmapFont)
                {
                    // Don't override the h_advance function since we use HarfBuzz's
                    // implementation for emoji for performance reasons. Note that it is
                    // technically possible for a TrueType font to have outline and embedded
                    // bitmap at the same time. We ignore modified advances of hinted outline
                    // glyphs in that case.
                }
                else
                {
                    // Override the h_advance function since we can't use HarfBuzz's
                    // implemenation. It may return the wrong value if the font uses hinting
                    // aggressively.
                    hb_font_funcs_set_glyph_h_advance_func(funcs[0], harfbuzzGetGlyphHorizontalAdvance, 0, 0);
                }
                hb_font_funcs_set_glyph_h_origin_func(funcs[0], harfbuzzGetGlyphHorizontalOrigin, 0, 0);
                hb_font_funcs_make_immutablefuncs;
            }
            return funcs[0];
        }

        internal static bool isColorBitmapFont(HarfBuzzSharp.Font font)
        {
            HarfBuzzSharp.Face face = hb_font_get_face(font);
            HbBlob cbdt = new HbBlob(hb_face_reference_table(face, HB_TAG('C', 'B', 'D', 'T')));
            return cbdt.size() > 0;
        }

        internal static float HBFixedToFloat(HarfBuzzSharp.Position v)
        {
            return scalbnf(v, -8);
        }

        internal static HarfBuzzSharp.Position HBFloatToFixed(float v)
        {
            return scalbnf(v, +8);
        }
        //C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
        private static hb_unicode_funcs_t codePointToScript_u = null;

        internal static hb_script_t codePointToScript(hb_codepoint_t codepoint)
        {
            //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
            //  static hb_unicode_funcs_t* u = 0;
            if (codePointToScript_u == null)
            {
                codePointToScript_u = LayoutEngine.getInstance().unicodeFunctions;
            }
            return hb_unicode_script(codePointToScript_u, codepoint);
        }

        internal static hb_codepoint_t decodeUtf16(UInt16[] chars, int len, ref uint iter)
        {
            UInt16 v = chars[iter++];
            // test whether v in (0xd800..0xdfff), lead or trail surrogate
            if ((v & 0xf800) == 0xd800)
            {
                // test whether v in (0xd800..0xdbff), lead surrogate
                if (int(iter) < len && (v & 0xfc00) == 0xd800)
                {
                    UInt16 v2 = chars[iter++];
                    // test whether v2 in (0xdc00..0xdfff), trail surrogate
                    if ((v2 & 0xfc00) == 0xdc00)
                    {
                        // (0xd800 0xdc00) in utf-16 maps to 0x10000 in ucs-32
                        hb_codepoint_t delta = (0xd800 << 10) + 0xdc00 - 0x10000;
                        return (((hb_codepoint_t)v) << 10) + v2 - delta;
                    }
                    iter -= 1;
                    return 0xFFFDu;
                }
                else
                {
                    return 0xFFFDu;
                }
            }
            else
            {
                return v;
            }
        }

        internal static hb_script_t getScriptRun(UInt16 chars, int len, ref uint iter)
        {
            if (iter == len)
            {
                return HB_SCRIPT_UNKNOWN;
            }
            uint cp = decodeUtf16(chars, len, ref iter);
            hb_script_t current_script = codePointToScript(new uint(cp));
            for (; ; )
            {
                if (iter == len)
                {
                    break;
                }
                uint prev_iter = iter;
                cp = decodeUtf16(chars, len, ref iter);
                hb_script_t script = codePointToScript(new uint(cp));
                if (script != current_script)
                {
                    if (current_script == HB_SCRIPT_INHERITED || current_script == HB_SCRIPT_COMMON)
                    {
                        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                        //ORIGINAL LINE: current_script = script;
                        current_script.CopyFrom(script);
                    }
                    else if (script == HB_SCRIPT_INHERITED || script == HB_SCRIPT_COMMON)
                    {
                        continue;
                    }
                    else
                    {
                        iter = prev_iter;
                        break;
                    }
                }
            }
            if (current_script == HB_SCRIPT_INHERITED)
            {
                current_script = HB_SCRIPT_COMMON;
            }

            return current_script;
        }

        /**
         * Disable certain scripts (mostly those with cursive connection) from having
         * letterspacing applied. See https://github.com/behdad/harfbuzz/issues/64 for
         * more details.
         */
        internal static bool isScriptOkForLetterspacing(hb_script_t script)
        {
            return !(script == HB_SCRIPT_ARABIC || script == HB_SCRIPT_NKO || script == HB_SCRIPT_PSALTER_PAHLAVI || script == HB_SCRIPT_MANDAIC || script == HB_SCRIPT_MONGOLIAN || script == HB_SCRIPT_PHAGS_PA || script == HB_SCRIPT_DEVANAGARI || script == HB_SCRIPT_BENGALI || script == HB_SCRIPT_GURMUKHI || script == HB_SCRIPT_MODI || script == HB_SCRIPT_SHARADA || script == HB_SCRIPT_SYLOTI_NAGRI || script == HB_SCRIPT_TIRHUTA || script == HB_SCRIPT_OGHAM);
        }
        //C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
        private static hb_feature_t addFeatures_feature = new hb_feature_t();

        internal static void addFeatures(string str, vector<hb_feature_t> features)
        {
            if (str.Length == 0)
            {
                return;
            }

            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
            //ORIGINAL LINE: const char* start = str.c_str();
            char start = str;
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
            //ORIGINAL LINE: const char* end = start + str.size();
            char end = start + str.Length;

            while (start < end)
            {
                //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
                //	static hb_feature_t feature;
                //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
                //ORIGINAL LINE: const char* p = strchr(start, ',');
                char p = StringFunctions.StrChr(start, ',');
                if (p == null)
                {
                    p = end;
                }
                /* We do not allow setting features on ranges.  As such, reject any
                 * setting that has non-universal range. */
                if (hb_feature_from_string(start, p - start, addFeatures_feature) && addFeatures_feature.start == 0 && addFeatures_feature.end == (uint)-1)
                {
                    features.push_back(addFeatures_feature);
                }
                start = p + 1;
            }
        }

        internal const hb_codepoint_t CHAR_HYPHEN = 0x2010; // HYPHEN

        internal static hb_codepoint_t determineHyphenChar(hb_codepoint_t preferredHyphen, HarfBuzzSharp.Font font)
        {
            hb_codepoint_t glyph = new hb_codepoint_t();
            if (preferredHyphen == 0x058A || preferredHyphen == 0x05BE || preferredHyphen == 0x1400)
            {
                if (hb_font_get_nominal_glyph(font, preferredHyphen, glyph))
                {
                    return preferredHyphen;
                }
                else
                {
                    // The original hyphen requested was not supported. Let's try and see if
                    // the Unicode hyphen is supported.
                    preferredHyphen = CHAR_HYPHEN;
                }
            }
            if (preferredHyphen == CHAR_HYPHEN)
            { // HYPHEN
              // Fallback to ASCII HYPHEN-MINUS if the font didn't have a glyph for the
              // preferred hyphen. Note that we intentionally don't do anything special if
              // the font doesn't have a HYPHEN-MINUS either, so a tofu could be shown,
              // hinting towards something missing.
                if (!hb_font_get_nominal_glyph(font, preferredHyphen, glyph))
                {
                    return 0x002D; // HYPHEN-MINUS
                }
            }
            return preferredHyphen;
        }

        internal static void addHyphenToHbBuffer(hb_buffer_t buffer, HarfBuzzSharp.Font font, uint hyphen, uint cluster)
        {
            //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged:
            uint* hyphenStr = HyphenEdit.getHyphenString(hyphen);
            while (*hyphenStr != 0)
            {
                hb_codepoint_t hyphenChar = determineHyphenChar(*hyphenStr, font);
                hb_buffer_add(buffer, hyphenChar, cluster);
                hyphenStr++;
            }
        }

        // Returns the cluster value assigned to the first codepoint added to the
        // buffer, which can be used to translate cluster values returned by HarfBuzz to
        // input indices.
        internal static uint addToHbBuffer(hb_buffer_t buffer, UInt16 buf, int start, int count, int bufSize, uint scriptRunStart, uint scriptRunEnd, HyphenEdit hyphenEdit, HarfBuzzSharp.Font hbFont)
        {
            // Only hyphenate the very first script run for starting hyphens.
            uint startHyphen = (scriptRunStart == 0) ? hyphenEdit.getStart() : HyphenEdit.NO_EDIT;
            // Only hyphenate the very last script run for ending hyphens.
            uint endHyphen = ((int)scriptRunEnd == count) != null ? hyphenEdit.getEnd() : HyphenEdit.NO_EDIT;

            // In the following code, we drop the pre-context and/or post-context if there
            // is a hyphen edit at that end. This is not absolutely necessary, since
            // HarfBuzz uses contexts only for joining scripts at the moment, e.g. to
            // determine if the first or last letter of a text range to shape should take
            // a joining form based on an adjacent letter or joiner (that comes from the
            // context).
            //
            // TODO: Revisit this for:
            // 1. Desperate breaks for joining scripts like Arabic (where it may be better
            // to keep
            //    the context);
            // 2. Special features like start-of-word font features (not implemented in
            // HarfBuzz
            //    yet).

            // We don't have any start-of-line replacement edit yet, so we don't need to
            // check for those.
            if (HyphenEdit.isInsertion(startHyphen))
            {
                // A cluster value of zero guarantees that the inserted hyphen will be in
                // the same cluster with the next codepoint, since there is no pre-context.
                addHyphenToHbBuffer(buffer, hbFont, new uint(startHyphen), 0);
            }

            UInt16 hbText;
            int hbTextLength;
            uint hbItemOffset;
            uint hbItemLength = scriptRunEnd - scriptRunStart; // This is >= 1.

            bool hasEndInsertion = HyphenEdit.isInsertion(endHyphen);
            bool hasEndReplacement = HyphenEdit.isReplacement(endHyphen);
            if (hasEndReplacement)
            {
                // Skip the last code unit while copying the buffer for HarfBuzz if it's a
                // replacement. We don't need to worry about non-BMP characters yet since
                // replacements are only done for code units at the moment.
                hbItemLength -= 1;
            }

            if (startHyphen == HyphenEdit.NO_EDIT)
            {
                // No edit at the beginning. Use the whole pre-context.
                hbText = buf;
                hbItemOffset = start + scriptRunStart;
            }
            else
            {
                // There's an edit at the beginning. Drop the pre-context and start the
                // buffer at where we want to start shaping.
                hbText = buf + start + scriptRunStart;
                hbItemOffset = 0;
            }

            if (endHyphen == HyphenEdit.NO_EDIT)
            {
                // No edit at the end, use the whole post-context.
                hbTextLength = (buf + bufSize) - hbText;
            }
            else
            {
                // There is an edit at the end. Drop the post-context.
                hbTextLength = hbItemOffset + hbItemLength;
            }

            hb_buffer_add_utf16(buffer, hbText, hbTextLength, hbItemOffset, hbItemLength);

            uint numCodepoints;
            hb_glyph_info_t[] cpInfo = hb_buffer_get_glyph_infos(buffer, numCodepoints);

            // Add the hyphen at the end, if there's any.
            if (hasEndInsertion || hasEndReplacement)
            {
                // When a hyphen is inserted, by assigning the added hyphen and the last
                // codepoint added to the HarfBuzz buffer to the same cluster, we can make
                // sure that they always remain in the same cluster, even if the last
                // codepoint gets merged into another cluster (for example when it's a
                // combining mark).
                //
                // When a replacement happens instead, we want it to get the cluster value
                // of the character it's replacing, which is one "codepoint length" larger
                // than the last cluster. But since the character replaced is always just
                // one code unit, we can just add 1.
                uint hyphenCluster = new uint();
                if (numCodepoints == 0)
                {
                    // Nothing was added to the HarfBuzz buffer. This can only happen if
                    // we have a replacement that is replacing a one-code unit script run.
                    hyphenCluster = 0;
                }
                else
                {
                    hyphenCluster = cpInfo[numCodepoints - 1].cluster + (uint)hasEndReplacement;
                }
                addHyphenToHbBuffer(buffer, hbFont, new uint(endHyphen), new uint(hyphenCluster));
                // Since we have just added to the buffer, cpInfo no longer necessarily
                // points to the right place. Refresh it.
                cpInfo = hb_buffer_get_glyph_infos(buffer, null);
            }
            return cpInfo[0].cluster;
        }

        /*
         * Determine whether the code unit is a word space for the purposes of
         * justification.
         */

        /*
         * Determine whether the code unit is a word space for the purposes of
         * justification.
         */
        public static bool isWordSpace(UInt16 code_unit)
        {
            return code_unit == ' ' || code_unit == CHAR_NBSP;
        }

        /**
         * Return offset of previous word break. It is either < offset or == 0.
         */

        /**
         * Return offset of previous word break. It is either < offset or == 0.
         *
         * For the purpose of layout, a word break is a boundary with no
         * kerning or complex script processing. This is necessarily a
         * heuristic, but should be accurate most of the time.
         */
        public static int getPrevWordBreakForCache(UInt16[] chars, int offset, int len)
        {
            if (offset == 0)
            {
                return 0;
            }
            if (offset > len)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                //ORIGINAL LINE: offset = len;
                offset.CopyFrom(len);
            }
            if (isWordBreakBefore(chars[offset - 1]))
            {
                return offset - 1;
            }
            for (int i = offset - 1; i > 0; i--)
            {
                if (isWordBreakBefore(chars[i]) || isWordBreakAfter(chars[i - 1]))
                {
                    return i;
                }
            }
            return 0;
        }

        /**
         * Return offset of next word break. It is either > offset or == len.
         */

        /**
         * Return offset of next word break. It is either > offset or == len.
         *
         * For the purpose of layout, a word break is a boundary with no
         * kerning or complex script processing. This is necessarily a
         * heuristic, but should be accurate most of the time.
         */
        public static int getNextWordBreakForCache(UInt16[] chars, int offset, int len)
        {
            if (offset >= len)
            {
                return len;
            }
            if (isWordBreakAfter(chars[offset]))
            {
                return offset + 1;
            }
            for (int i = offset + 1; i < len; i++)
            {
                // No need to check isWordBreakAfter(chars[i - 1]) since it is checked
                // in previous iteration.  Note that isWordBreakBefore returns true
                // whenever isWordBreakAfter returns true.
                if (isWordBreakBefore(chars[i]))
                {
                    return i;
                }
            }
            return len;
        }

        public static readonly UInt16 CHAR_NBSP = 0x00A0;

        /**
         * For the purpose of layout, a word break is a boundary with no
         * kerning or complex script processing. This is necessarily a
         * heuristic, but should be accurate most of the time.
         */
        internal static bool isWordBreakAfter(UInt16 c)
        {
            if (isWordSpace(new UInt16(c)) || (c >= 0x2000 && c <= 0x200a) || c == 0x3000)
            {
                // spaces
                return true;
            }
            // Note: kana is not included, as sophisticated fonts may kern kana
            return false;
        }

        internal static bool isWordBreakBefore(UInt16 c)
        {
            // CJK ideographs (and yijing hexagram symbols)
            return isWordBreakAfter(new UInt16(c)) || (c >= 0x3400 && c <= 0x9fff);
        }

        public static readonly int CHAR_TAB = 0x0009;

        // Large scores in a hierarchy; we prefer desperate breaks to an overfull line.
        // All these constants are larger than any reasonable actual width score.
        public static readonly float SCORE_INFTY = float.MaxValue;
        public static readonly float SCORE_OVERFULL = 1e12f;
        public static readonly float SCORE_DESPERATE = 1e10f;

        // Multiplier for hyphen penalty on last line.
        public static readonly float LAST_LINE_PENALTY_MULTIPLIER = 4.0f;
        // Penalty assigned to each line break (to try to minimize number of lines)
        // TODO: when we implement full justification (so spaces can shrink and
        // stretch), this is probably not the most appropriate method.
        public static readonly float LINE_PENALTY_MULTIPLIER = 2.0f;

        // Penalty assigned to shrinking the whitepsace.
        public static readonly float SHRINK_PENALTY_MULTIPLIER = 4.0f;

        // Very long words trigger O(n^2) behavior in hyphenation, so we disable
        // hyphenation for unreasonably long words. This is somewhat of a heuristic
        // because extremely long words are possible in some languages. This does mean
        // that very long real words can get broken by desperate breaks, with no
        // hyphens.
        public static readonly int LONGEST_HYPHENATED_WORD = 45;

        // When the text buffer is within this limit, capacity of vectors is retained at
        // finish(), to avoid allocation.
        public static readonly int MAX_TEXT_BUF_RETAIN = 32678;

        // Maximum amount that spaces can shrink, in justified text.
        public static readonly float SHRINKABILITY = 1.0 / 3.0;

        // This function determines whether a character is a space that disappears at
        // end of line. It is the Unicode set:
        // [[:General_Category=Space_Separator:]-[:Line_Break=Glue:]], plus '\n'. Note:
        // all such characters are in the BMP, so it's ok to use code units for this.
        public static bool isLineEndSpace(UInt16 c)
        {
            return c == '\n' || c == ' ' || c == 0x1680 || (0x2000 <= c != null && c <= 0x200A && c != 0x2007) || c == 0x205F || c == 0x3000;
        }

        // These could be considered helper methods of layout, but need only be loosely
        // coupled, so are separate.

        internal static float getRunAdvance(float[] advances, UInt16 buf, int layoutStart, int start, int count, int offset)
        {
            float advance = 0.0f;
            int lastCluster = start;
            float clusterWidth = 0.0f;
            for (int i = start; i < offset; i++)
            {
                float charAdvance = advances[i - layoutStart];
                if (charAdvance != 0.0f)
                {
                    advance += charAdvance;
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                    //ORIGINAL LINE: lastCluster = i;
                    lastCluster.CopyFrom(i);
                    clusterWidth = charAdvance;
                }
            }
            if (offset < start + count && advances[offset - layoutStart] == 0.0f)
            {
                // In the middle of a cluster, distribute width of cluster so that each
                // grapheme cluster gets an equal share.
                // TODO: get caret information out of font when that's available
                int nextCluster = new int();
                for (nextCluster = offset + 1; nextCluster < start + count; nextCluster++)
                {
                    if (advances[nextCluster - layoutStart] != 0.0f)
                    {
                        break;
                    }
                }
                int numGraphemeClusters = 0;
                int numGraphemeClustersAfter = 0;
                for (int i = lastCluster; i < nextCluster; i++)
                {
                    bool isAfter = i >= offset;
                    if (GraphemeBreak.isGraphemeBreak(advances + (start - layoutStart), buf, start, count, i))
                    {
                        numGraphemeClusters++;
                        if (isAfter)
                        {
                            numGraphemeClustersAfter++;
                        }
                    }
                }
                if (numGraphemeClusters > 0)
                {
                    advance -= clusterWidth * numGraphemeClustersAfter / numGraphemeClusters;
                }
            }
            return advance;
        }

        public static float getRunAdvance(float advances, UInt16 buf, int start, int count, int offset)
        {
            return getRunAdvance(advances, buf, start, start, count, offset);
        }

        /**
         * Essentially the inverse of getRunAdvance. Compute the value of offset for
         * which the measured caret comes closest to the provided advance param, and
         * which is on a grapheme cluster boundary.
         *
         * The actual implementation fast-forwards through clusters to get "close", then
         * does a finer-grain search within the cluster and grapheme breaks.
         */
        public static int getOffsetForAdvance(float[] advances, UInt16 buf, int start, int count, float advance)
        {
            float x = 0.0f;
            float xLastClusterStart = 0.0f;
            float xSearchStart = 0.0f;
            int lastClusterStart = start;
            int searchStart = start;
            for (int i = start; i < start + count; i++)
            {
                if (GraphemeBreak.isGraphemeBreak(advances, buf, start, count, i))
                {
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                    //ORIGINAL LINE: searchStart = lastClusterStart;
                    searchStart.CopyFrom(lastClusterStart);
                    xSearchStart = xLastClusterStart;
                }
                float width = advances[i - start];
                if (width != 0.0f)
                {
                    //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                    //ORIGINAL LINE: lastClusterStart = i;
                    lastClusterStart.CopyFrom(i);
                    xLastClusterStart = x;
                    x += width;
                    if (x > advance)
                    {
                        break;
                    }
                }
            }
            int best = searchStart;
            float bestDist = FLT_MAX;
            for (int i = searchStart; i <= start + count; i++)
            {
                if (GraphemeBreak.isGraphemeBreak(advances, buf, start, count, i))
                {
                    // "getRunAdvance(layout, buf, start, count, i) - advance" but more
                    // efficient
                    float delta = getRunAdvance(advances, buf, start, searchStart, count - searchStart, i) + xSearchStart - advance;
                    if (Math.Abs(delta) < bestDist)
                    {
                        bestDist = Math.Abs(delta);
                        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                        //ORIGINAL LINE: best = i;
                        best.CopyFrom(i);
                    }
                    if (delta >= 0.0f)
                    {
                        break;
                    }
                }
            }
            return best;
        }

        // All external Minikin interfaces are designed to be thread-safe.
        // Presently, that's implemented by through a global lock, and having
        // all external interfaces take that lock.

        //C++ TO C# CONVERTER NOTE: 'extern' variable declarations are not required in C#:
        //extern std::recursive_mutex gMinikinLock;

        // Aborts if gMinikinLock is not acquired. Do nothing on the release build.
        public static void assertMinikinLocked()
        {
#if ENABLE_RACE_DETECTION
	  LOG_ALWAYS_FATAL_IF(gMinikinLock.tryLock() == 0);
#endif
        }

        public static hb_blob_t getFontTable(MinikinFont minikinFont, uint tag)
        {
            assertMinikinLocked();
            HarfBuzzSharp.Font font = getHbFontLocked(minikinFont);
            HarfBuzzSharp.Face face = hb_font_get_face(font);
            hb_blob_t blob = hb_face_reference_table(face, tag);
            hb_font_destroy(font);
            return blob;
        }

        public const uint MAX_UNICODE_CODE_POINT = 0x10FFFF;

        public static std::recursive_mutex gMinikinLock = new std::recursive_mutex();

        public static readonly uint SparseBitSet.kNotFound = new uint ();

	public static readonly uint CHAR_SOFT_HYPHEN = 0x00AD;
        public static readonly uint CHAR_ZWJ = 0x200D;

        /**
         * Determine whether a line break at position i within the buffer buf is valid.
         *This represents customization beyond the ICU behavior, because plain ICU
         *provides some line break opportunities that we don't want.
         **/
        internal static bool isBreakValid(UInt16 buf, int bufEnd, int i)
        {
            uint codePoint = new uint();
            int prev_offset = i;
            U16_PREV(buf, 0, prev_offset, codePoint);
            // Do not break on hard or soft hyphens. These are handled by automatic
            // hyphenation.
            if (Hyphenator.isLineBreakingHyphen(codePoint) || codePoint == CHAR_SOFT_HYPHEN)
            {
                // txt addition: Temporarily always break on hyphen. Changed from false to
                // true.
                return true;
            }
            // For Myanmar kinzi sequences, created by <consonant, ASAT, VIRAMA,
            // consonant>. This is to go around a bug in ICU line breaking:
            // http://bugs.icu-project.org/trac/ticket/12561. To avoid too much looking
            // around in the strings, we simply avoid breaking after any Myanmar virama,
            // where no line break could be imagined, since the Myanmar virama is a pure
            // stacker.
            if (codePoint == 0x1039)
            { // MYANMAR SIGN VIRAMA
                return false;
            }

            uint next_codepoint = new uint();
            int next_offset = i;
            U16_NEXT(buf, next_offset, bufEnd, next_codepoint);

            // Rule LB8 for Emoji ZWJ sequences. We need to do this ourselves since we may
            // have fresher emoji data than ICU does.
            if (codePoint == CHAR_ZWJ && isEmoji(next_codepoint))
            {
                return false;
            }

            // Rule LB30b. We need to this ourselves since we may have fresher emoji data
            // than ICU does.
            if (isEmojiModifier(next_codepoint))
            {
                if (codePoint == 0xFE0F && prev_offset > 0)
                {
                    // skip over emoji variation selector
                    U16_PREV(buf, 0, prev_offset, codePoint);
                }
                if (isEmojiBase(codePoint))
                {
                    return false;
                }
            }
            return true;
        }

        // Chicago Manual of Style recommends breaking after these characters in URLs
        // and email addresses
        internal static bool breakAfter(UInt16 c)
        {
            return c == ':' || c == '=' || c == '&';
        }

        // Chicago Manual of Style recommends breaking before these characters in URLs
        // and email addresses
        internal static bool breakBefore(UInt16 c)
        {
            return c == '~' || c == '.' || c == ',' || c == '-' || c == '_' || c == '?' || c == '#' || c == '%' || c == '=' || c == '&';
        }

        // Returns true if c is emoji.
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //bool isEmoji(uint c);

        // Returns true if c is emoji modifier base.
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //bool isEmojiBase(uint c);

        // Returns true if c is emoji modifier.
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //bool isEmojiModifier(uint c);

        // Bidi override for ICU that knows about new emoji.
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //UCharDirection emojiBidiOverride(object context, UChar32 c);

        public static android.hash_t hash_type(FontStyle style)
        {
            return style.hash();
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //bool isLineEndSpace(UInt16 c);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //float getRunAdvance(float advances, UInt16 buf, int start, int count, int offset);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //int getOffsetForAdvance(float advances, UInt16 buf, int start, int count, float advance);
    }
}