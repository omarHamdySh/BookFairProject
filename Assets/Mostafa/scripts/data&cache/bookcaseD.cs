using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookcaseD : MonoBehaviour
{
    BookcaseData bookcaseData;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    [ContextMenu("foo1")]
    void foo1()
    {
        bookcaseData = Cache.Instance.getBookCase(4);    
    }

    [ContextMenu("foo2")]
    void foo2()
    {
        foreach (BookData bd in bookcaseData.booksData)
        {
            print(bd.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
