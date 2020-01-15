using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    #region DragHorizontal
    [HideInInspector] public bool IsDragged;

    private Vector3 offset;
    private float zCoord;

    private void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMousePos();
    }



    private Vector3 GetMousePos()
    {
        // Pixel Coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // Z Coordinate of game object on screen
        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    #endregion
}
