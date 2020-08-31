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
    public List<BoxCollider> bounds;

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
            if (SelectionManager.instance.canSelect)
            {
                if (current_state + 1 == 3 && !pageStateTransition.bookcasePathHandler.IsCurrentBookHasData())
                {
                    return;
                }
                SelectionManager.instance.selectThis(transitions[current_state]);

                //transitions[current_state].focus();
                current_state++;
                LevelUI.Instance.backFromPageModeBtn.SetActive(true);
                if (statistics)
                {
                    statistics.ToggleAllStatisticsUI(false);
                }
                enableBound(current_state);

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
            enableBound(current_state);
        }
    }

    public void tap()
    {
        if (checkBoundIsHit())
        {
            focus_state();
        }
    }

    IEnumerator ShowBackBtn()
    {
        yield return new WaitUntil(() => !CameraPath.instance.cameraMoving);
        LevelUI.Instance.backFromPageModeBtn.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    void enableBound(int index)
    {
        if (index >= 0 && index < bounds.Count)
        {
            foreach (BoxCollider b in bounds)
            {
                b.gameObject.SetActive(false);
            }
            bounds[index].gameObject.SetActive(true);
        }

        return;
    }

    public bool checkBoundIsHit()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag.Equals("bound"))
            {
                return true;
            }
        }
        return false;
    }
}
