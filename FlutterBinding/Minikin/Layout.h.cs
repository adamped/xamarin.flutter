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

public class LayoutGlyph
{
  // index into mFaces and mHbFonts vectors. We could imagine
  // moving this into a run length representation, because it's
  // more efficient for long strings, and we'll probably need
  // something like that for paint attributes (color, underline,
  // fake b/i, etc), as having those per-glyph is bloated.
  public int font_ix;

  public uint glyph_id;
  public float x;
  public float y;

  // libtxt extension: record the cluster (character index) that corresponds
  // to this glyph
  public uint cluster = new uint();
}

// Internal state used during layout operation
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct LayoutContext;

//C++ TO C# CONVERTER NOTE: Enums must be named in C#, so the following enum has been named AnonymousEnum:
public enum AnonymousEnum
{
  kBidi_LTR = 0,
  kBidi_RTL = 1,
  kBidi_Default_LTR = 2,
  kBidi_Default_RTL = 3,
  kBidi_Force_LTR = 4,
  kBidi_Force_RTL = 5,

  kBidi_Mask = 0x7
}

// Lifecycle and threading assumptions for Layout:
// The object is assumed to be owned by a single thread; multiple threads
// may not mutate it at the same time.
public class Layout
{
  public Layout()
  {
	  this.mGlyphs = new List<LayoutGlyph>();
	  this.mAdvances = new List<float>();
	  this.mFaces = new List<FakedFont>();
	  this.mAdvance = 0F;
	  this.mBounds = new MinikinRect();
	mBounds.setEmpty();
  }

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  Layout(Layout&& layout) = default;

  // Forbid copying and assignment.
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  Layout(const Layout&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  void operator =(const Layout&) = delete;

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void dump() const;
	public void dump()
	{
	  for (int i = 0; i < mGlyphs.size(); i++)
	  {
		LayoutGlyph glyph = mGlyphs[i];
		Console.Write(glyph.glyph_id);
		Console.Write(": ");
		Console.Write(glyph.x);
		Console.Write(", ");
		Console.Write(glyph.y);
		Console.Write("\n");
	  }
	}

	public void doLayout(UInt16 buf, int start, int count, int bufSize, bool isRtl, FontStyle style, MinikinPaint paint, FontCollection collection)
	{
	  std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
    
	  LayoutContext ctx = new LayoutContext();
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: ctx.style = style;
	  ctx.style.CopyFrom(style);
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: ctx.paint = paint;
	  ctx.paint.CopyFrom(paint);
    
	  reset();
	  mAdvances.resize(count, 0);
    
	  doLayoutRunCached(buf, start, count, bufSize, isRtl, ctx, start, collection, this, null);
    
	  ctx.clearHbFonts();
	}

	public float measureText(UInt16 buf, int start, int count, int bufSize, bool isRtl, FontStyle style, MinikinPaint paint, FontCollection collection, ref float advances)
	{
	  std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
    
	  LayoutContext ctx = new LayoutContext();
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: ctx.style = style;
	  ctx.style.CopyFrom(style);
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: ctx.paint = paint;
	  ctx.paint.CopyFrom(paint);
    
	  float advance = doLayoutRunCached(buf, start, count, bufSize, isRtl, ctx, 0, collection, null, advances);
    
	  ctx.clearHbFonts();
	  return advance;
	}

  // public accessors
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int nGlyphs() const;
	public int nGlyphs()
	{
	  return mGlyphs.size();
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const MinikinFont* getFont(int i) const;
	public MinikinFont getFont(int i)
	{
	  LayoutGlyph glyph = mGlyphs[i];
	  return mFaces[glyph.font_ix].font;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: FontFakery getFakery(int i) const;
	public FontFakery getFakery(int i)
	{
	  LayoutGlyph glyph = mGlyphs[i];
	  return mFaces[glyph.font_ix].fakery;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getGlyphId(int i) const;
	public uint getGlyphId(int i)
	{
	  LayoutGlyph glyph = mGlyphs[i];
	  return glyph.glyph_id;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getGlyphCluster(int i) const;
	public uint getGlyphCluster(int i)
	{
	  LayoutGlyph glyph = mGlyphs[i];
	  return glyph.cluster;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getX(int i) const;
	public float getX(int i)
	{
	  LayoutGlyph glyph = mGlyphs[i];
	  return glyph.x;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getY(int i) const;
	public float getY(int i)
	{
	  LayoutGlyph glyph = mGlyphs[i];
	  return glyph.y;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getAdvance() const;
	public float getAdvance()
	{
	  return mAdvance;
	}

  // Get advances, copying into caller-provided buffer. The size of this
  // buffer must match the length of the string (count arg to doLayout).
	public void getAdvances(ref float advances)
	{
	//C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
	  memcpy(advances, mAdvances[0], mAdvances.size() * sizeof(float));
	}

  // The i parameter is an offset within the buf relative to start, it is <
  // count, where start and count are the parameters to doLayout
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getCharAdvance(int i) const
  public float getCharAdvance(int i)
  {
	  return mAdvances[i];
  }

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void getBounds(MinikinRect* rect) const;
	public void getBounds(MinikinRect bounds)
	{
	  bounds.set(mBounds);
	}

  // Purge all caches, useful in low memory conditions
	public void purgeCaches()
	{
	  std::lock_guard<std::recursive_mutex> _l = new std::lock_guard<std::recursive_mutex>(gMinikinLock);
	  LayoutCache layoutCache = LayoutEngine.getInstance().layoutCache.functorMethod;
	  layoutCache.clear();
	  purgeHbFontCacheLocked();
	}

//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
//  friend class LayoutCacheKey;

  // Find a face in the mFaces vector, or create a new entry
	public int findFace(FakedFont face, LayoutContext ctx)
	{
	  uint ix;
	  for (ix = 0; ix < mFaces.size(); ix++)
	  {
		if (mFaces[ix].font == face.font)
		{
		  return ix;
		}
	  }
	  mFaces.push_back(face);
	  // Note: ctx == NULL means we're copying from the cache, no need to create
	  // corresponding hb_font object.
	  if (ctx != null)
	  {
		HarfBuzzSharp.Font font = getHbFontLocked(face.font);
		// Temporarily removed to fix advance integer rounding.
		// This is likely due to very old versions of harfbuzz and ICU.
		// hb_font_set_funcs(font, getHbFontFuncs(isColorBitmapFont(font)),
		// &ctx->paint, 0);
		ctx.hbFonts.Add(font);
	  }
	  return ix;
	}

  // Clears layout, ready to be used again
	public void reset()
	{
	  mGlyphs.clear();
	  mFaces.clear();
	  mBounds.setEmpty();
	  mAdvances.clear();
	  mAdvance = 0;
	}

  // Lay out a single bidi run
  // When layout is not null, layout info will be stored in the object.
  // When advances is not null, measurement results will be stored in the array.
	public float doLayoutRunCached(UInt16 buf, int start, int count, int bufSize, bool isRtl, LayoutContext ctx, int dstStart, FontCollection collection, Layout layout, ref float advances)
	{
	  uint originalHyphen = ctx.paint.hyphenEdit.getHyphen();
	  float advance = 0F;
	  if (!isRtl)
	  {
		// left to right
		int wordstart = start == bufSize != null ? start : getPrevWordBreakForCache(buf, start + 1, bufSize);
		int wordend = new int();
		for (int iter = start; iter < start + count; iter = wordend)
		{
		  wordend = getNextWordBreakForCache(buf, iter, bufSize);
		  // Only apply hyphen to the first or last word in the string.
		  uint hyphen = new uint(originalHyphen);
		  if (iter != start)
		  { // Not the first word
			hyphen &= ~HyphenEdit.MASK_START_OF_LINE;
		  }
		  if (wordend < start + count)
		  { // Not the last word
			hyphen &= ~HyphenEdit.MASK_END_OF_LINE;
		  }
		  ctx.paint.hyphenEdit = hyphen;
		  int wordcount = Math.Min(start + count, wordend) - iter;
		  advance += doLayoutWord(buf + wordstart, iter - wordstart, wordcount, wordend - wordstart, isRtl, ctx, iter - dstStart, collection, layout, advances != 0 ? advances + (iter - start) : advances);
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: wordstart = wordend;
		  wordstart.CopyFrom(wordend);
		}
	  }
	  else
	  {
		// right to left
		int wordstart = new int();
		int end = start + count;
		int wordend = end == 0 ? 0 : getNextWordBreakForCache(buf, end - 1, bufSize);
		for (int iter = end; iter > start; iter = wordstart)
		{
		  wordstart = getPrevWordBreakForCache(buf, iter, bufSize);
		  // Only apply hyphen to the first (rightmost) or last (leftmost) word in
		  // the string.
		  uint hyphen = new uint(originalHyphen);
		  if (wordstart > start)
		  { // Not the first word
			hyphen &= ~HyphenEdit.MASK_START_OF_LINE;
		  }
		  if (iter != end)
		  { // Not the last word
			hyphen &= ~HyphenEdit.MASK_END_OF_LINE;
		  }
		  ctx.paint.hyphenEdit = hyphen;
		  int bufStart = Math.Max(start, wordstart);
		  advance += doLayoutWord(buf + wordstart, bufStart - wordstart, iter - bufStart, wordend - wordstart, isRtl, ctx, bufStart - dstStart, collection, layout, advances != 0 ? advances + (bufStart - start) : advances);
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: wordend = wordstart;
		  wordend.CopyFrom(wordstart);
		}
	  }
	  return advance;
	}

  // Lay out a single word
	public float doLayoutWord(UInt16[] buf, int start, int count, int bufSize, bool isRtl, LayoutContext ctx, int bufStart, FontCollection collection, Layout layout, float[] advances)
	{
	  LayoutCache cache = LayoutEngine.getInstance().layoutCache.functorMethod;
	  LayoutCacheKey key = new LayoutCacheKey(collection, ctx.paint, new FontStyle(ctx.style), new UInt16(buf), start, count, bufSize, isRtl);
    
	  float wordSpacing = count == 1 && isWordSpace(buf[start]) ? ctx.paint.wordSpacing : 0F;
    
	  float advance;
	  if (ctx.paint.skipCache())
	  {
		Layout layoutForWord = new Layout();
		key.doLayout(layoutForWord, ctx, collection);
		if (layout != null)
		{
		  layout.appendLayout(layoutForWord, bufStart, wordSpacing);
		}
		if (advances != 0F)
		{
		  layoutForWord.getAdvances(advances);
		}
		advance = layoutForWord.getAdvance();
	  }
	  else
	  {
		Layout layoutForWord = cache.get(key, ctx, collection);
		if (layout != null)
		{
		  layout.appendLayout(layoutForWord, bufStart, wordSpacing);
		}
		if (advances != 0F)
		{
		  layoutForWord.getAdvances(advances);
		}
		advance = layoutForWord.getAdvance();
	  }
    
	  if (wordSpacing != 0F)
	  {
		advance += wordSpacing;
		if (advances != 0F)
		{
		  advances[0] += wordSpacing;
		}
	  }
	  return advance;
	}

  // Lay out a single bidi run
	public void doLayoutRun(UInt16[] buf, int start, int count, int bufSize, bool isRtl, LayoutContext ctx, FontCollection collection)
	{
	  hb_buffer_t buffer = LayoutEngine.getInstance().hbBuffer;
	  vector<FontCollection.Run> items = new vector<FontCollection.Run>();
	  collection.itemize(buf + start, count, ctx.style, items);
    
	  vector<hb_feature_t> features = new vector<hb_feature_t>();
	  // Disable default-on non-required ligature features if letter-spacing
	  // See http://dev.w3.org/csswg/css-text-3/#letter-spacing-property
	  // "When the effective spacing between two characters is not zero (due to
	  // either justification or a non-zero value of letter-spacing), user agents
	  // should not apply optional ligatures."
	  if (Math.Abs(ctx.paint.letterSpacing) > 0.03)
	  {
		hb_feature_t no_liga = new hb_feature_t(HB_TAG('l', 'i', 'g', 'a'), 0, 0, ~0u);
		hb_feature_t no_clig = new hb_feature_t(HB_TAG('c', 'l', 'i', 'g'), 0, 0, ~0u);
		features.push_back(no_liga);
		features.push_back(no_clig);
	  }
	  addFeatures(ctx.paint.fontFeatureSettings, features);
    
	  double size = ctx.paint.size;
	  double scaleX = ctx.paint.scaleX;
    
	  float x = mAdvance;
	  float y = 0F;
	  for (int run_ix = isRtl ? items.size() - 1 : 0; isRtl ? run_ix >= 0 : run_ix < (int)items.size(); isRtl ?--run_ix :++run_ix)
	  {
		FontCollection.Run run = items[run_ix];
		if (run.fakedFont.font == null)
		{
		  ALOGE("no font for run starting u+%04x length %d", buf[run.start], run.end - run.start);
		  continue;
		}
		int font_ix = findFace(run.fakedFont, ctx);
		ctx.paint.font = mFaces[font_ix].font;
		ctx.paint.fakery = mFaces[font_ix].fakery;
		HarfBuzzSharp.Font hbFont = ctx.hbFonts[font_ix];
	#if VERBOSE_DEBUG
		ALOGD("Run %zu, font %d [%d:%d]", run_ix, font_ix, run.start, run.end);
	#endif
    
		hb_font_set_ppem(hbFont, size * scaleX, size);
		hb_font_set_scale(hbFont, HBFloatToFixed(size * scaleX), HBFloatToFixed(size));
    
		bool is_color_bitmap_font = isColorBitmapFont(hbFont);
    
		// TODO: if there are multiple scripts within a font in an RTL run,
		// we need to reorder those runs. This is unlikely with our current
		// font stack, but should be done for correctness.
    
		// Note: scriptRunStart and scriptRunEnd, as well as run.start and run.end,
		// run between 0 and count.
		uint scriptRunEnd;
		for (uint scriptRunStart = run.start; scriptRunStart < run.end; scriptRunStart = scriptRunEnd)
		{
		  scriptRunEnd = scriptRunStart;
		  hb_script_t script = getScriptRun(buf + start, run.end, ref scriptRunEnd);
		  // After the last line, scriptRunEnd is guaranteed to have increased,
		  // since the only time getScriptRun does not increase its iterator is when
		  // it has already reached the end of the buffer. But that can't happen,
		  // since if we have already reached the end of the buffer, we should have
		  // had (scriptRunEnd == run.end), which means (scriptRunStart == run.end)
		  // which is impossible due to the exit condition of the for loop. So we
		  // can be sure that scriptRunEnd > scriptRunStart.
    
		  double letterSpace = 0.0;
		  double letterSpaceHalfLeft = 0.0;
		  double letterSpaceHalfRight = 0.0;
    
		  if (ctx.paint.letterSpacing != 0.0 && isScriptOkForLetterspacing(new hb_script_t(script)))
		  {
			letterSpace = ctx.paint.letterSpacing * size * scaleX;
			if ((ctx.paint.paintFlags & LinearTextFlag) == 0)
			{
			  letterSpace = Math.Round(letterSpace);
			  letterSpaceHalfLeft = Math.Floor(letterSpace * 0.5);
			}
			else
			{
			  letterSpaceHalfLeft = letterSpace * 0.5;
			}
			letterSpaceHalfRight = letterSpace - letterSpaceHalfLeft;
		  }
    
		  hb_buffer_clear_contents(buffer);
		  hb_buffer_set_script(buffer, script);
		  hb_buffer_set_direction(buffer, isRtl ? HB_DIRECTION_RTL : HB_DIRECTION_LTR);
		  FontLanguages langList = FontLanguageListCache.getById(ctx.style.getLanguageListId());
		  if (langList.size() != 0)
		  {
			FontLanguage hbLanguage = langList[0];
			for (int i = 0; i < langList.size(); ++i)
			{
			  if (langList[i].supportsHbScript(script))
			  {
				hbLanguage = langList[i];
				break;
			  }
			}
			hb_buffer_set_language(buffer, hbLanguage.getHbLanguage());
		  }
    
		  uint clusterStart = addToHbBuffer(buffer, new UInt16(buf), start, count, bufSize, scriptRunStart, scriptRunEnd, ctx.paint.hyphenEdit, hbFont);
    
		  hb_shape(hbFont, buffer, features.empty() ? null : features[0], features.size());
		  uint numGlyphs;
		  hb_glyph_info_t[] info = hb_buffer_get_glyph_infos(buffer, numGlyphs);
		  hb_glyph_position_t positions = hb_buffer_get_glyph_positions(buffer, null);
    
		  // At this point in the code, the cluster values in the info buffer
		  // correspond to the input characters with some shift. The cluster value
		  // clusterStart corresponds to the first character passed to HarfBuzz,
		  // which is at buf[start + scriptRunStart] whose advance needs to be saved
		  // into mAdvances[scriptRunStart]. So cluster values need to be reduced by
		  // (clusterStart - scriptRunStart) to get converted to indices of
		  // mAdvances.
		  uint clusterOffset = clusterStart - scriptRunStart;
    
		  if (numGlyphs != 0)
		  {
			mAdvances[info[0].cluster - clusterOffset] += letterSpaceHalfLeft;
			x += letterSpaceHalfLeft;
		  }
		  for (uint i = 0; i < numGlyphs; i++)
		  {
	#if VERBOSE_DEBUG
			ALOGD("%d %d %d %d", positions[i].x_advance, positions[i].y_advance, positions[i].x_offset, positions[i].y_offset);
			ALOGD("DoLayout %u: %f; %d, %d", info[i].codepoint, HBFixedToFloat(positions[i].x_advance), positions[i].x_offset, positions[i].y_offset);
	#endif
			if (i > 0 && info[i - 1].cluster != info[i].cluster)
			{
			  mAdvances[info[i - 1].cluster - clusterOffset] += letterSpaceHalfRight;
			  mAdvances[info[i].cluster - clusterOffset] += letterSpaceHalfLeft;
			  x += letterSpace;
			}
    
			hb_codepoint_t glyph_ix = info[i].codepoint;
			float xoff = HBFixedToFloat(positions[i].x_offset);
			float yoff = -HBFixedToFloat(positions[i].y_offset);
			xoff += yoff * ctx.paint.skewX;
			LayoutGlyph glyph = new LayoutGlyph(font_ix, glyph_ix, x + xoff, y + yoff, (uint)(info[i].cluster - clusterOffset));
			mGlyphs.push_back(glyph);
			float xAdvance = HBFixedToFloat(positions[i].x_advance);
			if ((ctx.paint.paintFlags & LinearTextFlag) == 0)
			{
			  xAdvance = roundf(xAdvance);
			}
			MinikinRect glyphBounds = new MinikinRect();
			hb_glyph_extents_t extents = new hb_glyph_extents_t();
			if (is_color_bitmap_font && hb_font_get_glyph_extents(hbFont, glyph_ix, extents))
			{
			  // Note that it is technically possible for a TrueType font to have
			  // outline and embedded bitmap at the same time. We ignore modified
			  // bbox of hinted outline glyphs in that case.
			  glyphBounds.mLeft = roundf(HBFixedToFloat(extents.x_bearing));
			  glyphBounds.mTop = roundf(HBFixedToFloat(-extents.y_bearing));
			  glyphBounds.mRight = roundf(HBFixedToFloat(extents.x_bearing + extents.width));
			  glyphBounds.mBottom = roundf(HBFixedToFloat(-extents.y_bearing - extents.height));
			}
			else
			{
			  ctx.paint.font.GetBounds(glyphBounds, glyph_ix, ctx.paint);
			}
			glyphBounds.offset(x + xoff, y + yoff);
			mBounds.join(glyphBounds);
			if ((int)(info[i].cluster - clusterOffset) < count)
			{
			  mAdvances[info[i].cluster - clusterOffset] += xAdvance;
			}
			else
			{
			  ALOGE("cluster %zu (start %zu) out of bounds of count %zu", info[i].cluster - clusterOffset, start, count);
			}
			x += xAdvance;
		  }
		  if (numGlyphs != 0)
		  {
			mAdvances[info[numGlyphs - 1].cluster - clusterOffset] += letterSpaceHalfRight;
			x += letterSpaceHalfRight;
		  }
		}
	  }
	  mAdvance = x;
	}

  // Append another layout (for example, cached value) into this one
	public void appendLayout(Layout src, int start, float extraAdvance)
	{
	  int[] fontMapStack = new int[16];
	//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
	//ORIGINAL LINE: int* fontMap;
	  int fontMap;
	//C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
	//ORIGINAL LINE: if (src->mFaces.size() < sizeof(fontMapStack) / sizeof(fontMapStack[0]))
	  if (src.mFaces.size() < fontMapStack.Length)
	  {
		fontMap = fontMapStack;
	  }
	  else
	  {
		fontMap = new int[src.mFaces.size()];
	  }
	  for (int i = 0; i < src.mFaces.size(); i++)
	  {
		int font_ix = findFace(src.mFaces[i], null);
		fontMap[i] = font_ix;
	  }
	  // LibTxt: Changed x0 from int to float to prevent rounding that causes text
	  // jitter.
	  float x0 = mAdvance;
	  for (int i = 0; i < src.mGlyphs.size(); i++)
	  {
		LayoutGlyph srcGlyph = src.mGlyphs[i];
		int font_ix = fontMap[srcGlyph.font_ix];
		uint glyph_id = srcGlyph.glyph_id;
		float x = x0 + srcGlyph.x;
		float y = srcGlyph.y;
		LayoutGlyph glyph = new LayoutGlyph(font_ix, glyph_id, x, y, (uint)(srcGlyph.cluster + start));
		mGlyphs.push_back(glyph);
	  }
	  for (int i = 0; i < src.mAdvances.size(); i++)
	  {
		mAdvances[i + start] = src.mAdvances[i];
		if (i == 0)
		{
		  mAdvances[i + start] += extraAdvance;
		}
	  }
	  MinikinRect srcBounds = new MinikinRect(src.mBounds);
	  srcBounds.offset(x0, 0);
	  mBounds.join(srcBounds);
	  mAdvance += src.mAdvance + extraAdvance;
    
	  if (fontMap != fontMapStack)
	  {
		fontMap = null;
	  }
	}

  private List<LayoutGlyph> mGlyphs = new List<LayoutGlyph>();
  private List<float> mAdvances = new List<float>();

  private List<FakedFont> mFaces = new List<FakedFont>();
  private float mAdvance;
  private MinikinRect mBounds = new MinikinRect();
}

} // namespace minikin

