using UnityEngine;
using TMPro;
using ArabicSupport;
using System.Collections.Generic;
using System.Linq;

[ExecuteAlways]
public class FixTextMeshPro : MonoBehaviour
{
    [Multiline] public string text;
    public bool tashkeel = true;
    public bool hinduNumbers = true;

    void Update()
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            if (CheckIfArabic())
            {
                List<char> convertedText = ArabicFixer.Fix(text, tashkeel, hinduNumbers).ToCharArray().ToList();
                gameObject.GetComponent<TMP_Text>().isRightToLeftText = true;
                convertedText.Reverse();
                gameObject.GetComponent<TMP_Text>().text = string.Join("", convertedText.ToArray());

            }
            else
            {
                gameObject.GetComponent<TMP_Text>().isRightToLeftText = false;
                gameObject.GetComponent<TMP_Text>().text = text;
            }
        }
        else
        {
            gameObject.GetComponent<TMP_Text>().text = "";
        }
    }

    private bool CheckIfArabic()
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
}
