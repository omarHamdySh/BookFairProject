using TMPro;
using ArabicSupport;
using UnityEngine;

[ExecuteInEditMode]
public class FixInputFieldMeshPro : MonoBehaviour
{
    [SerializeField] private bool tashkeel = true;
    [SerializeField] private bool hinduNumbers = true;

    public TextMeshProUGUI RealText;

    public void OnValueChange(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            RealText.text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
        }
        else
        {
            RealText.text = "";
        }
    }
}
