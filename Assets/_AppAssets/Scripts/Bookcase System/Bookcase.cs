using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

[RequireComponent(typeof(ObjectAlignerOverPath))]
public class Bookcase : MonoBehaviour, IScrollable, IClickable
{
    BookFair fair;
    List<Shelf> shelves;

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

    public void move()
    {
        print("move");
    }

    public void onDeparture()
    {
        print("onDeparture");
    }

    public void onLand()
    {
        print("onLand");
    }

    public void onMoving()
    {
        print("onMoving");
    }

    public void focus()
    {
        print("focus");
    }

    public void unfocus()
    {
        print("focus");
    }
}