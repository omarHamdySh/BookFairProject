using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;

public class Cache : MonoBehaviour
{
    public CacheSO cachedData;

    public List<Texture2D> cachedTextures;

    public RaqAPI api;


    public int maxLimit;//maxiumum number of books to be loaded at one time
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
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        api = GetComponent<RaqAPI>();
        api.Init();

        cachedData.allVendors = new List<Vendor>();
        cachedTextures = new List<Texture2D>();


    }

    [ContextMenu("Finish Loading Data")]
    public void FinishLoadingData()
    {
        UIHandller uiHandler = FindObjectOfType<UIHandller>();
        uiHandler.LoadLevel("TestUIScene_Bendary", true);
    }

    [ContextMenu("foo1")]
    void foo1()
    {
        retrieveCategoryInBookcase(4, 20, 10, 1);
    }

    [ContextMenu("foo2")]
    void foo2()
    {
        retrieveCategoryInBookcase(4, 20, 10, 2);
    }

    [ContextMenu("foo3")]
    void foo3()
    {
        retrieveCategoryInBookcase(4, 20, 10, 3);
    }

    [ContextMenu("foo4")]
    void foo4()
    {
        retrieveCategoryInBookcase(4, 23, 10, 1);
    }
    //purpose: gets book case by publisher id
    public void retrieveCategoryInBookcase(int publisherId, int categoryId, int limit, int page)
    {
        //all vendors and categories must be present first
        Vendor tmpVendorReference = cachedData.allVendors.Find(v => v.id == publisherId);

        if (tmpVendorReference != null)
        {
            BookcaseData tmpBookcase = tmpVendorReference.bookcaseData;
            CategoryData tmpCat = null;

            if (tmpBookcase != null)
            {

                if (tmpBookcase.categories != null)
                {
                    tmpCat = tmpBookcase.categories.Find(c => c.id == categoryId);
                }

                if (tmpCat == null)
                {
                    StartCoroutine(api.productsByPublisher(publisherId, categoryId, maxLimit, 1));
                }
                else if (limit * page > tmpCat.booksData.Count && tmpCat.booksData.Count < tmpCat.total)
                {
                    tmpCat.accessFrequency++;
                    StartCoroutine(api.productsByPublisher(publisherId, categoryId, maxLimit, tmpCat.page));
                }
                else
                {
                    tmpCat.accessFrequency++;
                    //call back
                }
            }
            else
            {
                StartCoroutine(api.productsByPublisher(publisherId, categoryId, maxLimit, 1));
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

    ////////////////////////////////caching functions//////////////////////////////////////////

    public void cacheCategoryInPublisher(ProductResult res, int publisherId, int categoryId)
    {
        Vendor tmpVendorReference = cachedData.allVendors.Find(v => v.id == publisherId);
        if (res != null && tmpVendorReference != null)
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
                tmpCat.page = 1;
                tmpBookcase.categories.Add(tmpCat);
                tmpCat = tmpBookcase.categories[tmpBookcase.categories.Count - 1];

            }

            tmpCat.page++;
            foreach (Product book in res.prodcutList)
            {
                BookData tmpBook = new BookData();
                tmpBook.id = book.id;
                tmpBook.texture = new Texture2D(1, 1);
                //tmpBook.description = book.shortDescription;
                tmpBook.name = book.name;
                //add picture and url later
                tmpBook.imgString = Convert.ToBase64String(Decompress(Convert.FromBase64String(book.defaultPicture)));
                if (book.defaultPicture != "" && book.defaultPicture != null)
                {
                    Texture2D tmpTexture = new Texture2D(1, 1);
                    tmpTexture.LoadImage(Convert.FromBase64String(tmpBook.imgString));
                    tmpTexture.Apply();

                    cachedTextures.Add(tmpTexture);
                    tmpBook.texture = tmpTexture;
                }
                //if(tmpBook.imgString != "" && tmpBook.imgString != null)tmpBook.texture.LoadImage(Decompress(Convert.FromBase64String(tmpBook.imgString)));
                tmpCat.booksData.Add(tmpBook);
            }


        }
        //TODO
        //call function here which fills physical bookcase with categories and books
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

}
