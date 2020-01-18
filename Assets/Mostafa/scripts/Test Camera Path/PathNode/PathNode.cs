using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathNode : MonoBehaviour
{
    public PathNode previous;
    public PathNode next;
    public byte nodeXIndex;
    public byte nodeYIndex;

    // Start is called before the first frame update
    void Start()
    {
        
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
