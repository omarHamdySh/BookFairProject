using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation.Examples;

[RequireComponent(typeof(ObjectAlignerOverPath))]
public class Book : MonoBehaviour, IScrollable, IClickable
{
    public string title;
    public Texture2D image;
    List<BookPage> pages;
    
    public float getScrollSpeed()
    {
        if (GameManager.Instance)
        {
            if (GameManager.Instance.pathData)
            {
                return GameManager.Instance.pathData.BookScrollSpeed;
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