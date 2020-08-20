using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEventTrigger : MonoBehaviour
{
    //private string _buttonName;
    private TMPro.TextMeshProUGUI _buttonTitle;

    public float TargetButtonScale = 1.5f;

    public void Start()
    {
        //_buttonName  = gameObject.name;
        _buttonTitle = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void OnButtonHovered()
    {
        //Debug.Log(_buttonName + " is hoverd !");
        _buttonTitle.enabled = true;
        //transform.localScale = new Vector3(TargetButtonScale.x, TargetButtonScale.y, TargetButtonScale.z);
        gameObject.GetComponent<RectTransform>().sizeDelta *= TargetButtonScale;
    }

    public void OnButtonNotHovered()
    {
        //Debug.Log(_buttonName + " is not hovered anymore !");
        _buttonTitle.enabled = false;
        //transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<RectTransform>().sizeDelta /= TargetButtonScale;
    }

}
