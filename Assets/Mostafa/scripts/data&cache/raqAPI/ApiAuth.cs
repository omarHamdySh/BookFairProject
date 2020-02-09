[System.Serializable]
public struct ApiAuth
{
    public string access_token;
    public string token_type;
    public int expires_in;
    public string refresh_token;
}

[System.Serializable]

public class Products
{
    public string name;
    public PictureModel defaultPictureModel;
}

[System.Serializable]
public class Result
{
    public Products products;
}

[System.Serializable]
public class PictureModel
{
    public string imageUrl;
}