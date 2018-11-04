//using System.Collections.Generic;

//namespace FlutterBinding.Txt
//{
//	public static class GlobalMembers
//	{
//	public static readonly minikin.FontFamily g_null_family;

//	public static hb_blob_t GetTable(HarfBuzzSharp.Face face, hb_tag_t tag, object context)
//	{
////C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
//	  SkTypeface typeface = reinterpret_cast<SkTypeface>(context);

//	  int table_size = typeface.getTableSize(tag);
//	  if (table_size == 0)
//	  {
//		return null;
//	  }
////C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
//	  object buffer = malloc(table_size);
//	  if (buffer == null)
//	  {
//		return null;
//	  }

//	  int actual_size = typeface.getTableData(tag, 0, table_size, buffer);
//	  if (table_size != actual_size)
//	  {
////C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
//		free(buffer);
//		return null;
//	  }
////C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
//	  return hb_blob_create(reinterpret_cast<char>(buffer), table_size, HB_MEMORY_MODE_WRITABLE, buffer, free);
//	}


////C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//	//FontSkia::~FontSkia() = default;

//	internal static void FontSkia_SetSkiaPaint(SKTypeface typeface, SKPaint skPaint, minikin.MinikinPaint paint)
//	{
//	  skPaint.setTypeface(typeface);
//	  skPaint.setTextEncoding(SKPaint.kGlyphID_TextEncoding);
//	  // TODO: set more paint parameters from Minikin
//	  skPaint.setTextSize(paint.size);
//	}

//	public static GlyphTypeface GetGlyphTypeface(minikin.Layout layout, int index)
//	{
//	  FontSkia font = (FontSkia)layout.getFont(index);
//	  return new GlyphTypeface(font.GetSkTypeface(), layout.getFakery(index));
//	}

//	// Return ranges of text that have the same typeface in the layout.
//	public static List<Paragraph.Range<int>> GetLayoutTypefaceRuns(minikin.Layout layout)
//	{
//	  List<Paragraph.Range<int>> result = new List<Paragraph.Range<int>>();
//	  if (layout.nGlyphs() == 0)
//	  {
//		return result;
//	  }
//	  int run_start = 0;
//	  GlyphTypeface run_typeface = GlobalMembers.GetGlyphTypeface(layout, run_start);
//	  for (int i = 1; i < layout.nGlyphs(); ++i)
//	  {
//		GlyphTypeface typeface = GlobalMembers.GetGlyphTypeface(layout, i);
//		if (typeface != run_typeface)
//		{
//		  result.Add(run_start, i);
////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
////ORIGINAL LINE: run_start = i;
//		  run_start.CopyFrom(i);
////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
////ORIGINAL LINE: run_typeface = typeface;
//		  run_typeface.CopyFrom(typeface);
//		}
//	  }
//	  result.Add(run_start, layout.nGlyphs());
//	  return result;
//	}

//	public static int GetWeight(FontWeight weight)
//	{
//	  switch (weight)
//	  {
//		case FontWeight.w100:
//		  return 1;
//		case FontWeight.w200:
//		  return 2;
//		case FontWeight.w300:
//		  return 3;
//		case FontWeight.w400: // Normal.
//		  return 4;
//		case FontWeight.w500:
//		  return 5;
//		case FontWeight.w600:
//		  return 6;
//		case FontWeight.w700: // Bold.
//		  return 7;
//		case FontWeight.w800:
//		  return 8;
//		case FontWeight.w900:
//		  return 9;
//		default:
//		  return -1;
//	  }
//	}

//	public static int GetWeight(TextStyle style)
//	{
//	  return GlobalMembers.GetWeight(style.font_weight);
//	}

//	public static bool GetItalic(TextStyle style)
//	{
//	  switch (style.font_style)
//	  {
//		case FontStyle.italic:
//		  return true;
//		case FontStyle.normal:
//		default:
//		  return false;
//	  }
//	}

//	public static minikin.FontStyle GetMinikinFontStyle(TextStyle style)
//	{
//	  uint language_list_id = string.IsNullOrEmpty(style.locale) ? minikin.FontLanguageListCache.kEmptyListId : minikin.FontStyle.registerLanguageList(style.locale);
////C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
////ORIGINAL LINE: return minikin::FontStyle(language_list_id, 0, GetWeight(style), GetItalic(style));
//	  return new minikin.FontStyle(new uint(language_list_id), 0, GlobalMembers.GetWeight(new TextStyle(style)), GlobalMembers.GetItalic(style));
//	}

//	public static void GetFontAndMinikinPaint(TextStyle style, minikin.FontStyle font, minikin.MinikinPaint paint)
//	{
////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
////ORIGINAL LINE: *font = GetMinikinFontStyle(style);
//	  font.CopyFrom(GlobalMembers.GetMinikinFontStyle(style));
//	  paint.size = style.font_size;
//	  // Divide by font size so letter spacing is pixels, not proportional to font
//	  // size.
//	  paint.letterSpacing = style.letter_spacing / style.font_size;
//	  paint.wordSpacing = style.word_spacing;
//	  paint.scaleX = 1.0f;
//	  // Prevent spacing rounding in Minikin. This causes jitter when switching
//	  // between same text content with different runs composing it, however, it
//	  // also produces more accurate layouts.
//	  paint.paintFlags |= minikin.MinikinPaintFlags.LinearTextFlag;
//	}

//	public static void FindWords(List<UInt16> text, int start, int end, List<Paragraph.Range<int>> words)
//	{
//	  bool in_word = false;
//	  int word_start = new int();
//	  for (int i = start; i < end; ++i)
//	  {
//		bool is_space = minikin.isWordSpace(text[i]);
//		if (!in_word && !is_space)
//		{
////C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
////ORIGINAL LINE: word_start = i;
//		  word_start.CopyFrom(i);
//		  in_word = true;
//		}
//		else if (in_word && is_space)
//		{
//		  words.Add(word_start, i);
//		  in_word = false;
//		}
//	  }
//	  if (in_word)
//	  {
//		words.Add(word_start, end);
//	  }
//	}


//	internal const float kDoubleDecorationSpacing = 3.0f;

////C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//	//Paragraph::~Paragraph() = default;

//	public static string GetDefaultFontFamily()
//	{
//	  return "Arial";
//	}

//	public static string GetDefaultFontFamily()
//	{
//	  return "sans-serif";
//	}
//	}
//}