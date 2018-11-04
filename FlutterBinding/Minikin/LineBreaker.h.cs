using System;
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

/**
 * A module for breaking paragraphs into lines, supporting high quality
 * hyphenation and justification.
 */



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
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/brkiter.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "unicode/locid.h"

namespace minikin
{

public enum BreakStrategy
{
  kBreakStrategy_Greedy = 0,
  kBreakStrategy_HighQuality = 1,
  kBreakStrategy_Balanced = 2
}

public enum HyphenationFrequency
{
  kHyphenationFrequency_None = 0,
  kHyphenationFrequency_Normal = 1,
  kHyphenationFrequency_Full = 2
}

// TODO: want to generalize to be able to handle array of line widths
public class LineWidths
{
  public void setWidths(float firstWidth, int firstWidthLineCount, float restWidth)
  {
	mFirstWidth = firstWidth;
	mFirstWidthLineCount = firstWidthLineCount;
	mRestWidth = restWidth;
  }
  public void setIndents(List<float> indents)
  {
	  mIndents = new List<float>(indents);
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isConstant() const
  public bool isConstant()
  {
	// technically mFirstWidthLineCount == 0 would count too, but doesn't
	// actually happen
	return mRestWidth == mFirstWidth && mIndents.Count == 0;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getLineWidth(int line) const
  public float getLineWidth(int line)
  {
	float width = (line < mFirstWidthLineCount) ? mFirstWidth : mRestWidth;
	if (mIndents.Count > 0)
	{
	  if ((int)line < mIndents.Count)
	  {
		width -= mIndents[line];
	  }
	  else
	  {
		width -= mIndents[mIndents.Count - 1];
	  }
	}
	return width;
  }
  public void clear()
  {
	  mIndents.Clear();
  }

  private float mFirstWidth;
  private int mFirstWidthLineCount;
  private float mRestWidth;
  private List<float> mIndents = new List<float>();
}

public class TabStops
{
  public void set(int stops, int nStops, int tabWidth)
  {
	if (stops != null)
	{
	  mStops.assign(stops, stops + nStops);
	}
	else
	{
	  mStops.Clear();
	}
	mTabWidth = tabWidth;
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float nextTab(float widthSoFar) const
  public float nextTab(float widthSoFar)
  {
	for (int i = 0; i < mStops.Count; i++)
	{
	  if (mStops[i] > widthSoFar)
	  {
		return mStops[i];
	  }
	}
	return Math.Floor(widthSoFar / mTabWidth + 1) * mTabWidth;
  }

  private List<int> mStops = new List<int>();
  private int mTabWidth;
}

public class LineBreaker
{
  public const int kTab_Shift = 29; // keep synchronized with TAB_MASK in StaticLayout.java

  // Note: Locale persists across multiple invocations (it is not cleaned up by
  // finish()), explicitly to avoid the cost of creating ICU BreakIterator
  // objects. It should always be set on the first invocation, but callers are
  // encouraged not to call again unless locale has actually changed. That logic
  // could be here but it's better for performance that it's upstream because of
  // the cost of constructing and comparing the ICU Locale object.
  // Note: caller is responsible for managing lifetime of hyphenator
	public void setLocale(icu.Locale locale, Hyphenator hyphenator)
	{
	  mWordBreaker.setLocale(locale);
	  mLocale = locale;
	  mHyphenator = hyphenator;
	}

  public void resize(int size)
  {
	mTextBuf.Resize(size);
	mCharWidths.Resize(size);
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int size() const
  public int size()
  {
	  return mTextBuf.Count;
  }

  public UInt16 buffer()
  {
	  return mTextBuf.data();
  }

//C++ TO C# CONVERTER WARNING: C# has no equivalent to methods returning pointers to value types:
//ORIGINAL LINE: float* charWidths()
  public float charWidths()
  {
	  return mCharWidths.data();
  }

  // set text to current contents of buffer
	public void setText()
	{
	  mWordBreaker.setText(mTextBuf.data(), mTextBuf.size());
    
	  // handle initial break here because addStyleRun may never be called
	  mWordBreaker.next();
	  mCandidates.clear();
	  Candidate cand = new Candidate(0, 0, 0.0, 0.0, 0.0, 0.0, 0, 0, 0, HyphenationType.DONT_BREAK);
	  mCandidates.push_back(cand);
    
	  // reset greedy breaker state
	  mBreaks.clear();
	  mWidths.clear();
	  mFlags.clear();
	  mLastBreak = 0;
	  mBestBreak = 0;
	  mBestScore = SCORE_INFTY;
	  mPreBreak = 0;
	  mLastHyphenation = HyphenEdit.NO_EDIT;
	  mFirstTabIndex = INT_MAX;
	  mSpaceCount = 0;
	}

	public void setLineWidths(float firstWidth, int firstWidthLineCount, float restWidth)
	{
	  mLineWidths.setWidths(firstWidth, firstWidthLineCount, restWidth);
	}

	public void setIndents(List<float> indents)
	{
	  mLineWidths.setIndents(indents);
	}

  public void setTabStops(int stops, int nStops, int tabWidth)
  {
	mTabStops.set(stops, nStops, tabWidth);
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: BreakStrategy getStrategy() const
  public BreakStrategy getStrategy()
  {
	  return mStrategy;
  }

  public void setStrategy(BreakStrategy strategy)
  {
	  mStrategy = strategy;
  }

  public void setJustified(bool justified)
  {
	  mJustified = justified;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: HyphenationFrequency getHyphenationFrequency() const
  public HyphenationFrequency getHyphenationFrequency()
  {
	return mHyphenationFrequency;
  }

  public void setHyphenationFrequency(HyphenationFrequency frequency)
  {
	mHyphenationFrequency = frequency;
  }

  // TODO: this class is actually fairly close to being general and not tied to
  // using Minikin to do the shaping of the strings. The main thing that would
  // need to be changed is having some kind of callback (or virtual class, or
  // maybe even template), which could easily be instantiated with Minikin's
  // Layout. Future work for when needed.
	public float addStyleRun(MinikinPaint paint, FontCollection typeface, FontStyle style, int start, int end, bool isRtl)
	{
	  float width = 0.0f;
	  int bidiFlags = isRtl ? kBidi_Force_RTL : kBidi_Force_LTR;
    
	  float hyphenPenalty = 0.0F;
	  if (paint != null)
	  {
		width = Layout.measureText(mTextBuf.data(), start, end - start, mTextBuf.size(), bidiFlags, style, paint, typeface, mCharWidths.data() + start);
    
		// a heuristic that seems to perform well
		hyphenPenalty = 0.5 * paint.size * paint.scaleX * mLineWidths.getLineWidth(0);
		if (mHyphenationFrequency == kHyphenationFrequency_Normal)
		{
		  hyphenPenalty *= 4.0; // TODO: Replace with a better value after some testing
		}
    
		if (mJustified)
		{
		  // Make hyphenation more aggressive for fully justified text (so that
		  // "normal" in justified mode is the same as "full" in ragged-right).
		  hyphenPenalty *= 0.25;
		}
		else
		{
		  // Line penalty is zero for justified text.
		  mLinePenalty = Math.Max(mLinePenalty, hyphenPenalty * LINE_PENALTY_MULTIPLIER);
		}
	  }
    
	  int current = (int)mWordBreaker.current();
	  int afterWord = start;
	  int lastBreak = start;
	  ParaWidth lastBreakWidth = mWidth;
	  ParaWidth postBreak = mWidth;
	  int postSpaceCount = mSpaceCount;
	  for (int i = start; i < end; i++)
	  {
		UInt16 c = mTextBuf[i];
		if (c == CHAR_TAB)
		{
		  mWidth = mPreBreak + mTabStops.nextTab(mWidth - mPreBreak);
		  if (mFirstTabIndex == INT_MAX)
		  {
			mFirstTabIndex = (int)i;
		  }
		  // fall back to greedy; other modes don't know how to deal with tabs
		  mStrategy = kBreakStrategy_Greedy;
		}
		else
		{
		  if (isWordSpace(new UInt16(c)))
		  {
			mSpaceCount += 1;
		  }
		  mWidth += mCharWidths[i];
		  if (!isLineEndSpace(new UInt16(c)))
		  {
			postBreak = mWidth;
			postSpaceCount = mSpaceCount;
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: afterWord = i + 1;
			afterWord.CopyFrom(i + 1);
		  }
		}
		if (i + 1 == current != null)
		{
		  int wordStart = mWordBreaker.wordStart();
		  int wordEnd = mWordBreaker.wordEnd();
		  if (paint != null && mHyphenator != null && mHyphenationFrequency != kHyphenationFrequency_None && wordStart >= start != null && wordEnd > wordStart && wordEnd - wordStart <= LONGEST_HYPHENATED_WORD)
		  {
			mHyphenator.hyphenate(mHyphBuf, mTextBuf[wordStart], wordEnd - wordStart, mLocale);
	#if VERBOSE_DEBUG
			string hyphenatedString;
			for (int j = wordStart; j < wordEnd; j++)
			{
			  if (mHyphBuf[j - wordStart] == HyphenationType.BREAK_AND_INSERT_HYPHEN)
			  {
				hyphenatedString.push_back('-');
			  }
			  // Note: only works with ASCII, should do UTF-8 conversion here
			  hyphenatedString.push_back(buffer()[j]);
			}
			ALOGD("hyphenated string: %s", hyphenatedString);
	#endif
    
			// measure hyphenated substrings
			for (int j = wordStart; j < wordEnd; j++)
			{
			  HyphenationType hyph = mHyphBuf[j - wordStart];
			  if (hyph != HyphenationType.DONT_BREAK)
			  {
				paint.hyphenEdit = HyphenEdit.editForThisLine(hyph);
				float firstPartWidth = Layout.measureText(mTextBuf.data(), lastBreak, j - lastBreak, mTextBuf.size(), bidiFlags, style, paint, typeface, null);
				ParaWidth hyphPostBreak = lastBreakWidth + firstPartWidth;
    
				paint.hyphenEdit = HyphenEdit.editForNextLine(hyph);
				float secondPartWidth = Layout.measureText(mTextBuf.data(), j, afterWord - j, mTextBuf.size(), bidiFlags, style, paint, typeface, null);
				ParaWidth hyphPreBreak = postBreak - secondPartWidth;
    
				addWordBreak(j, hyphPreBreak, hyphPostBreak, postSpaceCount, postSpaceCount, hyphenPenalty, hyph);
    
				paint.hyphenEdit = HyphenEdit.NO_EDIT;
			  }
			}
		  }
    
		  // Skip break for zero-width characters inside replacement span
		  if (paint != null || current == end || mCharWidths[current] > 0)
		  {
			float penalty = hyphenPenalty * mWordBreaker.breakBadness();
			addWordBreak(current, mWidth, postBreak, mSpaceCount, postSpaceCount, penalty, HyphenationType.DONT_BREAK);
		  }
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: lastBreak = current;
		  lastBreak.CopyFrom(current);
		  lastBreakWidth = mWidth;
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: current = (int)mWordBreaker.next();
		  current.CopyFrom((int)mWordBreaker.next());
		}
	  }
    
	  return width;
	}

	public void addReplacement(int start, int end, float width)
	{
	  mCharWidths[start] = width;
	  std::fill(mCharWidths[start + 1], mCharWidths[end], 0.0f);
	  addStyleRun(null, null, FontStyle(), start, end, false);
	}

	public int computeBreaks()
	{
	  if (mStrategy == kBreakStrategy_Greedy)
	  {
		computeBreaksGreedy();
	  }
	  else
	  {
		computeBreaksOptimal(mLineWidths.isConstant());
	  }
	  return mBreaks.size();
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const int* getBreaks() const
//C++ TO C# CONVERTER WARNING: C# has no equivalent to methods returning pointers to value types:
  public int getBreaks()
  {
	  return mBreaks.data();
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const float* getWidths() const
//C++ TO C# CONVERTER WARNING: C# has no equivalent to methods returning pointers to value types:
  public float getWidths()
  {
	  return mWidths.data();
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const int* getFlags() const
//C++ TO C# CONVERTER WARNING: C# has no equivalent to methods returning pointers to value types:
  public int getFlags()
  {
	  return mFlags.data();
  }

	public void finish()
	{
	  mWordBreaker.finish();
	  mWidth = 0;
	  mLineWidths.clear();
	  mCandidates.clear();
	  mBreaks.clear();
	  mWidths.clear();
	  mFlags.clear();
	  if (mTextBuf.size() > MAX_TEXT_BUF_RETAIN)
	  {
		mTextBuf.clear();
		mTextBuf.shrink_to_fit();
		mCharWidths.clear();
		mCharWidths.shrink_to_fit();
		mHyphBuf.clear();
		mHyphBuf.shrink_to_fit();
		mCandidates.shrink_to_fit();
		mBreaks.shrink_to_fit();
		mWidths.shrink_to_fit();
		mFlags.shrink_to_fit();
	  }
	  mStrategy = kBreakStrategy_Greedy;
	  mHyphenationFrequency = kHyphenationFrequency_Normal;
	  mLinePenalty = 0.0f;
	  mJustified = false;
	}

  // ParaWidth is used to hold cumulative width from beginning of paragraph.
  // Note that for very large paragraphs, accuracy could degrade using only
  // 32-bit float. Note however that float is used extensively on the Java side
  // for this. This is a typedef so that we can easily change it based on
  // performance/accuracy tradeoff.

  // A single candidate break
  private class Candidate
  {
	public int offset = new int(); // offset to text buffer, in code units
	public int prev = new int(); // index to previous break
	public ParaWidth preBreak = new ParaWidth(); // width of text until this point, if we decide to not
						  // break here
	public ParaWidth postBreak = new ParaWidth(); // width of text until this point, if we decide to
						  // break here
	public float penalty; // penalty of this break (for example, hyphen penalty)
	public float score; // best score found for this break
	public int lineNumber = new int(); // only updated for non-constant line widths
	public int preSpaceCount = new int(); // preceding space count before breaking
	public int postSpaceCount = new int(); // preceding space count after breaking
	public HyphenationType hyphenType;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float currentLineWidth() const;
	public float currentLineWidth()
	{
	  return mLineWidths.getLineWidth(mBreaks.size());
	}

	public void addWordBreak(int offset, ParaWidth preBreak, ParaWidth postBreak, int preSpaceCount, int postSpaceCount, float penalty, HyphenationType hyph)
	{
	  Candidate cand = new Candidate();
	  ParaWidth width = mCandidates.back().preBreak;
	  if (postBreak - width > currentLineWidth() != null)
	  {
		// Add desperate breaks.
		// Note: these breaks are based on the shaping of the (non-broken) original
		// text; they are imprecise especially in the presence of kerning,
		// ligatures, and Arabic shaping.
		int i = mCandidates.back().offset;
		width += mCharWidths[i++];
		for (; i < offset; i++)
		{
		  float w = mCharWidths[i];
		  if (w > 0F)
		  {
			cand.offset = i;
			cand.preBreak = width;
			cand.postBreak = width;
			// postSpaceCount doesn't include trailing spaces
			cand.preSpaceCount = postSpaceCount;
			cand.postSpaceCount = postSpaceCount;
			cand.penalty = SCORE_DESPERATE;
			cand.hyphenType = HyphenationType.BREAK_AND_DONT_INSERT_HYPHEN;
	#if VERBOSE_DEBUG
			ALOGD("desperate cand: %zd %g:%g", mCandidates.size(), cand.postBreak, cand.preBreak);
	#endif
			addCandidate(cand);
			width += w;
		  }
		}
	  }
    
	  cand.offset = offset;
	  cand.preBreak = preBreak;
	  cand.postBreak = postBreak;
	  cand.penalty = penalty;
	  cand.preSpaceCount = preSpaceCount;
	  cand.postSpaceCount = postSpaceCount;
	  cand.hyphenType = hyph;
	#if VERBOSE_DEBUG
	  ALOGD("cand: %zd %g:%g", mCandidates.size(), cand.postBreak, cand.preBreak);
	#endif
	  addCandidate(cand);
	}

	public void addCandidate(Candidate cand)
	{
	  int candIndex = mCandidates.size();
	  mCandidates.push_back(cand);
    
	  // mLastBreak is the index of the last line break we decided to do in
	  // mCandidates, and mPreBreak is its preBreak value. mBestBreak is the index
	  // of the best line breaking candidate we have found since then, and
	  // mBestScore is its penalty.
	  if (cand.postBreak - mPreBreak > currentLineWidth())
	  {
		// This break would create an overfull line, pick the best break and break
		// there (greedy)
		if (mBestBreak == mLastBreak)
		{
		  // No good break has been found since last break. Break here.
		  mBestBreak = candIndex;
		}
		pushGreedyBreak();
	  }
    
	  while (mLastBreak != candIndex && cand.postBreak - mPreBreak > currentLineWidth())
	  {
		// We should rarely come here. But if we are here, we have broken the line,
		// but the remaining part still doesn't fit. We now need to break at the
		// second best place after the last break, but we have not kept that
		// information, so we need to go back and find it.
		//
		// In some really rare cases, postBreak - preBreak of a candidate itself may
		// be over the current line width. We protect ourselves against an infinite
		// loop in that case by checking that we have not broken the line at this
		// candidate already.
		for (int i = mLastBreak + 1; i < candIndex; i++)
		{
		  float penalty = mCandidates[i].penalty;
		  if (penalty <= mBestScore)
		  {
			mBestBreak = i;
			mBestScore = penalty;
		  }
		}
		if (mBestBreak == mLastBreak)
		{
		  // We didn't find anything good. Break here.
		  mBestBreak = candIndex;
		}
		pushGreedyBreak();
	  }
    
	  if (cand.penalty <= mBestScore)
	  {
		mBestBreak = candIndex;
		mBestScore = cand.penalty;
	  }
	}
	public void pushGreedyBreak()
	{
	  Candidate bestCandidate = mCandidates[mBestBreak];
	  pushBreak(bestCandidate.offset, bestCandidate.postBreak - mPreBreak, mLastHyphenation | HyphenEdit.editForThisLine(bestCandidate.hyphenType));
	  mBestScore = SCORE_INFTY;
	#if VERBOSE_DEBUG
	  ALOGD("break: %d %g", mBreaks.back(), mWidths.back());
	#endif
	  mLastBreak = mBestBreak;
	  mPreBreak = bestCandidate.preBreak;
	  mLastHyphenation = HyphenEdit.editForNextLine(bestCandidate.hyphenType);
	}

  // push an actual break to the output. Takes care of setting flags for tab
	public void pushBreak(int offset, float width, byte hyphenEdit)
	{
	  mBreaks.push_back(offset);
	  mWidths.push_back(width);
	  int flags = (mFirstTabIndex < mBreaks.back()) << kTab_Shift;
	  flags |= hyphenEdit;
	  mFlags.push_back(flags);
	  mFirstTabIndex = INT_MAX;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getSpaceWidth() const;
	public float getSpaceWidth()
	{
	  for (int i = 0; i < mTextBuf.size(); i++)
	  {
		if (isWordSpace(mTextBuf[i]))
		{
		  return mCharWidths[i];
		}
	  }
	  return 0.0f;
	}

	public void computeBreaksGreedy()
	{
	  // All breaks but the last have been added in addCandidate already.
	  int nCand = mCandidates.size();
	  if (nCand > 0 && (nCand == 1 || mLastBreak != nCand - 1))
	  {
		pushBreak(mCandidates[nCand - 1].offset, mCandidates[nCand - 1].postBreak - mPreBreak, mLastHyphenation);
		// don't need to update mBestScore, because we're done
	#if VERBOSE_DEBUG
		ALOGD("final break: %d %g", mBreaks.back(), mWidths.back());
	#endif
	  }
	}

	public void computeBreaksOptimal(bool isRectangle)
	{
	  int active = 0;
	  int nCand = mCandidates.size();
	  float width = mLineWidths.getLineWidth(0);
	  float shortLineFactor = mJustified ? 0.75f : 0.5f;
	  float maxShrink = mJustified ? SHRINKABILITY * getSpaceWidth() : 0.0f;
    
	  // "i" iterates through candidates for the end of the line.
	  for (int i = 1; i < nCand; i++)
	  {
		bool atEnd = i == nCand - 1;
		float best = SCORE_INFTY;
		int bestPrev = 0;
		int lineNumberLast = 0;
    
		if (!isRectangle)
		{
		  int lineNumberLast = mCandidates[active].lineNumber;
		  width = mLineWidths.getLineWidth(lineNumberLast);
		}
		ParaWidth leftEdge = mCandidates[i].postBreak - width;
		float bestHope = 0F;
    
		// "j" iterates through candidates for the beginning of the line.
		for (int j = active; j < i; j++)
		{
		  if (!isRectangle)
		  {
			int lineNumber = mCandidates[j].lineNumber;
			if (lineNumber != lineNumberLast)
			{
			  float widthNew = mLineWidths.getLineWidth(lineNumber);
			  if (widthNew != width)
			  {
				leftEdge = mCandidates[i].postBreak - width;
				bestHope = 0F;
				width = widthNew;
			  }
			  lineNumberLast = lineNumber;
			}
		  }
		  float jScore = mCandidates[j].score;
		  if (jScore + bestHope >= best)
		  {
			continue;
		  }
		  float delta = mCandidates[j].preBreak - leftEdge;
    
		  // compute width score for line
    
		  // Note: the "bestHope" optimization makes the assumption that, when delta
		  // is non-negative, widthScore will increase monotonically as successive
		  // candidate breaks are considered.
		  float widthScore = 0.0f;
		  float additionalPenalty = 0.0f;
		  if ((atEnd || !mJustified) && delta < 0F)
		  {
			widthScore = SCORE_OVERFULL;
		  }
		  else if (atEnd && mStrategy != kBreakStrategy_Balanced)
		  {
			// increase penalty for hyphen on last line
			additionalPenalty = LAST_LINE_PENALTY_MULTIPLIER * mCandidates[j].penalty;
			// Penalize very short (< 1 - shortLineFactor of total width) lines.
			float underfill = delta - shortLineFactor * width;
			widthScore = underfill > 0F ? underfill * underfill : 0;
		  }
		  else
		  {
			widthScore = delta * delta;
			if (delta < 0F)
			{
			  if (-delta < maxShrink * (mCandidates[i].postSpaceCount - mCandidates[j].preSpaceCount))
			  {
				widthScore *= SHRINK_PENALTY_MULTIPLIER;
			  }
			  else
			  {
				widthScore = SCORE_OVERFULL;
			  }
			}
		  }
    
		  if (delta < 0F)
		  {
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: active = j + 1;
			active.CopyFrom(j + 1);
		  }
		  else
		  {
			bestHope = widthScore;
		  }
    
		  float score = jScore + widthScore + additionalPenalty;
		  if (score <= best)
		  {
			best = score;
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: bestPrev = j;
			bestPrev.CopyFrom(j);
		  }
		}
		mCandidates[i].score = best + mCandidates[i].penalty + mLinePenalty;
		mCandidates[i].prev = bestPrev;
		mCandidates[i].lineNumber = mCandidates[bestPrev].lineNumber + 1;
	#if VERBOSE_DEBUG
		ALOGD("break %zd: score=%g, prev=%zd", i, mCandidates[i].score, mCandidates[i].prev);
	#endif
	  }
	  finishBreaksOptimal();
	}

	public void finishBreaksOptimal()
	{
	  // clear existing greedy break result
	  mBreaks.clear();
	  mWidths.clear();
	  mFlags.clear();
	  int nCand = mCandidates.size();
	  int prev = new int();
	  for (int i = nCand - 1; i > 0; i = prev)
	  {
		prev = mCandidates[i].prev;
		mBreaks.push_back(mCandidates[i].offset);
		mWidths.push_back(mCandidates[i].postBreak - mCandidates[prev].preBreak);
		int flags = HyphenEdit.editForThisLine(mCandidates[i].hyphenType);
		if (prev > 0)
		{
		  flags |= HyphenEdit.editForNextLine(mCandidates[prev].hyphenType);
		}
		mFlags.push_back(flags);
	  }
	  std::reverse(mBreaks.begin(), mBreaks.end());
	  std::reverse(mWidths.begin(), mWidths.end());
	  std::reverse(mFlags.begin(), mFlags.end());
	}

  private WordBreaker mWordBreaker = new WordBreaker();
  private icu.Locale mLocale = new icu.Locale();
  private List<UInt16> mTextBuf = new List<UInt16>();
  private List<float> mCharWidths = new List<float>();

  private Hyphenator mHyphenator;
  private List<HyphenationType> mHyphBuf = new List<HyphenationType>();

  // layout parameters
  private BreakStrategy mStrategy = BreakStrategy.kBreakStrategy_Greedy;
  private HyphenationFrequency mHyphenationFrequency = HyphenationFrequency.kHyphenationFrequency_Normal;
  private bool mJustified;
  private LineWidths mLineWidths = new LineWidths();
  private TabStops mTabStops = new TabStops();

  // result of line breaking
  private List<int> mBreaks = new List<int>();
  private List<float> mWidths = new List<float>();
  private List<int> mFlags = new List<int>();

  private double mWidth = 0;
  private List<Candidate> mCandidates = new List<Candidate>();
  private float mLinePenalty = 0.0f;

  // the following are state for greedy breaker (updated while adding style
  // runs)
  private int mLastBreak = new int();
  private int mBestBreak = new int();
  private float mBestScore;
  private double mPreBreak; // prebreak of last break
  private uint mLastHyphenation = new uint(); // hyphen edit of last break kept for next line
  private int mFirstTabIndex;
  private int mSpaceCount = new int();
}

} // namespace minikin

