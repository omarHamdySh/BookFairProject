﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node previous;
    public Node next;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Node getNext()
    {
        return next;
    }

    public Node getPrevious()
    {
        return previous;
    }
}
