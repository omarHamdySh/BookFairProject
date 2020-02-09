using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Text;
using System;
public class RaqAPI : MonoBehaviour
{

    private string baseUrl = "https://raaqeem.com:1000";
    public ApiAuth authInfo;
   
    // Start is called before the first frame update
    void Start()
    {
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
    void productDetails(int id)
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

    IEnumerator productDetails2(int id)
    {
        string uri = baseUrl + "/api/products/product_details" + "?productId=" + id.ToString() + "&bookFairId=";
        string resString;
        Result res = new Result();

        UnityWebRequest www = UnityWebRequest.Get(uri);
        
        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
    }

    public IEnumerator productsByPublisher(int publisherId)
    {
        string uri = baseUrl + "/api/products_sample_data?" + "vendorId=" + publisherId.ToString() + "&bookFairId=";
        ProductResult res = new ProductResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<ProductResult>(www.downloadHandler.text);
        
        Cache.Instance.cacheResult(res, publisherId);
        
        /*
        Debug.Log(res.totalRecord);
        foreach(ProdcutList productList in res.prodcutList)
        {
            Debug.Log(productList.name);
        }*/

        
    }


}



