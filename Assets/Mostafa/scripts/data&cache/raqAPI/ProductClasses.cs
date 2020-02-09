using System.Collections.Generic;

[System.Serializable]
public class ProductResult
{
    public List<ProdcutList> prodcutList;
    public int totalRecord;

}

[System.Serializable]
public class ProdcutList
{
    public string name;
    public string shortDescription;
    public string defaultPicture;
    public int id;
}