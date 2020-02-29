using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functions to load data in different states
public class DataLoader : MonoBehaviour
{
    #region Singleton
    public static DataLoader instance { private set; get; }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void funcFloorMode()
    {
        Debug.Log("floor load");
        StopAllCoroutines();
        foreach (Vendor vendor in Cache.Instance.cachedData.allVendors)
        {
            foreach (ProductCategory pc in Cache.Instance.cachedData.allCategories)
            {
                Cache.Instance.retrieveCategoryInBookcase(vendor.id, pc.id);
            }
        }
    }

    public int categoryIndex = 0;
    private int lastPublisherId = 0;
    //load categories in bookcase mode
    public void funcBookcaseMode(int publisherId)
    {
        print(publisherId);

        if (GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.BookCase)
        {
            StopAllCoroutines();
            BookcaseData tmpBookcase = Cache.Instance.cachedData.allVendors.Find(v => v.id == publisherId).bookcaseData;

            if (tmpBookcase != null)
            {
                if (tmpBookcase.categories != null)
                {
                    if (categoryIndex >= tmpBookcase.categories.Count) categoryIndex = 0;
                    Cache.Instance.retrieveCategoryInBookcase(publisherId, tmpBookcase.categories[categoryIndex++].id);
                }
            }
        }
    }

    public void funcShelfMode(int publisherId, int categoryId)
    {
        StopAllCoroutines();
        Cache.Instance.retrieveCategoryInBookcase(publisherId, categoryId);
    }

    public void funcDataArrivedCallback()
    {
        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.gameplayFSMManager.getCurrentState())
            {
                case GameplayState.BookCase:
                    funcBookcaseMode(lastPublisherId);
                    print("loading " + categoryIndex.ToString());
                    break;
            }
        }
    }
}
