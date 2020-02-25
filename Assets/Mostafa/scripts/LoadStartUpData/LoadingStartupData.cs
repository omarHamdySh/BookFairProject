using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingStartupData : MonoBehaviour
{
    public int loadedBooksLimit;

    public CacheSO cachedata;


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
        foreach(Vendor vendor in cachedata.allVendors)
        {
            foreach(ProductCategory pc in cachedata.allCategories)
            {
                Cache.Instance.retrieveCategoryInBookcase(vendor.id, pc.id);
            }
        }
    }
}
