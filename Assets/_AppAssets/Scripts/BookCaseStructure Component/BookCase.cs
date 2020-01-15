using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCase : MonoBehaviour
{
    public List<BookCaseShelf> ActiveRow;
    public BookCaseShelf upRowDomy, downRowDomy;

    private void Start()
    {
        FetchRows();
    }

    public void FetchRows()
    {
        foreach (BookCaseShelf i in GetComponentsInChildren<BookCaseShelf>())
        {
            if (!i.IsDomy)
            {
                ActiveRow.Add(i.GetComponent<BookCaseShelf>());
            }
            else
            {
                if (i.IsDomyUp)
                {
                    upRowDomy = i;
                }
                else
                {
                    downRowDomy = i;
                }
            }
        }
    }
}
