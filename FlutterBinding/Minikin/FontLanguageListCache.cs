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

public class FontLanguageListCache : System.IDisposable
{
  // A special ID for the empty language list.
  // This value must be 0 since the empty language list is inserted into
  // mLanguageLists by default.
  public const uint kEmptyListId = 0;

  // Returns language list ID for the given string representation of
  // FontLanguages. Caller should acquire a lock before calling the method.

  // static
  public static uint getId(string languages)
  {
	FontLanguageListCache inst = FontLanguageListCache.getInstance();
	Dictionary<string, uint>.Enumerator it = inst.mLanguageListLookupTable.find(languages);
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
	if (it != inst.mLanguageListLookupTable.end())
	{
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
	  return it.second;
	}

	// Given language list is not in cache. Insert it and return newly assigned
	// ID.
	uint nextId = inst.mLanguageLists.Count;
	FontLanguages fontLanguages = new FontLanguages(minikin.GlobalMembers.parseLanguageList(languages));
	if (fontLanguages.empty())
	{
	  return kEmptyListId;
	}
	inst.mLanguageLists.Add(std::move(fontLanguages));
	inst.mLanguageListLookupTable.Add(languages, nextId);
	return nextId;
  }

  // Caller should acquire a lock before calling the method.

  // static
  public static FontLanguages getById(uint id)
  {
	FontLanguageListCache inst = FontLanguageListCache.getInstance();
	LOG_ALWAYS_FATAL_IF(id >= inst.mLanguageLists.Count, "Lookup by unknown language list ID.");
	return inst.mLanguageLists[id];
  }

  private FontLanguageListCache()
  {
  } // Singleton
  public void Dispose()
  {
  }

  // Caller should acquire a lock before calling the method.

  // static
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
  private static FontLanguageListCache getInstance_instance = null;
  private static FontLanguageListCache getInstance()
  {
	assertMinikinLocked();
//C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
//	static FontLanguageListCache* instance = null;
	if (getInstance_instance == null)
	{
	  getInstance_instance = new FontLanguageListCache();

	  // Insert an empty language list for mapping default language list to
	  // kEmptyListId. The default language list has only one FontLanguage and it
	  // is the unsupported language.
	  getInstance_instance.mLanguageLists.Add(new FontLanguages());
	  getInstance_instance.mLanguageListLookupTable.Add("", kEmptyListId);
	}
	return getInstance_instance;
  }

  private List<FontLanguages> mLanguageLists = new List<FontLanguages>();

  // A map from string representation of the font language list to the ID.
  private Dictionary<string, uint> mLanguageListLookupTable = new Dictionary<string, uint>();
}

} // namespace minikin






