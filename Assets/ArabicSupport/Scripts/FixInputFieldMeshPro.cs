using TMPro;
using ArabicSupport;
using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class FixInputFieldMeshPro : MonoBehaviour
{
    public bool tashkeel = true;
    public bool hinduNumbers = true;
    public string text = "";

    public TMP_InputField inputField;
    public TextMeshProUGUI t;

    private void Update()
    {
        if (!text.Equals(inputField.text))
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                text = inputField.text;
                inputField.textComponent.text = ArabicFixer.Fix(inputField.text, tashkeel, hinduNumbers);
                print(ArabicFixer.Fix(text, tashkeel, hinduNumbers));
            }
        }
    }

    public void FixTextForUI()
    {
        //    string rtlText = ArabicSupport.ArabicFixer.Fix(text, tashkeel, hinduNumbers);
        //    rtlText = rtlText.Replace("\r", ""); // the Arabix fixer Return \r\n for everyy \n .. need to be removed

        //    string finalText = "";
        //    string[] rtlParagraph = rtlText.Split('\n');

        //    inputField.textComponent.text = "";
        //    for (int lineIndex = 0; lineIndex < rtlParagraph.Length; lineIndex++)
        //    {
        //        string[] words = rtlParagraph[lineIndex].Split(' ');
        //        Array.Reverse(words);
        //        inputField.textComponent.text = string.Join(" ", words);

        //        Canvas.ForceUpdateCanvases();
        //        for (int i = 0; i < inputField.textComponent.text.cachedTextGenerator.lines.Count; i++)
        //        {
        //            int startIndex = inputField.textComponent.cachedTextGenerator.lines[i].startCharIdx;
        //            int endIndex = (i == txt.cachedTextGenerator.lines.Count - 1) ? inputField.textComponent.text.Length
        //                : txt.cachedTextGenerator.lines[i + 1].startCharIdx;
        //            int length = endIndex - startIndex;

        //            string[] lineWords = txt.text.Substring(startIndex, length).Split(' ');
        //            Array.Reverse(lineWords);

        //            finalText = finalText + string.Join(" ", lineWords).Trim() + "\n";
        //        }
        //    }
        //    inputField.textComponent.text = finalText.TrimEnd('\n');
    }
}
