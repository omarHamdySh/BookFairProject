﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSpeed : MonoBehaviour
{

   
    private PathDataSO pathData;

    private float swipeTime;
    public float LastSwipeTime;

    public int horizontalDirection;
    public int verticalDirection;

    public float horizontalScrollSpeed;
    public float verticalScrollSpeed;

    public float distance = 0;

    private Vector2 pivot;

    public float minSpeed;
    public float maxSpeed;

    //drag objeccts while finger is pressed
    public bool drag;


    public float dragDecay;
    public float scrollDecay;

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
        pathData = GameManager.Instance.pathData;
        reset();
        horizontalScrollSpeed = 0.1f;
        verticalScrollSpeed = 0.1f;
        drag = false;
    }

    private void Update()
    {
        if (drag)
        {
            Vector2 tmpFingerPos = Lean.Touch.LeanTouch.Fingers[0].ScreenPosition;
            horizontalScrollSpeed = (tmpFingerPos.x - pivot.x) * dragDecay;
            verticalScrollSpeed = (tmpFingerPos.y - pivot.y) * dragDecay;
        }
        if (Mathf.Abs(horizontalScrollSpeed) > 0 || Mathf.Abs(verticalScrollSpeed) > 0)
        {
            decay();
        }
    }

    public void setHDirection(int dir)
    {
        horizontalDirection = dir;
        calculateTimeHorizontal();
    }

    public void setVDirection(int dir)
    {
        horizontalDirection = dir;
        calculateTimeVertical();
    }
    public void add()
    {
        drag = true;
        swipeTime = Time.time;
        pivot = Lean.Touch.LeanTouch.Fingers[0].StartScreenPosition;
    }

    
    //move objects horizontally
    public void calculateTimeHorizontal()
    {
        LastSwipeTime = Time.time - swipeTime;
        distance = Lean.Touch.LeanTouch.Fingers[0].GetScreenDistance(pivot);
        horizontalScrollSpeed = (distance/LastSwipeTime)/20 * horizontalDirection;

        reset();
    }

    //move objects horizontally
    public void calculateTimeVertical()
    {
        LastSwipeTime = Time.time - swipeTime;
        distance = Lean.Touch.LeanTouch.Fingers[0].GetScreenDistance(pivot);
        verticalScrollSpeed = (distance / LastSwipeTime) / 20 * verticalDirection;

        reset();
    }

    public void reset()
    {        
        horizontalDirection = 0;
        verticalDirection = 0;
        distance = 0;
        LastSwipeTime = 0;
        drag = false;
    }

    public void decay()
    {
        float hSpeed = Mathf.Abs(horizontalScrollSpeed);
        float vSpeed = Mathf.Abs(horizontalScrollSpeed);
        if (hSpeed < 0.7f) horizontalScrollSpeed = 0;
        if (vSpeed < 0.7f) horizontalScrollSpeed = 0;
        horizontalScrollSpeed *= scrollDecay;
        verticalScrollSpeed *= scrollDecay;
    }
}
