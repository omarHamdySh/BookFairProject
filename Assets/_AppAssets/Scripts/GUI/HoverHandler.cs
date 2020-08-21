using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject button;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button)
            button.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button)
            button.SetActive(false);
    }
}
