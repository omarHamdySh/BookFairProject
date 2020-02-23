using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RaqAPI : MonoBehaviour
{

    private string baseUrl = "https://raaqeem.com:1000";
    public ApiAuth authInfo;
   
    // Start is called before the first frame update
   

    public void Init()
    {
        StartCoroutine(GetAuthToken());
    }

    [ContextMenu("foo")]
    void foo()
    {
        Debug.Log(authInfo.access_token);
    }


    [ContextMenu("foo1")]
    void foo1()
    {
        //StartCoroutine(productIdsByPublisher(4, 0, 2, 1));
        StartCoroutine(getAllCategories(0, 0));
    }

    public IEnumerator GetAuthToken()
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
            Cache.Instance.retrieveVendors();
            Cache.Instance.retrieveCategories();
        }
    }
    

    public IEnumerator productsByPublisher(int publisherId, int categoryId, int limit, int page)
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/products_sample_data?" + "vendorId=" + publisherId.ToString();

        if (limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();
        if (categoryId > 0) uri += "&categoryId=" + categoryId.ToString();

        ProductResult res = new ProductResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");


        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<ProductResult>(www.downloadHandler.text);

        if (res != null)
        {
            Cache.Instance.cacheCategoryInPublisher(res, publisherId, categoryId);
        }
    }

    public IEnumerator productIdsByPublisher(int publisherId, int categoryId, int limit, int page)
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/products?" + "vendorId=" + publisherId.ToString();

        if(limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();
        if (categoryId > 0) uri += "&categoryId=" + categoryId.ToString();

        ProducIdstResult res = new ProducIdstResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<ProducIdstResult>(www.downloadHandler.text);

        
        Debug.Log(res.prodcutList.Count);
        foreach(BookId productList in res.prodcutList)
        {
            Debug.Log(productList.id);
        }


    }

    public IEnumerator getAllCategories(int limit, int page)
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/categories/categories_list";

        if (limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();
        
        AllCategoriesResult res = new AllCategoriesResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        yield return www.SendWebRequest();

        if (res != null)
        {
            res = JsonUtility.FromJson<AllCategoriesResult>(www.downloadHandler.text);
        }
        

    }

    public IEnumerator getAllVendors(int limit, int page)
    {
        string uri = baseUrl + "/api/products/PublishersHousesListSampleData";

        if (limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();

        AllVendorsResult res = new AllVendorsResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<AllVendorsResult>(www.downloadHandler.text);

        if (res != null)
        {
            Cache.Instance.cacheAllVendors(res);
        }

    }
    public IEnumerator allSponsors()
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/products/sponsors_list";

        SponsorsResult res = new SponsorsResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        yield return www.SendWebRequest();
        
        if (res != null)
        {
            res = JsonUtility.FromJson<SponsorsResult>(www.downloadHandler.text);
        }


    }
}



