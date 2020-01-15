using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamerPath : MonoBehaviour
{
    [SerializeField] private float speedToMove;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform[] rootWayPoints;

    [SerializeField] private bool isRoot = true, isMoving;
    [SerializeField] private int row = 0, column = 0;

    private void Update()
    {
        if (!isMoving)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && isRoot)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && row < rootWayPoints.Length - 1)
                {
                    row++;
                    isMoving = true;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && row > 0)
                {
                    row--;
                    isMoving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (Input.GetKeyDown(KeyCode.Return) && column < rootWayPoints[row].childCount - 1)
                {
                    column++;
                    isMoving = true;
                    isRoot = false;
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && column > 0)
                {
                    column--;
                    isMoving = true;
                    if (column == 0)
                    {
                        isRoot = true;
                    }
                }
            }
        }
        else if (isMoving)
        {
            cameraPos.position = Vector3.Lerp(cameraPos.position, rootWayPoints[row].GetChild(column).position, speedToMove * Time.deltaTime);
            if (cameraPos.position == rootWayPoints[row].GetChild(column).position)
            {
                isMoving = false;
            }
        }
    }
}
