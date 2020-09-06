using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    public List<Transition> Transitions = new List<Transition>();

    private int _transitionIndex;
    public bool _moveTransition;

    [Header("Text Properties")]
    public FixTextMeshPro Text;

    [Header("UIMask Properties")]
    public GameObject Mask;

    [Header("Animation Properties")]
    public GameObject ClickAnimation;
    //public GameObject SwipeAnimation_Left;
    //public GameObject SwipeAnimation_Right;


    private void Start()
    {
        _transitionIndex = 0;
        _moveTransition = false;


        if (_transitionIndex < (Transitions.Count))
        {
            StartCoroutine(DisplayTransition());
        }

    }

    IEnumerator DisplayTransition()
    {
        _moveTransition = false;

        SetTransitionText();

        SetTransitionMask();

        SetTransitionImage();

        yield return new WaitUntil(() => _moveTransition);

        UpdateTransition();
    }

    void SetTransitionText()
    {
        Text.text = Transitions[_transitionIndex].TransitionText.Text.ToString();
        Text.GetComponent<TextMeshProUGUI>().text = Transitions[_transitionIndex].TransitionText.Text.ToString();
    }

    void SetTransitionImage()
    {
        ClickAnimation.transform.position = new Vector3(
                                                          Transitions[_transitionIndex].TransitionAnimation.ImageTransform.position.x,
                                                          Transitions[_transitionIndex].TransitionAnimation.ImageTransform.position.y,
                                                          Transitions[_transitionIndex].TransitionAnimation.ImageTransform.position.z
                                                       );
    }

    void SetTransitionMask()
    {
        //Transition Mask.
        Mask.transform.position = new Vector3(Transitions[_transitionIndex].TransitionMask.MaskTransform.position.x,
                                     Transitions[_transitionIndex].TransitionMask.MaskTransform.position.y,
                                     Transitions[_transitionIndex].TransitionMask.MaskTransform.position.z);

        Mask.transform.localScale = new Vector3(Transitions[_transitionIndex].TransitionMask.MaskTransform.localScale.x,
                                    Transitions[_transitionIndex].TransitionMask.MaskTransform.localScale.y,
                                    Transitions[_transitionIndex].TransitionMask.MaskTransform.localScale.z);
    }

    void UpdateTransition()
    {
        _transitionIndex++;

        if (_transitionIndex < (Transitions.Count))
            StartCoroutine(DisplayTransition());
    }

}
