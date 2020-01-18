using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPath : MonoBehaviour
{
    private Node currentNode;
    private int currentLevel;

    public List<Node> levels;

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

}
