using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Book : MonoBehaviour, IScrollable, IClickable
{
    public string title;
    public Texture2D image;
    List<BookPage> pages;

    [Space, SerializeField, Tooltip("This identifies the object index in the array")]
    private int objectIndex;

    private bool isLanded = true;

    private void Awake()
    {
        objectIndex = transform.GetSiblingIndex();
    }

    public int ObjectIndex
    {
        get => objectIndex;
        set => objectIndex = value;
    }

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

    public void move(Vector3 destination, float duration)
    {
        print("Book, move");
        isLanded = false;

        transform.DOMove(destination, duration)
            .SetEase(GameManager.Instance.pathData.MovementEase)
            .OnComplete(onLand);
    }

    public void move(Vector3 destination, float duration, bool visibility)
    {
        print("Book, move, 2");
        isLanded = false;

        transform.DOMove(destination, duration)
            .SetEase(GameManager.Instance.pathData.MovementEase)
            .OnComplete(onLand);

        gameObject.SetActive(visibility);
    }

    public void onDeparture()
    {
        print("Book, onDeparture");
    }

    public void onLand()
    {
        print("Book, onLand");

        gameObject.SetActive(true);

        isLanded = true;

        Debug.Log("onLand(), isLanded: " + isLanded);
    }

    public void onMoving()
    {
        print("Book, onMoving");
    }

    public void focus()
    {
        print("Book, focus");
    }

    public void unfocus()
    {
        print("Book, unfocus");
    }
    
    public int getObjectIndex()
    {
        Debug.Log("Book, getObjectIndex");
        return objectIndex;
    }

    public void setObjectIndex(int _objectIndex)
    {
        Debug.Log("Book, setObjectIndex");
        objectIndex = _objectIndex;
    }

    public bool getLandStatus()
    {
        Debug.Log("Book, getLandStatus, isLanded : " + isLanded);
        return isLanded;
    }
}