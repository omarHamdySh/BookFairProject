using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using Lean.Touch;
using DG.Tweening;

[RequireComponent(typeof(BookcaseObjectAlignerOverPath))]

public class Bookcase : MonoBehaviour, IScrollable, IClickable
{
    BookFair fair;
    List<Shelf> shelves;
    public CameraPathNode pathNode;
    LeanSelectable leanSelectable;

    private int objectIndex;

    private bool isLanded = true;

    public float getScrollSpeed()
    {
        if (GameManager.Instance)
        {
            if (GameManager.Instance.pathData)
            {
                return GameManager.Instance.pathData.BookcaseScrollSpeed;
            }
        }
        return 0;
    }

    void Start()
    {
        leanSelectable = GetComponent<LeanSelectable>();
    }

    public void focus()
    {
        print("focus");
    }

    public void unfocus()
    {
        print("focus");
    }

    public void move(Vector3 destination, float duration)
    {
        if (isLanded)
        {
            isLanded = false;
            transform.DOMove(destination, duration).OnComplete(onLand);

            transform.DORotate(new Vector3(0, GetRotRank(getObjectIndex()), 0), duration);
            if (getObjectIndex() != 0)
            {
                transform.DOLookAt(transform.parent.position, duration);
                GetComponent<ShelfPathHandler>().enabled = false;
            }
            else
            {
                GetComponent<ShelfPathHandler>().enabled = true;
            }
        }
    }

    public float GetRotRank(int index)
    {
        return GetComponent<BookcaseObjectAlignerOverPath>().pathHandler.GetRank(index).rankRotation;
    }

    public void onMoving()
    {
        print("Moving");

    }

    public bool IsLanded()
    {
        return isLanded;
    }
    public void onLand()
    {
        isLanded = true;
        GameManager.Instance.pathData.BookcaseScrollSpeed = 0;
        print("OnLand");
    }

    public void onDeparture()
    {
        print("OnDeparture");
    }

    public int getObjectIndex()
    {
        return objectIndex;
    }

    public void setObjectIndex(int objectIndex)
    {
        this.objectIndex = objectIndex;
    }

    public bool getLandStatus()
    {
        // throw new System.NotImplementedException();
        return isLanded;
    }

    public void move(Vector3 destination, float duration, bool visibilty)
    {
        throw new System.NotImplementedException();
    }
}