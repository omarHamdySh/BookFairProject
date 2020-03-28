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
        Debug.Log(Cache.Instance.getFairId());
        if(Cache.Instance.getFairId() == -1)
        {
            Cache.Instance.setFairId(Cache.Instance.cachedData.allFairs[0].id);
            PlayerPrefs.SetInt(ImportantStrings.fairIDKey, Cache.Instance.getFairId());
        }
        else
        {
            Cache.Instance.setFairId(PlayerPrefs.GetInt(ImportantStrings.fairIDKey));
        }
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
