using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PageStateTransition : MonoBehaviour, IClickable
{
    public ShelfStateTransition previous;
    [SerializeField] private BookcasePathHandller_Bendary bookcasePathHandler;
    [SerializeField] private TestBookRotation_Bendary animatedBook;
    [SerializeField] private float closeBookAnimationDelay;

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
        CameraPath.instance.setTarget(CameraPath.instance.pageNode);
        CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toBookPageState();

        // Bendary modify
        bookcasePathHandler.MoveRealBookForward(CameraPath.instance.cameraSpeed, animatedBook);
        LevelUI.Instance.backFromBookModeBtn.SetActive(false);
        LevelUI.Instance.backFromShelfModeBtn.SetActive(false);
        LevelUI.Instance.backFromPageModeBtn.SetActive(true);
    }

    public void unfocus()
    {
        // Bendary modify
        LevelUI.Instance.backToUIModeBtn.interactable = false;
        animatedBook.CloseBook();
        animatedBook.RotateToOrign(closeBookAnimationDelay, unfocusCallback);
        LevelUI.Instance.backFromPageModeBtn.SetActive(false);
    }

    void unfocusCallback()
    {

        bookcasePathHandler.MoveRealBookBackword(CameraPath.instance.cameraSpeed, animatedBook, OnBookBackToOrign);

    }

    private void OnBookBackToOrign()
    {
        GetComponent<BoxCollider>().enabled = true;
        SelectionManager.instance.selectThis(previous);
        //CameraPath.instance.setTarget(CameraPath.instance.bookNode);
        //CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toShelfState();
    }

}
