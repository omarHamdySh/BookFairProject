using System.Collections.Generic;

[System.Serializable]
public class ProductResult
{
    public List<Product> prodcutList;
    public int totalRecord;

}

//main book class
[System.Serializable]
public class Product
{
    public string name;
    public string shortDescription;
    public string defaultPicture;
    public int id;
}


[System.Serializable]
public class ProducIdstResult
{
    public List<BookId> prodcutList;
    public int totalRecord;

}

//book ID
//book ID class
[System.Serializable]
public class BookId
{
    public int id;
}


[System.Serializable]
public class AllCategoriesResult
{
    public List<ProductCategory> categories;
    public int totalRecord;
}

[System.Serializable]
public class ProductCategory
{
    public string name;
    public int id;
}

[System.Serializable]
public class SponsorsResult
{
    public List<Sponsor> sponsorList;
    public int totalRecord;
}

[System.Serializable]
public class Sponsor
{
    public string pictureThumbnailUrl;
    public string name;
    public object email;
    public object description;
    public string phone;
    public int id;
    public object form;
}

[System.Serializable]
public class Vendor
{
    public string name;
    public string description;
    public string defualtPicture;
}

public class AllVendorsResult
{
    public List<Vendor> vendorList;
    public int totalRecord;
}