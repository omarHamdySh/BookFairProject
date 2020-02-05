using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;


public class TestGetDataFromServer_Bendary : MonoBehaviour
{
    private string baseUrl = "https://raaqeem.com:1000";
    [SerializeField] private ApiAuth authInfo;

    // Start is called before the first frame update
    void Start()
    {
        //auth();
        //productDetails("55");
        StartCoroutine(GetAuthToken());
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("foo")]
    void foo()
    {
        Debug.Log(authInfo.access_token);
    }

    [ContextMenu("auth")]
    void auth()
    {

        string uri = baseUrl + "/api/authorize/get_token";
        string res;
        WebClient client = new WebClient();
        string reqparm = "{\"clientId\":\"402f4c7d-1453-4f4c-9041-684cbb5dad8c\",\"clientSecret\":\"200fae09-d003-46f9-a305-13469fabc7e6\",\"serverUrl\":\"https://raaqeem.com:1000\"}"; client.Headers.Add("Content-Type", "application / json");
        res = client.UploadString(uri, "POST", reqparm);
        authInfo = JsonUtility.FromJson<ApiAuth>(res);


    }

    [System.Serializable]
    public class ss
    {
        public string clientId;
        public string clientSecret;
        public string serverUrl;
    }

    IEnumerator GetAuthToken()
    {
        // Prebare data for request
        string jsonAttributes = "{\"clientId\":\"402f4c7d-1453-4f4c-9041-684cbb5dad8c\",\"clientSecret\":\"200fae09-d003-46f9-a305-13469fabc7e6\",\"serverUrl\":\"https://raaqeem.com:1000\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonAttributes);

        UnityWebRequest www = new UnityWebRequest(baseUrl + "/api/authorize/get_token", "Post");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Send Request
        yield return www.SendWebRequest();

        // Validate respond
        if (www.error != null)
        {
            Debug.LogError("Error : " + www.error);
        }
        else
        {
            authInfo = JsonUtility.FromJson<ApiAuth>(www.downloadHandler.text);
        }
    }

    void productDetails(string id)
    {
        string uri = baseUrl + "/api/products/product_details";
        string resString;
        Result res = new Result();
        WebClient client = new WebClient();
        client.QueryString.Add("productId", id.ToString());
        client.QueryString.Add("bookFairId", "");

        client.Headers.Add("Authorization", authInfo.token_type + " " + authInfo.access_token);
        client.Headers.Add("customerId", "1");
        client.Headers.Add("Content-Type", "application/json");
        client.Headers.Add("LanguageId", "1");

        resString = client.DownloadString(uri);
        res = JsonUtility.FromJson<Result>(resString);

        Debug.Log(res.products.defaultPictureModel.imageUrl);
    }

    void pdd()
    {
        string uri = baseUrl + "/api/products/product_details?productId=55&bookFairId=";
        string res;
        WebClient client = new WebClient();

        client.Headers.Add("Authorization", "Bearer " + authInfo.access_token);
        client.Headers.Add("customerId", "1");
        client.Headers.Add("Content-Type", "application/json");
        client.Headers.Add("LanguageId", "1");

        res = client.UploadString(uri, "GET");

        Debug.Log(res);


    }


}

[System.Serializable]
public class ApiAuth
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