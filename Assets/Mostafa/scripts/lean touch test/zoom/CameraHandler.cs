using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    private Vector3 initialPos;

    

    void Start()
    {
        initialPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    
           
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            resetCamera();
        }

        
    }

    void resetCamera()
    {
        transform.position = initialPos;
    }

    

    public void select(Transform t)
    {
        //if(current != 0)
          transform.position = t.position;        
    }

}
