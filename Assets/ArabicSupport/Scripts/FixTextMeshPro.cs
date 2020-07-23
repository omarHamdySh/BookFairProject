using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class FixTextMeshPro : MonoBehaviour
{
    [SerializeField] private TMP_Text myText;
    [SerializeField] [Multiline] private string text;
    public bool tashkeel = true;
    public bool hinduNumbers = true;

    private void Awake()
    {
        myText = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            SetText(text);
        }
    }

    public void SetText(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            if (ImportantMesthods.CheckIfArabic(text))
            {
                myText.isRightToLeftText = true;
                myText.text = ImportantMesthods.FixRTLForArabic(text, tashkeel, hinduNumbers);
            }
            else
            {
                myText.isRightToLeftText = false;
                myText.text = text;
            }
        }
        else
        {
            myText.text = "";
        }
    }

    public void SetTextColor(Color color)
    {
        gameObject.GetComponent<TMP_Text>().color = color;
    }
}
