using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;
using DG.Tweening;

public class StateTransition : MonoBehaviour
{
    public ShelfStateTransition shelfStateTransition;
    public BookcaseStateTransition bookcaseStateTransition;
    public PageStateTransition pageStateTransition;
    public BoxCollider2D bound;
    [SerializeField] private StatisticsUIHandller statistics;

    private List<IClickable> transitions;

    // Start is called before the first frame update
    void Start()
    {
        transitions = new List<IClickable>();
        transitions.Add(bookcaseStateTransition);
        transitions.Add(shelfStateTransition);
        transitions.Add(pageStateTransition);

    }

    public UInt16 current_state = 0;
    // Update is called once per frame
    void Update()
    {
        if (!CameraPath.instance.cameraMoving)
        {

            if (Input.GetKeyDown(KeyCode.Escape) && !LevelUI.Instance.isUIOpen)
            {
                unfocus_state();
            }

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !LevelUI.Instance.isUIOpen)
            {
                focus_state();
            }
        }
    }


    public void focus_state()
    {
        if (current_state < transitions.Count && !LevelUI.Instance.isUIOpen)
        {
            SelectionManager.instance.selectThis(transitions[current_state]);
            //transitions[current_state].focus();
            current_state++;
            LevelUI.Instance.backFromPageModeBtn.SetActive(true);
            if (statistics)
            {
                statistics.ToggleAllStatisticsUI(false);
            }
        }
    }


    public void unfocus_state()
    {
        if (current_state > 0)
        {
            if (SelectionManager.instance.canSelect)
            {
                SelectionManager.instance.deselectCurrent();
            }
            current_state--;
            LevelUI.Instance.backFromPageModeBtn.SetActive(false);
            if (current_state > 0)
            {
                StartCoroutine(ShowBackBtn());
            }
            else if (current_state == 0)
            {
                if (statistics)
                {
                    statistics.ToggleAllStatisticsUI(true);
                }
            }
        }
    }

    public void tap()
    {
  
        Vector3 mouse_position = Input.mousePosition;
        mouse_position.z = 1f;
        mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);

        bool collides = mouse_position.x < bound.bounds.max.x && mouse_position.x > bound.bounds.min.x && mouse_position.y < bound.bounds.max.y && mouse_position.y > bound.bounds.min.y;
        Debug.Log(mouse_position);
        if (collides){
            focus_state();
        }
    }

    IEnumerator ShowBackBtn()
    {
        yield return new WaitUntil(() => !CameraPath.instance.cameraMoving);
        LevelUI.Instance.backFromPageModeBtn.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
