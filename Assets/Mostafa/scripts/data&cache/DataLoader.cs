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

    public class DataRequest
    {
        public int vendorId;
        public int categoryId;
    }

    public Queue<DataRequest> requestQueue;
    private void Start()
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


        Cache.Instance.api.abortRetrieve();

        requestQueue = new Queue<DataRequest>();

    }

    void FixedUpdate()
    {
        if (currentInterval >= maxInterval)
        {
            if (Cache.Instance.api.transmitting == false)
            {
                requestStateData();
                currentInterval = 0;

                if (requestQueue.Count > 0)
                {
                    Debug.Log("request");
                    Cache.Instance.retrieveCategoryInBookcase(requestQueue.Peek().vendorId, requestQueue.Peek().categoryId);
                    requestQueue.Dequeue();
                }
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
        categoryIndex = 0;
        int publisherId = 0;

        if (floorModeVendorEnumrator.Current != null)
        {
            publisherId = floorModeVendorEnumrator.Current.id;
        }


        if (floorModeVendorEnumrator.Current.bookcaseData.categories != null)
        {
            foreach (CategoryData c in floorModeVendorEnumrator.Current.bookcaseData.categories)
            {
                if (c.total > c.booksData.Count)
                {
                    Debug.Log("floor enq");
                    DataRequest dr = new DataRequest();
                    dr.vendorId = publisherId;
                    dr.categoryId = c.id;
                    requestQueue.Enqueue(dr);
                }

            }

            if (!floorModeVendorEnumrator.MoveNext())
            {
                floorModeVendorEnumrator = Cache.Instance.cachedData.allVendors.GetEnumerator();
                floorModeVendorEnumrator.MoveNext();
            }

        }

    }
    public int categoryIndex = 0;
    private int lastPublisherId = 0;
    //load categories in bookcase mode
    public void funcBookcaseMode()
    {
        int publisherId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].id;
        BookcaseData tmpBookcase = Cache.Instance.cachedData.allVendors.Find(v => v.id == publisherId).bookcaseData;

        Cache.Instance.api.abortRetrieve();
        if (tmpBookcase.categories != null)
        {
            if (categoryIndex < tmpBookcase.categories.Count)
            {
                if (tmpBookcase.categories[categoryIndex].total > tmpBookcase.categories[categoryIndex].booksData.Count)
                {
                    Debug.Log("bookcase enq");
                    DataRequest dr = new DataRequest();
                    dr.vendorId = publisherId;
                    dr.categoryId = tmpBookcase.categories[categoryIndex++].id;
                    requestQueue.Enqueue(dr);
                }
                else
                {
                    categoryIndex = 0;
                }
            }
            else
            {
                categoryIndex = 0;
            }
        }


    }

    public void funcShelfMode()
    {
        categoryIndex = 0;
        shelfPathHandler = bookcasePathHandler.getCurrentShelfPathHandler();
        int publisherId = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].id;
        if (Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData != null)
        {
            Cache.Instance.api.abortRetrieve();
            if (Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData.categories != null)
            {
                CategoryData tmpCat = Cache.Instance.cachedData.allVendors[bookcasePathHandler.vendorIndex].bookcaseData.categories[shelfPathHandler.GetCurrentShelf().categoryIndex];

                if (tmpCat.total > tmpCat.booksData.Count)
                {
                    int categoryId = 0;
                    categoryId = tmpCat.id;

                    Debug.Log("shelf enq");

                    DataRequest dr = new DataRequest();
                    dr.vendorId = publisherId;
                    dr.categoryId = categoryId;
                    requestQueue.Enqueue(dr);
                }

            }
        }
    }


    void dataArrivedCallBack()
    {
        if (requestQueue.Count > 0)
            requestQueue.Dequeue();
    }
}
