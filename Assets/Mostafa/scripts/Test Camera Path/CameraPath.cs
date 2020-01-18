using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraPath : MonoBehaviour,ITraverseable
{
    private PathNode currentNode,endNode;
    private int currentLevel;

    public List<PathNode> levels;

    public Transform cameraTransform;


    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
        gotoNode(0, 0);
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



    public void move()
    {
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
