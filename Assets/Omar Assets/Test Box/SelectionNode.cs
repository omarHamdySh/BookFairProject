using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionNode : MonoBehaviour
{

    public void ShowSelectionOutline()
    {

    }

    public void DisableSelectionOutline()
    {

    }

    public void select() {
        SelectionManager.instance.selectedObject = gameObject;
    }

    public void unSelect() { 
    
    }

}
