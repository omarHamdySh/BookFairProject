using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class BookcaseStateTransition : MonoBehaviour, IClickable
{
    [SerializeField] private BookcasePathHandller_Bendary bookcasePathHandler;
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
        CameraPath.instance.setTarget(CameraPath.instance.bookcaseNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toBookCaseState();

        // Bendary modify
        bookcasePathHandler.MoveRealBookcaseForward(CameraPath.instance.cameraSpeed);
        LevelUI.Instance.backFromPageModeBtn.SetActive(false);
        LevelUI.Instance.backFromBookModeBtn.SetActive(false);
        LevelUI.Instance.backFromShelfModeBtn.SetActive(true);
    }

    public void unfocus()
    {
        GetComponent<BoxCollider>().enabled = true;
        CameraPath.instance.setTarget(CameraPath.instance.floorNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toFloorState();

        // Bendary modify
        bookcasePathHandler.MoveRealBookcaseBackword(CameraPath.instance.cameraSpeed);
        LevelUI.Instance.backFromShelfModeBtn.SetActive(false);
    }

}
