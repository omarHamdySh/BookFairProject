using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ZoomIn : MonoBehaviour
{
    [SerializeField] private TestCamerPath myTestCameraPath;

    private LeanSelectable ls;

    void Start()
    {
        LeanTouch.OnFingerTap += zoomIn;
        ls = GetComponent<LeanSelectable>();
    }

    void zoomIn(LeanFinger finger)
    {
        if (ls.IsSelected)
        {
            if (myTestCameraPath)
            {
                myTestCameraPath.isGettingFocus = true;
            }
            else
            {
                print("testcamera not added");
            }
        }
    }
}
