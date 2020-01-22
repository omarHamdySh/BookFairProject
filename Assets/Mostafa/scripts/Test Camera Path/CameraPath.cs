using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using mostafa;
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

    #region Singleton
    public static CameraPath instance { private set; get; }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cameraState = CameraMoveState.NotMoving;

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].nodeYIndex = i;
        }

    }
    #endregion


    void Start()
    {
        //endNode = currentNode;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        
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
        if(!areNodesEqual(currentNode, endNode)){
        
            step();
            cameraTransform.DOMove(currentNode.transform.position, .1f).OnComplete(move).OnUpdate(onMoving);
        
        }else{

            onLand();
        
        }
    }

    public void step()
    {
        

        if (cameraState != CameraMoveState.NotMoving)
        {

            switch (cameraState)
            {
                case CameraMoveState.MoveOut:
                    if(currentNode.previous != null) currentNode = currentNode.previous;
                    if (areX_AtRoot(currentNode)) {
                        cameraState = CameraMoveState.MoveVertically;
                    }
                    break;

                case CameraMoveState.MoveVertically:

                        if (currentNode.nodeYIndex > endNode.nodeYIndex) currentNode = levels[currentNode.nodeYIndex - 1];
                        else if (currentNode.nodeYIndex < endNode.nodeYIndex) currentNode = levels[currentNode.nodeYIndex + 1];
                       
                        if (areYsEqual(currentNode, endNode))
                        {
                            cameraState = CameraMoveState.MoveIn;
                        }
                        
                        
                    break;
                case CameraMoveState.MoveIn:
                        
                        if(currentNode.next != null) currentNode = currentNode.next;
                        
                        if (areXsEquals(currentNode, endNode))
                        {
                            cameraState = CameraMoveState.NotMoving;
                        }
                            
                        
                    break;


            }


            if (areNodesEqual(currentNode, endNode)){
                cameraState = CameraMoveState.NotMoving;
            }
        }

    }

    int calculateDistanceToNode(PathNode targetNode)
    {
        return currentNode.nodeXIndex + targetNode.nodeXIndex + Mathf.Abs(currentNode.nodeYIndex - targetNode.nodeYIndex);
    }

    public void setTarget(PathNode targetNode)
    {
        endNode = targetNode;
    }

    public void gotoTarget()
    {
        onDeparture();
        cameraState = CameraMoveState.MoveOut;
        move();
    }

    public void onLand()
    {
        print("done");
    }


    public void onDeparture()
    {
        print("depart");   
    }

    public void onMoving()
    {
        print("moving");
    }
}
