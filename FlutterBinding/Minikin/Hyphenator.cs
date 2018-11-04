using System;

/*
 * Copyright (C) 2015 The Android Open Source Project
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


// HACK: for reading pattern file


/*
 * Copyright (C) 2015 The Android Open Source Project
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

/**
 * An implementation of Liang's hyphenation algorithm.
 */


//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/locid.h"


namespace minikin
{

public enum HyphenationType : byte
{
  // Note: There are implicit assumptions scattered in the code that DONT_BREAK
  // is 0.

  // Do not break.
  DONT_BREAK = 0,
  // Break the line and insert a normal hyphen.
  BREAK_AND_INSERT_HYPHEN = 1,
  // Break the line and insert an Armenian hyphen (U+058A).
  BREAK_AND_INSERT_ARMENIAN_HYPHEN = 2,
  // Break the line and insert a maqaf (Hebrew hyphen, U+05BE).
  BREAK_AND_INSERT_MAQAF = 3,
  // Break the line and insert a Canadian Syllabics hyphen (U+1400).
  BREAK_AND_INSERT_UCAS_HYPHEN = 4,
  // Break the line, but don't insert a hyphen. Used for cases when there is
  // already a hyphen
  // present or the script does not use a hyphen (e.g. in Malayalam).
  BREAK_AND_DONT_INSERT_HYPHEN = 5,
  // Break and replace the last code unit with hyphen. Used for Catalan "l·l"
  // which hyphenates
  // as "l-/l".
  BREAK_AND_REPLACE_WITH_HYPHEN = 6,
  // Break the line, and repeat the hyphen (which is the last character) at the
  // beginning of the
  // next line. Used in Polish, where "czerwono-niebieska" should hyphenate as
  // "czerwono-/-niebieska".
  BREAK_AND_INSERT_HYPHEN_AT_NEXT_LINE = 7,
  // Break the line, insert a ZWJ and hyphen at the first line, and a ZWJ at the
  // second line.
  // This is used in Arabic script, mostly for writing systems of Central Asia.
  // It's our default
  // behavior when a soft hyphen is used in Arabic script.
  BREAK_AND_INSERT_HYPHEN_AND_ZWJ = 8
}

// The hyphen edit represents an edit to the string when a word is
// hyphenated. The most common hyphen edit is adding a "-" at the end
// of a syllable, but nonstandard hyphenation allows for more choices.
// Note that a HyphenEdit can hold two types of edits at the same time,
// One at the beginning of the string/line and one at the end.
public class HyphenEdit
{
  public const uint NO_EDIT = 0x00;

  public const uint INSERT_HYPHEN_AT_END = 0x01;
  public const uint INSERT_ARMENIAN_HYPHEN_AT_END = 0x02;
  public const uint INSERT_MAQAF_AT_END = 0x03;
  public const uint INSERT_UCAS_HYPHEN_AT_END = 0x04;
  public const uint INSERT_ZWJ_AND_HYPHEN_AT_END = 0x05;
  public const uint REPLACE_WITH_HYPHEN_AT_END = 0x06;
  public const uint BREAK_AT_END = 0x07;

  public uint INSERT_HYPHEN_AT_START = 0x01 << 3;
  public uint INSERT_ZWJ_AT_START = 0x02 << 3;
  public uint BREAK_AT_START = 0x03 << 3;

  // Keep in sync with the definitions in the Java code at:
  // frameworks/base/graphics/java/android/graphics/Paint.java
  public const uint MASK_END_OF_LINE = 0x07;
  public uint MASK_START_OF_LINE = 0x03 << 3;

  public static bool isReplacement(uint hyph)
  {
	return hyph == REPLACE_WITH_HYPHEN_AT_END;
  }

  public static bool isInsertion(uint hyph)
  {
	return (hyph == INSERT_HYPHEN_AT_END || hyph == INSERT_ARMENIAN_HYPHEN_AT_END || hyph == INSERT_MAQAF_AT_END || hyph == INSERT_UCAS_HYPHEN_AT_END || hyph == INSERT_ZWJ_AND_HYPHEN_AT_END || hyph == INSERT_HYPHEN_AT_START || hyph == INSERT_ZWJ_AT_START);
  }

  public static uint getHyphenString(uint hyph)
  {
	switch (hyph)
	{
	  case INSERT_HYPHEN_AT_END:
	  case REPLACE_WITH_HYPHEN_AT_END:
	  case INSERT_HYPHEN_AT_START:
		return GlobalMembers.HYPHEN_STR;
	  case INSERT_ARMENIAN_HYPHEN_AT_END:
		return GlobalMembers.ARMENIAN_HYPHEN_STR;
	  case INSERT_MAQAF_AT_END:
		return GlobalMembers.MAQAF_STR;
	  case INSERT_UCAS_HYPHEN_AT_END:
		return GlobalMembers.UCAS_HYPHEN_STR;
	  case INSERT_ZWJ_AND_HYPHEN_AT_END:
		return GlobalMembers.ZWJ_AND_HYPHEN_STR;
	  case INSERT_ZWJ_AT_START:
		return GlobalMembers.ZWJ_STR;
	  default:
		return null;
	}
  }
  public static uint editForThisLine(HyphenationType type)
  {
	switch (type)
	{
	  case HyphenationType.DONT_BREAK:
		return NO_EDIT;
	  case HyphenationType.BREAK_AND_INSERT_HYPHEN:
		return INSERT_HYPHEN_AT_END;
	  case HyphenationType.BREAK_AND_INSERT_ARMENIAN_HYPHEN:
		return INSERT_ARMENIAN_HYPHEN_AT_END;
	  case HyphenationType.BREAK_AND_INSERT_MAQAF:
		return INSERT_MAQAF_AT_END;
	  case HyphenationType.BREAK_AND_INSERT_UCAS_HYPHEN:
		return INSERT_UCAS_HYPHEN_AT_END;
	  case HyphenationType.BREAK_AND_REPLACE_WITH_HYPHEN:
		return REPLACE_WITH_HYPHEN_AT_END;
	  case HyphenationType.BREAK_AND_INSERT_HYPHEN_AND_ZWJ:
		return INSERT_ZWJ_AND_HYPHEN_AT_END;
	  default:
		return BREAK_AT_END;
	}
  }
  public static uint editForNextLine(HyphenationType type)
  {
	switch (type)
	{
	  case HyphenationType.DONT_BREAK:
		return NO_EDIT;
	  case HyphenationType.BREAK_AND_INSERT_HYPHEN_AT_NEXT_LINE:
		return INSERT_HYPHEN_AT_START;
	  case HyphenationType.BREAK_AND_INSERT_HYPHEN_AND_ZWJ:
		return INSERT_ZWJ_AT_START;
	  default:
		return BREAK_AT_START;
	}
  }

  public HyphenEdit()
  {
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.hyphen = NO_EDIT;
	  this.hyphen.CopyFrom(NO_EDIT);
  }
  public HyphenEdit(uint hyphenInt)
  {
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.hyphen = hyphenInt;
	  this.hyphen.CopyFrom(hyphenInt);
  } // NOLINT(implicit)
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getHyphen() const
  public uint getHyphen()
  {
	  return hyphen;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const HyphenEdit& other) const
  public static bool operator == (HyphenEdit ImpliedObject, HyphenEdit other)
  {
	return ImpliedObject.hyphen == other.hyphen;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getEnd() const
  public uint getEnd()
  {
	  return hyphen & MASK_END_OF_LINE;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getStart() const
  public uint getStart()
  {
	  return hyphen & MASK_START_OF_LINE;
  }

  private uint hyphen = new uint();
}

// hyb file header; implementation details are in the .cpp file
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct Header;

public class Hyphenator
{
  // Compute the hyphenation of a word, storing the hyphenation in result
  // vector. Each entry in the vector is a "hyphenation type" for a potential
  // hyphenation that can be applied at the corresponding code unit offset in
  // the word.
  //
  // Example: word is "hyphen", result is the following, corresponding to
  // "hy-phen": [DONT_BREAK, DONT_BREAK, BREAK_AND_INSERT_HYPHEN, DONT_BREAK,
  // DONT_BREAK, DONT_BREAK]
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//  void hyphenate(ClassicVector<HyphenationType> result, UInt16 word, int len, icu::Locale locale);

  // Returns true if the codepoint is like U+2010 HYPHEN in line breaking and
  // usage: a character immediately after which line breaks are allowed, but
  // words containing it should not be automatically hyphenated.

  // This function determines whether a character is like U+2010 HYPHEN in
  // line breaking and usage: a character immediately after which line breaks
  // are allowed, but words containing it should not be automatically
  // hyphenated using patterns. This is a curated set, created by manually
  // inspecting all the characters that have the Unicode line breaking
  // property of BA or HY and seeing which ones are hyphens.
  public static bool isLineBreakingHyphen(uint c)
  {
	return (c == 0x002D || c == 0x058A || c == 0x05BE || c == 0x1400 || c == 0x2010 || c == 0x2013 || c == 0x2027 || c == 0x2E17 || c == 0x2E40); // DOUBLE HYPHEN
  }

  // pattern data is in binary format, as described in doc/hyb_file_format.md.
  // Note: the caller is responsible for ensuring that the lifetime of the
  // pattern data is at least as long as the Hyphenator object.

  // Note: nullptr is valid input, in which case the hyphenator only processes
  // soft hyphens.
  public static Hyphenator loadBinary(byte patternData, int minPrefix, int minSuffix)
  {
	Hyphenator result = new Hyphenator();
	result.patternData = patternData;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: result->minPrefix = minPrefix;
	result.minPrefix.CopyFrom(minPrefix);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: result->minSuffix = minSuffix;
	result.minSuffix.CopyFrom(minSuffix);
	return result;
  }

  // apply various hyphenation rules including hard and soft hyphens, ignoring
  // patterns

  // Use various recommendations of UAX #14 Unicode Line Breaking Algorithm for
  // hyphenating words that didn't match patterns, especially words that contain
  // hyphens or soft hyphens (See sections 5.3, Use of Hyphen, and 5.4, Use of
  // Soft Hyphen).
  private void hyphenateWithNoPatterns(HyphenationType[] result, UInt16[] word, int len, icu.Locale locale)
  {
	result[0] = HyphenationType.DONT_BREAK;
	for (int i = 1; i < len; i++)
	{
	  UInt16 prevChar = word[i - 1];
	  if (i > 1 && isLineBreakingHyphen(new UInt16(prevChar)))
	  {
		// Break after hyphens, but only if they don't start the word.

		if ((prevChar == GlobalMembers.CHAR_HYPHEN_MINUS || prevChar == GlobalMembers.CHAR_HYPHEN) && string.Compare(locale.getLanguage(), "pl") == 0 && minikin.GlobalMembers.getScript(word[i]) == USCRIPT_LATIN)
		{
		  // In Polish, hyphens get repeated at the next line. To be safe,
		  // we will do this only if the next character is Latin.
		  result[i] = HyphenationType.BREAK_AND_INSERT_HYPHEN_AT_NEXT_LINE;
		}
		else
		{
		  result[i] = HyphenationType.BREAK_AND_DONT_INSERT_HYPHEN;
		}
	  }
	  else if (i > 1 && prevChar == GlobalMembers.CHAR_SOFT_HYPHEN)
	  {
		// Break after soft hyphens, but only if they don't start the word (a soft
		// hyphen starting the word doesn't give any useful break opportunities).
		// The type of the break is based on the script of the character we break
		// on.
		if (minikin.GlobalMembers.getScript(word[i]) == USCRIPT_ARABIC)
		{
		  // For Arabic, we need to look and see if the characters around the soft
		  // hyphen actually join. If they don't, we'll just insert a normal
		  // hyphen.
		  result[i] = minikin.GlobalMembers.getHyphTypeForArabic(new UInt16(word), len, i);
		}
		else
		{
		  result[i] = minikin.GlobalMembers.hyphenationTypeBasedOnScript(word[i]);
		}
	  }
	  else if (prevChar == GlobalMembers.CHAR_MIDDLE_DOT && minPrefix < i && i <= len - minSuffix && ((word[i - 2] == 'l' && word[i] == 'l') || (word[i - 2] == 'L' && word[i] == 'L')) && string.Compare(locale.getLanguage(), "ca") == 0)
	  {
		// In Catalan, "l·l" should break as "l-" on the first line
		// and "l" on the next line.
		result[i] = HyphenationType.BREAK_AND_REPLACE_WITH_HYPHEN;
	  }
	  else
	  {
		result[i] = HyphenationType.DONT_BREAK;
	  }
	}
  }

  // Try looking up word in alphabet table, return DONT_BREAK if any code units
  // fail to map. Otherwise, returns BREAK_AND_INSERT_HYPHEN,
  // BREAK_AND_INSERT_ARMENIAN_HYPHEN, or BREAK_AND_DONT_INSERT_HYPHEN based on
  // the the script of the characters seen. Note that this method writes len+2
  // entries into alpha_codes (including start and stop)
  private HyphenationType alphabetLookup(UInt16[] alpha_codes, UInt16[] word, int len)
  {
	Header header = getHeader();
	HyphenationType result = HyphenationType.BREAK_AND_INSERT_HYPHEN;
	// TODO: check header magic
	uint alphabetVersion = header.alphabetVersion();
	if (alphabetVersion == 0)
	{
	  AlphabetTable0 alphabet = header.alphabetTable0();
	  uint min_codepoint = new uint(alphabet.min_codepoint);
	  uint max_codepoint = new uint(alphabet.max_codepoint);
	  alpha_codes[0] = 0; // word start
	  for (int i = 0; i < len; i++)
	  {
		UInt16 c = word[i];
		if (c < min_codepoint || c >= max_codepoint)
		{
		  return HyphenationType.DONT_BREAK;
		}
		byte code = alphabet.data[c - min_codepoint];
		if (code == 0)
		{
		  return HyphenationType.DONT_BREAK;
		}
		if (result == HyphenationType.BREAK_AND_INSERT_HYPHEN)
		{
		  result = minikin.GlobalMembers.hyphenationTypeBasedOnScript(new UInt16(c));
		}
		alpha_codes[i + 1] = code;
	  }
	  alpha_codes[len + 1] = 0; // word termination
	  return result;
	}
	else if (alphabetVersion == 1)
	{
	  AlphabetTable1 alphabet = header.alphabetTable1();
	  int n_entries = alphabet.n_entries;
	  uint[] begin = new uint(alphabet.data);
	  uint end = begin + n_entries;
	  alpha_codes[0] = 0;
	  for (int i = 0; i < len; i++)
	  {
		UInt16 c = word[i];
		var p = std::lower_bound<uint*, uint>(begin, end, c << 11);
		if (p == end)
		{
		  return HyphenationType.DONT_BREAK;
		}
		uint entry = p;
		if (AlphabetTable1.codepoint(new uint(entry)) != c)
		{
		  return HyphenationType.DONT_BREAK;
		}
		if (result == HyphenationType.BREAK_AND_INSERT_HYPHEN)
		{
		  result = minikin.GlobalMembers.hyphenationTypeBasedOnScript(new UInt16(c));
		}
		alpha_codes[i + 1] = AlphabetTable1.value(new uint(entry));
	  }
	  alpha_codes[len + 1] = 0;
	  return result;
	}
	return HyphenationType.DONT_BREAK;
  }

  // calculate hyphenation from patterns, assuming alphabet lookup has already
  // been done

  /**
   * Internal implementation, after conversion to codes. All case folding and
   *normalization has been done by now, and all characters have been found in the
   *alphabet. Note: len here is the padded length including 0 codes at start and
   *end.
   **/
  private void hyphenateFromCodes(HyphenationType[] result, UInt16[] codes, int len, HyphenationType hyphenValue)
  {
  //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
  //  static_assert(sizeof(HyphenationType) == sizeof(byte), "HyphnationType must be byte.");
	// Reuse the result array as a buffer for calculating intermediate hyphenation
	// numbers.
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	byte[] buffer = reinterpret_cast<byte>(result);

	Header header = getHeader();
	Trie trie = header.trieTable();
	Pattern pattern = header.patternTable();
	uint char_mask = new uint(trie.char_mask);
	uint link_shift = new uint(trie.link_shift);
	uint link_mask = new uint(trie.link_mask);
	uint pattern_shift = new uint(trie.pattern_shift);
	int maxOffset = len - minSuffix - 1;
	for (int i = 0; i < len - 1; i++)
	{
	  uint node = 0; // index into Trie table
	  for (int j = i; j < len; j++)
	  {
		UInt16 c = codes[j];
		uint entry = trie.data[node + c];
		if ((entry & char_mask) == c)
		{
		  node = (entry & link_mask) >> link_shift;
		}
		else
		{
		  break;
		}
		uint pat_ix = trie.data[node] >> pattern_shift;
		// pat_ix contains a 3-tuple of length, shift (number of trailing zeros),
		// and an offset into the buf pool. This is the pattern for the substring
		// (i..j) we just matched, which we combine (via point-wise max) into the
		// buffer vector.
		if (pat_ix != 0)
		{
		  uint pat_entry = pattern.data[pat_ix];
		  int pat_len = Pattern.len(new uint(pat_entry));
		  int pat_shift = Pattern.shift(new uint(pat_entry));
		  byte[] pat_buf = pattern.buf(new uint(pat_entry));
		  int offset = j + 1 - (pat_len + pat_shift);
		  // offset is the index within buffer that lines up with the start of
		  // pat_buf
		  int start = Math.Max((int)minPrefix - offset, 0);
		  int end = Math.Min(pat_len, (int)maxOffset - offset);
		  for (int k = start; k < end; k++)
		  {
			buffer[offset + k] = Math.Max(buffer[offset + k], pat_buf[k]);
		  }
		}
	  }
	}
	// Since the above calculation does not modify values outside
	// [minPrefix, len - minSuffix], they are left as 0 = DONT_BREAK.
	for (int i = minPrefix; i < maxOffset; i++)
	{
	  // Hyphenation opportunities happen when the hyphenation numbers are odd.
	  result[i] = ((buffer[i] & 1u) != null) ? hyphenValue : HyphenationType.DONT_BREAK;
	}
  }

  // See also LONGEST_HYPHENATED_WORD in LineBreaker.cpp. Here the constant is
  // used so that temporary buffers can be stack-allocated without waste, which
  // is a slightly different use case. It measures UTF-16 code units.
  private const int MAX_HYPHENATED_SIZE = 64;

  private readonly byte patternData;
  private int minPrefix = new int();
  private int minSuffix = new int();

  // accessors for binary data
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Header* getHeader() const
  private Header getHeader()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const Header>(patternData);
  }
}

} // namespace minikin


//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define DISABLE_TEST_WINDOWS(TEST_NAME) DISABLED_##TEST_NAME
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FRIEND_TEST_WINDOWS_DISABLED_EXPANDED(SUITE, TEST_NAME) FRIEND_TEST(SUITE, TEST_NAME)
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FRIEND_TEST_WINDOWS_DISABLED(SUITE, TEST_NAME) FRIEND_TEST_WINDOWS_DISABLED_EXPANDED(SUITE, DISABLE_TEST_WINDOWS(TEST_NAME))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define DISABLE_TEST_WINDOWS(TEST_NAME) TEST_NAME
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define FRIEND_TEST_WINDOWS_DISABLED(SUITE, TEST_NAME) FRIEND_TEST(SUITE, TEST_NAME)


namespace minikin
{

// The following are structs that correspond to tables inside the hyb file
// format

public class AlphabetTable0
{
  public uint version = new uint();
  public uint min_codepoint = new uint();
  public uint max_codepoint = new uint();
  public byte[] data = Arrays.InitializeWithDefaultInstances<byte>(1); // actually flexible array, size is known at runtime
}

public class AlphabetTable1
{
  public uint version = new uint();
  public uint n_entries = new uint();
  public uint[] data = Arrays.InitializeWithDefaultInstances<uint>(1); // actually flexible array, size is known at runtime

  public static uint codepoint(uint entry)
  {
	  return entry >> 11;
  }
  public static uint value(uint entry)
  {
	  return entry & 0x7ff;
  }
}

public class Trie
{
  public uint version = new uint();
  public uint char_mask = new uint();
  public uint link_shift = new uint();
  public uint link_mask = new uint();
  public uint pattern_shift = new uint();
  public uint n_entries = new uint();
  public uint[] data = Arrays.InitializeWithDefaultInstances<uint>(1); // actually flexible array, size is known at runtime
}

public class Pattern
{
  public uint version = new uint();
  public uint n_entries = new uint();
  public uint pattern_offset = new uint();
  public uint pattern_size = new uint();
  public uint[] data = Arrays.InitializeWithDefaultInstances<uint>(1); // actually flexible array, size is known at runtime

  // accessors
  public static uint len(uint entry)
  {
	  return entry >> 26;
  }
  public static uint shift(uint entry)
  {
	  return (entry >> 20) & 0x3f;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const byte* buf(uint entry) const
  public byte buf(uint entry)
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const byte>(this) + pattern_offset + (entry & 0xfffff);
  }
}

public class Header
{
  public uint magic = new uint();
  public uint version = new uint();
  public uint alphabet_offset = new uint();
  public uint trie_offset = new uint();
  public uint pattern_offset = new uint();
  public uint file_size = new uint();

  // accessors
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const byte* bytes() const
  public byte bytes()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const byte>(this);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint alphabetVersion() const
  public uint alphabetVersion()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return *reinterpret_cast<const uint>(bytes() + alphabet_offset);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const AlphabetTable0* alphabetTable0() const
  public AlphabetTable0 alphabetTable0()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const AlphabetTable0>(bytes() + alphabet_offset);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const AlphabetTable1* alphabetTable1() const
  public AlphabetTable1 alphabetTable1()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const AlphabetTable1>(bytes() + alphabet_offset);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Trie* trieTable() const
  public Trie trieTable()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const Trie>(bytes() + trie_offset);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Pattern* patternTable() const
  public Pattern patternTable()
  {
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	return reinterpret_cast<const Pattern>(bytes() + pattern_offset);
  }
}


} // namespace minikin
