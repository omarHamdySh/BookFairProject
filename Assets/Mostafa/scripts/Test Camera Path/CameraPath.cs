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

    public enum CameraMoveState
    {
        NotMoving,
        MoveOut,
        MoveIn,
        MoveVertically
    }

    public CameraMoveState cameraState;
    
    // Start is called before the first frame update
    void Awake()
    {
        
        cameraState = CameraMoveState.NotMoving;

        for(int i = 0; i < levels.Count; i++)
        {
            levels[i].nodeYIndex = i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        gotoNode(currentNode.nodeXIndex, currentNode.nodeYIndex);

        move();
        Debug.Log(calculateDistanceToNode(endNode));
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

        if (currentNode.nodeXIndex == targetNode.nodeXIndex)
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
        

        if (cameraState != CameraMoveState.NotMoving)
        {

            switch (cameraState)
            {
                case CameraMoveState.MoveOut:
                    if (areX_AtRoot(currentNode)) {
                        Debug.Log("0");
                        cameraState = CameraMoveState.MoveVertically;
                    }
                    else currentNode = currentNode.previous;
                    break;

                case CameraMoveState.MoveVertically:
                        if (areYsEqual(currentNode, endNode))
                        {
                            Debug.Log("1");
                            cameraState = CameraMoveState.MoveIn;
                        }
                        else
                        {
                            if (currentNode.nodeYIndex > endNode.nodeYIndex) currentNode = levels[currentNode.nodeYIndex - 1];
                            else if (currentNode.nodeYIndex < endNode.nodeYIndex) currentNode = levels[currentNode.nodeYIndex + 1];
                        
                        }
                    break;
                case CameraMoveState.MoveIn:
                    
                        if (areXsEquals(currentNode, endNode))
                        {
                            Debug.Log("2");
                            cameraState = CameraMoveState.NotMoving;
                        }
                        else
                        {
                            
                            currentNode = currentNode.next;
                        }
                    break;


            }

            if (areNodesEqual(currentNode, endNode)){
                cameraState = CameraMoveState.NotMoving;
            }
            gotoNode(currentNode.nodeXIndex, currentNode.nodeYIndex);
            //transform.DOMove(currentNode.transform.position, 0.5f);
            
        }
    }

    int calculateDistanceToNode(PathNode targetNode)
    {
        return currentNode.nodeXIndex + targetNode.nodeXIndex + Mathf.Abs(currentNode.nodeYIndex - targetNode.nodeYIndex);
    }
    public void onMoving()
    {
       
    }

    public void onLand()
    {
        
    }


    public void onDeparture()
    {
       
    }
}
