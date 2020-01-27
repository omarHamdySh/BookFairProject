using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shelf : MonoBehaviour, IScrollable, IClickable
{
    BookFair fair;
    public List<Book> bookList = new List<Book>();

    [Space, SerializeField, Tooltip("This identifies the object index in the array")]
    private int objectIndex;

    private bool isLanded = true;

    private void Awake()
    {
        objectIndex = transform.GetSiblingIndex();
    }

    public int ObjectIndex {
        get => objectIndex; 
        set => objectIndex = value;
    }

    public float getScrollSpeed()
    {
        if (GameManager.Instance)
        {
            if (GameManager.Instance.pathData)
            {
                return GameManager.Instance.pathData.ShelfScrollSpeed;
            }
        }
        return 0;
    }

    public void move(Vector3 destination, float duration)
    {
        print("Shelf, move");
        isLanded = false;

        transform.DOMove(destination, duration)
            .SetEase(GameManager.Instance.pathData.MovementEase)
            .OnComplete(onLand);
    }

    public void move(Vector3 destination, float duration, bool visibility)
    {
        print("Shelf, move, 2");
        isLanded = false;

        transform.DOMove(destination, duration)
            .SetEase(GameManager.Instance.pathData.MovementEase)
            .OnComplete(onLand);

        gameObject.SetActive(visibility);
    }

    public void onDeparture()
    {
        print("Shelf, onDeparture");
        //isLanded = false;
    }

    public void onLand()
    {
        print("Shelf, onLand");

        gameObject.SetActive(true);

        isLanded = true;

        Debug.Log("onLand(), isLanded: " + isLanded);
    }

    public void onMoving()
    {
        print("Shelf, onMoving");
    }

    public void focus()
    {
        print("Shelf, focus");
    }

    public void unfocus()
    {
        print("Shelf, unfocus");
    }

    public int getObjectIndex()
    {
        Debug.Log("Shelf, getObjectIndex");
        return objectIndex;
    }

    public void setObjectIndex(int _objectIndex)
    {
        Debug.Log("Shelf, setObjectIndex");
        objectIndex = _objectIndex;
    }

    public bool getLandStatus()
    {
        Debug.Log("Shelf, getLandStatus, isLanded : " + isLanded);
        return isLanded;
    }
}
