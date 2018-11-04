using FlutterBinding.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlutterBinding.Engine.Text
{
    // https://github.com/flutter/engine/blob/master/lib/ui/text/paragraph_builder.h
    // https://github.com/flutter/engine/blob/master/lib/ui/text/paragraph_builder.cc

    public class NativeParagraphBuilder
    {

        // TextStyle



        const int tsColorIndex = 1;

        const int tsTextDecorationIndex = 2;

        const int tsTextDecorationColorIndex = 3;

        const int tsTextDecorationStyleIndex = 4;

        const int tsFontWeightIndex = 5;

        const int tsFontStyleIndex = 6;

        const int tsTextBaselineIndex = 7;

        const int tsFontFamilyIndex = 8;

        const int tsFontSizeIndex = 9;

        const int tsLetterSpacingIndex = 10;

        const int tsWordSpacingIndex = 11;

        const int tsHeightIndex = 12;

        const int tsLocaleIndex = 13;

        const int tsBackgroundIndex = 14;

        const int tsForegroundIndex = 15;

        const int tsTextShadowsIndex = 16;



        const int tsColorMask = 1 << tsColorIndex;

        const int tsTextDecorationMask = 1 << tsTextDecorationIndex;

        const int tsTextDecorationColorMask = 1 << tsTextDecorationColorIndex;

        const int tsTextDecorationStyleMask = 1 << tsTextDecorationStyleIndex;

        const int tsFontWeightMask = 1 << tsFontWeightIndex;

        const int tsFontStyleMask = 1 << tsFontStyleIndex;

        const int tsTextBaselineMask = 1 << tsTextBaselineIndex;

        const int tsFontFamilyMask = 1 << tsFontFamilyIndex;

        const int tsFontSizeMask = 1 << tsFontSizeIndex;

        const int tsLetterSpacingMask = 1 << tsLetterSpacingIndex;

        const int tsWordSpacingMask = 1 << tsWordSpacingIndex;

        const int tsHeightMask = 1 << tsHeightIndex;

        const int tsLocaleMask = 1 << tsLocaleIndex;

        const int tsBackgroundMask = 1 << tsBackgroundIndex;

        const int tsForegroundMask = 1 << tsForegroundIndex;

        const int tsTextShadowsMask = 1 << tsTextShadowsIndex;



        // ParagraphStyle



        const int psTextAlignIndex = 1;

        const int psTextDirectionIndex = 2;

        const int psFontWeightIndex = 3;

        const int psFontStyleIndex = 4;

        const int psMaxLinesIndex = 5;

        const int psFontFamilyIndex = 6;

        const int psFontSizeIndex = 7;

        const int psLineHeightIndex = 8;

        const int psEllipsisIndex = 9;

        const int psLocaleIndex = 10;



        const int psTextAlignMask = 1 << psTextAlignIndex;

        const int psTextDirectionMask = 1 << psTextDirectionIndex;

        const int psFontWeightMask = 1 << psFontWeightIndex;

        const int psFontStyleMask = 1 << psFontStyleIndex;

        const int psMaxLinesMask = 1 << psMaxLinesIndex;

        const int psFontFamilyMask = 1 << psFontFamilyIndex;

        const int psFontSizeMask = 1 << psFontSizeIndex;

        const int psLineHeightMask = 1 << psLineHeightIndex;

        const int psEllipsisMask = 1 << psEllipsisIndex;

        const int psLocaleMask = 1 << psLocaleIndex;



        // TextShadows decoding

        const uint kColorDefault = 0xFF000000;

        const uint kBytesPerShadow = 16;

        const uint kShadowPropertiesCount = 4;

        const uint kColorOffset = 0;

        const uint kXOffset = 1;

        const uint kYOffset = 2;

        const uint kBlurOffset = 3;

        //Txt.ParagraphBuilder m_paragraphBuilder;

        protected NativeParagraphBuilder(List<int> encoded,
                         string fontFamily,
                         double fontSize,
                         double lineHeight,
                         string ellipsis,
                         string locale)
        {

            //int mask = encoded[0];

            //Txt.ParagraphStyle style;

            //if (mask & psTextAlignMask)

            //    style.text_align = TextAlign(encoded[psTextAlignIndex]);



            //if (mask & psTextDirectionMask)

            //    style.text_direction = txt::TextDirection(encoded[psTextDirectionIndex]);



            //if (mask & psFontWeightMask)

            //    style.font_weight =

            //        static_cast<txt::FontWeight>(encoded[psFontWeightIndex]);



            //if (mask & psFontStyleMask)

            //    style.font_style = static_cast<txt::FontStyle>(encoded[psFontStyleIndex]);



            //if (mask & psFontFamilyMask)

            //    style.font_family = fontFamily;



            //if (mask & psFontSizeMask)

            //    style.font_size = fontSize;



            //if (mask & psLineHeightMask)

            //    style.line_height = lineHeight;



            //if (mask & psMaxLinesMask)

            //    style.max_lines = encoded[psMaxLinesIndex];



            //if (mask & psEllipsisMask)

            //    style.ellipsis = ellipsis;



            //if (mask & psLocaleMask)

            //    style.locale = locale;



            //Txt.FontCollection font_collection =

            //    UIDartState::Current()->window()->client()->GetFontCollection();

            //m_paragraphBuilder = new Txt.ParagraphBuilder(
            //    style, font_collection.GetFontCollection());

        }  // namespace blink


        string _text = "";
        protected string AddText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            _text += text;
            
            // TODO:
            //m_paragraphBuilder->AddText(text);

            return null;
        }

        protected Paragraph Build()
        {
            return new Paragraph() { Text = _text };
        }
    }
}
