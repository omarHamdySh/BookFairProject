using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bookcase_Bendary : MonoBehaviour, IScrollable
{
    public BookcasePathHandller_Bendary bookcasePathHandller;

    private int objPathIndex = 0;
    private bool isLanded = true;
    private bool isCurrent = false;

    #region Helper
    public void Init(BookcasePathHandller_Bendary bookcasePathHandller)
    {
        // Set path handller
        this.bookcasePathHandller = bookcasePathHandller;

        // Set the object path index by sibling index
        objPathIndex = transform.GetSiblingIndex();

        // Set current accoridng to object index
        isCurrent = (objPathIndex == bookcasePathHandller.IndexOfCurrent) ? true : false;

        // Set path position by object path index
        transform.position = bookcasePathHandller.bookCasePathPoints[objPathIndex].position;

        // Set path rotation by object path index
        SetBookcaseRotation(bookcasePathHandller.bookCasePathPoints[objPathIndex].GetComponent<NodeRank>().rankRotation);

        if (isCurrent)
        {
            ToggleMeshRenderer(false);
            bookcasePathHandller.realBookcases[bookcasePathHandller.currentRealBookcaseInUse].position = transform.position;
            bookcasePathHandller.realBookcases[bookcasePathHandller.currentRealBookcaseInUse].localRotation = transform.localRotation;
            bookcasePathHandller.ToggleCurrentRealBookcaseMeshRenderer(true);
            bookcasePathHandller.ToggleCurrentRealBookcase(true);
            bookcasePathHandller.ToggleTexts(true);
        }
    }

    public void ToggleMeshRenderer(bool enabled)
    {
        foreach (MeshRenderer i in GetComponentsInChildren<MeshRenderer>())
        {
            i.enabled = enabled;
        }
    }

    /// <summary>
    /// rotate the object instantly
    /// </summary>
    /// <param name="rot">the rotation in degree</param>
    public void SetBookcaseRotation(int rot)
    {
        transform.DOLocalRotate(new Vector3(0, rot /*- transform.localRotation.eulerAngles.y*/, 0), duration, RotateMode.Fast);
    }

    /// <summary>
    /// rotate the object with tweening
    /// </summary>
    /// <param name="rot">the rotation in degree</param>
    /// <param name="duration">the duration to rech the rotation</param>
    public void SetBookcaseRotation(int rot, float duration)
    {
        transform.DORotate(new Vector3(0, rot - transform.localRotation.eulerAngles.y, 0), duration, RotateMode.LocalAxisAdd);
    }

    public void ToggleAsCurrent(bool isCurrent)
    {
        this.isCurrent = isCurrent;
    }
    #endregion

    #region Scrollable

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

            if (getObjectIndex() == bookcasePathHandller.IndexOfCurrent)
            {
                bookcasePathHandller.realBookcases[bookcasePathHandller.currentRealBookcaseInUse].position = transform.position;
                bookcasePathHandller.realBookcases[bookcasePathHandller.currentRealBookcaseInUse].localRotation = transform.localRotation;
                bookcasePathHandller.realBookcases[bookcasePathHandller.currentRealBookcaseInUse].DOMove(destination, duration);
                int rot = bookcasePathHandller.bookCasePathPoints[objPathIndex].GetComponent<NodeRank>().rankRotation;
                bookcasePathHandller.realBookcases[bookcasePathHandller.currentRealBookcaseInUse].DOLocalRotate(new Vector3(0, rot /*- transform.localRotation.eulerAngles.y*/, 0), duration, RotateMode.Fast);
            }

            transform.DOMove(destination, duration).OnUpdate(onMoving).OnComplete(onLand);

            SetBookcaseRotation(bookcasePathHandller.bookCasePathPoints[objPathIndex].GetComponent<NodeRank>().rankRotation, duration);
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
