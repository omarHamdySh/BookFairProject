using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcasePathHandller_Bendary : MonoBehaviour
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    [HideInInspector] public int currentBookcaseIndex;
    [HideInInspector] public int currentRealBookcaseInUse = 0;

    public int IndexOfCurrent;
    public Transform[] bookCasePathPoints;
    public Bookcase_Bendary[] bookcases;
    public Transform[] realBookcases;


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
            int nextPosIndex = 0, indexInUse = currentRealBookcaseInUse;
            if (currentScrollSpeed < 0)
            {
                nextPosIndex = (bookcase.getObjectIndex() + 1) % bookCasePathPoints.Length;
                indexInUse = (indexInUse + 1) % realBookcases.Length;
            }
            else
            {
                nextPosIndex = (bookcase.getObjectIndex() == 0) ? bookCasePathPoints.Length - 1 : bookcase.getObjectIndex() - 1;
                indexInUse = (indexInUse == 0) ? realBookcases.Length - 1 : indexInUse - 1;
            }

            Vector3 newDestination = bookCasePathPoints[nextPosIndex].transform.position;

            if (nextPosIndex == IndexOfCurrent)
            {
                bookcase.ToggleAsCurrent(true);
                currentBookcaseIndex = bookcase.transform.GetSiblingIndex();
                bookcase.ToggleMeshRenderer(false);
                realBookcases[currentRealBookcaseInUse].gameObject.SetActive(false);
                currentRealBookcaseInUse = indexInUse;
            }
            else
            {
                bookcase.ToggleAsCurrent(false);
                bookcase.ToggleMeshRenderer(true);
            }

            //int index = realBookcasesPosIndex.FindIndex(x => x == bookcase.getObjectIndex());


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

