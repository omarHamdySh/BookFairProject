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
    
    [Header("Text Properties")]
    public FixTextMeshPro Text;

    [Header("UIMask Properties")]
    public GameObject Mask;

    [Header("Animation Properties")]
    public GameObject ClickAnimation;
    //public GameObject SwipeAnimation_Left;
    //public GameObject SwipeAnimation_Right;

    //#region Singelton
    //public static TransitionManager Instance;
    //private void Awake()
    //{
    //    Instance = this;
    //}
    //#endregion

    private void Start()
    {
        _transitionIndex = 0;
        
        if (_transitionIndex < (Transitions.Count))
        {
            DisplayTransition();
        }

    }

    private void DisplayTransition()
    {
        SetTransitionText();

        SetTransitionMask();

        SetTransitionImage();

        StartCoroutine(UpdateTransition());

        //yield return new WaitUntil(() => _moveTransition);       
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

    IEnumerator UpdateTransition()
    {
        yield return new WaitForSeconds(4f);

        _transitionIndex++;

        if (_transitionIndex < (Transitions.Count))
            DisplayTransition();
       
    }

}
