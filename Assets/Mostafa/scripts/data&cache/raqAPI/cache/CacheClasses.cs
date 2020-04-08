using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BookData
{
    public int id;

    public string name;
    public string description;
    public Texture2D texture;
    public string url;
    public string imgString;
}

[System.Serializable]
public class CategoryData
{
    public int id;

    public string name;

    public int total;//maximum number of books to be loaded

    public int page;

    public int accessFrequency;
    
    public List<BookData> booksData;
}

[System.Serializable]
public class BookcaseData
{
    public int id;
    public string name;
    public List<CategoryData> categories;
}


[System.Serializable]
public class FairData
{
    public int id;
    public string shortName;
    public string fullName;
    public string briefDescription;
    public string logoPictureBinary;
    public Texture2D pic;
    public int booksCount;
}

