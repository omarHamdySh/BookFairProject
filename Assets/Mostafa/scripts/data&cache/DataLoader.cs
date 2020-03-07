using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//functions to load data in different states
public class DataLoader : MonoBehaviour
{


    public BookcasePathHandller_Bendary bookcasePathHandler;
    public ShelfPathHandller_Bendary shelfPathHandler;

    public float maxInterval = 5;
    private float currentInterval;

    public bool active = true;

    private void Start()
    {
        if (Cache.Instance)
        {

            if (Cache.Instance.cachedData.allVendors != null)
            {
                floorModeVendorEnumrator = Cache.Instance.cachedData.allVendors.GetEnumerator();
            }

            if (Cache.Instance.cachedData.allCategories != null)
            {
                floorModeCategoryEnumrator = Cache.Instance.cachedData.allCategories.GetEnumerator();
            }
            floorModeCategoryEnumrator.MoveNext();
            floorModeVendorEnumrator.MoveNext();

            //Cache.Instance.dataArrivedEvent.AddListener(requestStateData);
            requestStateData();
        }
    }

    void FixedUpdate()
    {
        if (currentInterval >= maxInterval)
        {
            if (Cache.Instance.api.transmitting == false)
            {
                requestStateData();
                currentInterval = 0;
            }
        }

        currentInterval += Time.deltaTime;
    }
    public void requestStateData()
    {
        if (active)
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
                case GameplayState.BookPage:
                    funcShelfMode();
                    break;
                default:
                    print("not loading");
                    break;
            }
        }
    }

    List<Vendor>.Enumerator floorModeVendorEnumrator;
    List<ProductCategory>.Enumerator floorModeCategoryEnumrator;
    public void funcFloorMode()
    {
        int publisherId = 0;

        if (floorModeVendorEnumrator.Current != null)
        {
            publisherId = floorModeVendorEnumrator.Current.id;
        }


        if (floorModeVendorEnumrator.Current.bookcaseData.categories != null)
        {

            Cache.Instance.api.abortRetrieve();
            foreach (CategoryData c in floorModeVendorEnumrator.Current.bookcaseData.categories)
            {
                if(c.total < c.booksData.Count)
                    Cache.Instance.retrieveCategoryInBookcase(publisherId, c.id);
            }

        }

        if (!floorModeVendorEnumrator.MoveNext())
        {
            floorModeVendorEnumrator = Cache.Instance.cachedData.allVendors.GetEnumerator();
            floorModeVendorEnumrator.MoveNext();
        }

    }


    public int categoryIndex = 0;
    private int lastPublisherId = 0;
    //load categories in bookcase mode
    public void funcBookcaseMode()
    {
        Debug.Log("bookcase mode");
        int publisherId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].id;
        BookcaseData tmpBookcase = Cache.Instance.cachedData.allVendors.Find(v => v.id == publisherId).bookcaseData;

        if (tmpBookcase.categories != null)
        {
            if (categoryIndex >= tmpBookcase.categories.Count) categoryIndex = 0;
            Debug.Log("bc mode req " + tmpBookcase.categories[categoryIndex].name);
            Cache.Instance.retrieveCategoryInBookcase(publisherId, tmpBookcase.categories[categoryIndex++].id);
        }


    }

    public void funcShelfMode()
    {
        shelfPathHandler = bookcasePathHandler.getCurrentShelfPathHandler();
        int publisherId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].id;
        if (Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData != null)
        {
            if (Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData.categories != null)
            {
                int categoryId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData.categories[shelfPathHandler.GetCurrentShelf().categoryIndex].id;
                Cache.Instance.retrieveCategoryInBookcase(publisherId, categoryId);
            }
        }
    }


}
