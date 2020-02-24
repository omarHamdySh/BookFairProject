using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPath : MonoBehaviour, ITraverseable
{
    public CameraPathNode currentNode, endNode;
    private CameraPathNode defaultNode;
    private int currentLevel;

    public List<CameraPathNode> levels;

    public Transform cameraTransform;

    public CameraPathNode floorNode;
    public CameraPathNode bookcaseNode;
    public CameraPathNode shelfNode;
    public CameraPathNode bookNode;
    public CameraPathNode pageNode;

    public float cameraSpeed;

    public bool cameraMoving;

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

        defaultNode = levels[0];
    }
    #endregion


    void Start()
    {
        //endNode = currentNode;
        cameraMoving = false;
        GameManager.Instance.pathData.FloorScrollSpeed = 0;
        gotoTarget();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (GameManager.Instance.pathData.FloorScrollSpeed != 0)
        {
            if (areX_AtRoot(currentNode))
            {
                int tmpLevel = currentNode.nodeYIndex;

                if (GameManager.Instance.pathData.FloorScrollSpeed < 0)
                {
                    if(tmpLevel > 0)setTarget(levels[tmpLevel - 1]);
                }
                else 
                if (GameManager.Instance.pathData.FloorScrollSpeed > 0)
                {
                    if (tmpLevel < levels.Count - 1) setTarget(levels[tmpLevel + 1]);
                }
                gotoTarget();


            }
        }
    }


    /// <summary>
    /// To Check whether y-axis of the currentNode and the target node are equal or not.
    /// It will be used 2 times in the path finding algorithms
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public bool areYsEqual(CameraPathNode currentNode, CameraPathNode targetNode)
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
    public bool areXsEquals(CameraPathNode currentNode, CameraPathNode targetNode)
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

    public void reset()
    {
        setTarget(levels[currentNode.nodeYIndex]);
        gotoTarget();
    }
    /// <summary>
    /// To Check whether x-axis of the currentNode reached the root or not yet.
    /// </summary>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    public bool areX_AtRoot(CameraPathNode currentNode)
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

    private CameraPathNode getNodeInLevel(int n)
    {

        CameraPathNode tmpNode = levels[currentNode.nodeYIndex];

        for (int i = 0; i < n && tmpNode.next != null; i++) tmpNode = tmpNode.next;

        return tmpNode;
    }
    public bool areNodesEqual(CameraPathNode currentNode, CameraPathNode targetNode)
    {
        if (currentNode.nodeXIndex == targetNode.nodeXIndex && currentNode.nodeYIndex == targetNode.nodeYIndex)
        {
            return true;
        }

        return false;
    }

    
    public void move()
    {
        
        if (!areNodesEqual(currentNode, endNode))
        {
            step();
            cameraTransform.DOMove(currentNode.transform.position, cameraSpeed).OnComplete(move).OnUpdate(onMoving);

        }
        else
        {
            cameraMoving = false;
            onLand();

        }
    }

    public void step()
    {
        if (cameraState != CameraMoveState.NotMoving){
            cameraMoving = true;
            switch (cameraState)
            {
                case CameraMoveState.MoveOut:
                    if (currentNode.previous != null) currentNode = currentNode.previous;
                    if (areX_AtRoot(currentNode))
                    {
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

                    if (currentNode.next != null) currentNode = currentNode.next;

                    if (areXsEquals(currentNode, endNode))
                    {
                        cameraState = CameraMoveState.NotMoving;
                    }


                    break;


            }


            if (areNodesEqual(currentNode, endNode))
            {
                cameraState = CameraMoveState.NotMoving;
            }
        }

    }

    int calculateDistanceToNode(CameraPathNode targetNode)
    {
        return currentNode.nodeXIndex + targetNode.nodeXIndex + Mathf.Abs(currentNode.nodeYIndex - targetNode.nodeYIndex);
    }

    public void setTarget(CameraPathNode targetNode)
    {
        endNode = targetNode;
    }

    public void gotoTarget()
    {
        onDeparture();
        
            if (areYsEqual(currentNode, endNode) && currentNode.nodeXIndex < endNode.nodeXIndex)
            {
                cameraState = CameraMoveState.MoveIn;
            }
            else
            {
                cameraState = CameraMoveState.MoveOut;
            }
        

        move();
    }

    public void onLand()
    {
        floorNode = getNodeInLevel(0);
        bookcaseNode = getNodeInLevel(1);
        shelfNode = getNodeInLevel(2);
        bookNode = getNodeInLevel(3);
    }


    public void onDeparture()
    {
        //print("depart");
    }

    public void onMoving()
    {
        //print("moving");
    }

    public void move(Vector3 destination, float duration)
    {
        throw new System.NotImplementedException();
    }

    public int getObjectIndex()
    {
        throw new System.NotImplementedException();
    }

    public int setObjectIndex(int objectIndex)
    {
        throw new System.NotImplementedException();
    }
}
