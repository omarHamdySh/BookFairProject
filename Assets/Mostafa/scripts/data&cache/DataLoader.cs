using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functions to load data in different states
public class DataLoader : MonoBehaviour
{


    public BookcasePathHandller_Bendary bookcasePathHandler;
    public ShelfPathHandller_Bendary shelfPathHandler;

    private void Start()
    {
        if (Cache.Instance)
        {
            Cache.Instance.dataArrivedEvent.AddListener(requestStateData);
        }
    }

    public void requestStateData()
    {
        switch (GameManager.Instance.gameplayFSMManager.getCurrentState())
        {
            case GameplayState.Floor:
                funcFloorMode();
                break;
            case GameplayState.BookCase:
                funcBookcaseMode();
                break;
            case GameplayState.Shelf:
                funcShelfMode();
                break;
            default:
                print("noth");
                break;
        }
    }
    public void funcFloorMode()
    {
        if (Cache.Instance.loadedBooks < Cache.Instance.booksLimit / 2)
        {
            Debug.Log("floor mode");
            StopAllCoroutines();
            foreach (Vendor vendor in Cache.Instance.cachedData.allVendors)
            {
                foreach (ProductCategory pc in Cache.Instance.cachedData.allCategories)
                {
                    Cache.Instance.retrieveCategoryInBookcase(vendor.id, pc.id);
                }
            }
        }
    }

    public int categoryIndex = 0;
    private int lastPublisherId = 0;
    //load categories in bookcase mode
    public void funcBookcaseMode()
    {
        StopAllCoroutines();
        int publisherId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].id;
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

    public void funcShelfMode()
    {
        shelfPathHandler = bookcasePathHandler.getCurrentShelfPathHandler();
        StopAllCoroutines();
        int publisherId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].id;
        if (Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData != null)
        {
            int categoryId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData.categories[0].id;
            Cache.Instance.retrieveCategoryInBookcase(publisherId, categoryId);
        }
    }


}
