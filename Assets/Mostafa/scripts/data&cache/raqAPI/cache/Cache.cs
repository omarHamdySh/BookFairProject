﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.IO;
using System.IO.Compression;
using UnityEngine.Events;

public class Cache : MonoBehaviour
{
    public CachedData cachedData;

    public RaqAPI api;

    public UnityEvent dataArrivedEvent;

    public delegate void SearchCallBack(List<BookData> result, int total);
    SearchCallBack searchCallBack;

    public int oneTimeLoadLimit;//maxiumum number of books to be loaded at one time
    #region singleton
    private static Cache _instance;
    public static Cache Instance { get { return _instance; } }

    public int booksLimit = 5;
    public int loadedBooks;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    
    
    void Start()
    {
        api = GetComponent<RaqAPI>();
        api.Init();

        cachedData = new CachedData();
        cachedData.allVendors = new List<Vendor>();
        cachedData.allCategories = new List<ProductCategory>();
        cachedData.searchResult = new List<BookData>();
        cachedData.allSponsors = new List<Sponsor>();
    }

    [ContextMenu("Finish Loading Data")]
    public void FinishLoadingData()
    {
        UIHandller uiHandler = FindObjectOfType<UIHandller>();
        uiHandler.LoadLevel("TestUIScene_Bendary", true);

    }


    
    void clearCache()
    {
        if(cachedData.allVendors != null)
        {
            foreach(Vendor v in cachedData.allVendors)
            {
                v.bookcaseData = null;
            }
        }
    }



    //purpose: gets book case by publisher id
    public void retrieveCategoryInBookcase(int publisherId, int categoryId)
    {
        //all vendors and categories must be present first
        Vendor tmpVendorReference = cachedData.allVendors.Find(v => v.id == publisherId);
        if (tmpVendorReference != null)
        {

            BookcaseData tmpBookcase = tmpVendorReference.bookcaseData;
            CategoryData tmpCat = null;


            if (tmpBookcase.categories != null)
            {
                tmpCat = tmpBookcase.categories.Find(c => c.id == categoryId);
            }

            if (tmpCat == null)
            {
                StartCoroutine(api.productsByPublisher(publisherId, categoryId, oneTimeLoadLimit, 1));
            }
            else if (tmpCat.booksData.Count < tmpCat.total)
            {
                tmpCat.accessFrequency++;
                StartCoroutine(api.productsByPublisher(publisherId, categoryId, oneTimeLoadLimit, tmpCat.page));
            }
            else
            {
                tmpCat.accessFrequency++;
                //call back
            }

        }
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

    public void retrieveFairs()
    {
        StartCoroutine(api.getCurrentFairs());
    }

    public void retrieveSponsors()
    {
        StartCoroutine(api.getAllSponsors());
    }

    public void search(SearchCallBack callBack, int limit, int page, string keyword, int categoryId = -1)
    {
        cachedData.searchResult = new List<BookData>();
        StartCoroutine(api.searchWithFilter(keyword, categoryId, limit, page));
        searchCallBack = callBack;

    }

    public void setFairId(int id)
    {
        api.fairId = id;
    }
    public int getFairId()
    {
        return api.fairId;
    }
    ////////////////////////////////caching functions//////////////////////////////////////////
    public void cacheCategoryInPublisher(ProductResult res, int publisherId, int categoryId)
    {

        Vendor tmpVendorReference = cachedData.allVendors.Find(v => v.id == publisherId);
        if (res.prodcutList.Count > 0 && tmpVendorReference != null)
        {
            if (loadedBooks >= booksLimit)
            {
                removeExcess();
            }
            if (res.prodcutList.Count > 0)
            {
                BookcaseData tmpBookcase = null;
                CategoryData tmpCat = null;

                tmpBookcase = tmpVendorReference.bookcaseData;

                if (tmpBookcase == null) tmpBookcase = new BookcaseData();

                if (tmpBookcase.categories != null)
                {
                    tmpCat = tmpBookcase.categories.Find(c => c.id == categoryId);
                }
                else
                {
                    tmpBookcase.categories = new List<CategoryData>();
                }

                if (tmpCat == null)
                {
                    tmpCat = new CategoryData();
                    tmpCat.booksData = new List<BookData>();
                    tmpCat.id = categoryId;
                    tmpCat.name = cachedData.allCategories.Find(x => x.id == categoryId).name;
                    tmpCat.total = res.totalRecord;
                    tmpCat.page = 0;
                    tmpBookcase.categories.Add(tmpCat);
                }

                tmpCat.page++;

                foreach (Product book in res.prodcutList)
                {
                    if (tmpCat.booksData.Find(b => book.id == b.id) == null){
                        BookData tmpBook = new BookData();
                        tmpBook.url = api.makeBookUrl(book.id);
                        tmpBook.id = book.id;
                        tmpBook.texture = null;
                        tmpBook.description = book.shortDescription;
                        tmpBook.name = book.name;
                        tmpCat.booksData.Add(tmpBook);
                        if (book.defaultPicture != "" && book.defaultPicture != null)
                        {
                            tmpBook.imgString = Convert.ToBase64String(Decompress(Convert.FromBase64String(book.defaultPicture)));
                            Texture2D tmpTexture = new Texture2D(1, 1);
                            tmpTexture.LoadImage(Convert.FromBase64String(tmpBook.imgString));
                            tmpTexture.Apply();
                            tmpBook.texture = tmpTexture;
                        }
                        loadedBooks++;
                    }
                }


            }
        }
    }

    public void cacheSearchResult(ProductResult res)
    {
        if (res != null)
        {
            foreach (Product book in res.prodcutList)
            {
                BookData tmpBook = new BookData();
                tmpBook.url = api.makeBookUrl(book.id);
                tmpBook.id = book.id;
                tmpBook.texture = null;
                tmpBook.description = book.shortDescription;
                tmpBook.name = book.name;
                if (book.defaultPicture != "" && book.defaultPicture != null)
                {
                    tmpBook.imgString = Convert.ToBase64String(Decompress(Convert.FromBase64String(book.defaultPicture)));
                    Texture2D tmpTexture = new Texture2D(1, 1);
                    tmpTexture.LoadImage(Convert.FromBase64String(tmpBook.imgString));
                    tmpTexture.Apply();
                    tmpBook.texture = tmpTexture;
                }
                //if(tmpBook.imgString != "" && tmpBook.imgString != null)tmpBook.texture.LoadImage(Decompress(Convert.FromBase64String(tmpBook.imgString)));

                cachedData.searchResult.Add(tmpBook);
            }
            if (searchCallBack != null)
                searchCallBack(cachedData.searchResult, res.totalRecord);
        }
    }
    public void cacheAllCategories(AllCategoriesResult categoriesResult)
    {
        cachedData.allCategories = categoriesResult.categories;
    }

    public void cacheAllVendors(AllVendorsResult vendorsResult)
    {
        cachedData.allVendors = vendorsResult.vendorList;
    }

    public void cacheAllFairs(FairResult fairResult)
    {
        cachedData.allFairs = fairResult.fairsList;
        //api.fairId = cachedData.allFairs[0].id;
    }
    public void cacheAllSponsors(SponsorsResult sponsorsResult)
    {
        cachedData.allSponsors = sponsorsResult.sponsorList;
    }

    public static byte[] Compress(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(output, System.IO.Compression.CompressionLevel.Optimal))
        {
            dstream.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, System.IO.Compression.CompressionMode.Decompress))
        {
            dstream.CopyTo(output);
        }
        return output.ToArray();
    }

    public void removeLeastAccessedCategory(BookcaseData bookCaseData)
    {

        if (bookCaseData != null)
        {
            if (bookCaseData.categories != null)
            {
                if (bookCaseData.categories.Count > 0)
                {
                    CategoryData tmpLeastAccessCat = bookCaseData.categories[0];

                    foreach (CategoryData c in bookCaseData.categories)
                    {
                        if (c.accessFrequency < tmpLeastAccessCat.accessFrequency) tmpLeastAccessCat = c;
                    }

                    //remove pictures
                    foreach (BookData bd in tmpLeastAccessCat.booksData)
                    {
                        bd.texture = null;
                    }

                    loadedBooks -= tmpLeastAccessCat.booksData.Count;
                    tmpLeastAccessCat.page = 1;
                    tmpLeastAccessCat.accessFrequency = 0;
                    tmpLeastAccessCat.booksData.Clear();
                }
            }
        }

    }

    public void removeExcess()
    {
        foreach (Vendor v in cachedData.allVendors)
        {
            if (loadedBooks > booksLimit)
            {
                removeLeastAccessedCategory(v.bookcaseData);
            }
            else
            {
                break;
            }
        }
    }


}
