using System;
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

// #define VERBOSE_DEBUG



//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/unistr.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/unorm2.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/utf16.h"



namespace minikin
{

//C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <typename T>

//C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:



// Calculates a font score.
// The score of the font family is based on three subscores.
//  - Coverage Score: How well the font family covers the given character or
//  variation sequence.
//  - Language Score: How well the font family is appropriate for the language.
//  - Variant Score: Whether the font family matches the variant. Note that this
//  variant is not the
//    one in BCP47. This is our own font variant (e.g., elegant, compact).
//
// Then, there is a priority for these three subscores as follow:
//   Coverage Score > Language Score > Variant Score
// The returned score reflects this priority order.
//
// Note that there are two special scores.
//  - kUnsupportedFontScore: When the font family doesn't support the variation
//  sequence or even its
//    base character.
//  - kFirstFontScore: When the font is the first font family in the collection
//  and it supports the
//    given character or variation sequence.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint FontCollection::calcFamilyScore(uint ch, uint vs, int variant, uint langListId, const FontFamily*& fontFamily) const

// Calculates a font score based on variation sequence coverage.
// - Returns kUnsupportedFontScore if the font doesn't support the variation
// sequence or its base
//   character.
// - Returns kFirstFontScore if the font family is the first font family in the
// collection and it
//   supports the given character or variation sequence.
// - Returns 3 if the font family supports the variation sequence.
// - Returns 2 if the vs is a color variation selector (U+FE0F) and if the font
// is an emoji font.
// - Returns 2 if the vs is a text variation selector (U+FE0E) and if the font
// is not an emoji font.
// - Returns 1 if the variation selector is not specified or if the font family
// only supports the
//   variation sequence's base character.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint FontCollection::calcCoverageScore(uint ch, uint vs, const FontFamily*& fontFamily) const

// Calculate font scores based on the script matching, subtag matching and
// primary langauge matching.
//
// 1. If only the font's language matches or there is no matches between
// requested font and
//    supported font, then the font obtains a score of 0.
// 2. Without a match in language, considering subtag may change font's
// EmojiStyle over script,
//    a match in subtag gets a score of 2 and a match in scripts gains a score
//    of 1.
// 3. Regarding to two elements matchings, language-and-subtag matching has a
// score of 4, while
//    language-and-script obtains a socre of 3 with the same reason above.
//
// If two languages in the requested list have the same language score, the font
// matching with higher priority language gets a higher score. For example, in
// the case the user requested language list is "ja-Jpan,en-Latn". The score of
// for the font of "ja-Jpan" gets a higher score than the font of "en-Latn".
//
// To achieve score calculation with priorities, the language score is
// determined as follows:
//   LanguageScore = s(0) * 5^(m - 1) + s(1) * 5^(m - 2) + ... + s(m - 2) * 5 +
//   s(m - 1)
// Here, m is the maximum number of languages to be compared, and s(i) is the
// i-th language's matching score. The possible values of s(i) are 0, 1, 2, 3
// and 4.

// Calculates a font score based on variant ("compact" or "elegant") matching.
//  - Returns 1 if the font doesn't have variant or the variant matches with the
//  text style.
//  - No score if the font has a variant but it doesn't match with the text
//  style.

// Implement heuristic for choosing best-match font. Here are the rules:
// 1. If first font in the collection has the character, it wins.
// 2. Calculate a score for the font family. See comments in calcFamilyScore for
// the detail.
// 3. Highest score wins, with ties resolved to the first font.
// This method never returns nullptr.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const FontFamily*& FontCollection::getFamilyForChar(uint ch, uint vs, uint langListId, int variant) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool FontCollection::hasVariationSelector(uint baseCodepoint, uint variationSelector) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void FontCollection::itemize(const UInt16* string, int string_size, FontStyle style, vector<Run>* result) const



//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint FontCollection::getId() const

} // namespace minikin
