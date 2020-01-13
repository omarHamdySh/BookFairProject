using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManagerManager : MonoBehaviour
{
    public List<PathManager> pathManagers;


    public void Start()
    {
        if (pathManagers.Count>0)
        {
            foreach (var item in pathManagers)
            {
                //item.Init(this);
            }
        }
    }
}
