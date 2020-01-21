using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathNode : MonoBehaviour
{
    public PathNode previous;
    public PathNode next;
    public int nodeXIndex;
    public int nodeYIndex;

    // Start is called before the first frame update
    void Start()
    {
        PathNode tmpNode = GetComponent<PathNode>();

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

    public PathNode getNext()
    {
        return next;
    }

    public PathNode getPrevious()
    {
        return previous;
    }

}
