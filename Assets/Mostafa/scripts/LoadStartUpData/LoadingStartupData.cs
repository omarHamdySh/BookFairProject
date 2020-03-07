using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingStartupData : MonoBehaviour
{
    public int loadedBooksLimit;

    public UnityEvent startUpDataEvent;//data loaded at start up

    
    void Update()
    {
        if(Cache.Instance.loadedBooks >= loadedBooksLimit)
        {
            startUpDataEvent.Invoke();            
        }    
    }

    public void loadStartupData()
    {
        //loadedBooksLimit = Cache.Instance.cachedData.allVendors.Count * 10;
        foreach(Vendor vendor in Cache.Instance.cachedData.allVendors)
        {
            foreach(ProductCategory pc in Cache.Instance.cachedData.allCategories)
            {
                Cache.Instance.retrieveCategoryInBookcase(vendor.id, pc.id);
            }
        }
    }
}
