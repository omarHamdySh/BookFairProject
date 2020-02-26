/*
 * Created by ahmed sayd 
 * modified by ahmed bendary
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingBarManger : MonoBehaviour
{
    private bool up;

    [SerializeField] private RectTransform rectComponent;
    [SerializeField] private Image imageComp;

    [SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private float openSpeed = .005f;
    [SerializeField] private float closeSpeed = .01f;

    private void Start()
    {
        up = true;
    }

    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        changeSize();
    }

    private void changeSize()
    {
        float currentSize = imageComp.fillAmount;

        if (currentSize < .30f && up)
        {
            imageComp.fillAmount += openSpeed;
        }
        else if (currentSize >= .30f && up)
        {
            up = false;
        }
        else if (currentSize >= .02f && !up)
        {
            imageComp.fillAmount -= closeSpeed;
        }
        else if (currentSize < .02f && !up)
        {
            up = true;
        }
    }

    public void LoadingBarState(bool state, string text)
    {
        gameObject.SetActive(state);
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }
}
