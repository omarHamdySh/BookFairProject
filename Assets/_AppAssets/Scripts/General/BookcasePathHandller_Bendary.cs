using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Awake()
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
    [ContextMenu("Set Bookcases on the path")]
    public void Init()
    {
        foreach (Bookcase_Bendary bookcase in bookcases)
        {
            bookcase.Init(this);
        }

        for (int i = 0; i < realBookcases.Length; i++)
        {
            if (i != currentRealBookcaseInUse)
            {
                ToggleTexts(false, i);
            }
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
                ToggleCurrentRealBookcaseMeshRenderer(false);
                currentRealBookcaseInUse = indexInUse;

                realBookcases[currentBookcaseIndex].GetComponent<ShelfPathHandller_Bendary>().isCurrentBookcase = true;
                ToggleTexts(true);
            }
            else
            {
                bookcase.ToggleAsCurrent(false);
                bookcase.ToggleMeshRenderer(true);

                realBookcases[currentBookcaseIndex].GetComponent<ShelfPathHandller_Bendary>().isCurrentBookcase = false;
                ToggleTexts(false);
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

    public void ToggleCurrentRealBookcaseMeshRenderer(bool enabled)
    {
        foreach (MeshRenderer i in realBookcases[currentRealBookcaseInUse].GetComponentsInChildren<MeshRenderer>())
        {
            i.enabled = enabled;
        }
    }

    public void ToggleTexts(bool enabled)
    {
        foreach (TMP_Text i in realBookcases[currentRealBookcaseInUse].GetComponentsInChildren<TMP_Text>())
        {
            i.enabled = enabled;
        }

        foreach (Canvas i in realBookcases[currentRealBookcaseInUse].GetComponentsInChildren<Canvas>())
        {
            i.enabled = enabled;
        }
    }

    public void ToggleTexts(bool enabled, int index)
    {
        foreach (TMP_Text i in realBookcases[index].GetComponentsInChildren<TMP_Text>())
        {
            i.enabled = enabled;
        }

        foreach (Canvas i in realBookcases[index].GetComponentsInChildren<Canvas>())
        {
            i.enabled = enabled;
        }
    }
    #endregion
}

