using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public CacheSO cachedData;

    public RaqAPI api;


    #region singleton
    private static Cache _instance;
    public static Cache Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //cachedData.bookcasesData = new List<BookcaseData>();
        
    }

    [ContextMenu("foo")]
    void foo()
    {
        retrieveCategories();
        retrieveVendors();
    }
    [ContextMenu("foo1")]
    void foo1()
    {
        retrieveCategoryInBookcase(4, 20, 0, 0);
    }

    [ContextMenu("foo2")]
    void foo2()
    {
        retrieveCategoryInBookcase(4, 23, 10, 1);
    }


    //purpose: gets book case by publisher id
    public void retrieveCategoryInBookcase(int publisherId, int categoryId, int limit, int page)
    {
        StartCoroutine(api.productsByPublisher(publisherId, categoryId, limit, page));
    }

    //retrieves every single category
    public void retrieveCategories()
    {
        StartCoroutine(api.getAllCategories(0, 0));
    }

    public void retrieveVendors()
    {
        StartCoroutine(api.getAllVendors(0, 0));
    }

    ////////////////////////////////caching functions//////////////////////////////////////////

    public void cacheCategoryInPublisher(ProductResult res, int publisherId, int categoryId)
    {

        if (res != null)
        {
            BookcaseData tmpBookcase;
            tmpBookcase = cachedData.bookcasesData.Find(bd => bd.id == publisherId);
            if (tmpBookcase == null)
            {
                tmpBookcase = new BookcaseData();
                tmpBookcase.id = publisherId;
                tmpBookcase.name = cachedData.allVendors.Find(x => x.id == publisherId).name;
                tmpBookcase.categories = new List<CategoryData>();

                CategoryData tmpCat = new CategoryData();
                tmpCat.booksData = new List<BookData>();
                tmpCat.id = categoryId;
                tmpCat.name = cachedData.allCategories.Find(x => x.id == categoryId).name;
                tmpCat.total = res.totalRecord;

                foreach (Product book in res.prodcutList)
                {
                    BookData tmpBook = new BookData();
                    tmpBook.id = book.id;
                    //tmpBook.description = book.shortDescription;
                    tmpBook.name = book.name;
                    //add picture and url later
                    tmpBook.imgString = book.defaultPicture;
                    tmpCat.booksData.Add(tmpBook);
                    tmpCat.loadedBooks++;
                }
                tmpBookcase.categories.Add(tmpCat);
                cachedData.bookcasesData.Add(tmpBookcase);
            }
            else
            {
                CategoryData tmpCat = tmpBookcase.categories.Find(cat => cat.id == categoryId);
                if (tmpCat == null)
                {
                    tmpCat = new CategoryData();
                    tmpCat.booksData = new List<BookData>();
                    tmpCat.id = categoryId;
                    tmpCat.name = cachedData.allCategories.Find(x => x.id == categoryId).name;
                    tmpCat.total = res.totalRecord;

                    tmpBookcase.categories.Add(tmpCat);

                    tmpCat = tmpBookcase.categories[tmpBookcase.categories.Count - 1];
                }
                foreach (Product book in res.prodcutList)
                {
                    BookData tmpBook = new BookData();
                    tmpBook.id = book.id;
                    //tmpBook.description = book.shortDescription;
                    tmpBook.name = book.name;
                    //add picture and url later
                    tmpBook.imgString = book.defaultPicture;
                    tmpCat.booksData.Add(tmpBook);
                    tmpCat.loadedBooks++;
                }
            }
        }
        //TODO
    }

    public void cacheAllCategories(AllCategoriesResult categoriesResult)
    {
        cachedData.allCategories = categoriesResult.categories;
    }

    public void cacheAllVendors(AllVendorsResult vendorsResult)
    {
        cachedData.allVendors = vendorsResult.vendorList;

        foreach (var vendor in cachedData.allVendors)
        {
            Debug.Log(vendor.name);
        }
    }


}
