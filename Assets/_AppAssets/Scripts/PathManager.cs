using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.DemiLib;
using DG.Tweening;

public class PathManager : MonoBehaviour
{
    DOTweenPath doTweenPath;
    int currentIndex;
    int initialIndex;
    // Start is called before the first frame update'
    private void Awake()
    {
        doTweenPath = GetComponentInParent<DOTweenPath>();
        Init();
    }


    public void Init() {
        PathManagerManager  pmm = GetComponentInParent<PathManagerManager>();
        initialIndex = pmm.pathManagers.IndexOf(this);
        transform.position = doTweenPath.wps[initialIndex];
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (transform.position == doTweenPath.wps[currentIndex])
            //{
            //    currentIndex++;
            //}


            transform.DOMove(doTweenPath.wps[Mathf.Clamp(initialIndex++, 0, doTweenPath.wps.Count )],0.5f);

            if (initialIndex == doTweenPath.wps.Count)
            {
                initialIndex = 0;
            }
        }
    }
}
