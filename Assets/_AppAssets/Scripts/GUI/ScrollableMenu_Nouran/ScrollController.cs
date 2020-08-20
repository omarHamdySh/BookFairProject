using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{

    public float ScrollSpeed;
    public GameObject scrollbar;


    public void ScrollTolLeft()
    {
        if(scrollbar.GetComponent<Scrollbar>().value < 1)
           scrollbar.GetComponent<Scrollbar>().value += ScrollSpeed;
    }

    public void ScrollToRight()
    {
        if (scrollbar.GetComponent<Scrollbar>().value > 0)
            scrollbar.GetComponent<Scrollbar>().value -= ScrollSpeed;
    }


}
