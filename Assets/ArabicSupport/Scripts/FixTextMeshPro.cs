using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class FixTextMeshPro : MonoBehaviour
{
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private TMP_Text myText;
    [Multiline] public string text;
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
        this.text = text;
    }

    public void SetTextColor(Color color)
    {
        myText.color = color;
    }

    public void ToggleCanvasContainer(bool enabled)
    {
        if (myCanvas)
        {
            myCanvas.enabled = enabled;
        }
    }

    public bool IsCanvasEnabled()
    {
        if (myCanvas)
        {
            return myCanvas.enabled;
        }

        return false;
    }
}
