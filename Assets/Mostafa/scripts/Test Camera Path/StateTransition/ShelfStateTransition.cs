using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ShelfStateTransition : MonoBehaviour, IClickable
{

    public BookcaseStateTransition previous;

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
        CameraPath.instance.setTarget(CameraPath.instance.shelfNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toShelfState();
    }

    public void unfocus()
    {
        SelectionManager.instance.selectThis(previous);
        //CameraPath.instance.setTarget(CameraPath.instance.bookcaseNode);
        //CameraPath.instance.gotoTarget();

        GameManager.Instance.gameplayFSMManager.toBookCaseState();
    }

}
