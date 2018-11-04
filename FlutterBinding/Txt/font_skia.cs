//using System.Collections.Generic;

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

//namespace FlutterBinding.Txt
//{

//public class FontSkia : minikin.MinikinFont, System.IDisposable
//{
//  public FontSkia(SKTypeface typeface) : base(typeface.uniqueID())
//  {
//	  this.typeface_ = typeface;
//  }

////C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
////  public void Dispose();

////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
////ORIGINAL LINE: float GetHorizontalAdvance(uint glyph_id, const minikin::MinikinPaint& paint) const override
//  public override float GetHorizontalAdvance(uint glyph_id, minikin.MinikinPaint paint)
//  {
//	SKPaint skPaint = new SKPaint();
//	UInt16 glyph16 = glyph_id;
//	SkScalar skWidth = new SkScalar();
//	GlobalMembers.FontSkia_SetSkiaPaint(new SKTypeface(typeface_), skPaint, paint);
//	skPaint.getTextWidths(glyph16, sizeof(UInt16), skWidth, null);
//	return skWidth;
//  }

////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
////ORIGINAL LINE: void GetBounds(minikin::MinikinRect* bounds, uint glyph_id, const minikin::MinikinPaint& paint) const override
//  public override void GetBounds(minikin.MinikinRect bounds, uint glyph_id, minikin.MinikinPaint paint)
//  {
//	SKPaint skPaint = new SKPaint();
//	UInt16 glyph16 = glyph_id;
//	SkRect skBounds = new SkRect();
//	GlobalMembers.FontSkia_SetSkiaPaint(new SKTypeface(typeface_), skPaint, paint);
//	skPaint.getTextWidths(glyph16, sizeof(UInt16), null, skBounds);
//	bounds.mLeft = skBounds.Left;
//	bounds.mTop = skBounds.fTop;
//	bounds.mRight = skBounds.Right;
//	bounds.mBottom = skBounds.fBottom;
//  }

////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
////ORIGINAL LINE: HarfBuzzSharp.Face* CreateHarfBuzzFace() const override
//  public override HarfBuzzSharp.Face CreateHarfBuzzFace()
//  {
//	return hb_face_create_for_tables(GlobalMembers.GetTable, typeface_.get(), 0);
//  }

////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
////ORIGINAL LINE: const ClassicVector<minikin::FontVariation>& GetAxes() const override
//  public override List<minikin.FontVariation> GetAxes()
//  {
//	return variations_;
//  }

////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
////ORIGINAL LINE: const SKTypeface& GetSkTypeface() const
//  public SKTypeface GetSkTypeface()
//  {
//	return typeface_;
//  }

//  private SKTypeface typeface_ = new SKTypeface();
//  private List<minikin.FontVariation> variations_ = new List<minikin.FontVariation>();

////C++ TO C# CONVERTER TODO TASK: The following statement was not recognized, possibly due to an unrecognized macro:
//  FML_DISALLOW_COPY_AND_ASSIGN(FontSkia);
//}

//} // namespace FlutterBinding.Txt



//namespace FlutterBinding.Txt
//{
////C++ TO C# CONVERTER NOTE: C# does not allow anonymous namespaces:
////namespace

//} // namespace FlutterBinding.Txt
