using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandlerv2 : MonoBehaviour
{
    [SerializeField] private ObjectAlignerOverPathv2[] bookcaseOverPath;
    [SerializeField] private ShelfPathTransforms[] bookCasePathTransforms;

    #region Private Varibales

    private Bookcase[] scrollables;

    private float currentScrollSpeed;

    private bool motionStarted = false;

    public bool x = false;

    #endregion

    #region Getters/Setters



    #endregion

    #region MonoBehaviours

    private void Awake()
    {
        scrollables = GetComponentsInChildren<Bookcase>();

        bookcaseOverPath = GetComponentsInChildren<ObjectAlignerOverPathv2>();
        bookCasePathTransforms = GetComponentsInChildren<ShelfPathTransforms>();
    }

    public Vector3 GetPosOverPath(int pathPointIndex)
    {
        return bookCasePathTransforms[pathPointIndex].transform.position;
    }

    private void Update()
    {
        currentScrollSpeed = scrollables[1].getScrollSpeed();// Getting the updating scrolling speed;

        if (motionStarted && currentScrollSpeed == 0)// if the objects is not moving, declare land State and fire land event
        {
            motionStarted = false;

            foreach (var scrollable in scrollables)// Change each scrollable State to --> onLand()
            {
                scrollable.onLand();
            }
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


            if (x)
            {
                foreach (var scrollable in scrollables)
                {

                    if (currentScrollSpeed > 0)
                    {

                        var nextTransformIndex = (scrollable.getObjectIndex() + 1) % bookCasePathTransforms.Length;

                        Vector3 newDestination = bookCasePathTransforms[nextTransformIndex].transform.position;

                        Debug.Log("newDestination: " + newDestination);

                        if (scrollable.getLandStatus())
                        {
                            scrollable.setObjectIndex(nextTransformIndex);
                        }

                        scrollable.move(newDestination, currentScrollSpeed);
                    }

                    else if (currentScrollSpeed < 0)
                    {
                        Debug.Log("else if (currentScrollSpeed < 0)");

                        var nextTransformIndex = (scrollable.getObjectIndex() - 1 < 0) ? bookCasePathTransforms.Length - 1 : scrollable.getObjectIndex() - 1;

                        Vector3 newDestination = bookCasePathTransforms[nextTransformIndex].transform.position;

                        Debug.Log("newDestination: " + newDestination);

                        if (scrollable.getLandStatus())
                        {
                            scrollable.setObjectIndex(nextTransformIndex);
                        }

                        scrollable.move(newDestination, -currentScrollSpeed);
                    }

                }
                x = false;
            }
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