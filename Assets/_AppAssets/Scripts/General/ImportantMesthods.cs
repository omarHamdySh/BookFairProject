using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArabicSupport;
using System.Linq;

public static class ImportantMesthods
{
    public static bool CheckIfArabic(string text)
    {
        char[] glyphs = text.ToCharArray();
        foreach (char glyph in glyphs)
        {
            if (glyph >= 0x600 && glyph <= 0x6ff) return true;
            if (glyph >= 0x750 && glyph <= 0x77f) return true;
            if (glyph >= 0xfb50 && glyph <= 0xfc3f) return true;
            if (glyph >= 0xfe70 && glyph <= 0xfefc) return true;
        }
        return false;
    }

    public static string FixRTLForArabic(string text, bool tashkeel = false, bool hinduNumbers = false)
    {
        List<char> convertedText = ArabicFixer.Fix(text, tashkeel, hinduNumbers).ToCharArray().ToList();
        convertedText.Reverse();
        return string.Join("", convertedText.ToArray());
    }
}
