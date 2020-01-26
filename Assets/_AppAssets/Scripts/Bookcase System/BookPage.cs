using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class BookPage : MonoBehaviour, IClickable
{
    
    public PathNode pathNode;
    LeanSelectable leanSelectable;
    public string pageInfo;

    void Start()
    {
        leanSelectable = GetComponent<LeanSelectable>();
    }

    public void focus()
    {
        throw new System.NotImplementedException();
    }

    public void unfocus()
    {
        throw new System.NotImplementedException();
    }
}