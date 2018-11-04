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






using System;
using System.Collections.Generic;

namespace minikin
{

public class LayoutContext
{
  public MinikinPaint paint = new MinikinPaint();
  public FontStyle style = new FontStyle();
  public List<HarfBuzzSharp.Font> hbFonts = new List<HarfBuzzSharp.Font>(); // parallel to mFaces

  public void clearHbFonts()
  {
	for (int i = 0; i < hbFonts.Count; i++)
	{
	  hb_font_set_funcs(hbFonts[i], null, null, null);
	  hb_font_destroy(hbFonts[i]);
	}
	hbFonts.Clear();
  }
}

// Layout cache datatypes

public class LayoutCacheKey
{
  public LayoutCacheKey(FontCollection collection, MinikinPaint paint, FontStyle style, UInt16 chars, int start, int count, int nchars, bool dir)
  {
	  this.mChars = chars;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mNchars = nchars;
	  this.mNchars.CopyFrom(nchars);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mStart = start;
	  this.mStart.CopyFrom(start);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mCount = count;
	  this.mCount.CopyFrom(count);
	  this.mId = collection.getId();
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mStyle = style;
	  this.mStyle.CopyFrom(style);
	  this.mSize = paint.size;
	  this.mScaleX = paint.scaleX;
	  this.mSkewX = paint.skewX;
	  this.mLetterSpacing = paint.letterSpacing;
	  this.mPaintFlags = paint.paintFlags;
	  this.mHyphenEdit = paint.hyphenEdit;
	  this.mIsRtl = dir;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mHash = computeHash();
	  this.mHash.CopyFrom(computeHash());
  }
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const LayoutCacheKey& other) const
  public static bool operator == (LayoutCacheKey ImpliedObject, LayoutCacheKey other)
  {
//C++ TO C# CONVERTER TODO TASK: The memory management function 'memcmp' has no equivalent in C#:
	return ImpliedObject.mId == other.mId && ImpliedObject.mStart == other.mStart && ImpliedObject.mCount == other.mCount && ImpliedObject.mStyle == other.mStyle && ImpliedObject.mSize == other.mSize && ImpliedObject.mScaleX == other.mScaleX && ImpliedObject.mSkewX == other.mSkewX && ImpliedObject.mLetterSpacing == other.mLetterSpacing && ImpliedObject.mPaintFlags == other.mPaintFlags && ImpliedObject.mHyphenEdit == other.mHyphenEdit && ImpliedObject.mIsRtl == other.mIsRtl && ImpliedObject.mNchars == other.mNchars && !memcmp(ImpliedObject.mChars, other.mChars, ImpliedObject.mNchars * sizeof(UInt16));
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: android::hash_t hash() const
  public android.hash_t hash()
  {
	  return mHash;
  }

  public void copyText()
  {
	UInt16[] charsCopy = Arrays.InitializeWithDefaultInstances<UInt16>(mNchars);
//C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
	memcpy(charsCopy, mChars, mNchars * sizeof(UInt16));
	mChars = charsCopy;
  }
  public void freeText()
  {
	mChars = null;
	mChars = null;
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void doLayout(Layout* layout, LayoutContext* ctx, const FontCollection*& collection) const
  public void doLayout(Layout layout, LayoutContext ctx, FontCollection collection)
  {
	layout.mAdvances.resize(mCount, 0);
	ctx.clearHbFonts();
	layout.doLayoutRun(mChars, mStart, mCount, mNchars, mIsRtl, ctx, collection);
  }

  private readonly UInt16 mChars;
  private int mNchars = new int();
  private int mStart = new int();
  private int mCount = new int();
  private uint mId = new uint(); // for the font collection
  private FontStyle mStyle = new FontStyle();
  private float mSize;
  private float mScaleX;
  private float mSkewX;
  private float mLetterSpacing;
  private int mPaintFlags = new int();
  private HyphenEdit mHyphenEdit = new HyphenEdit();
  private bool mIsRtl;
  // Note: any fields added to MinikinPaint must also be reflected here.
  // TODO: language matching (possibly integrate into style)
  private android.hash_t mHash = new android.hash_t();

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: android::hash_t computeHash() const
  private android.hash_t computeHash()
  {
	uint hash = android.JenkinsHashMix(0, mId);
	hash = android.JenkinsHashMix(hash, mStart);
	hash = android.JenkinsHashMix(hash, mCount);
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mStyle));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mSize));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mScaleX));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mSkewX));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mLetterSpacing));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mPaintFlags));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mHyphenEdit.getHyphen()));
	hash = android.JenkinsHashMix(hash, minikin.GlobalMembers.hash_type(mIsRtl));
	hash = android.JenkinsHashMixShorts(hash, mChars, mNchars);
	return android.JenkinsHashWhiten(hash);
  }
}

//C++ TO C# CONVERTER TODO TASK: C# has no concept of 'private' inheritance:
//ORIGINAL LINE: class LayoutCache : private android::OnEntryRemoved<LayoutCacheKey, Layout*>
public class LayoutCache : android.OnEntryRemoved<LayoutCacheKey, Layout*>
{
  public LayoutCache()
  {
	  this.mCache = kMaxEntries;
	mCache.setOnEntryRemovedListener(this.functorMethod);
  }

  public void clear()
  {
	  mCache.clear();
  }

  public Layout get(LayoutCacheKey key, LayoutContext ctx, FontCollection collection)
  {
	Layout layout = mCache.get(key);
	if (layout == null)
	{
	  key.copyText();
	  layout = new Layout();
	  key.doLayout(layout, ctx, collection);
	  mCache.put(key, layout);
	}
	return layout;
  }

  // callback for OnEntryRemoved
  private static void functorMethod(LayoutCacheKey key, ref Layout value)
  {
	key.freeText();
	value = null;
  }

  private android.LruCache<LayoutCacheKey, Layout> mCache = new android.LruCache<LayoutCacheKey, Layout>();

  // static const int kMaxEntries = LruCache<LayoutCacheKey,
  // Layout*>::kUnlimitedCapacity;

  // TODO: eviction based on memory footprint; for now, we just use a constant
  // number of strings
  private const int kMaxEntries = 5000;
}

public class LayoutEngine
{
  public LayoutEngine()
  {
	unicodeFunctions = hb_unicode_funcs_create(hb_icu_get_unicode_funcs());
	/* Disable the function used for compatibility decomposition */
	hb_unicode_funcs_set_decompose_compatibility_func(unicodeFunctions, minikin.GlobalMembers.disabledDecomposeCompatibility, null, null);
	hbBuffer = hb_buffer_create();
	hb_buffer_set_unicode_funcs(hbBuffer, unicodeFunctions);
  }

  public hb_buffer_t hbBuffer;
  public hb_unicode_funcs_t unicodeFunctions;
  public LayoutCache layoutCache = new LayoutCache();

//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
  private static LayoutEngine getInstance_instance = new LayoutEngine();
  public static LayoutEngine getInstance()
  {
//C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//	static LayoutEngine* instance = new LayoutEngine();
	return getInstance_instance;
  }
}



//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void Layout::dump() const


public class BidiText : System.IDisposable
{
  public class Iter
  {
	public class RunInfo
	{
	  public int mRunStart = new int();
	  public int mRunLength = new int();
	  public bool mIsRtl;
	}

	public Iter(UBiDi bidi, int start, int end, int runIndex, int runCount, bool isRtl)
	{
		this.mBidi = bidi;
		this.mIsEnd = runIndex == runCount;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mNextRunIndex = runIndex;
		this.mNextRunIndex.CopyFrom(runIndex);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mRunCount = runCount;
		this.mRunCount.CopyFrom(runCount);
		this.mStart = start;
		this.mEnd = end;
		this.mRunInfo = new minikin.BidiText.Iter.RunInfo();
	  if (mRunCount == 1)
	  {
		mRunInfo.mRunStart = start;
		mRunInfo.mRunLength = end - start;
		mRunInfo.mIsRtl = isRtl;
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: mNextRunIndex = mRunCount;
		mNextRunIndex.CopyFrom(mRunCount);
		return;
	  }
	  updateRunInfo();
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(const Iter& other) const
	public static bool operator != (Iter ImpliedObject, Iter other)
	{
	  return ImpliedObject.mIsEnd != other.mIsEnd || ImpliedObject.mNextRunIndex != other.mNextRunIndex || ImpliedObject.mBidi != other.mBidi;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const RunInfo& operator *() const
	public RunInfo Indirection()
	{
		return mRunInfo;
	}

	public static Iter operator ++(Iter ImpliedObject)
	{
	  ImpliedObject.updateRunInfo();
	  return *ImpliedObject;
	}

	private readonly UBiDi mBidi;
	private bool mIsEnd;
	private int mNextRunIndex = new int();
	private readonly int mRunCount = new int();
	private readonly int mStart = new int();
	private readonly int mEnd = new int();
	private RunInfo mRunInfo = new RunInfo();

	private void updateRunInfo()
	{
	  if (mNextRunIndex == mRunCount)
	  {
		// All runs have been iterated.
		mIsEnd = true;
		return;
	  }
	  int startRun = -1;
	  int lengthRun = -1;
	  UBiDiDirection runDir = ubidi_getVisualRun(mBidi, mNextRunIndex, startRun, lengthRun);
	  mNextRunIndex++;
	  if (startRun == -1 || lengthRun == -1)
	  {
		ALOGE("invalid visual run");
		// skip the invalid run.
		updateRunInfo();
		return;
	  }
	  int runEnd = Math.Min(startRun + lengthRun, mEnd);
	  mRunInfo.mRunStart = Math.Max(startRun, mStart);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: mRunInfo.mRunLength = runEnd - mRunInfo.mRunStart;
	  mRunInfo.mRunLength.CopyFrom(runEnd - mRunInfo.mRunStart);
	  if (mRunInfo.mRunLength <= 0)
	  {
		// skip the empty run.
		updateRunInfo();
		return;
	  }
	  mRunInfo.mIsRtl = (runDir == UBIDI_RTL);
	}
  }

  public BidiText(UInt16 buf, int start, int count, int bufSize, int bidiFlags)
  {
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mStart = start;
	  this.mStart.CopyFrom(start);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mEnd = start + count;
	  this.mEnd.CopyFrom(start + count);
//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
//ORIGINAL LINE: this.mBufSize = bufSize;
	  this.mBufSize.CopyFrom(bufSize);
	  this.mBidi = null;
	  this.mRunCount = 1;
	  this.mIsRtl = (bidiFlags & GlobalMembers.kDirection_Mask) != 0;
	if (bidiFlags == kBidi_Force_LTR || bidiFlags == kBidi_Force_RTL)
	{
	  // force single run.
	  return;
	}
	mBidi = ubidi_open();
	if (mBidi == null)
	{
	  ALOGE("error creating bidi object");
	  return;
	}
	UErrorCode status = U_ZERO_ERROR;
	// Set callbacks to override bidi classes of new emoji
	ubidi_setClassCallback(mBidi, emojiBidiOverride, null, null, null, status);
	if (!U_SUCCESS(status))
	{
	  ALOGE("error setting bidi callback function, status = %d", status);
	  return;
	}

	UBiDiLevel bidiReq = bidiFlags;
	if (bidiFlags == kBidi_Default_LTR)
	{
	  bidiReq = UBIDI_DEFAULT_LTR;
	}
	else if (bidiFlags == kBidi_Default_RTL)
	{
	  bidiReq = UBIDI_DEFAULT_RTL;
	}
//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
	ubidi_setPara(mBidi, reinterpret_cast<const UChar>(buf), mBufSize, bidiReq, null, status);
	if (!U_SUCCESS(status))
	{
	  ALOGE("error calling ubidi_setPara, status = %d", status);
	  return;
	}
	int paraDir = ubidi_getParaLevel(mBidi) & GlobalMembers.kDirection_Mask;
	uint rc = ubidi_countRuns(mBidi, status);
	if (!U_SUCCESS(status) || rc < 0)
	{
	  ALOGW("error counting bidi runs, status = %d", status);
	}
	if (!U_SUCCESS(status) || rc <= 1)
	{
	  mIsRtl = (paraDir == kBidi_RTL);
	  return;
	}
	mRunCount = rc;
  }

  public void Dispose()
  {
	if (mBidi != null)
	{
	  ubidi_close(mBidi);
	}
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Iter begin() const
  public Iter begin()
  {
	  return new Iter(mBidi, mStart, mEnd, 0, mRunCount, mIsRtl);
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Iter end() const
  public Iter end()
  {
	return new Iter(mBidi, mStart, mEnd, mRunCount, mRunCount, mIsRtl);
  }

  private readonly int mStart = new int();
  private readonly int mEnd = new int();
  private readonly int mBufSize = new int();
  private UBiDi mBidi;
  private int mRunCount = new int();
  private bool mIsRtl;

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  BidiText(const BidiText&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  void operator =(const BidiText&) = delete;
}







//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int Layout::nGlyphs() const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const MinikinFont* Layout::getFont(int i) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: FontFakery Layout::getFakery(int i) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint Layout::getGlyphId(int i) const

// libtxt extension
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint Layout::getGlyphCluster(int i) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float Layout::getX(int i) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float Layout::getY(int i) const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float Layout::getAdvance() const


//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void Layout::getBounds(MinikinRect* bounds) const


} // namespace minikin
