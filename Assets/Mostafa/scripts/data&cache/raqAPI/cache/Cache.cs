using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public List<ProductCategory> allCategories;
    public List<BookcaseData> bookcasesData;
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
        bookcasesData = new List<BookcaseData>();
    }

    [ContextMenu("foo")]
    void foo()
    {
        retrieveCategories();
    }
    [ContextMenu("foo1")]
    void foo1()
    {
        retrieveCategoryInBookcase(4, 22, 1, 1);
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
    public void cacheCategoryInPublisher(ProductResult res, int publisherId, int categoryId)
    {

        if (res != null)
        {
            BookcaseData tmpBookcase;
            tmpBookcase = bookcasesData.Find(bd => bd.id == publisherId);
            if(tmpBookcase == null)
            {
                tmpBookcase = new BookcaseData();
                tmpBookcase.id = publisherId;
                tmpBookcase.categories = new List<CategoryData>();

                CategoryData tmpCat = new CategoryData();
                tmpCat.booksData = new List<BookData>();
                tmpCat.id = categoryId;
                tmpCat.total = res.totalRecord;
                
                foreach(Product book in res.prodcutList)
                {
                    BookData tmpBook = new BookData();
                    tmpBook.id = book.id;
                    tmpBook.description = book.shortDescription;
                    tmpBook.name = book.name;
                    //add picture and url later
                    tmpCat.booksData.Add(tmpBook);
                    tmpCat.loadedBooks++;
                }
                tmpBookcase.categories.Add(tmpCat);
                bookcasesData.Add(tmpBookcase);
            }
            else
            {
                CategoryData tmpCat = tmpBookcase.categories.Find(cat => cat.id == categoryId);
                tmpCat.booksData = new List<BookData>();
                if (tmpCat == null) {
                    tmpCat = new CategoryData();
                    tmpCat.id = categoryId;
                    tmpCat.total = res.totalRecord;
                }
                foreach (Product book in res.prodcutList)
                {
                    BookData tmpBook = new BookData();
                    tmpBook.id = book.id;
                    tmpBook.description = book.shortDescription;
                    tmpBook.name = book.name;
                    //add picture and url later
                    tmpCat.booksData.Add(tmpBook);
                    tmpCat.loadedBooks++;
                }
            }
            


        }
    }

    public void cacheAllCategories(AllCategoriesResult categoriesResult)
    {
        allCategories = categoriesResult.categories;
    }


}
