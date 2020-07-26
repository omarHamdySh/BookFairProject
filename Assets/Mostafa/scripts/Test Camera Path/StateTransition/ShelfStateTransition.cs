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

        // Bendary modify
        LevelUI.Instance.backFromPageModeBtn.SetActive(false);
        LevelUI.Instance.backFromShelfModeBtn.SetActive(false);
        LevelUI.Instance.backFromBookModeBtn.SetActive(true);
        
    }

    public void unfocus()
    {
        
        GetComponent<BoxCollider>().enabled = true;
        SelectionManager.instance.selectThis(previous);
        //CameraPath.instance.setTarget(CameraPath.instance.bookcaseNode);
        //CameraPath.instance.gotoTarget();

        GameManager.Instance.gameplayFSMManager.toBookCaseState();

        // Bendary modify
        LevelUI.Instance.backFromBookModeBtn.SetActive(false);
    }

}
