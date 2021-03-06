﻿using System.Collections.Generic;
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
    public string bookFairSlug;
    public string briefDescription;
    public string logoPictureBinary;
    public Texture2D pic;
    public int mainColorNumber;
    public int booksCount;
}

[System.Serializable]
public class Vendor
{
    public int id;
    public string vendorSlug;
    public string name;
    public string description;
    public string defualtPicture;
    public int vendorColorNumber;

    public int accessFrequency;
    public BookcaseData bookcaseData;
    public Texture2D pic;
}

[System.Serializable]
public class BookfairVendorsBook
{
    public string vendorName;
    public int booksCount;
}

[System.Serializable]
public class FairStats
{
    public int booksCount;
    public int vendorsCount;
    public int sponsorsCount;
    public List<BookfairVendorsBook> bookfairVendorsBooks;
}


[System.Serializable]
public class FairVideo
{
    public string videoName;
    public string description;
    public string fileBinary;
    public bool useDownloadUrl;
    public string downloadUrl;
    public string contentType;
    public string filename;
    public string extension;
}

[System.Serializable]
public class FairVideoResult
{
    public FairVideo result;
}