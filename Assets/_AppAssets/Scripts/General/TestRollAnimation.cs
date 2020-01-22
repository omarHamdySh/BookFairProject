using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRollAnimation : MonoBehaviour
{
    [SerializeField] private Animator myAnim;

    public void ToggleRollAnimation(bool check)
    {
        myAnim.SetBool("Open", check);
    }
}
