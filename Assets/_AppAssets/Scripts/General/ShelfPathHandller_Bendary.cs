using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfPathHandller_Bendary : MonoBehaviour, IClickable
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    public int currentShelfIndex;
    public bool isCurrentBookcase = false;

    public int IndexOfCurrent, upperDomyIndex, lowerDomyIndex;
    public Transform[] shelfPathPoints;
    public Shelf_Bendary[] shelves;

    private float currentScrollSpeed;
    private bool isObjMoving = false;

    private void Start()
    {
        currentShelfIndex = IndexOfCurrent;
    }

    private void Update()
    {
        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.BookCase && isCurrentBookcase)
        {
            currentScrollSpeed = GameManager.Instance.pathData.BookcaseScrollSpeed;
            if (isObjMoving && currentScrollSpeed == 0)
            {
                isObjMoving = !CheckAllObjectsLanded();
                return;
            }
            else if (!isObjMoving && currentScrollSpeed != 0)
            {
                isObjMoving = true;

                if (CheckAllObjectsLanded())
                {
                    OnDepartureCall();
                    MoveAccordingToScrollSpeed();
                }
            }
        }
    }

    #region Helper
    private void MoveAccordingToScrollSpeed()
    {
        foreach (var shelf in shelves)
        {
            int nextPosIndex = 0;

            if (currentScrollSpeed > 0)
                nextPosIndex = (shelf.getObjectIndex() + 1) % shelfPathPoints.Length;

            if (currentScrollSpeed < 0)
                nextPosIndex = (shelf.getObjectIndex() == 0) ? shelfPathPoints.Length - 1 : shelf.getObjectIndex() - 1;

            Vector3 newDestination = shelfPathPoints[nextPosIndex].transform.position;

            if (nextPosIndex == IndexOfCurrent)
            {
                shelf.ToggleAsCurrent(true);
                currentShelfIndex = shelf.transform.GetSiblingIndex();
            }
            else
            {
                shelf.ToggleAsCurrent(false);

                if ((nextPosIndex == upperDomyIndex && shelf.getObjectIndex() == lowerDomyIndex) ||
                    (nextPosIndex == lowerDomyIndex && shelf.getObjectIndex() == upperDomyIndex))
                {
                    shelf.ToggleLoopingDomy(true);
                }
                else
                {
                    shelf.ToggleLoopingDomy(false);
                }
            }

            shelf.setObjectIndex(nextPosIndex);
            shelf.move(newDestination, objectScrollDuration);
        }
    }

    private bool CheckAllObjectsLanded()
    {
        foreach (var scrollable in shelves)
        {
            if (!scrollable.getLandStatus())
            {
                return false;
            }
        }
        return true;
    }

    private void OnDepartureCall()
    {
        foreach (var scrollable in shelves)// Change each scrollable State to --> onDeparture()
        {
            scrollable.onDeparture();
        }
    }

    public void AwakeCurrent()
    {
        if (isCurrentBookcase)
        {
            foreach (Shelf_Bendary i in shelves)
            {
                i.Init();
            }
        }
    }

    public void SelectThisBookCase()
    {
        SelectionManager.instance.selectThis(this);
    }
    #endregion

    #region Iclickable
    public void focus()
    {
        //GetComponent<BoxCollider>().enabled = false;
        //CameraPath.instance.setTarget(CameraPath.instance.bookcaseNode);
        //CameraPath.instance.gotoTarget();
        //GameManager.Instance.gameplayFSMManager.toShelfState();

    }

    public void unfocus()
    {

        //GetComponent<BoxCollider>().enabled = true;
        //SelectionManager.instance.selectThis(GetComponentInParent<IClickable>());
        //CameraPath.instance.setTarget(CameraPath.instance.floorNode);
        //CameraPath.instance.gotoTarget();
        //GameManager.Instance.gameplayFSMManager.toBookCaseState();
    }
    #endregion
}
