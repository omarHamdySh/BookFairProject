using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSpeed : MonoBehaviour
{
    private float swipeTime;
    public float LastSwipeTime;

    public int direction;

    public float scrollSpeed = 1;

    public float distance = 0;

    private Vector2 pivot;

    #region Singleton
    public static SwipeSpeed instance { private set; get; }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void setDirection(int dir)
    {
        direction = dir;
        calculateTime();
    }

    public void add()
    {
        swipeTime = Time.time;
        pivot = Lean.Touch.LeanTouch.Fingers[0].ScreenPosition;
    }

    public void calculateTime()
    {
        LastSwipeTime = Time.time - swipeTime;
        distance = Lean.Touch.LeanTouch.Fingers[0].GetScaledDistance(pivot);
        scrollSpeed = distance/(LastSwipeTime*100)*direction;

        reset();
    }
    public void reset()
    {
        swipeTime = 0;
        direction = 0;
        distance = 0;
        LastSwipeTime = 0;
    }
}
