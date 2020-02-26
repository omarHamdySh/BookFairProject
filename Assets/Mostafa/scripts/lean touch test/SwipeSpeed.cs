using System.Collections;
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

    private Vector2 swipePivot;
    private Vector2 dragPivot;

    public float minSpeed;
    public float maxSpeed;

    //drag objeccts while finger is pressed
    public bool drag;

    public bool swipeEnabled = true;

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
        horizontalScrollSpeed = 0f;
        verticalScrollSpeed = 0f;
        drag = false;
    }

    private void Update()
    {
        swipeEnabled = !CameraPath.instance.cameraMoving;
        if (drag && swipeEnabled)
        {
            Vector2 tmpFingerPos = Lean.Touch.LeanTouch.Fingers[0].ScreenPosition;
            horizontalScrollSpeed = (tmpFingerPos.x - dragPivot.x) * dragDecay;
            verticalScrollSpeed = (tmpFingerPos.y - dragPivot.y) * dragDecay;
            dragPivot = tmpFingerPos;
        }
        if ((Mathf.Abs(horizontalScrollSpeed) > 0 || Mathf.Abs(verticalScrollSpeed) > 0) && swipeEnabled)
        {
            decay();
        }

        if (swipeEnabled )
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                verticalScrollSpeed = Input.mouseScrollDelta.y;
                horizontalScrollSpeed = Input.mouseScrollDelta.y;
            }

            if (Input.GetKeyDown("left"))
            {
                horizontalScrollSpeed -= 1;
            }

            if (Input.GetKeyDown("right"))
            {
                horizontalScrollSpeed += 1;
            }

            if (Input.GetKeyDown("down"))
            {
                verticalScrollSpeed -= 1;
            }

            if (Input.GetKeyDown("up"))
            {
                verticalScrollSpeed += 1;
            }
        }

    }

    public void setHDirection(int dir)
    {
        if (swipeEnabled)
        {
            horizontalDirection = dir;
            calculateTimeHorizontal();
        }
    }

    public void setVDirection(int dir)
    {
        if (swipeEnabled)
        {
            horizontalDirection = dir;
            calculateTimeVertical();
        }
    }
    public void add()
    {
        drag = true;
        swipeTime = Time.time;
        swipePivot = Lean.Touch.LeanTouch.Fingers[0].StartScreenPosition;
        dragPivot = Lean.Touch.LeanTouch.Fingers[0].StartScreenPosition; 
    }

    
    //move objects horizontally
    public void calculateTimeHorizontal()
    {
        LastSwipeTime = Time.time - swipeTime;
        distance = Lean.Touch.LeanTouch.Fingers[0].GetScreenDistance(swipePivot);
        horizontalScrollSpeed = (distance/LastSwipeTime)/20 * horizontalDirection;

        reset();
    }

    //move objects horizontally
    public void calculateTimeVertical()
    {
        LastSwipeTime = Time.time - swipeTime;
        distance = Lean.Touch.LeanTouch.Fingers[0].GetScreenDistance(swipePivot);
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
        float vSpeed = Mathf.Abs(verticalScrollSpeed);
        if (hSpeed < 0.7f) horizontalScrollSpeed = 0;
        if (vSpeed < 0.7f) verticalScrollSpeed = 0;
        horizontalScrollSpeed *= scrollDecay;
        verticalScrollSpeed *= scrollDecay;
    }
}
