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

    public bool IsCurrent = false;

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

    public void select()
    {
        SelectionManager.instance.selectThis(this);
    }

    public void focus()
    {
        GetComponent<BoxCollider>().enabled = false;
        CameraPath.instance.setTarget(CameraPath.instance.bookcaseNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toShelfState();

    }

    public void unfocus()
    {
        
        GetComponent<BoxCollider>().enabled = true;
        //SelectionManager.instance.selectThis(GetComponentInParent<IClickable>());
        CameraPath.instance.setTarget(CameraPath.instance.floorNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toBookCaseState();
    }

    public void move(Vector3 destination, float duration)
    {
        if (isLanded)
        {
            isLanded = false;
            transform.DOMove(destination, duration).OnComplete(onLand);


            if (getObjectIndex() != 0)
            {
                transform.DORotate(new Vector3(0, GetRotRank(getObjectIndex()) - transform.localRotation.eulerAngles.y, 0), duration, RotateMode.LocalAxisAdd);
                //GetComponent<ShelfPathHandler>().enabled = false;
            }
            else
            {
                transform.DORotate(new Vector3(0, GetRotRank(getObjectIndex()), 0), duration);
                //GetComponent<ShelfPathHandler>().enabled = true;
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