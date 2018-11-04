using System.Collections.Generic;

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




namespace minikin
{

// FontLanguage is a compact representation of a BCP 47 language tag. It
// does not capture all possible information, only what directly affects
// font rendering.
public class FontLanguage
{
  public enum EmojiStyle : byte
  {
	EMSTYLE_EMPTY = 0,
	EMSTYLE_DEFAULT = 1,
	EMSTYLE_EMOJI = 2,
	EMSTYLE_TEXT = 3,
  }
  // Default constructor creates the unsupported language.
  public FontLanguage()
  {
	  this.mScript = 0U;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mLanguage = INVALID_CODE;
	  this.mLanguage.CopyFrom(GlobalMembers.INVALID_CODE);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mRegion = INVALID_CODE;
	  this.mRegion.CopyFrom(GlobalMembers.INVALID_CODE);
	  this.mHbLanguage = HB_LANGUAGE_INVALID;
	  this.mSubScriptBits = 0U;
	  this.mEmojiStyle = new minikin.FontLanguage.EmojiStyle.EMSTYLE_EMPTY;
  }

  // Parse from string

  // Parse BCP 47 language identifier into internal structure
  public FontLanguage(string buf, int length) : this()
  {
	int firstDelimiterPos = minikin.GlobalMembers.nextDelimiterIndex(buf, length, 0);
	if (minikin.GlobalMembers.isValidLanguageCode(buf, firstDelimiterPos))
	{
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: mLanguage = packLanguageOrRegion(buf, firstDelimiterPos, 'a', 'a');
	  mLanguage.CopyFrom(minikin.GlobalMembers.packLanguageOrRegion(buf, firstDelimiterPos, 'a', 'a'));
	}
	else
	{
	  // We don't understand anything other than two-letter or three-letter
	  // language codes, so we skip parsing the rest of the string.
	  return;
	}

	if (firstDelimiterPos == length)
	{
	  mHbLanguage = hb_language_from_string(getString(), -1);
	  return; // Language code only.
	}

	int nextComponentStartPos = firstDelimiterPos + 1;
	int nextDelimiterPos = minikin.GlobalMembers.nextDelimiterIndex(buf, length, nextComponentStartPos);
	int componentLength = nextDelimiterPos - nextComponentStartPos;

	if (componentLength == 4)
	{
	  // Possibly script code.
	  string p = buf + nextComponentStartPos;
	  if (minikin.GlobalMembers.isValidScriptCode(p))
	  {
		mScript = (((uint)(p[0])) << 24 | ((uint)(p[1])) << 16 | ((uint)(p[2])) << 8 | ((uint)(p[3])));
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: mSubScriptBits = scriptToSubScriptBits(mScript);
		mSubScriptBits.CopyFrom(scriptToSubScriptBits(new uint(mScript)));
	  }

	  if (nextDelimiterPos == length)
	  {
		mHbLanguage = hb_language_from_string(getString(), -1);
		mEmojiStyle = resolveEmojiStyle(buf, length, new uint(mScript));
		return; // No region code.
	  }

//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: nextComponentStartPos = nextDelimiterPos + 1;
	  nextComponentStartPos.CopyFrom(nextDelimiterPos + 1);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: nextDelimiterPos = nextDelimiterIndex(buf, length, nextComponentStartPos);
	  nextDelimiterPos.CopyFrom(minikin.GlobalMembers.nextDelimiterIndex(buf, length, nextComponentStartPos));
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: componentLength = nextDelimiterPos - nextComponentStartPos;
	  componentLength.CopyFrom(nextDelimiterPos - nextComponentStartPos);
	}

	if (componentLength == 2 || componentLength == 3)
	{
	  // Possibly region code.
	  string p = buf + nextComponentStartPos;
	  if (minikin.GlobalMembers.isValidRegionCode(p, componentLength))
	  {
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: mRegion = packLanguageOrRegion(p, componentLength, 'A', '0');
		mRegion.CopyFrom(minikin.GlobalMembers.packLanguageOrRegion(p, componentLength, 'A', '0'));
	  }
	}

	mHbLanguage = hb_language_from_string(getString(), -1);
	mEmojiStyle = resolveEmojiStyle(buf, length, new uint(mScript));
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const FontLanguage other) const
  public static bool operator == (FontLanguage ImpliedObject, FontLanguage other)
  {
	return !ImpliedObject.isUnsupported() && ImpliedObject.isEqualScript(other) && ImpliedObject.mLanguage == other.mLanguage && ImpliedObject.mRegion == other.mRegion && ImpliedObject.mEmojiStyle == other.mEmojiStyle;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(const FontLanguage other) const
  public static bool operator != (FontLanguage ImpliedObject, FontLanguage other)
  {
	  return !(*ImpliedObject == other);
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isUnsupported() const
  public bool isUnsupported()
  {
	  return mLanguage == GlobalMembers.INVALID_CODE;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: EmojiStyle getEmojiStyle() const
  public EmojiStyle getEmojiStyle()
  {
	  return mEmojiStyle;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: hb_language_t getHbLanguage() const
  public hb_language_t getHbLanguage()
  {
	  return mHbLanguage;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isEqualScript(const FontLanguage& other) const
  public bool isEqualScript(FontLanguage other)
  {
	return other.mScript == mScript;
  }

  // Returns true if this script supports the given script. For example, ja-Jpan
  // supports Hira, ja-Hira doesn't support Jpan.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool supportsHbScript(hb_script_t script) const
  public bool supportsHbScript(hb_script_t script)
  {
  //C++ TO C# CONVERTER TODO TASK: There is no equivalent in C# to 'static_assert':
  //  static_assert((((uint)('J')) << 24 | ((uint)('p')) << 16 | ((uint)('a')) << 8 | ((uint)('n'))) == HB_TAG('J', 'p', 'a', 'n'), "The Minikin script and HarfBuzz hb_script_t have different encodings.");
	if (script == mScript)
	{
	  return true;
	}
	return supportsScript(new byte(mSubScriptBits), scriptToSubScriptBits(new hb_script_t(script)));
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string getString() const
  public string getString()
  {
	if (isUnsupported())
	{
	  return "und";
	}
	string buf = new string(new char[16]);
	int i = minikin.GlobalMembers.unpackLanguageOrRegion(new UInt16(mLanguage), ref buf, 'a', 'a');
	if (mScript != 0)
	{
	  buf = StringFunctions.ChangeCharacter(buf, i++, '-');
	  buf = StringFunctions.ChangeCharacter(buf, i++, (mScript >> 24) & 0xFFu);
	  buf = StringFunctions.ChangeCharacter(buf, i++, (mScript >> 16) & 0xFFu);
	  buf = StringFunctions.ChangeCharacter(buf, i++, (mScript >> 8) & 0xFFu);
	  buf = StringFunctions.ChangeCharacter(buf, i++, mScript & 0xFFu);
	}
	if (mRegion != GlobalMembers.INVALID_CODE)
	{
	  buf = StringFunctions.ChangeCharacter(buf, i++, '-');
	  i += minikin.GlobalMembers.unpackLanguageOrRegion(new UInt16(mRegion), ref buf + i, 'A', '0');
	}
	return (string)(buf, i);
  }

  // Calculates a matching score. This score represents how well the input
  // languages cover this language. The maximum score in the language list is
  // returned. 0 = no match, 1 = script match, 2 = script and primary language
  // match.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int calcScoreFor(const FontLanguages& supported) const
  public int calcScoreFor(FontLanguages supported)
  {
	bool languageScriptMatch = false;
	bool subtagMatch = false;
	bool scriptMatch = false;

	for (int i = 0; i < supported.size(); ++i)
	{
	  if (mEmojiStyle != EmojiStyle.EMSTYLE_EMPTY && mEmojiStyle == supported[i].mEmojiStyle)
	  {
		subtagMatch = true;
		if (mLanguage == supported[i].mLanguage)
		{
		  return 4;
		}
	  }
	  if (isEqualScript(supported[i]) || supportsScript(supported[i].mSubScriptBits, new byte(mSubScriptBits)))
	  {
		scriptMatch = true;
		if (mLanguage == supported[i].mLanguage)
		{
		  languageScriptMatch = true;
		}
	  }
	}

	if (supportsScript(supported.getUnionOfSubScriptBits(), new byte(mSubScriptBits)))
	{
	  scriptMatch = true;
	  if (mLanguage == supported[0].mLanguage && supported.isAllTheSameLanguage())
	  {
		return 3;
	  }
	}

	if (languageScriptMatch)
	{
	  return 3;
	}
	else if (subtagMatch)
	{
	  return 2;
	}
	else if (scriptMatch)
	{
	  return 1;
	}
	return 0;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UInt64 getIdentifier() const
  public UInt64 getIdentifier()
  {
	return ((UInt64)mLanguage << 49) | ((UInt64)mScript << 17) | ((UInt64)mRegion << 2) | (int)mEmojiStyle;
  }

//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
//  friend class FontLanguages; // for FontLanguages constructor

  // ISO 15924 compliant script code. The 4 chars script code are packed into a
  // 32 bit integer.
  private uint mScript = new uint();

  // ISO 639-1 or ISO 639-2 compliant language code.
  // The two- or three-letter language code is packed into a 15 bit integer.
  // mLanguage = 0 means the FontLanguage is unsupported.
  private UInt16 mLanguage = new UInt16();

  // ISO 3166-1 or UN M.49 compliant region code. The two-letter or three-digit
  // region code is packed into a 15 bit integer.
  private UInt16 mRegion = new UInt16();

  // The language to be passed HarfBuzz shaper.
  private hb_language_t mHbLanguage = new hb_language_t();

  // For faster comparing, use 7 bits for specific scripts.
  private const byte kBopomofoFlag = 1u;
  private byte kHanFlag = 1u << 1;
  private byte kHangulFlag = 1u << 2;
  private byte kHiraganaFlag = 1u << 3;
  private byte kKatakanaFlag = 1u << 4;
  private byte kSimplifiedChineseFlag = 1u << 5;
  private byte kTraditionalChineseFlag = 1u << 6;
  private byte mSubScriptBits = new byte();

  private EmojiStyle mEmojiStyle;


  // static
  private static byte scriptToSubScriptBits(uint script)
  {
	byte subScriptBits = 0u;
	switch (script)
	{
	  case (((uint)('B')) << 24 | ((uint)('o')) << 16 | ((uint)('p')) << 8 | ((uint)('o'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kBopomofoFlag;
		subScriptBits.CopyFrom(kBopomofoFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('a')) << 16 | ((uint)('n')) << 8 | ((uint)('g'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHangulFlag;
		subScriptBits.CopyFrom(kHangulFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('a')) << 16 | ((uint)('n')) << 8 | ((uint)('b'))):
		// Bopomofo is almost exclusively used in Taiwan.
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHanFlag | kBopomofoFlag;
		subScriptBits.CopyFrom(kHanFlag | kBopomofoFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('a')) << 16 | ((uint)('n')) << 8 | ((uint)('i'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHanFlag;
		subScriptBits.CopyFrom(kHanFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('a')) << 16 | ((uint)('n')) << 8 | ((uint)('s'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHanFlag | kSimplifiedChineseFlag;
		subScriptBits.CopyFrom(kHanFlag | kSimplifiedChineseFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('a')) << 16 | ((uint)('n')) << 8 | ((uint)('t'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHanFlag | kTraditionalChineseFlag;
		subScriptBits.CopyFrom(kHanFlag | kTraditionalChineseFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('i')) << 16 | ((uint)('r')) << 8 | ((uint)('a'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHiraganaFlag;
		subScriptBits.CopyFrom(kHiraganaFlag);
		break;
	  case (((uint)('H')) << 24 | ((uint)('r')) << 16 | ((uint)('k')) << 8 | ((uint)('t'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kKatakanaFlag | kHiraganaFlag;
		subScriptBits.CopyFrom(kKatakanaFlag | kHiraganaFlag);
		break;
	  case (((uint)('J')) << 24 | ((uint)('p')) << 16 | ((uint)('a')) << 8 | ((uint)('n'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHanFlag | kKatakanaFlag | kHiraganaFlag;
		subScriptBits.CopyFrom(kHanFlag | kKatakanaFlag | kHiraganaFlag);
		break;
	  case (((uint)('K')) << 24 | ((uint)('a')) << 16 | ((uint)('n')) << 8 | ((uint)('a'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kKatakanaFlag;
		subScriptBits.CopyFrom(kKatakanaFlag);
		break;
	  case (((uint)('K')) << 24 | ((uint)('o')) << 16 | ((uint)('r')) << 8 | ((uint)('e'))):
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: subScriptBits = kHanFlag | kHangulFlag;
		subScriptBits.CopyFrom(kHanFlag | kHangulFlag);
		break;
	}
	return subScriptBits;
  }


  // static
  private static FontLanguage.EmojiStyle resolveEmojiStyle(string buf, int length, uint script)
  {
	// First, lookup emoji subtag.
	// 10 is the length of "-u-em-text", which is the shortest emoji subtag,
	// unnecessary comparison can be avoided if total length is smaller than 10.
	const int kMinSubtagLength = 10;
	if (length >= kMinSubtagLength)
	{
	  const string kPrefix = "-u-em-";
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged:
	  char * pos = std::search(buf, buf + length, kPrefix, kPrefix.Substring(kPrefix.Length));
	  if (pos != buf + length)
	  { // found
		pos += kPrefix.Length;
		int remainingLength = length - (pos - buf);
		if (minikin.GlobalMembers.isEmojiSubtag(pos, remainingLength, "emoji", 5))
		{
		  return EmojiStyle.EMSTYLE_EMOJI;
		}
		else if (minikin.GlobalMembers.isEmojiSubtag(pos, remainingLength, "text", 4))
		{
		  return EmojiStyle.EMSTYLE_TEXT;
		}
		else if (minikin.GlobalMembers.isEmojiSubtag(pos, remainingLength, "default", 7))
		{
		  return EmojiStyle.EMSTYLE_DEFAULT;
		}
	  }
	}

	// If no emoji subtag was provided, resolve the emoji style from script code.
	if (script == (((uint)('Z')) << 24 | ((uint)('s')) << 16 | ((uint)('y')) << 8 | ((uint)('e'))))
	{
	  return EmojiStyle.EMSTYLE_EMOJI;
	}
	else if (script == (((uint)('Z')) << 24 | ((uint)('s')) << 16 | ((uint)('y')) << 8 | ((uint)('m'))))
	{
	  return EmojiStyle.EMSTYLE_TEXT;
	}

	return EmojiStyle.EMSTYLE_EMPTY;
  }

  // Returns true if the provide subscript bits has the requested subscript
  // bits. Note that this function returns false if the requested subscript bits
  // are empty.

  // static
  private static bool supportsScript(byte providedBits, byte requestedBits)
  {
	return requestedBits != 0 && (providedBits & requestedBits) == requestedBits;
  }
}

// An immutable list of languages.
public class FontLanguages
{
//C++ TO C# CONVERTER TODO TASK: 'rvalue references' have no equivalent in C#:
  public FontLanguages(List<FontLanguage>&& languages)
  {
	  this.mLanguages = std::move(languages);
	if (mLanguages.Count == 0)
	{
	  return;
	}

	FontLanguage lang = mLanguages[0];

	mIsAllTheSameLanguage = true;
	mUnionOfSubScriptBits = lang.mSubScriptBits;
	for (int i = 1; i < mLanguages.Count; ++i)
	{
	  mUnionOfSubScriptBits |= mLanguages[i].mSubScriptBits;
	  if (mIsAllTheSameLanguage && lang.mLanguage != mLanguages[i].mLanguage)
	  {
		mIsAllTheSameLanguage = false;
	  }
	}
  }
  public FontLanguages()
  {
	  this.mUnionOfSubScriptBits = 0;
	  this.mIsAllTheSameLanguage = false;
  }
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  FontLanguages(FontLanguages&&) = default;

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int size() const
  public int size()
  {
	  return mLanguages.Count;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool empty() const
  public bool empty()
  {
	  return mLanguages.Count == 0;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const FontLanguage& operator [](int n) const
  public FontLanguage this[int n]
  {
	  get
	  {
		  return mLanguages[n];
	  }
	  set
	  {
		  mLanguages[n] = value;
	  }
  }

//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
//  friend struct FontLanguage; // for calcScoreFor

  private List<FontLanguage> mLanguages = new List<FontLanguage>();
  private byte mUnionOfSubScriptBits = new byte();
  private bool mIsAllTheSameLanguage;

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte getUnionOfSubScriptBits() const
  private byte getUnionOfSubScriptBits()
  {
	  return mUnionOfSubScriptBits;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isAllTheSameLanguage() const
  private bool isAllTheSameLanguage()
  {
	  return mIsAllTheSameLanguage;
  }

  // Do not copy and assign.
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  FontLanguages(const FontLanguages&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  void operator =(const FontLanguages&) = delete;
}

} // namespace minikin




