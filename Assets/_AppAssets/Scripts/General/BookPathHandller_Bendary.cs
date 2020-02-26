using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPathHandller_Bendary : MonoBehaviour
{
    [SerializeField] private float objectScrollDuration = 0.7f;
    [HideInInspector] public int currentBookIndex;

    public int IndexOfCurrent, rightDomyIndex, leftDomyIndex;
    public Transform[] bookPathPoints;
    public Book_Bendary[] books;

    private float currentScrollSpeed;
    private bool isObjMoving = false;

    #region Data
    [SerializeField] private Texture dummyTexture;
    #endregion

    private void Update()
    {
        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.Shelf && GetComponent<Shelf_Bendary>().GetIsCurretn() && !LevelUI.Instance.isUIOpen)
        {
            currentScrollSpeed = GameManager.Instance.pathData.ShelfScrollSpeed;
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

                    #region Data
                    if (currentScrollSpeed < 0 && book.getObjectIndex() == rightDomyIndex)
                    {
                        // Retrive the next data 
                        // Prepare cache more data on that direction
                    }
                    else if (currentScrollSpeed > 0 && book.getObjectIndex() == leftDomyIndex)
                    {
                        // Retrive the next data 
                        // Prepare cache more data on that direction
                    }
                    #endregion
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
        foreach (var scrollable in books)// Change each scrollable State to --> onDeparture()
        {
            scrollable.onDeparture();
        }
    }

    public void AwakeCurrent()
    {
        foreach (Book_Bendary book in books)
        {
            book.Init();
        }

        if (GetComponent<Shelf_Bendary>().GetIsCurretn())
        {
            for (int i = 0; i < books.Length; i++)
            {
                books[i].transform.localRotation = Quaternion.Euler(0, 0, 0);

                books[i].transform.Rotate(new Vector3(
                    0,
                    GetNodeRank(i).rankRotation,
                    0));
            }
        }
        else
        {
            foreach (Book_Bendary i in books)
            {
                i.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
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

    #region Data
    public void SetAllVisibleBooks(List<BookData> booksData)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (i < booksData.Count)
            {
                if (!booksData[i].texture)
                {
                    booksData[i].texture = (Texture2D)dummyTexture;
                }

                books[i].SetBookData(booksData[i]);
            }
            else
            {
                //But the Dommy Data
                books[i].SetBookData(new BookData()
                {
                    texture = (Texture2D)dummyTexture
                });
            }
        }
    }
    #endregion
    #endregion
}
