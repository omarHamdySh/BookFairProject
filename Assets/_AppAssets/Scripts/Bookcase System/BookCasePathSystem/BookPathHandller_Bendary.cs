using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPathHandller_Bendary : MonoBehaviour
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    public int currentBookIndex;

    public int IndexOfCurrent, rightDomyIndex, leftDomyIndex;
    public Transform[] bookPathPoints;
    public Book_Bendary[] books;

    private float currentScrollSpeed;
    private bool isObjMoving = false;
    [SerializeField] private Shelf_Bendary myShelf;
    private bool toggleCategoryPanelOnce;

    private void Start()
    {
        currentBookIndex = IndexOfCurrent;
    }

    #region Data
    [SerializeField] private Texture dummyTexture;
    [HideInInspector] public int vendorIndex;
    public int categoryIndex;
    #endregion

    private void Update()
    {
        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.Shelf && myShelf.GetIsCurretn() && myShelf.GetIsCurrentBookcase() && !LevelUI.Instance.isUIOpen)
        {
            #region CategoryUIPanel
            if (!toggleCategoryPanelOnce && myShelf.categoryText.IsCanvasEnabled())
            {
                LevelUI.Instance.ToggleCategoryPanel(true, myShelf.categoryText.text);
                myShelf.categoryText.ToggleCanvasContainer(false);
                toggleCategoryPanelOnce = true;
            }
            #endregion

            currentScrollSpeed = GameManager.Instance.pathData.ShelfScrollSpeed;
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
        else if (GameManager.Instance.gameplayFSMManager.getCurrentState() != GameplayState.Shelf)
        {
            if (toggleCategoryPanelOnce)
            {
                toggleCategoryPanelOnce = false;
                LevelUI.Instance.ToggleCategoryPanel(false);
                myShelf.categoryText.ToggleCanvasContainer(true);
            }
        }
    }

    #region Helper
    private void MoveAccordingToScrollSpeed()
    {
        foreach (Book_Bendary book in books)
        {
            int nextPosIndex = 0;

            if (currentScrollSpeed > 0)
                nextPosIndex = (book.getObjectIndex() + 1) % bookPathPoints.Length;

            if (currentScrollSpeed < 0)
                nextPosIndex = (book.getObjectIndex() == 0) ? bookPathPoints.Length - 1 : book.getObjectIndex() - 1;

            Vector3 newDestination = bookPathPoints[nextPosIndex].transform.position;

            if (nextPosIndex == IndexOfCurrent)
            {
                book.ToggleAsCurrent(true);
                currentBookIndex = book.transform.GetSiblingIndex();
            }
            else
            {
                book.ToggleAsCurrent(false);
                if ((nextPosIndex == rightDomyIndex && book.getObjectIndex() == leftDomyIndex) ||
                   (nextPosIndex == leftDomyIndex && book.getObjectIndex() == rightDomyIndex))
                {
                    book.ToggleLoopingDomy(true);
                }
                else
                {
                    book.ToggleLoopingDomy(false);
                }
            }

            book.setObjectIndex(nextPosIndex);
            book.move(newDestination, objectScrollDuration);
        }
    }

    private bool CheckAllObjectsLanded()
    {
        foreach (var scrollable in books)
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
        foreach (var scrollable in books)// Change each scrollable State to --> onDeparture()
        {
            scrollable.onDeparture();
        }
    }

    public void AwakeCurrent()
    {
        currentBookIndex = IndexOfCurrent;
        bool isNewConceptArrange = false;

        if (Cache.Instance && Cache.Instance.cachedData.allVendors.Count > 0 && categoryIndex != -1)
        {
            if (Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData != null && Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories != null)
            {
                List<BookData> booksData = Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories[categoryIndex].booksData;
                if (booksData != null)
                {
                    if (Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories[categoryIndex].total < books.Length)
                    {
                        isNewConceptArrange = true;
                    }
                }
            }
        }

        //objs is the list of the physical books  (objs.Count = Physical books count)
        int oddIndexFatcor = (int)((books.Length / 2f) - .5f); //Less than the center index with 1
        int evenIndexFactor = (int)((books.Length / 2f) + .5f); //Center Index; [Zero]
        for (int i = 0; i < books.Length; i++)
        {
            if (isNewConceptArrange)
            {
                int mappedIndex = mapBooksIndicies(books[i].transform.GetSiblingIndex(), oddIndexFatcor, evenIndexFactor, out oddIndexFatcor, out evenIndexFactor, books.Length);
                books[i].Init(mappedIndex, bookPathPoints[mappedIndex].position, isNewConceptArrange);
            }

        }

        if (GetComponent<Shelf_Bendary>().GetIsCurretn())
        {
            for (int i = 0; i < books.Length; i++)
            {
                books[i].transform.localRotation = Quaternion.Euler(0, 0, 0);

                books[i].transform.Rotate(new Vector3(
                    0,
                    GetNodeRank(books[i].getObjectIndex()).rankRotation,
                    0));
            }
        }
        else
        {
            foreach (Book_Bendary i in books)
            {
                i.transform.localRotation = Quaternion.Euler(0, myShelf.nonCurrentBookRotataion[i.getObjectIndex()], 0);
            }
        }
    }

    /// <summary>
    /// Get the Index of the new arrange of odd list
    /// </summary>
    /// <param name="index"></param>
    /// <param name="oddIndexFatcor"></param>
    /// <param name="evenIndexFactor"></param>
    /// <param name="oddIndexFatcorOut"></param>
    /// <param name="evenIndexFactorOut"></param>
    /// <param name="count">Must be odd and biger than one</param>
    /// <returns></returns>
    public int mapBooksIndicies(int index, int oddIndexFatcor, int evenIndexFactor, out int oddIndexFatcorOut, out int evenIndexFactorOut, int count)
    {
        if (count % 2 == 0)
        {//Avoid excuting the algorithm because the list isn't odd number
            oddIndexFatcorOut = oddIndexFatcor;
            evenIndexFactorOut = evenIndexFactor;
            return index;
        }


        if (index % 2 == 0)
        {//even
            if (index == 0)
            {
                oddIndexFatcorOut = oddIndexFatcor;
                evenIndexFactorOut = evenIndexFactor;
                return evenIndexFactor - 1;
            }

            oddIndexFatcorOut = oddIndexFatcor;
            evenIndexFactorOut = ++evenIndexFactor;
            return evenIndexFactor - 1;

        }
        else
        {//odd

            oddIndexFatcorOut = oddIndexFatcor - 1;
            evenIndexFactorOut = evenIndexFactor;
            return oddIndexFatcor - 1;
        }
    }

    public NodeRank GetNodeRank(int index)
    {
        return bookPathPoints[index].GetComponent<NodeRank>();
    }

    public Book_Bendary GetCurrentBook()
    {
        return books[currentBookIndex];
    }
    #endregion

    #region Data
    private void PrepareData()
    {
        if (Cache.Instance && Cache.Instance.cachedData.allVendors.Count > 0 && categoryIndex != -1)
        {
            if (Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData != null && Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories != null)
            {
                List<BookData> booksData = Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories[categoryIndex].booksData;
                if (booksData != null)
                {
                    if (Cache.Instance.cachedData.allVendors[vendorIndex].bookcaseData.categories[categoryIndex].total > books.Length)
                    {
                        for (int i = 0; i < books.Length; i++)
                        {
                            if (currentScrollSpeed < 0 && books[i].getObjectIndex() == rightDomyIndex)
                            {
                                // Retrive the next data
                                int index = GetBookIndexFromOtherDommy(false);
                                if (index == -1)
                                {
                                    Debug.LogError("can't find the other dommy");
                                }
                                index = (index + 1) % booksData.Count;

                                if (!booksData[index].texture)
                                {
                                    booksData[index].texture = (Texture2D)dummyTexture;
                                }
                                books[i].SetBookData(booksData[index], index, (booksData[index].texture != (Texture2D)dummyTexture));
                                //New Book Scrolling Mapping Algorithm

                                break;
                                // Prepare cache more data on that direction
                            }
                            else if (currentScrollSpeed > 0 && books[i].getObjectIndex() == leftDomyIndex)
                            {
                                // Retrive the next data 
                                int index = GetBookIndexFromOtherDommy(true);
                                if (index == -1)
                                {
                                    Debug.LogError("can't find the other dommy");
                                }
                                index = (index == 0) ? booksData.Count - 1 : index - 1;

                                if (!booksData[index].texture)
                                {
                                    booksData[index].texture = (Texture2D)dummyTexture;
                                }
                                books[i].SetBookData(booksData[index], index, (booksData[index].texture != (Texture2D)dummyTexture));

                                break;
                                // Prepare cache more data on that direction
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetAllVisibleBooks(List<BookData> booksData, int vendorIndex, int categoryIndex)
    {
        this.vendorIndex = vendorIndex;
        this.categoryIndex = categoryIndex;
        AwakeCurrent();

        for (int i = 0; i < books.Length; i++)
        {
            if (i < booksData.Count)
            {
                if (!booksData[i].texture)
                {
                    booksData[i].texture = (Texture2D)dummyTexture;
                }

                books[i].SetBookData(booksData[i], i, (booksData[i].texture != (Texture2D)dummyTexture));
            }
            else
            {
                //Put the Dommy Data
                books[i].SetBookData(new BookData()
                {
                    texture = (Texture2D)dummyTexture
                }, -1, false);
            }
        }
    }

    /// <summary>
    /// get the Book index of the other dommy
    /// </summary>
    /// <param name="isRightDommy">the other dommy type</param>
    /// <returns>Book index</returns>
    private int GetBookIndexFromOtherDommy(bool isRightDommy)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (isRightDommy && books[i].getObjectIndex() == rightDomyIndex)
            {
                return books[i].bookDataIndex;
            }
            else if (!isRightDommy && books[i].getObjectIndex() == leftDomyIndex)
            {
                return books[i].bookDataIndex;
            }
        }
        return -1;
    }

    #endregion
}
