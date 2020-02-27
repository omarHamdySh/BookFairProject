using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BookcasePathHandller_Bendary : MonoBehaviour
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    [HideInInspector] public int currentBookcaseIndex;
    [HideInInspector] public int currentRealBookcaseInUse = 0;
    [SerializeField] private Transform realBookCaseForwordPos;
    [SerializeField] private Transform BookFowordPos;

    public int IndexOfCurrent;
    public Transform[] bookCasePathPoints;
    public Bookcase_Bendary[] bookcases;
    public Transform[] realBookcases;

    private float currentScrollSpeed;
    private bool isObjMoving = false;
    private Vector3 bookBackwordPos;
    #region Data
    [SerializeField] private int bookcaseCasheIndex = 0;
    private List<CategoryData> dummy = new List<CategoryData>();
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if (Cache.Instance)
        {
            //mostafa
            DataLoader.instance.funcFloorMode();
            PutDataOnCurrent();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.Floor && !LevelUI.Instance.isUIOpen)
        {
            currentScrollSpeed = GameManager.Instance.pathData.FloorScrollSpeed;
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
        int newIndexInUse = currentRealBookcaseInUse;
        newIndexInUse = (newIndexInUse + 1) % realBookcases.Length;

        #region Data
        if (Cache.Instance)
        {
            if (currentScrollSpeed < 0)
            {
                bookcaseCasheIndex = (bookcaseCasheIndex + 1) % Cache.Instance.cachedData.allVendors.Count;
            }
            else
            {
                bookcaseCasheIndex = (bookcaseCasheIndex == 0) ? Cache.Instance.cachedData.allVendors.Count - 1 : bookcaseCasheIndex - 1;
            }
        }
        #endregion

        foreach (var bookcase in bookcases)
        {
            int nextPosIndex = 0;
            if (currentScrollSpeed < 0)
            {
                nextPosIndex = (bookcase.getObjectIndex() + 1) % bookCasePathPoints.Length;
            }
            else
            {
                nextPosIndex = (bookcase.getObjectIndex() == 0) ? bookCasePathPoints.Length - 1 : bookcase.getObjectIndex() - 1;
            }

            Vector3 newDestination = bookCasePathPoints[nextPosIndex].transform.position;

            if (nextPosIndex == IndexOfCurrent)
            {
                // Domy Bookcase Logic
                bookcase.ToggleAsCurrent(true);
                currentBookcaseIndex = bookcase.transform.GetSiblingIndex();
                bookcase.ToggleMeshRenderer(false);

                // Real Bookcase Logic
                // Close the old current
                ToggleCurrentRealBookcaseMeshRenderer(false);
                ToggleCurrentRealBookcase(false);
                ToggleTexts(false);

                // Place the new real Bookcase current
                currentRealBookcaseInUse = newIndexInUse;

                //open the new current
                ToggleCurrentRealBookcaseMeshRenderer(true);
                ToggleCurrentRealBookcase(true);
                ToggleTexts(true);

                #region Data
                if (Cache.Instance)
                {
                    PutDataOnCurrent();
                }
                #endregion
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

    public void ToggleCurrentRealBookcase(bool enabled)
    { 
        realBookcases[currentRealBookcaseInUse].GetComponent<ShelfPathHandller_Bendary>().isCurrentBookcase = enabled;
        realBookcases[currentRealBookcaseInUse].GetComponent<ShelfPathHandller_Bendary>().AwakeCurrent();
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

    public void MoveRealBookcaseForward(float delay)
    {
        realBookcases[currentRealBookcaseInUse].DOMove(realBookCaseForwordPos.position, delay);
    }

    public void MoveRealBookcaseBackword(float delay)
    {
        realBookcases[currentRealBookcaseInUse].DOMove(bookCasePathPoints[IndexOfCurrent].position, delay);
    }

    public void MoveRealBookForward(float delay, TestBookRotation_Bendary animatedBook)
    {
        // Get the currentBook 
        Book_Bendary book = realBookcases[currentRealBookcaseInUse].GetComponent<ShelfPathHandller_Bendary>().GetCurrentBook();

        // Get Backword Pos
        bookBackwordPos = book.transform.position;

        // Assign it book backword pos to animated book
        animatedBook.transform.position = bookBackwordPos;

        // Assign current book cover material to animated book
        animatedBook.AssignCoverMaterials(book.bookBodyMeshRenderer.material);

        // Close current book renderers
        book.ToggleRenderers(false);

        // Open Animated book renderers
        animatedBook.ToggleRenderers(true);

        // Move animated book forword
        animatedBook.transform.DOMove(BookFowordPos.position, delay).OnComplete(animatedBook.OpenBook);
    }

    public void MoveRealBookBackword(float delay, TestBookRotation_Bendary animatedBook, TweenCallback tw)
    {
        // Move Animated book to backword pos
        animatedBook.transform.DOMove(bookBackwordPos, delay).OnComplete(OnCompleteBookMoveToBackwordPos + tw);
        StartCoroutine(CloseAnimatedBookRenderers(delay, animatedBook));
    }

    private void OnCompleteBookMoveToBackwordPos()
    {
        // Get the currentBook 
        Book_Bendary book = realBookcases[currentRealBookcaseInUse].GetComponent<ShelfPathHandller_Bendary>().GetCurrentBook();

        // Open current book renderers
        book.ToggleRenderers(true);
    }

    IEnumerator CloseAnimatedBookRenderers(float delay, TestBookRotation_Bendary animatedBook)
    {
        yield return new WaitForSeconds(delay);

        // Close Animated book renderers
        animatedBook.ToggleRenderers(false);
    }


    #endregion

    #region Data
    public void StartCallData()
    {

    }

    public void PutDataOnCurrent()
    {
        BookcaseData tmpBookcaseData = Cache.Instance.cachedData.allVendors[bookcaseCasheIndex].bookcaseData;

        if (tmpBookcaseData != null && tmpBookcaseData.categories != null)
        {
            realBookcases[currentRealBookcaseInUse].GetComponent<ShelfPathHandller_Bendary>().SetAllVisibleCategory(tmpBookcaseData.categories, bookcaseCasheIndex);
        }
        else if (tmpBookcaseData.categories == null)
        {
            realBookcases[currentRealBookcaseInUse].GetComponent<ShelfPathHandller_Bendary>().SetAllVisibleCategory(dummy, bookcaseCasheIndex);
        }
    }

    public void retrieveDataOfCurrentBookcase()
    {
        Debug.Log(currentRealBookcaseInUse);
        if (Cache.Instance.cachedData.allVendors != null)
            DataLoader.instance.funcBookcaseMode(Cache.Instance.cachedData.allVendors[currentRealBookcaseInUse].id);

    }
    #endregion
}

