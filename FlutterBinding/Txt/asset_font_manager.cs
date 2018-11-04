//using SkiaSharp;

///*
// * Copyright 2017 Google Inc.
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *      http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

///*
// * Copyright 2017 Google Inc.
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *      http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */


////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "flutter/fml/macros.h"
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/skia/include/core/SkFontMgr.h"
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/skia/include/core/SkStream.h"

//namespace FlutterBinding.Txt
//{

//    public class AssetFontManager : SkiaSharp.SKFontManager, System.IDisposable
//    {
//        public AssetFontManager(FontAssetProvider font_provider)
//        {
//            this.font_provider_ = font_provider;
//            //FML_DCHECK(font_provider_ != null);
//        }

//        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//        //  public void Dispose();

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SkFontStyleSet* onMatchFamily(const char family_name_string[]) const override
//        protected override SKFontStyleSet onMatchFamily(string family_name_string)
//        {
//            string family_name = family_name_string;
//            return font_provider_.MatchFamily(family_name);
//        }

//        protected std::unique_ptr<FontAssetProvider> font_provider_ = new std::unique_ptr<FontAssetProvider>();

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: int onCountFamilies() const override
//        private override int onCountFamilies()
//        {
//            return font_provider_.GetFamilyCount();
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: void onGetFamilyName(int index, SkString* familyName) const override
//        private override void onGetFamilyName(int index, SkString familyName)
//        {
//            familyName.set(font_provider_.GetFamilyName(index).c_str());
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SkFontStyleSet* onCreateStyleSet(int index) const override
//        private override SkFontStyleSet onCreateStyleSet(int index)
//        {
//            FML_DCHECK(false);
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SkTypeface* onMatchFamilyStyle(const char familyName[], const SkFontStyle& style) const override
//        private override SkTypeface onMatchFamilyStyle(string familyName, SkFontStyle style)
//        {
//            SkFontStyleSet font_style_set = font_provider_.MatchFamily((string)familyName);
//            if (font_style_set == null)
//            {
//                return null;
//            }
//            return font_style_set.matchStyle(style);
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SkTypeface* onMatchFamilyStyleCharacter(const char familyName[], const SkFontStyle&, const char* bcp47[], int bcp47Count, SkUnichar character) const override
//        private override SkTypeface onMatchFamilyStyleCharacter(string familyName, SkFontStyle UnnamedParameter, string[] bcp47, int bcp47Count, SkUnichar character)
//        {
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SkTypeface* onMatchFaceStyle(const SkTypeface*, const SkFontStyle&) const override
//        private override SkTypeface onMatchFaceStyle(SkTypeface UnnamedParameter, SkFontStyle UnnamedParameter2)
//        {
//            FML_DCHECK(false);
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SKTypeface onMakeFromData(sk_sp<SkData>, int ttcIndex) const override
//        private override SKTypeface onMakeFromData(sk_sp<SkData> UnnamedParameter, int ttcIndex)
//        {
//            FML_DCHECK(false);
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SKTypeface onMakeFromStreamIndex(std::unique_ptr<SkStreamAsset>, int ttcIndex) const override
//        private override SKTypeface onMakeFromStreamIndex(std::unique_ptr<SkStreamAsset> UnnamedParameter, int ttcIndex)
//        {
//            FML_DCHECK(false);
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SKTypeface onMakeFromStreamArgs(std::unique_ptr<SkStreamAsset>, const SkFontArguments&) const override
//        private override SKTypeface onMakeFromStreamArgs(std::unique_ptr<SkStreamAsset> UnnamedParameter, SkFontArguments UnnamedParameter2)
//        {
//            FML_DCHECK(false);
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SKTypeface onMakeFromFile(const char path[], int ttcIndex) const override
//        private override SKTypeface onMakeFromFile(string path, int ttcIndex)
//        {
//            FML_DCHECK(false);
//            return null;
//        }

//        // |SkFontMgr|
//        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//        //ORIGINAL LINE: SKTypeface onLegacyMakeTypeface(const char familyName[], SkFontStyle) const override
//        private override SKTypeface onLegacyMakeTypeface(string familyName, SkFontStyle UnnamedParameter)
//        {
//            //FML_DCHECK(false);
//            return null;
//        }

//        //C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
//        //FML_DISALLOW_COPY_AND_ASSIGN(AssetFontManager);
//    }

//    public class DynamicFontManager : AssetFontManager
//    {
//        public DynamicFontManager() : base(new TypefaceFontAssetProvider())
//        {
//        }

//        public TypefaceFontAssetProvider font_provider()
//        {
//            return (TypefaceFontAssetProvider)(*font_provider_);
//        }
//    }

//} // namespace FlutterBinding.Txt




////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "flutter/fml/logging.h"
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/skia/include/core/SkString.h"
////C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
////#include "third_party/skia/include/core/SkTypeface.h"

//namespace FlutterBinding.Txt
//{

//    //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//    //AssetFontManager::~AssetFontManager() = default;

//} // namespace FlutterBinding.Txt
