using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ObjectAlignerOverPathv2))]
public class Shelf : MonoBehaviour, IScrollable, IClickable
{
    BookFair fair;
    public List<Book> bookList = new List<Book>();

    [Space, SerializeField, Tooltip("This identifies the object index in the array")]
    private int objectIndex;

    private void Awake()
    {
        objectIndex = transform.GetSiblingIndex();
    }

    public int ObjectIndex { get => objectIndex; set => objectIndex = value; }

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
        print("move");
        transform.DOMove(destination, duration);
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

    public int getObjectIndex() { return objectIndex; }

    public void setObjectIndex(int _objectIndex) {
        objectIndex = _objectIndex;
    }
}
