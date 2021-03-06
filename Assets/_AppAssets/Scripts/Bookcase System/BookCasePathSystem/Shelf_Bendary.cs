﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shelf_Bendary : MonoBehaviour, IScrollable
{
    public ShelfPathHandller_Bendary shelfPathHandller;
    public float[] nonCurrentBookRotataion;
    [SerializeField] private MeshRenderer shelfMesh;

    private int objPathIndex = 0;
    private bool isLanded = true;
    private bool isCurrent = false;
    private bool isLoopingDomy = false;

    #region Data
    public FixTextMeshPro categoryText;
    public int categoryIndex = -1;
    #endregion

    private void Start()
    {
        //Init();
    }

    #region Helper
    public void Init()
    {
        // Set the object path index by sibling index
        objPathIndex = transform.GetSiblingIndex();

        // Set current accoridng to object index
        ToggleAsCurrent(((objPathIndex == shelfPathHandller.IndexOfCurrent) ? true : false));

        // Set path position by object path index
        transform.position = shelfPathHandller.shelfPathPoints[objPathIndex].position;

        // Set Domy accoridng to object index
        if (objPathIndex == shelfPathHandller.upperDomyIndex || objPathIndex == shelfPathHandller.lowerDomyIndex)
        {
            ToggleShelfRenderer_Books(false);
        }
        else
        {
            ToggleShelfRenderer(true);
        }

        GetComponent<BookPathHandller_Bendary>().AwakeCurrent();
    }

    public void ToggleAsCurrent(bool isCurrent)
    {
        this.isCurrent = isCurrent;
    }

    public bool GetIsCurretn()
    {
        return isCurrent;
    }

    public bool GetIsCurrentBookcase()
    {
        return shelfPathHandller.isCurrentBookcase;
    }

    public void ToggleLoopingDomy(bool isLoopingDomy)
    {
        this.isLoopingDomy = isLoopingDomy;
    }

    public void ToggleShelfRenderer(bool enabled)
    {
        shelfMesh.enabled = enabled;
    }

    public void ToggleShelfRenderer_Books(bool enabled)
    {
        foreach (MeshRenderer i in GetComponentsInChildren<MeshRenderer>())
        {
            i.enabled = enabled;
        }
    }

    #region Data
    public void SetCategoryData(string categoryTextData, int categoryIndex)
    {
        if (!string.IsNullOrEmpty(categoryTextData))
        {
            categoryText.ToggleCanvasContainer(true);
            categoryText.SetText(categoryTextData);
        }
        else
        {
            categoryText.ToggleCanvasContainer(false);
        }

        this.categoryIndex = categoryIndex;
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
                transform.position = destination;
                onLand();
            }
            else
            {
                if (isCurrent)
                {
                    for (int i = 0; i < GetComponent<BookPathHandller_Bendary>().books.Length; i++)
                    {
                        GetComponent<BookPathHandller_Bendary>().books[i].transform.DOLocalRotate(new Vector3(
                            0,
                            GetComponent<BookPathHandller_Bendary>().bookPathPoints[GetComponent<BookPathHandller_Bendary>().books[i].getObjectIndex()].GetComponent<NodeRank>().rankRotation,
                            0), duration, RotateMode.Fast);
                    }
                }
                else
                {
                    foreach (Book_Bendary i in GetComponent<BookPathHandller_Bendary>().books)
                    {
                        i.transform.localRotation = Quaternion.Euler(0, nonCurrentBookRotataion[i.getObjectIndex()], 0);
                    }
                }
                GetComponent<BookPathHandller_Bendary>().ToggleRendererOfAllBooksHaveData();
                ToggleShelfRenderer(true);
                transform.DOMove(destination, duration).OnUpdate(onMoving).OnComplete(onLand);
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
        if (isLoopingDomy)
        {
            isLoopingDomy = false;
            ToggleShelfRenderer_Books(false);
        }
        else if (objPathIndex == shelfPathHandller.upperDomyIndex || objPathIndex == shelfPathHandller.lowerDomyIndex)
        {
            ToggleShelfRenderer_Books(false);
        }

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
