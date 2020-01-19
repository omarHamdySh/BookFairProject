using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraPath : MonoBehaviour,ITraverseable
{
    public PathNode currentNode, endNode;
    private int currentLevel;

    public List<PathNode> levels;

    public Transform cameraTransform;

    enum CameraMoveState
    {
        NotMoving,
        MoveOut,
        MoveIn,
        MoveVertically
    }

    CameraMoveState cameraState;
    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
        gotoNode(0, 0);

        for(int i = 0; i < levels.Count; i++)
        {
            levels[i].nodeYIndex = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gotoNode(x, y);    
    }

    public void gotoNode(int index, int level)
    {
        if(level < levels.Count){
            currentLevel = level;
            currentNode = levels[level];
        }

        for(int i = 0; i < index && currentNode.next != null; i++){
            currentNode = currentNode.next;
        }

        cameraTransform.position = currentNode.transform.position;

    }

    /// <summary>
    /// To Check whether y-axis of the currentNode and the target node are equal or not.
    /// It will be used 2 times in the path finding algorithms
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public bool areYsEqual(PathNode currentNode, PathNode targetNode)
    {
        if (currentNode.nodeYIndex == targetNode.nodeYIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// To Check whether x-axis of the currentNode and the target node are equal or not.
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public bool areXsEquals(PathNode currentNode, PathNode targetNode)
    {

        if (currentNode.nodeXIndex == targetNode.nodeYIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// To Check whether x-axis of the currentNode reached the root or not yet.
    /// </summary>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    public bool areX_AtRoot(PathNode currentNode)
    {
        
        if (currentNode.nodeXIndex == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool areNodesEqual(PathNode currentNode, PathNode targetNode)
    {
        if(currentNode.nodeXIndex == targetNode.nodeXIndex && currentNode.nodeYIndex == targetNode.nodeYIndex)
        {
            return true;
        }

        return false;
    }

    public void move()
    {
        if(!areNodesEqual(currentNode, endNode)){
            cameraState = CameraMoveState.NotMoving;
        }

        switch (cameraState)
        {
            case CameraMoveState.MoveOut:
                if (areX_AtRoot(currentNode)) currentNode = currentNode.previous;
                else cameraState = cameraState = CameraMoveState.MoveVertically;
                break;

            case CameraMoveState.MoveVertically:
                if (!areYsEqual(currentNode, endNode))
                {
                    if (currentNode.nodeYIndex < endNode.nodeYIndex) currentNode = levels[currentNode.nodeYIndex - 1];
                }
                break;

        }
        transform.DOMove(currentNode.next.transform.position, 0.5f).OnComplete(onLand);
    }

    public void onMoving()
    {
       
    }

    public void onLand()
    {
        if (currentNode != endNode)
        {
            move();
        }
    }


    public void onDeparture()
    {
       
    }
}
