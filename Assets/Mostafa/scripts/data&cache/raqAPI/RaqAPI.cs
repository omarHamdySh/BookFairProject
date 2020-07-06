using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class RaqAPI : MonoBehaviour
{

    private string baseUrl = "https://raaqeem.com:1000";
    public ApiAuth authInfo;
    public int languageId = 1;
    public int fairId = -1;
    public bool transmitting; //true if data is being requested

    public UnityEvent authTokenLoadedEvent;
    public UnityEvent vendorsRetrievedEvent;
    public UnityEvent fairsRetrievedEvent;
    public UnityEvent dataArrivedEvent;

    [DllImport("__Internal")]
    private static extern string getCookie(string cName);

    private void Awake()
    {
#if !UNITY_EDITOR
        string lang = getCookie("lang");
        PlayerPrefs.SetString(ImportantStrings.langPPKey, lang);

        switch (lang)
        {
            case "ar":
                languageId = 2;
                break;
            case "en":
                languageId = 1;
                break;
        }
#endif
    }

    public void Init()
    {
        StartCoroutine(GetAuthToken());
    }

    void Start()
    {
        if (authTokenLoadedEvent == null) authTokenLoadedEvent = new UnityEvent();
        if (vendorsRetrievedEvent == null) vendorsRetrievedEvent = new UnityEvent();
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

        transmitting = true;
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

            authTokenLoadedEvent.Invoke();
        }
        transmitting = false;
    }


    public IEnumerator productsByPublisher(int publisherId, int categoryId, int limit, int page)
    {
        string uri = baseUrl + "/api/products_sample_data?" + "vendorId=" + publisherId.ToString();
        if (limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();
        if (categoryId > 0) uri += "&categoryId=" + categoryId.ToString();
        if (fairId >= 0) uri += "&fairId=" + fairId.ToString();
        ProductResult res = new ProductResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", languageId.ToString());
        transmitting = true;

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<ProductResult>(www.downloadHandler.text);

        if (res != null)
        {
            Cache.Instance.cacheCategoryInPublisher(res, publisherId, categoryId);
            dataArrivedEvent.Invoke();
        }
        transmitting = false;

    }

    public IEnumerator searchWithFilter(string keyword, int categoryId, int fairId, int vendorId, int limit, int page)
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/products_sample_data?" + "&keyword=" + keyword;

        if (limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();
        if (categoryId >= 0) uri += "&categoryId=" + categoryId.ToString();
        if (fairId >= 0) uri += "&fairId=" + fairId.ToString();
        if (vendorId >= 0) uri += "&vendorId=" + vendorId.ToString();
        ProductResult res = new ProductResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", languageId.ToString());

        transmitting = true;

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<ProductResult>(www.downloadHandler.text);

        if (res != null)
        {
            Cache.Instance.cacheSearchResult(res);
        }

        //transmitting = false;
    }

    public IEnumerator productIdsByPublisher(int publisherId, int categoryId, int limit, int page)
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/products?" + "vendorId=" + publisherId.ToString();

        if (limit > 0) uri += "&limit=" + limit.ToString() + "&page=" + page.ToString();
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
        foreach (BookId productList in res.prodcutList)
        {
            Debug.Log(productList.id);
        }


    }

    public void abortRetrieve()
    {
        transmitting = false;
        StopAllCoroutines();
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
        www.SetRequestHeader("LanguageId", languageId.ToString());
        transmitting = true;

        yield return www.SendWebRequest();
        res = JsonUtility.FromJson<AllCategoriesResult>(www.downloadHandler.text);

        if (res != null)
        {
            Cache.Instance.cacheAllCategories(res);
        }

        transmitting = false;
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
        www.SetRequestHeader("LanguageId", languageId.ToString());

        transmitting = true;

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<AllVendorsResult>(www.downloadHandler.text);

        if (res != null)
        {
            Cache.Instance.cacheAllVendors(res);
            vendorsRetrievedEvent.Invoke();//load peliminary data
            vendorsRetrievedEvent.RemoveAllListeners();
        }

        transmitting = false;

    }
    public IEnumerator getAllSponsors()
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/products/sponsors_list";

        SponsorsResult res = new SponsorsResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        transmitting = true;

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<SponsorsResult>(www.downloadHandler.text);
        if (res != null)
        {
            Cache.Instance.cacheAllSponsors(res);
        }

        transmitting = false;
    }

    public IEnumerator getCurrentFairs()
    {
        //temporary until badawy gives us another endpoint
        string uri = baseUrl + "/api/fairs/FairsList";

        FairResult res = new FairResult();

        UnityWebRequest www = UnityWebRequest.Get(uri);

        www.SetRequestHeader("Authorization", authInfo.token_type + " " + authInfo.access_token);
        www.SetRequestHeader("customerId", "1");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("LanguageId", "1");

        transmitting = true;

        yield return www.SendWebRequest();

        res = JsonUtility.FromJson<FairResult>(www.downloadHandler.text);
        if (res != null)
        {
            Cache.Instance.cacheAllFairs(res);
            fairsRetrievedEvent.Invoke();
        }

        transmitting = false;
    }
    public string makeBookUrl(string slug)
    {
        return "https://raaqeem.com/" + (languageId == 1 ? "ar" : "en") + "/" + "book" + "/" + slug;
    }
}

