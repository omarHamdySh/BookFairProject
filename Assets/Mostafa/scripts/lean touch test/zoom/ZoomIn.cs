using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ZoomIn : MonoBehaviour
{
    public CameraHandler cameraHandler;
    private LeanSelectable ls;


    // Start is called before the first frame update
    void Start()
    {
        Lean.Touch.LeanTouch.OnFingerTap += zoomIn;
        ls = GetComponent<LeanSelectable>();
    }

    void zoomIn(Lean.Touch.LeanFinger finger)
    {
        
            if(ls.IsSelected)
            {
                    cameraHandler.select(transform);
            }
        
    }

    
}
