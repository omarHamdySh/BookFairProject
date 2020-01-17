using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class SwipeHandler : MonoBehaviour
{

    public void swipedLeft()
    {
        Debug.Log("swipe has left");
    }

    public void swipedRight()
    {
        Debug.Log("swipe has right");
    }
}
