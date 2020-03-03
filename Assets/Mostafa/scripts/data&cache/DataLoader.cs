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

            if(Cache.Instance.cachedData.allVendors != null)
            {
                floorModeVendorEnumrator = Cache.Instance.cachedData.allVendors.GetEnumerator();
            }
            
            if (Cache.Instance.cachedData.allCategories != null)
            {
                floorModeCategoryEnumrator = Cache.Instance.cachedData.allCategories.GetEnumerator();
            }
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

    List<Vendor>.Enumerator floorModeVendorEnumrator;
    List<ProductCategory>.Enumerator floorModeCategoryEnumrator;
    public void funcFloorMode()
    {
        int publisherId = 0;
        int categoryId = 0;

        if (floorModeVendorEnumrator.Current != null)
        {
            publisherId = floorModeVendorEnumrator.Current.id;
        }

        if (floorModeCategoryEnumrator.Current != null)
        {
            categoryId = floorModeCategoryEnumrator.Current.id;
        }
        StopAllCoroutines();
        Cache.Instance.retrieveCategoryInBookcase(publisherId, categoryId);

        if(!floorModeCategoryEnumrator.MoveNext())
        {
            if (!floorModeVendorEnumrator.MoveNext())
            {
                floorModeVendorEnumrator = Cache.Instance.cachedData.allVendors.GetEnumerator();
            }
            floorModeCategoryEnumrator = Cache.Instance.cachedData.allCategories.GetEnumerator();
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
