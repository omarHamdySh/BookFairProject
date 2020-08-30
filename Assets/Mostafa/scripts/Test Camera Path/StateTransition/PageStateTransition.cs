using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PageStateTransition : MonoBehaviour, IClickable
{
    public ShelfStateTransition previous;
    public BookcasePathHandller_Bendary bookcasePathHandler;
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
        if (bookcasePathHandler.IsCurrentBookHasData())
        {
            CameraPath.instance.setTarget(CameraPath.instance.pageNode);
            CameraPath.instance.gotoTarget();
            GameManager.Instance.gameplayFSMManager.toBookPageState();

            // Bendary modify
            bookcasePathHandler.MoveRealBookForward(CameraPath.instance.cameraSpeed, animatedBook);
        }
    }

    public void unfocus()
    {
        // Bendary modify
        LevelUI.Instance.backToUIModeBtn.interactable = false;
        animatedBook.CloseBook();
        animatedBook.RotateToOrign(closeBookAnimationDelay, unfocusCallback);
    }

    void unfocusCallback()
    {

        bookcasePathHandler.MoveRealBookBackword(CameraPath.instance.cameraSpeed, animatedBook, OnBookBackToOrign);

    }

    private void OnBookBackToOrign()
    {
        SelectionManager.instance.selectThis(previous);
        //CameraPath.instance.setTarget(CameraPath.instance.bookNode);
        //CameraPath.instance.gotoTarget();
        GameManager.Instance.gameplayFSMManager.toShelfState();
    }

}
