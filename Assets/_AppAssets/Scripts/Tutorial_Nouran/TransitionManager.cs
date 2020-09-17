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

    [Header("Image Properties")]
    public Image Image;

    [Header("UIMask Properties")]
    public GameObject Mask;

    [Header("Animation Properties")]
    public List<GameObject> Animations;

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

        SetTransitionImage(Transitions[_transitionIndex].TransitionAnimation.AnimationType);

        SetTransitionGIF();

        StartCoroutine(UpdateTransition());

        //yield return new WaitUntil(() => _moveTransition);       
    }

    void SetTransitionText()
    {
        Text.text = Transitions[_transitionIndex].TransitionText.Text.ToString();
        Text.GetComponent<TextMeshProUGUI>().text = Transitions[_transitionIndex].TransitionText.Text.ToString();
    }

    void SetTransitionImage(AnimationType animationType)
    {
        Animations[ReturnEnumIdex(animationType)].SetActive(true);
        Animations[ReturnEnumIdex(animationType)].transform.position = new Vector3(
                                                          Transitions[_transitionIndex].TransitionAnimation.ImageTransform.position.x,
                                                          Transitions[_transitionIndex].TransitionAnimation.ImageTransform.position.y,
                                                          Transitions[_transitionIndex].TransitionAnimation.ImageTransform.position.z
                                                       );
    }

    void SetTransitionMask()
    {
        if(Transitions[_transitionIndex].TransitionMask.HasMask)
        {
            //Transition Mask.
            Mask.SetActive(true);
            Mask.transform.position = new Vector3(Transitions[_transitionIndex].TransitionMask.MaskTransform.position.x,
                                         Transitions[_transitionIndex].TransitionMask.MaskTransform.position.y,
                                         Transitions[_transitionIndex].TransitionMask.MaskTransform.position.z);

            Mask.transform.localScale = new Vector3(Transitions[_transitionIndex].TransitionMask.MaskTransform.localScale.x,
                                        Transitions[_transitionIndex].TransitionMask.MaskTransform.localScale.y,
                                        Transitions[_transitionIndex].TransitionMask.MaskTransform.localScale.z);
        }       
    }

    void SetTransitionGIF()
    {
        if (Transitions[_transitionIndex].TransitionImage.HasImage)
        {
            Image.transform.gameObject.SetActive(true);
            Image.sprite = Transitions[_transitionIndex].TransitionImage.Image;
            Image.GetComponent<Animator>().runtimeAnimatorController = Transitions[_transitionIndex].TransitionImage.AnimatorController;
        }
        else
        {
            return;
        }
            
    }

    IEnumerator UpdateTransition()
    {
        yield return new WaitForSeconds(4f);

        Reset();

        _transitionIndex++;

        if (_transitionIndex < (Transitions.Count))
            DisplayTransition();
       
    }

    private void Reset()
    {
        Animations[ReturnEnumIdex(Transitions[_transitionIndex].TransitionAnimation.AnimationType)].SetActive(false);
        Image.transform.gameObject.SetActive(false);
        Mask.SetActive(false);
    }

    private int ReturnEnumIdex(AnimationType animationType)
    {
        if (animationType == AnimationType.ClickAnimation)
            return 0;

        if (animationType == AnimationType.SwipeAnimation_Left)
            return 1;

        if (animationType == AnimationType.SwipeAnimation_Right)
            return 2;

        if (animationType == AnimationType.SwipeAnimation_Up)
            return 3;

        else
            return 4;
    }

}
