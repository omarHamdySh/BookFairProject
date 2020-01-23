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

    #endregion

    #region Getters/Setters



    #endregion

    #region MonoBehaviours

    private void Start()
    {
        scrollables = GetComponentsInChildren<IScrollable>();
        foreach (var ss in scrollables)
        {
            print("fFFFFF" + ss.GetHashCode() + "dsfsd" + ss.GetType());
            //currentScrollSpeed = ss.getScrollSpeed();
        }
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

        if (currentScrollSpeed > 0)
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
                    if (currentScrollSpeed > 0)
                    {
                        Vector3 newDistenation = shelfPathTransforms[(scrollable.getObjectIndex() + 1) % shelfPathTransforms.Length].transform.position;
                        scrollable.setObjectIndex((scrollable.getObjectIndex() + 1) % shelfPathTransforms.Length);

                        scrollable.move(newDistenation, currentScrollSpeed);
                    }

                    else if (currentScrollSpeed < 0)
                    {
                        Debug.Log("else if (currentScrollSpeed < 0)");
                    }
                }
            }
        }
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