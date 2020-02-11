using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PageStateTransition : MonoBehaviour, IClickable
{

    public BookStateTransition previous;

    LeanSelectable leanSelectable;
    void Start()
    {
        leanSelectable = GetComponent<LeanSelectable>();
    }

    public void select()
    {
        SelectionManager.instance.selectThis(this);
    }

    public void focus()
    {
        GetComponent<BoxCollider>().enabled = false;
        CameraPath.instance.setTarget(CameraPath.instance.pageNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toBookPageState();

    }

    public void unfocus()
    {
        
        GetComponent<BoxCollider>().enabled = true;
        SelectionManager.instance.selectThis(previous);
        CameraPath.instance.setTarget(CameraPath.instance.bookNode);
        CameraPath.instance.gotoTarget();

        GameManager.Instance.gameplayFSMManager.toBookState();
    }

}
