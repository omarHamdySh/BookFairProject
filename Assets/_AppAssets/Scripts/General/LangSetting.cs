﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LangSetting : MonoBehaviour
{
    [Multiline] [SerializeField] private string arText;
    [Multiline] [SerializeField] private string enText;
    [SerializeField] private bool enableAlignment;

    private FixTextMeshPro fixTextMeshPro;
    private FixText fixText;

    private void Start()
    {
        fixTextMeshPro = GetComponent<FixTextMeshPro>();
        fixText = GetComponent<FixText>();
        //if (SettingManger.Instance)
        //{
        //    SettingManger.Instance.LangSettings.Add(this);
        //}

        if (!PlayerPrefs.HasKey(ImportantStrings.langPPKey))
        {
            PlayerPrefs.SetString(ImportantStrings.langPPKey, ImportantStrings.arabicPPValue);
        }

        if (fixTextMeshPro)
        {
            fixTextMeshPro.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? arText : enText;
            if (enableAlignment)
            {
                fixTextMeshPro.GetComponent<TextMeshProUGUI>().alignment = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
            }
        }
        else if (fixText)
        {
            fixText.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? arText : enText;
            if (enableAlignment)
            {
                fixText.GetComponent<Text>().alignment = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
            }
        }
    }

    public void OnLangStateChanged(string lang)
    {
        switch (lang)
        {
            case ImportantStrings.arabicPPValue:
                if (fixTextMeshPro)
                    fixTextMeshPro.text = arText;
                else
                    fixText.text = arText;
                break;
            case ImportantStrings.englishPPValue:
                if (fixTextMeshPro)
                    fixTextMeshPro.text = enText;
                else
                    fixText.text = enText;
                break;
        }
    }

    //public void ChangeTxt(string arText,string enText)
    //{
    //    this.
    //}
}
