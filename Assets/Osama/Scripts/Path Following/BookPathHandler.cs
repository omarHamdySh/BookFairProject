﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPathHandler : MonoBehaviour
{
    public BookObjectAlignerOverPath[] booksOverPath;
    public BookPathTransforms[] bookPathTransforms;

    #region Private Varibales

    private Book[] scrollables;

    private float currentScrollSpeed;

    private bool motionStarted = false;

    public bool x = false;

    #endregion

    #region Getters/Setters

    public NodeRank GetRank(int index)
    {
        return bookPathTransforms[index].GetComponent<NodeRank>();
    }

    #endregion

    #region MonoBehaviours

    private void Awake()
    {
        scrollables = GetComponentsInChildren<Book>();

        booksOverPath = GetComponentsInChildren<BookObjectAlignerOverPath>();
        bookPathTransforms = GetComponentsInChildren<BookPathTransforms>();
    }

    public Vector3 GetPosOverPath(int pathPointIndex)
    {
        return bookPathTransforms[pathPointIndex].transform.position;
    }

    private void Update()
    {
        currentScrollSpeed = GameManager.Instance.pathData.BookScrollSpeed;// Getting the updating scrolling speed;
        print(currentScrollSpeed);
        if (motionStarted && currentScrollSpeed == 0)// if the objects is not moving, declare land State and fire land event
        {
            foreach (var scrollable in scrollables)
            {
                if (!scrollable.getLandStatus())
                {
                    motionStarted = true;
                    return;
                }
            }
            motionStarted = false;

            //foreach (var scrollable in scrollables)// Change each scrollable State to --> onLand()
            //{
            //    scrollable.onLand();
            //}
            return;
        }
        else if (currentScrollSpeed == 0)// If scrolling speed reaches 0, return to skip frame
        {
            return;
        }


        if (currentScrollSpeed != 0)
        {
            if (!motionStarted)
            {
                motionStarted = true;

                foreach (var scrollable in scrollables)// Change each scrollable State to --> onDeparture()
                {
                    scrollable.onDeparture();
                }
            }

            foreach (var scrollable in scrollables)
            {
                scrollable.onMoving();// // Change each scrollable State to --> onMoving()
            }

            moveAccordingToScrollSpeed();

        }
    }


    /// <summary>
    /// This method clamps the new index that you need to scroll to, just put the current index + or - 1 to scroll forward or backward
    /// </summary>
    /// <param name="newIndex"></param>
    /// <returns></returns>
    public int clampScrollIndex(int newIndex)
    {

        if (newIndex < 0)
        {
            return 5;
        }
        else if (newIndex > 5)
        {
            return 0;
        }
        else
        {
            return newIndex;
        }
    }

    [ContextMenu("sdfsfdds")]
    private void turnx()
    {
        x = true;
    }
    #endregion

    private void moveAccordingToScrollSpeed()
    {
        foreach (var scrollable in scrollables)
        {
            if (!scrollable.getLandStatus())
            {
                motionStarted = true;
                return;
            }
        }

        foreach (var scrollable in scrollables)
        {
            int nextTransformIndex = 0;

            if (currentScrollSpeed > 0)
                nextTransformIndex = (scrollable.getObjectIndex() + 1) % bookPathTransforms.Length;

            if (currentScrollSpeed < 0)
                nextTransformIndex = (scrollable.getObjectIndex() == 0) ? bookPathTransforms.Length - 1 : scrollable.getObjectIndex() - 1;

            Vector3 newDestination = bookPathTransforms[nextTransformIndex].transform.position;
            //Debug.Log("newDestination: " + newDestination);

            if (scrollable.getLandStatus())
            {
                scrollable.setObjectIndex(nextTransformIndex);
                scrollable.move(newDestination, 0.5f);
            }
        }

    }
    //[ContextMenu("Another cycle of replacing the objects")]
    //private void alignShelvesOverPath()
    //{
    //    objectsOverPath = GetComponentsInChildren<ObjectAlignerOverPathv2>();
    //    shelfPathTransforms = GetComponentsInChildren<ShelfPathTransforms>();

    //    foreach (var obj in objectsOverPath)
    //    {
    //        if (direaction > 0)
    //        {
    //            Vector3 newDistenation = shelfPathTransforms[(obj.ObjectIndex + 1) % shelfPathTransforms.Length].transform.position;
    //            obj.ObjectIndex = (obj.ObjectIndex + 1) % shelfPathTransforms.Length;
    //            obj.DOMOve(newDistenation);
    //        }
    //        else if (direaction < 0)
    //        {

    //        }
    //    }
    //}
}