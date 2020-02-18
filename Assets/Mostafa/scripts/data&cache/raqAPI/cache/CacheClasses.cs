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


public class CategoryData
{
    public int id;

    public int total;//maximum number of books to be loaded
    
    public int loadedPages;
    public int loadedBooks;
    
    public List<BookData> booksData;
}
public class BookcaseData
{
    public int id;
    public List<CategoryData> categories;
}


