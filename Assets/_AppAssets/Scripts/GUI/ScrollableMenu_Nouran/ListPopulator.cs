using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;


public class ListPopulator : MonoBehaviour
{
    public GameObject BtnTemplate;  
    private int ListCount;        //the size of the vendors/publishers list.

    // Start is called before the first frame update
    void Start()
    {
        ListCount = Cache.Instance.cachedData.allVendors.Count;
        Populate();
    }

    public void Populate()
    {
        for (int i = 0; i < ListCount; i++)
        {
            GameObject NewButton = Instantiate(BtnTemplate) as GameObject;
            NewButton.transform.parent = gameObject.transform;
            NewButton.transform.GetChild(1).GetComponent<FixTextMeshPro>().SetText(Cache.Instance.cachedData.allVendors[i].name);
            //Debug.Log("Vendor " + i + "name is " + Cache.Instance.cachedData.allVendors[i].name);
        }
    }

    
}
