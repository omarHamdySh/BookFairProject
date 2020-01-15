using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamerPath : MonoBehaviour
{
    #region CameraPathMovement
    [SerializeField] private float toFloorSpeed, toBockcaseSpeed, toBookcaseShelfSpeed, toBookcaseBook;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform[] rootWayPoints;

    private bool isRoot = true, isMoving;
    private int row = 0, column = 0, prevRow = 0, prevColumn = 0;
    private float currSpeed = 0;

    private void DecideMovingAccordingToInput()
    {
        if (!isMoving)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && isRoot)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && row < rootWayPoints.Length - 1)
                {
                    row++;
                    isMoving = true;
                    DecideSpeed();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && row > 0)
                {
                    row--;
                    isMoving = true;
                    DecideSpeed();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (Input.GetKeyDown(KeyCode.Return) && column < rootWayPoints[row].childCount - 1)
                {
                    column++;
                    isMoving = true;
                    isRoot = false;
                    DecideSpeed();
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
                }
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
            cameraPos.position = Vector3.Lerp(cameraPos.position, rootWayPoints[row].GetChild(column).position,
                currSpeed * Time.deltaTime);
            if (cameraPos.position == rootWayPoints[row].GetChild(column).position)
            {
                prevColumn = column;
                prevRow = row;
                isMoving = false;
            }
        }
    }
    #endregion

    private void Update()
    {
        DecideMovingAccordingToInput();
        MoveCamera();
    }
}
