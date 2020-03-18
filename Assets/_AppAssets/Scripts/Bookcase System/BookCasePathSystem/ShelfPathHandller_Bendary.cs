using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfPathHandller_Bendary : MonoBehaviour
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    public int currentShelfIndex;
    public bool isCurrentBookcase = false;

    public int IndexOfCurrent, upperDomyIndex, lowerDomyIndex;
    public Transform[] shelfPathPoints;
    public Shelf_Bendary[] shelves;

    private float currentScrollSpeed;
    private bool isObjMoving = false;

    public FixTextMeshPro vendorNameOntheBookcase;

    #region Data
    private int vendorIndex = -1;
    private List<BookData> dummyBooksData = new List<BookData>();
    #endregion

    private void Start()
    {
        currentShelfIndex = IndexOfCurrent;
    }

    private void Update()
    {
        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.BookCase && isCurrentBookcase && !LevelUI.Instance.isUIOpen)
        {
            currentScrollSpeed = GameManager.Instance.pathData.BookcaseScrollSpeed;
            if (isObjMoving && currentScrollSpeed == 0)
            {
                isObjMoving = !CheckAllObjectsLanded();
                if (!isObjMoving)
                {
                    SelectionManager.instance.canSelect = true;
                }
                return;
            }
            else if (!isObjMoving && currentScrollSpeed != 0)
            {
                isObjMoving = true;
                if (CheckAllObjectsLanded())
                {
                    OnDepartureCall();
                    PrepareData();
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
        SelectionManager.instance.canSelect = false;
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

    public Book_Bendary GetCurrentBook()
    {
        return shelves[currentShelfIndex].GetComponent<BookPathHandller_Bendary>().GetCurrentBook();
    }

    public Shelf_Bendary GetCurrentShelf()
    {
        return shelves[currentShelfIndex];
    }

    #endregion

    #region Data
    private void PrepareData()
    {
        if (Cache.Instance && Cache.Instance.cachedData.allVendors.Count > 0)
        {
            if (Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData != null && Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories != null)
            {
                if (Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories.Count > shelves.Length)
                {
                    for (int i = 0; i < shelves.Length; i++)
                    {
                        if (currentScrollSpeed < 0 && shelves[i].getObjectIndex() == upperDomyIndex)
                        {
                            //Retrive the next data
                            List<CategoryData> categories = Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories;

                            int index = GetCategoryIndexFromOtherDommy(false);
                            if (index == -1)
                            {
                                Debug.LogError("can't find the other dommy");
                            }
                            index = (index + 1) % categories.Count;

                            shelves[i].SetCategoryData(categories[index].name, index);
                            shelves[i].GetComponent<BookPathHandller_Bendary>().SetAllVisibleBooks(categories[index].booksData, vendorIndex, index);

                            break;
                            //Prepare cache more data on that direction
                        }
                        else if (currentScrollSpeed > 0 && shelves[i].getObjectIndex() == lowerDomyIndex)
                        {
                            //Retrive the next data
                            List<CategoryData> categories = Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories;

                            int index = GetCategoryIndexFromOtherDommy(true);
                            if (index == -1)
                            {
                                Debug.LogError("can't find the other dommy");
                            }
                            index = (index == 0) ? categories.Count - 1 : index - 1;

                            shelves[i].SetCategoryData(categories[index].name, index);
                            shelves[i].GetComponent<BookPathHandller_Bendary>().SetAllVisibleBooks(categories[index].booksData, vendorIndex, index);

                            break;
                            //Prepare cache more data on that direction
                        }
                    }
                }
            }
        }
    }

    public void SetAllVisibleCategory(List<CategoryData> categories, int vendorIndex)
    {
        this.vendorIndex = vendorIndex;
        vendorNameOntheBookcase.text = Cache.Instance.cachedData.allVendors[vendorIndex].name;
        for (int i = 0; i < shelves.Length; i++)
        {
            if (i < categories.Count)
            {
                shelves[i].SetCategoryData(categories[i].name, i);
                shelves[i].GetComponent<BookPathHandller_Bendary>().SetAllVisibleBooks(categories[i].booksData, vendorIndex, i);
            }
            else
            {
                shelves[i].SetCategoryData("", -1);
                shelves[i].GetComponent<BookPathHandller_Bendary>().SetAllVisibleBooks(dummyBooksData, vendorIndex, -1);
            }
        }
    }

    /// <summary>
    /// get the category index of the other dommy
    /// </summary>
    /// <param name="isUpdommy">the other dommy type</param>
    /// <returns>category index</returns>
    private int GetCategoryIndexFromOtherDommy(bool isUpdommy)
    {
        for (int i = 0; i < shelves.Length; i++)
        {
            if (isUpdommy && shelves[i].getObjectIndex() == upperDomyIndex)
            {
                return shelves[i].categoryIndex;
            }
            else if (!isUpdommy && shelves[i].getObjectIndex() == lowerDomyIndex)
            {
                return shelves[i].categoryIndex;
            }
        }
        return -1;
    }

    #endregion
}
