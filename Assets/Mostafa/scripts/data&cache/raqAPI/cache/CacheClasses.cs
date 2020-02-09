using System.Collections.Generic;
using UnityEngine;

public class BookData
{
    public int id;
    public string name;
    public string description;
    public Texture2D texture;
    public string category;
}

public class BookcaseData
{
    public int id;
    public bool active;//if not active dont use it
    public List<BookData> booksData;
}