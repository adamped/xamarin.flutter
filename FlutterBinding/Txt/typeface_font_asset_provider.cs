using SkiaSharp;
using System.Collections.Generic;

/*
 * Copyright 2018 Google Inc.
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
 * Copyright 2018 Google Inc.
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



//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "flutter/fml/macros.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SkFontMgr.h"

namespace FlutterBinding.Txt
{

    public class TypefaceFontStyleSet : SkFontStyleSet, System.IDisposable
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  TypefaceFontStyleSet();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void registerTypeface(SKTypeface typeface)
        {
            if (typeface == null)
            {
                return;
            }
            typefaces_.Add(typeface);
        }

        // |SkFontStyleSet|
        public override int count()
        {
            return typefaces_.Count;
        }

        // |SkFontStyleSet|
        public override void getStyle(int index, SkFontStyle UnnamedParameter, SkString style)
        {
            FML_DCHECK(false);
        }

        // |SkFontStyleSet|
        public override SkTypeface createTypeface(int i)
        {
            int index = i;
            if (index >= typefaces_.Count)
            {
                return null;
            }
            return SkRef(typefaces_[index].get());
        }

        // |SkFontStyleSet|
        public override SKTypeface matchStyle(SKFontStyle pattern)
        {
            if (typefaces_.Count == 0)
            {
                return null;
            }

            foreach (SKTypeface typeface in typefaces_)
            {
                if (typeface.fontStyle() == pattern)
                {
                    return SkRef(typeface.get());
                }
            }

            return SkRef(typefaces_[0].get());
        }

        private List<SKTypeface> typefaces_ = new List<SKTypeface>();

        //C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        FML_DISALLOW_COPY_AND_ASSIGN(TypefaceFontStyleSet);
    }

    public class TypefaceFontAssetProvider : FontAssetProvider
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  TypefaceFontAssetProvider();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void RegisterTypeface(SKTypeface typeface)
        {
            if (typeface == null)
            {
                return;
            }

            SkString sk_family_name = new SKString();
            typeface.getFamilyName(sk_family_name);

            string family_name = new string(sk_family_name.c_str(), sk_family_name.size());
            RegisterTypeface(typeface, family_name);
        }

        public void RegisterTypeface(SKTypeface typeface, string family_name_alias)
        {
            if (string.IsNullOrEmpty(family_name_alias))
            {
                return;
            }

            string canonical_name = CanonicalFamilyName(family_name_alias);
            var family_it = registered_families_.find(canonical_name);
            if (family_it == registered_families_.end())
            {
                family_names_.Add(family_name_alias);
                family_it = registered_families_.emplace(std::piecewise_construct, std::forward_as_tuple(canonical_name), std::forward_as_tuple()).first;
            }
            //C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
            family_it.second.registerTypeface(typeface);
        }

        // |FontAssetProvider|

        // |FontAssetProvider|
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int GetFamilyCount() const override
        public override int GetFamilyCount()
        {
            return family_names_.Count;
        }

        // |FontAssetProvider|

        // |FontAssetProvider|
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: string GetFamilyName(int index) const override
        public override string GetFamilyName(int index)
        {
            return family_names_[index];
        }

        // |FontAssetProvider|

        // |FontAssetProvider|
        public override SkFontStyleSet MatchFamily(string family_name)
        {
            var found = registered_families_.find(CanonicalFamilyName(family_name));
            if (found == registered_families_.end())
            {
                return null;
            }
            //C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
            return SkRef(found.second);
        }

        private Dictionary<string, TypefaceFontStyleSet> registered_families_ = new Dictionary<string, TypefaceFontStyleSet>();
        private List<string> family_names_ = new List<string>();

        //C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
        //FML_DISALLOW_COPY_AND_ASSIGN(TypefaceFontAssetProvider);
    }

} // namespace FlutterBinding.Txt



//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "flutter/fml/logging.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SkString.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SkTypeface.h"

namespace FlutterBinding.Txt
{

    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
    //TypefaceFontAssetProvider::TypefaceFontAssetProvider() = default;

    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
    //TypefaceFontAssetProvider::~TypefaceFontAssetProvider() = default;

    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
    //TypefaceFontStyleSet::TypefaceFontStyleSet() = default;

    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
    //TypefaceFontStyleSet::~TypefaceFontStyleSet() = default;

} // namespace FlutterBinding.Txt
