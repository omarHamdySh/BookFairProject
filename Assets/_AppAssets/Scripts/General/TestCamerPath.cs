using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TestCamerPath : MonoBehaviour
{
    #region CameraPathMovement
    [SerializeField] private float toFloorSpeed, toBockcaseSpeed, toBookcaseShelfSpeed, toBookcaseBook;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform[] rootWayPoints;
    [SerializeField] private float animationDelay;

    [HideInInspector] public bool isGettingFocus = false;

    private bool isRoot = true, isMoving;
    private int row = 0, column = 0, prevRow = 0, prevColumn = 0;
    private float currSpeed = 0;

    private void DecideMovingAccordingToInput()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) && isRoot)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && row < rootWayPoints.Length - 1)
                {
                    row++;
                    isMoving = true;
                    DecideSpeed();
                    MoveCamera();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && row > 0)
                {
                    row--;
                    isMoving = true;
                    DecideSpeed();
                    MoveCamera();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || isGettingFocus)
            {
                if ((Input.GetKeyDown(KeyCode.Return) || isGettingFocus) && column < rootWayPoints[row].childCount - 1)
                {
                    column++;
                    isMoving = true;
                    isRoot = false;
                    DecideSpeed();
                    MoveCamera();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && column > 0)
                {
                    column--;
                    isMoving = true;
                    DecideSpeed();
                    if (column == 0)
                    {
                        isRoot = true;
                    }
                    MoveCamera();
                }
                isGettingFocus = false;
            }
        }
    }

    private void DecideSpeed()
    {
        if (row != prevRow)
        {// Move from floor to floor
            currSpeed = toFloorSpeed;
        }
        else if ((prevColumn == 0 && column == 1) || (prevColumn == 1 && column == 0))
        {// Move from floor to bookcase or vice versa
            currSpeed = toBockcaseSpeed;
        }
        else if ((prevColumn == 1 && column == 2) || (prevColumn == 2 && column == 1))
        {// Move from bookcase to bookcase shelf or vice versa
            currSpeed = toBookcaseShelfSpeed;
        }
        else if ((prevColumn == 2 && column == 3) || (prevColumn == 3 && column == 2))
        {// Move from bookcase shelf to bookcase book or vice versa
            currSpeed = toBookcaseBook;
        }
    }

    private void MoveCamera()
    {
        if (isMoving)
        {
            cameraPos.DOMove(rootWayPoints[row].GetChild(column).position, animationDelay).OnComplete(OnFinishMoving);
            //cameraPos.position = Vector3.Lerp(cameraPos.position, rootWayPoints[row].GetChild(column).position,
            //    currSpeed * Time.deltaTime);
            //if (cameraPos.position == rootWayPoints[row].GetChild(column).position)
            //{
            //    prevColumn = column;
            //    prevRow = row;
            //    isMoving = false;
            //}
        }
    }

    private void OnFinishMoving()
    {
        prevColumn = column;
        prevRow = row;
        isMoving = false;
    }

    #endregion
    public void moveUp()
    {
        print("suppose to move up");
        if (!isMoving)
        {
            if (isRoot)
            {
                if (row < rootWayPoints.Length - 1)
                {
                    row++;
                    isMoving = true;
                    DecideSpeed();
                    MoveCamera();
                }

            }
        }
    }

    public void moveDown()
    {
        print("suppose to move down");
        if (!isMoving)
        {
            if (isRoot)
            {
                if (row > 0)
                {
                    row--;
                    isMoving = true;
                    DecideSpeed();
                    MoveCamera();
                                   }
            }
        }
    }

    /// <summary>
    /// This method for moving right in any list of object
    /// </summary>
    /// <param name="objectListCount">list of object count</param>
    /// <param name="currentIndex">Index of the current Selected</param>
    public void MoveRight(int objectListCount, ref int currentIndex)
    {
        if (!isMoving)
        {
            currentIndex = (currentIndex + 1) % objectListCount;
        }
    }

    /// <summary>
    /// This method for moving left in any list of object
    /// </summary>
    /// <param name="objectListCount">list of object count</param>
    /// <param name="currentIndex">Index of the current Selected</param>
    public void MoveLeft(int objectListCount, ref int currentIndex)
    {
        if (!isMoving)
        {
            currentIndex = (currentIndex - 1 < 0) ? objectListCount - 1 : currentIndex - 1;
        }
    }

    private void Update()
    {
        DecideMovingAccordingToInput();
        //MoveCamera();
    }



}
