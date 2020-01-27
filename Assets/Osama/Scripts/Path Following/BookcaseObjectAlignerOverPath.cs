using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BookcaseObjectAlignerOverPath : MonoBehaviour
{
    [SerializeField] private BookcasePathHandler pathHandler;

    private Bookcase scrollable;
    private float currentScrollSpeed;
    private bool motionStarted = false;

    private void Start()
    {
        scrollable = GetComponent<Bookcase>();
        scrollable.setObjectIndex(transform.GetSiblingIndex());
        transform.position = pathHandler.GetPosOverPath(transform.GetSiblingIndex());
    }

    private void Update()
    {
        //currentScrollSpeed = scrollable.getScrollSpeed();

        //print(currentScrollSpeed);
        //if (motionStarted && currentScrollSpeed == 0) // If the object is not moving, declare land State and fire land event
        //{
        //    motionStarted = false;
        //    scrollable.onLand();
        //    return;
        //}
        //else if (currentScrollSpeed == 0)
        //{
        //    return;
        //}

        //if (currentScrollSpeed > 0)
        //{
        //    if (!motionStarted)
        //    {
        //        motionStarted = true;
        //        scrollable.onDeparture();
        //    }

        //    scrollable.onMoving();
        //}
    }

    //#region Private Varibales

    //private int objectIndex;// object index in the array of the objects (Shelves, Books)

    //private IScrollable scrollable;

    //private float currentScrollSpeed;

    //private bool motionStarted = false;

    //#endregion

    //#region Getters/Setters

    //public int ObjectIndex { get => objectIndex; set => objectIndex = value; }

    //#endregion

    //#region MonoBehaviours

    //private void Awake()
    //{
    //    objectIndex = transform.GetSiblingIndex();// Getting the object index in the array by it's order as child
    //}

    //private void Start()
    //{
    //    scrollable = GetComponent<IScrollable>();
    //}

    //private void Update()
    //{
    //    currentScrollSpeed = scrollable.getScrollSpeed();// Getting the updating scrolling speed;

    //    if (motionStarted && currentScrollSpeed == 0)// if hte object is not moving, declare land State and fire land event
    //    {
    //        motionStarted = false;
    //        scrollable.onLand();
    //        return;
    //    }

    //    else if(currentScrollSpeed == 0)
    //    {
    //        return;
    //    }

    //    if (currentScrollSpeed > 0)
    //    {
    //        if (!motionStarted)
    //        {
    //            motionStarted = true;
    //            scrollable.onDeparture();
    //        }

    //        scrollable.onMoving();
    //    }
    //}

    //    #endregion

    //    #region public Methods

    //    public void DOMOve(Vector3 destination)
    //    {
    //        transform.DOMove(destination,3);
    //    }

    //    #endregion
}
