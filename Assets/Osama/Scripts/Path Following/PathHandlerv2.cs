using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandlerv2 : MonoBehaviour
{
    [SerializeField] private ObjectAlignerOverPathv2[] objectsOverPath;
    [SerializeField] private ShelfPathTransforms[] shelfPathTransforms;

    #region Private Varibales

    private IScrollable[] scrollables;

    private float currentScrollSpeed;

    private bool motionStarted = false;

    bool x = false;

    #endregion

    #region Getters/Setters



    #endregion

    #region MonoBehaviours

    private void Start()
    {
        scrollables = GetComponentsInChildren<IScrollable>();

        objectsOverPath = GetComponentsInChildren<ObjectAlignerOverPathv2>();
        shelfPathTransforms = GetComponentsInChildren<ShelfPathTransforms>();
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

            if (currentScrollSpeed != 0)
            {
                foreach (var scrollable in scrollables)
                {
                    if (x)
                    {
                        if (currentScrollSpeed > 0)
                        {

                            var nextTransformIndex = (scrollable.getObjectIndex() + 1) % shelfPathTransforms.Length;

                            Vector3 newDestination = shelfPathTransforms[nextTransformIndex].transform.position;

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

                            var nextTransformIndex = ((shelfPathTransforms.Length - 1) + scrollable.getObjectIndex()) % shelfPathTransforms.Length;

                            Vector3 newDestination = shelfPathTransforms[nextTransformIndex].transform.position;

                            Debug.Log("newDestination: " + newDestination);

                            if (scrollable.getLandStatus())
                            {
                                scrollable.setObjectIndex(nextTransformIndex);
                            }

                            scrollable.move(newDestination, -currentScrollSpeed);
                        }
                       
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
    public int clampScrollIndex(int newIndex) {

        if (newIndex < 0)
        {
            return 5;
        }
        else if(newIndex>5) {
            return 0;
        } else {
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