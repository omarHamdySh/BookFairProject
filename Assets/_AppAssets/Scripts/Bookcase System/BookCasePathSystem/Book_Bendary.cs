using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Book_Bendary : MonoBehaviour, IScrollable
{
    public BookPathHandller_Bendary bookPathHandller;
    public MeshRenderer bookBodyMeshRenderer;
    [SerializeField] private FixTextMeshPro bookTitle1, bookTitle2;

    private int objPathIndex = 0;
    private bool isLanded = true;
    private bool isCurrent = false;
    private bool isLoopingDomy = false;

    #region Data
    [HideInInspector] public int bookDataIndex;
    [HideInInspector] public string buyURL;
    [HideInInspector] public string description;
    #endregion

    #region Helper
    public void Init(int newConceptIndex, Vector3 newpos, bool IsNewConceptArrange = false)
    {
        // Set the object path index by sibling index
        if (IsNewConceptArrange)
        {
            objPathIndex = newConceptIndex;
        }
        else
        {
            objPathIndex = transform.GetSiblingIndex();
        }

        // Set current accoridng to object index
        ToggleAsCurrent(((objPathIndex == bookPathHandller.IndexOfCurrent) ? true : false));

        // Set path position by object path index
        if (IsNewConceptArrange)
        {
            transform.position = newpos;
        }
        else
        {
            transform.position = bookPathHandller.bookPathPoints[objPathIndex].position;
        }

    }

    public void ToggleLoopingDomy(bool isLoopingDomy)
    {
        this.isLoopingDomy = isLoopingDomy;
    }

    public void ToggleAsCurrent(bool isCurrent)
    {
        this.isCurrent = isCurrent;
    }

    public float GetRotRank(int index)
    {
        return bookPathHandller.GetNodeRank(index).rankRotation;
    }

    public void ToggleRenderers(bool enabled)
    {
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = enabled;
        }
    }

    #region Data
    public void SetBookData(BookData bookData, int bookDataIndex, bool hasCover)
    {
        bookBodyMeshRenderer.material.mainTexture = bookData.texture;
        this.bookDataIndex = bookDataIndex;
        buyURL = bookData.url;
        description = bookData.description;
        if (!hasCover && bookDataIndex != -1)
        {
            bookTitle1.gameObject.SetActive(true);
            bookTitle2.gameObject.SetActive(true);

            bookTitle1.SetText(bookData.name);
            bookTitle2.SetText(bookData.name);
        }
        else
        {
            bookTitle1.gameObject.SetActive(false);
            bookTitle2.gameObject.SetActive(false);
        }
    }
    #endregion
    #endregion

    #region IScrollable
    public bool getLandStatus()
    {
        return isLanded;
    }

    public int getObjectIndex()
    {
        return objPathIndex;
    }

    public float getScrollSpeed()
    {
        throw new System.NotImplementedException();
    }

    public void move(Vector3 destination, float duration)
    {
        if (isLanded)
        {
            isLanded = false;

            if (isLoopingDomy)
            {
                isLoopingDomy = false;
                transform.position = destination;
                onLand();
            }
            else
            {
                var rotate = GetRotRank(getObjectIndex());
                transform.DOLocalRotate(new Vector3(0, rotate, 0), duration, RotateMode.Fast);
                //if (rotate != 0)
                //{
                //    rotate = (rotate - transform.localRotation.eulerAngles.y);
                //    transform.DORotate(new Vector3(0, rotate, 0), duration, RotateMode.LocalAxisAdd);
                //}
                //else
                //{
                //    transform.localRotation = Quaternion.Euler(0, 0, 0);
                //}
                transform.DOMove(destination, duration).OnComplete(onLand);
            }
        }
    }

    public void move(Vector3 destination, float duration, bool visibilty)
    {
        throw new System.NotImplementedException();
    }

    public void onDeparture()
    {
        //throw new System.NotImplementedException();
    }

    public void onLand()
    {
        isLanded = true;
    }

    public void onMoving()
    {
        //throw new System.NotImplementedException();
    }

    public void setObjectIndex(int _objectIndex)
    {
        objPathIndex = _objectIndex;
    }
    #endregion
}
