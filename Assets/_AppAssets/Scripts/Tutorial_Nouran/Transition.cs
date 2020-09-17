using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Transition
{
    public TransitionText TransitionText;
    public TransitionMask TransitionMask;
    public TransitionAnimation TransitionAnimation;
    public TransitionImage TransitionImage;
}

[System.Serializable]
public struct TransitionText
{
    public string Text;
}

[System.Serializable]
public struct TransitionMask
{
    public bool HasMask;
    public Transform MaskTransform;
}


[System.Serializable]
public struct TransitionAnimation
{
    public AnimationType AnimationType;
    public Transform ImageTransform;
}

[System.Serializable]
public struct TransitionImage
{
    public bool HasImage;
    public Sprite Image;
    public RuntimeAnimatorController AnimatorController;
}


[System.Serializable]
public enum AnimationType
{
    ClickAnimation,
    SwipeAnimation_Left,
    SwipeAnimation_Right,
    SwipeAnimation_Up,
    SwipeAnimation_Down
}

