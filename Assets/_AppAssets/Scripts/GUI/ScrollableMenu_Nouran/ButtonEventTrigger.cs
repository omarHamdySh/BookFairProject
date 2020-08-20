using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEventTrigger : MonoBehaviour
{
    private string _buttonName;
    private Text _buttonTitle;

    public Vector2 TargetButtonScale;

    public void Start()
    {
        _buttonName  = gameObject.name;
        _buttonTitle = gameObject.transform.GetChild(1).GetComponent<Text>();
    }

    public void OnButtonHovered()
    {
        //Debug.Log(_buttonName + " is hoverd !");
        _buttonTitle.enabled = true;
        //transform.localScale = new Vector3(TargetButtonScale.x, TargetButtonScale.y, TargetButtonScale.z);
        gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(TargetButtonScale.x, TargetButtonScale.y);
    }

    public void OnButtonNotHovered()
    {
        //Debug.Log(_buttonName + " is not hovered anymore !");
        _buttonTitle.enabled = false;
        //transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(TargetButtonScale.x, TargetButtonScale.y);
    }

}
