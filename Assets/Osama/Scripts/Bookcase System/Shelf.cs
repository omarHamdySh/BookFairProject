using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Touch;

[RequireComponent(typeof(BookcaseObjectAlignerOverPath))]
public class Shelf : MonoBehaviour, IScrollable, IClickable
{
    BookFair fair;
    public BookPathHandler bookPathHandler;
    public List<Book> bookList = new List<Book>();

    [Space, SerializeField, Tooltip("This identifies the object index in the array")]
    private int objectIndex;
    LeanSelectable leanSelectable;

    private bool isLanded = true;

    private void Awake()
    {
        leanSelectable = GetComponent<LeanSelectable>();
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
        if (isLanded)
        {
            isLanded = false;

            if (getObjectIndex() == 1)
            {
                for (int i = 0; i < bookPathHandler.booksOverPath.Length; i++)
                {
                    bookPathHandler.booksOverPath[i].transform.Rotate(new Vector3(
                        0,
                        bookPathHandler.bookPathTransforms[i].GetComponent<NodeRank>().rankRotation,
                        0));
                }
            }
            else
            {
                if (getObjectIndex() == 4 || getObjectIndex() == 5)
                {
                    transform.position = destination;
                    onLand();
                }
                else
                {
                    SetShelfVisibility(true);
                    transform.DOMove(destination, duration).OnComplete(onLand);
                }

                for (int i = 0; i < bookPathHandler.booksOverPath.Length; i++)
                {
                    bookPathHandler.booksOverPath[i].transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    public void SetShelfVisibility(bool visibility)
    {
        foreach (MeshRenderer i in GetComponentsInChildren<MeshRenderer>())
        {
            i.enabled = visibility;
        }
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
        //print("Shelf, onDeparture");
        //isLanded = false;
    }

    public void onLand()
    {
        //print("Shelf, onLand");

        if (getObjectIndex() == 4 || getObjectIndex() == 5)
        {
            SetShelfVisibility(false);
        }


        isLanded = true;

        //Debug.Log("onLand(), isLanded: " + isLanded);
    }

    public void onMoving()
    {
        //print("Shelf, onMoving");
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
        //Debug.Log("Shelf, getObjectIndex");
        return objectIndex;
    }

    public void setObjectIndex(int _objectIndex)
    {
        //Debug.Log("Shelf, setObjectIndex");
        objectIndex = _objectIndex;
    }

    public bool getLandStatus()
    {
        //Debug.Log("Shelf, getLandStatus, isLanded : " + isLanded);
        return isLanded;
    }
}
