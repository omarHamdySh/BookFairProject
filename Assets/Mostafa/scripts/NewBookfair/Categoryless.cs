using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Categoryless : MonoBehaviour
{

    public GameObject[] data;

    public int rows;
    public int columns;

    public float x_pad;
    public float y_pad;

    [Range(0.0F, 20.0F)]
    public int movement_count;//how many TIMES did we move

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i = 0;
        foreach(GameObject gb in data)
        {
            Vector3 new_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 root = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            new_pos.y = ((movement_count + i) / columns) * y_pad;
            
            if (((movement_count + i) / columns + 1) % 2 == 0)//reverse
            {
                new_pos.x = ((columns - 1)-((movement_count + i) % columns)) * x_pad;
            }
            else
            {
                new_pos.x = ((movement_count + i) % columns) * x_pad;
            }

            gb.transform.position = new_pos;
            i++;
        }
    }
}
