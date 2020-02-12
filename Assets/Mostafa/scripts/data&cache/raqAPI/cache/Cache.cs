using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public Dictionary<int, BookcaseData> bookcasesData;
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
        
        bookcasesData = new Dictionary<int, BookcaseData>();
        
    }

    [ContextMenu("foo")]
    void foo()
    {
        getBookCase(4);
    }
    [ContextMenu("foo1")]
    void foo1()
    {
        getBookCase(1);
    }


    [ContextMenu("listdic")]
    void listdic()
    {
        foreach(BookData bc in bookcasesData[4].booksData)
        {
            Debug.Log(bc.name);
        }
    }
    //purpose: gets book case by publisher id
    public BookcaseData getBookCase(int id)
    {
        if (!bookcasesData.ContainsKey(id))
        {
            print("asd");
            bookcasesData[id] = new BookcaseData();
            StartCoroutine(api.productsByPublisher(id));
        }

        return bookcasesData[id];
    }
    
    public void cacheResult(ProductResult res, int publisherId)
    {
        if (res != null)
        {
            
            bookcasesData[publisherId].id = publisherId;
            bookcasesData[publisherId].booksData = new List<BookData>();
            bookcasesData[publisherId].active = true;

            foreach (ProdcutList productList in res.prodcutList)
            {
                BookData bookData = new BookData();
                bookData.name = productList.name;
                bookData.id = productList.id;
                bookData.description = productList.shortDescription;
                bookData.texture = new Texture2D(1, 1);
                bookData.texture.LoadImage(Convert.FromBase64String(productList.defaultPicture));

                bookcasesData[publisherId].booksData.Add(bookData);
                

            }
        }
    }
    
}
