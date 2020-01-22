using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSpeed : MonoBehaviour
{

    [SerializeField]
    private PathDataSO pathData;

    private float swipeTime;
    public float LastSwipeTime;

    public int direction;

    public float scrollSpeed = 0;

    public float distance = 0;

    private Vector2 pivot;

    public float minSpeed;
    public float maxSpeed;

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
        reset();
        pathData.BookcaseScrollSpeed = scrollSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pathData.BookcaseScrollSpeed = scrollSpeed;

        scrollSpeed *= 0.1f;
    }

    public void setDirection(int dir)
    {
        direction = dir;
        calculateTime();
    }

    public void add()
    {
        swipeTime = Time.time;
        pivot = Lean.Touch.LeanTouch.Fingers[0].StartScreenPosition;
    }

    public void calculateTime()
    {
        LastSwipeTime = Time.time - swipeTime;
        distance = Lean.Touch.LeanTouch.Fingers[0].GetScreenDistance(pivot);
        scrollSpeed = Mathf.Clamp(distance/(LastSwipeTime), 20, 50) * direction;
           
        reset();
    }


    public void reset()
    {        
        direction = 0;
        distance = 0;
        LastSwipeTime = 0;
    }
}
