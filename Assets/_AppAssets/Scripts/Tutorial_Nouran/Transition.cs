﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Transition
{
    public TransitionText TransitionText;
    public TransitionMask TransitionMask;
    public TransitionAnimation TransitionAnimation;
}

[System.Serializable]
public struct TransitionText
{
    public string Text;
}

[System.Serializable]
public struct TransitionMask
{
    public Transform MaskTransform;
}


[System.Serializable]
public struct TransitionAnimation
{
    public AnimationType AnimationType;

    public Transform ImageTransform;
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
