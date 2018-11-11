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

/**
 * A wrapper around ICU's line break iterator, that gives customized line
 * break opportunities, as well as identifying words for the purpose of
 * hyphenation.
 */


//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/brkiter.h"
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

public class WordBreaker : System.IDisposable
{
  public void Dispose()
  {
	  finish();
  }

	public void setLocale(icu.Locale locale)
	{
	  UErrorCode status = U_ZERO_ERROR;
	  mBreakIterator.reset(icu.BreakIterator.createLineInstance(locale, status));
	  // TODO: handle failure status
	  if (mText != null)
	  {
		mBreakIterator.setText(mUText, status);
	  }
	  mIteratorWasReset = true;
	}

	public void setText(UInt16 data, int size)
	{
	  mText = data;
	  mTextSize = size;
	  mIteratorWasReset = false;
	  mLast = 0;
	  mCurrent = 0;
	  mScanOffset = 0;
	  mInEmailOrUrl = false;
	  UErrorCode status = U_ZERO_ERROR;
	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	  utext_openUChars(mUText, reinterpret_cast<const UChar>(data), size, status);
	  mBreakIterator.setText(mUText, status);
	  mBreakIterator.first();
	}

  // Advance iterator to next word break. Return offset, or -1 if EOT
	public uint next()
	{
	  mLast = mCurrent;
    
	  detectEmailOrUrl();
	  if (mInEmailOrUrl)
	  {
		mCurrent = findNextBreakInEmailOrUrl();
	  }
	  else
	  { // Business as usual
		mCurrent = (uint)iteratorNext();
	  }
	  return mCurrent;
	}

  // Current offset of iterator, equal to 0 at BOT or last return from next()
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: System.IntPtr current() const;
	public uint current()
	{
	  return mCurrent;
	}

  // After calling next(), wordStart() and wordEnd() are offsets defining the
  // previous word. If wordEnd <= wordStart, it's not a word for the purpose of
  // hyphenation.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: System.IntPtr wordStart() const;
	public uint wordStart()
	{
	  if (mInEmailOrUrl)
	  {
		return mLast;
	  }
	  uint result = mLast;
	  while (result < mCurrent)
	  {
		UChar32 c = new UChar32();
		uint ix = result;
		U16_NEXT(mText, ix, mCurrent, c);
		int lb = u_getIntPropertyValue(c, UCHAR_LINE_BREAK);
		// strip leading punctuation, defined as OP and QU line breaking classes,
		// see UAX #14
		if (!(lb == U_LB_OPEN_PUNCTUATION || lb == U_LB_QUOTATION))
		{
		  break;
		}
		result = ix;
	  }
	  return result;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: System.IntPtr wordEnd() const;
	public uint wordEnd()
	{
	  if (mInEmailOrUrl)
	  {
		return mLast;
	  }
	  uint result = mCurrent;
	  while (result > mLast)
	  {
		UChar32 c = new UChar32();
		uint ix = result;
		U16_PREV(mText, mLast, ix, c);
		int gc_mask = U_GET_GC_MASK(c);
		// strip trailing space and punctuation
		if ((gc_mask & (U_GC_ZS_MASK | U_GC_P_MASK)) == 0)
		{
		  break;
		}
		result = ix;
	  }
	  return result;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int breakBadness() const;
	public int breakBadness()
	{
	  return (mInEmailOrUrl && mCurrent < mScanOffset) ? 1 : 0;
	}

	public void finish()
	{
	  mText = null;
	  // Note: calling utext_close multiply is safe
	  utext_close(mUText);
	}

	public int iteratorNext()
	{
	  int result = new int();
	  do
	  {
		if (mIteratorWasReset)
		{
		  result = mBreakIterator.following(mCurrent);
		  mIteratorWasReset = false;
		}
		else
		{
		  result = mBreakIterator.next();
		}
	  } while (!(result == icu.BreakIterator.DONE || (int)result == mTextSize || isBreakValid(mText, mTextSize, result)));
	  return result;
	}
	public void detectEmailOrUrl()
	{
	  // scan forward from current ICU position for email address or URL
	  if (mLast >= mScanOffset)
	  {
		ScanState state = ScanState.START;
		int i = new int();
		for (i = mLast; i < mTextSize; i++)
		{
		  UInt16 c = mText[i];
		  // scan only ASCII characters, stop at space
		  if (!(' ' < c && c <= 0x007E))
		  {
			break;
		  }
		  if (state == ScanState.START && c == '@')
		  {
			state = ScanState.SAW_AT;
		  }
		  else if (state == ScanState.START && c == ':')
		  {
			state = ScanState.SAW_COLON;
		  }
		  else if (state == ScanState.SAW_COLON || state == ScanState.SAW_COLON_SLASH)
		  {
			if (c == '/')
			{
			  state = (ScanState)((int)state + 1); // next state adds a slash
			}
			else
			{
			  state = ScanState.START;
			}
		  }
		}
		if (state == ScanState.SAW_AT || state == ScanState.SAW_COLON_SLASH_SLASH)
		{
		  if (!mBreakIterator.isBoundary(i))
		  {
			// If there are combining marks or such at the end of the URL or the
			// email address, consider them a part of the URL or the email, and skip
			// to the next actual boundary.
			i = mBreakIterator.following(i);
		  }
		  mInEmailOrUrl = true;
		  mIteratorWasReset = true;
		}
		else
		{
		  mInEmailOrUrl = false;
		}
		mScanOffset = i;
	  }
	}
	public uint findNextBreakInEmailOrUrl()
	{
            // special rules for email addresses and URL's as per Chicago Manual of Style
            // (16th ed.)

            UInt16 lastChar = mText[mLast];
	  uint i;
	  for (i = mLast + 1; i < mScanOffset; i++)
	  {
		if (breakAfter(new UInt16(lastChar)))
		{
		  break;
		}
		// break after double slash
		if (lastChar == '/' && i >= mLast + 2 && mText[i - 2] == '/')
		{
		  break;
		}
		UInt16 thisChar = mText[i];
		// never break after hyphen
		if (lastChar != '-')
		{
		  if (breakBefore(new UInt16(thisChar)))
		  {
			break;
		  }
		  // break before single slash
		  if (thisChar == '/' && lastChar != '/' && !(i + 1 < mScanOffset && mText[i + 1] == '/'))
		  {
			break;
		  }
		}
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: lastChar = thisChar;
		lastChar.CopyFrom(thisChar);
	  }
	  return i;
	}

  private std::unique_ptr<icu.BreakIterator> mBreakIterator = new std::unique_ptr<icu.BreakIterator>();
  private UText mUText = UTEXT_INITIALIZER;
  private UInt16[] mText = null;
  private int mTextSize = new int();
  private IntPtr mLast;
  private IntPtr mCurrent;
  private bool mIteratorWasReset;

  // state for the email address / url detector
  private IntPtr mScanOffset;
  private bool mInEmailOrUrl;
}

} // namespace minikin

