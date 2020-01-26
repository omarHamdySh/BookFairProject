using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPathNode : MonoBehaviour
{
    public CameraPathNode previous;
    public CameraPathNode next;
    public int nodeXIndex;
    public int nodeYIndex;

    // Start is called before the first frame update
    void Start()
    {
        CameraPathNode tmpNode = GetComponent<CameraPathNode>();

        int xIndexCounter = 0;
        while(tmpNode.previous != null)
        {
            xIndexCounter++;
            tmpNode = tmpNode.previous;
        }

        nodeXIndex = xIndexCounter;
        nodeYIndex = tmpNode.nodeYIndex;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CameraPathNode getNext()
    {
        return next;
    }

    public CameraPathNode getPrevious()
    {
        return previous;
    }

}
