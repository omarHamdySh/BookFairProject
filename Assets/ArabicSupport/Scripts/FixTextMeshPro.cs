using UnityEngine;
using TMPro;

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
            if (ImportantMesthods.CheckIfArabic(text))
            {
                gameObject.GetComponent<TMP_Text>().isRightToLeftText = true;
                gameObject.GetComponent<TMP_Text>().text = ImportantMesthods.FixRTLForArabic(text, tashkeel, hinduNumbers);
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

    public void SetTextColor(Color color)
    {
        gameObject.GetComponent<TMP_Text>().color = color;
    }

}
