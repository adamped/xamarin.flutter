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


//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct HarfBuzzSharp.Font;

namespace minikin
{
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class MinikinFont;

} // namespace minikin





namespace minikin
{

//C++ TO C# CONVERTER TODO TASK: C# has no concept of 'private' inheritance:
//ORIGINAL LINE: class HbFontCache : private android::OnEntryRemoved<int, HarfBuzzSharp.Font*>
public class HbFontCache : android.OnEntryRemoved<int, HarfBuzzSharp.Font*>
{
  public HbFontCache()
  {
	  this.mCache = kMaxEntries;
	mCache.setOnEntryRemovedListener(this.functorMethod);
  }

  // callback for OnEntryRemoved
  public static void functorMethod(int UnnamedParameter, ref HarfBuzzSharp.Font value)
  {
	hb_font_destroy(value);
  }

  public HarfBuzzSharp.Font get(int fontId)
  {
	  return mCache.get(fontId);
  }

  public void put(int fontId, HarfBuzzSharp.Font font)
  {
	  mCache.put(fontId, font);
  }

  public void clear()
  {
	  mCache.clear();
  }

  public void remove(int fontId)
  {
	  mCache.remove(fontId);
  }

  private const int kMaxEntries = 100;

  private android.LruCache<int, HarfBuzzSharp.Font> mCache = new android.LruCache<int, HarfBuzzSharp.Font>();
}

} // namespace minikin
