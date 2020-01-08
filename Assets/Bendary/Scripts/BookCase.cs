using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCase : MonoBehaviour
{
    public List<BookCaseRow> ActiveRow;
    public BookCaseRow upRowDomy, downRowDomy;

    private void Start()
    {
        FetchRows();
    }

    public void FetchRows()
    {
        foreach (BookCaseRow i in GetComponentsInChildren<BookCaseRow>())
        {
            if (!i.IsDomy)
            {
                ActiveRow.Add(i.GetComponent<BookCaseRow>());
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
