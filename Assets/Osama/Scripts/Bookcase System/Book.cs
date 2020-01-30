using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Lean.Touch.LeanSelectable))]
[RequireComponent(typeof(BookObjectAlignerOverPath))]
public class Book : MonoBehaviour, IScrollable, IClickable
{
    public string title;
    public string url;
    public Texture2D image;
    List<BookPage> pages;
    public CameraPathNode pathNode;

    [Space, SerializeField, Tooltip("This identifies the object index in the array")]
    private int objectIndex;

    private bool isLanded = true;
    public bool IsLooping;
    public bool IsCurrent = false;

    private void Awake()
    {
        //objectIndex = transform.GetSiblingIndex();
        url = "www.duckduckgo.com";
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
        if (isLanded)
        {
            isLanded = false;

            if (IsLooping)
            {
                IsLooping = false;
                transform.position = destination;
                onLand();
            }
            else
            {
                var rotate = GetRotRank(getObjectIndex());
                if (rotate != 0)
                {
                    rotate = (rotate - transform.localRotation.eulerAngles.y);
                    //print(name + "  " + GetRotRank(getObjectIndex()) + "  " + rotate + "  " + transform.localRotation.eulerAngles.y);
                    transform.DORotate(new Vector3(0, rotate, 0), duration, RotateMode.LocalAxisAdd);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                transform.DOMove(destination, duration).OnComplete(onLand);
            }
        }
    }

    public float GetRotRank(int index)
    {
        return GetComponent<BookObjectAlignerOverPath>().bookPathHandler.GetRank(index).rankRotation;
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

    public void select()
    {
        SelectionManager.instance.selectThis(this);
    }

    public void onDeparture()
    {
        print("Book, onDeparture");
    }

    private void AnotherCheck()
    {
    }
    public void onLand()
    {
        //print("Book, onLand");



        isLanded = true;

        //Debug.Log("onLand(), isLanded: " + isLanded);
    }

    public void onMoving()
    {
        //print("Book, onMoving");
    }

    public void focus()
    {
        print("Book, focus");

        //go to url
        Application.OpenURL(url);
        //CameraPath.instance.setTarget(pathNode);
        //CameraPath.instance.gotoTarget();
    }

    public void unfocus()
    {
        print("Book, unfocus");
    }

    public int getObjectIndex()
    {
        //Debug.Log("Book, getObjectIndex");
        return objectIndex;
    }

    public void setObjectIndex(int _objectIndex)
    {
        //Debug.Log("Book, setObjectIndex");
        objectIndex = _objectIndex;
    }

    public bool getLandStatus()
    {
        //Debug.Log("Book, getLandStatus, isLanded : " + isLanded);
        return isLanded;
    }
}