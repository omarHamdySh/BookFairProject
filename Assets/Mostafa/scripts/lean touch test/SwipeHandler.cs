using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class SwipeHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swipedLeft()
    {
        Debug.Log("swipe has left");
    }

    public void swipedRight()
    {
        Debug.Log("swipe has right");
    }
}
