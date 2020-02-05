using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcasePathHandller_Bendary : MonoBehaviour
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    [HideInInspector] public int currentBookcaseIndex;

    public int IndexOfCurrent;
    public Transform[] bookCasePathPoints;
    public Bookcase_Bendary[] bookcases;

    private float currentScrollSpeed;
    private bool isObjMoving = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.BookCase)
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
    public void Init()
    {
        foreach (Bookcase_Bendary bookcase in bookcases)
        {
            bookcase.Init(this);
        }
    }

    private void MoveAccordingToScrollSpeed()
    {
        foreach (var bookcase in bookcases)
        {
            int nextPosIndex = 0;
            if (currentScrollSpeed < 0)
                nextPosIndex = (bookcase.getObjectIndex() + 1) % bookCasePathPoints.Length;
            else
                nextPosIndex = (bookcase.getObjectIndex() == 0) ? bookCasePathPoints.Length - 1 : bookcase.getObjectIndex() - 1;

            Vector3 newDestination = bookCasePathPoints[nextPosIndex].transform.position;

            if (nextPosIndex == IndexOfCurrent)
            {
                bookcase.ToggleAsCurrent(true);
                currentBookcaseIndex = bookcase.transform.GetSiblingIndex();
            }
            else
            {
                bookcase.ToggleAsCurrent(false);
            }

            bookcase.setObjectIndex(nextPosIndex);
            bookcase.move(newDestination, objectScrollDuration);
        }
    }

    private bool CheckAllObjectsLanded()
    {
        foreach (var scrollable in bookcases)
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
        foreach (var scrollable in bookcases)// Change each scrollable State to --> onDeparture()
        {
            scrollable.onDeparture();
        }
    }
    #endregion
}

