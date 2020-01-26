using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using Lean.Touch;

public class Bookcase : MonoBehaviour, IScrollable, IClickable
{
    BookFair fair;
    List<Shelf> shelves;
    public CameraPathNode pathNode;
    LeanSelectable leanSelectable;

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
        //throw new System.NotImplementedException();
    }

    public void onMoving()
    {
        
    }

    public void onLand()
    {
        
    }

    public void onDeparture()
    {

    }

    public int getObjectIndex()
    {
        return 0;
    }

    public void setObjectIndex(int objectIndex)
    {
        ////throw new System.NotImplementedException();
    }

    public bool getLandStatus()
    {
        // throw new System.NotImplementedException();
        return true;
    }
}