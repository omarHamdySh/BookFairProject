using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shelf_Bendary : MonoBehaviour, IScrollable
{
    public ShelfPathHandller_Bendary shelfPathHandller;

    private int objPathIndex = 0;
    private bool isLanded = true;
    private bool isCurrent = false;
    private bool isLoopingDomy = false;

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
            ToggleRenderer(false);
        }
        else
        {
            ToggleRenderer(true);
        }


        if (isCurrent)
        {

        }
    }

    public void ToggleAsCurrent(bool isCurrent)
    {
        this.isCurrent = isCurrent;
        GetComponent<BoxCollider>().enabled = isCurrent;
    }

    public void ToggleLoopingDomy(bool isLoopingDomy)
    {
        this.isLoopingDomy = isLoopingDomy;
    }

    public void ToggleRenderer(bool enabled)
    {
        foreach (MeshRenderer i in GetComponentsInChildren<MeshRenderer>())
        {
            i.enabled = enabled;
        }
    }
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
                if (getObjectIndex() == shelfPathHandller.IndexOfCurrent)
                {

                }
                else
                {

                }

                ToggleRenderer(true);
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
            ToggleRenderer(false);
        }
        else if (objPathIndex == shelfPathHandller.upperDomyIndex || objPathIndex == shelfPathHandller.lowerDomyIndex)
        {
            ToggleRenderer(false);
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
