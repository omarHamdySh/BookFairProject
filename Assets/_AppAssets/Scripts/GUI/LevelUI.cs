using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using System.Linq;
using ArabicSupport;

public class LevelUI : UIHandller
{
    #region Singleton
    public static LevelUI Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        isUIOpen = true;
    }
    #endregion

    private void Start()
    {
        if (Cache.Instance)
        {
            InitSearchFilters();
        }
    }

    #region LoadingBar
    public GameObject endlessLoadingBar;
    #endregion

    #region SwitchFromUI to Game Mode
    [HideInInspector] public bool isUIOpen;

    public void ToggleUI(bool check)
    {
        isUIOpen = check;
    }
    #endregion

    #region Menus
    [Header("Menus")]
    [SerializeField] private GameObject regularScrollItem;
    [SerializeField] private GameObject toggleScrollItem;

    private int currentFairIndex;

    #region Gameplay
    [Header("Gameplay")]
    public GameObject backFromPageModeBtn;
    #endregion

    #region SearchMenu
    [Header("SearchMenu")]
    [SerializeField] private Button prevSearchPageBtn;
    [SerializeField] private Button nextSearchPageBtn;
    [SerializeField] private FixTextMeshPro seachResultCountTxt;
    [SerializeField] private Transform searchedBookContainer;
    [SerializeField] private FixInputFieldMeshPro searchIN;
    [SerializeField] private FixTextMeshPro searchPageIndexTxt;
    [SerializeField] private Sprite noBookCover;
    [SerializeField] private TMP_Dropdown fairsDD;
    [SerializeField] private TMP_Dropdown publishersDD;
    [SerializeField] private TMP_Dropdown categoriesDD;

    private int searchPageIndex = 0;
    private int filterCategoryID = -1;
    private int totalSearchedBooksCount = 0;
    private List<BookData> searchPageResult;
    private int fairID, publisherID, categoryID;

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    private void InitSearchFilters()
    {
        List<TMP_Dropdown.OptionData> options;
        TMP_Dropdown.OptionData option;

        #region Fairs
        fairsDD.ClearOptions();
        options = new List<TMP_Dropdown.OptionData>();
        option = new TMP_Dropdown.OptionData();

        option.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ?
            ImportantMesthods.FixRTLForArabic("المعرض") : ImportantMesthods.FixRTLForArabic("Fair");
        options.Add(option);
        foreach (FairData fair in Cache.Instance.cachedData.allFairs)
        {
            option = new TMP_Dropdown.OptionData();
            option.text = ImportantMesthods.FixRTLForArabic(fair.fullName);
            options.Add(option);
        }

        fairsDD.AddOptions(options);
        #endregion

        options.Clear();

        #region Publisher
        publishersDD.ClearOptions();
        options = new List<TMP_Dropdown.OptionData>();
        option = new TMP_Dropdown.OptionData();

        option.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ?
            ImportantMesthods.FixRTLForArabic("دار النشر") : ImportantMesthods.FixRTLForArabic("Publisher");
        options.Add(option);
        foreach (Vendor vendor in Cache.Instance.cachedData.allVendors)
        {
            option = new TMP_Dropdown.OptionData();
            option.text = ImportantMesthods.FixRTLForArabic(vendor.name);
            options.Add(option);
        }

        publishersDD.AddOptions(options);
        #endregion

        options.Clear();

        #region Categories
        categoriesDD.ClearOptions();
        options = new List<TMP_Dropdown.OptionData>();
        option = new TMP_Dropdown.OptionData();

        option.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ?
           ImportantMesthods.FixRTLForArabic("القسم") : ImportantMesthods.FixRTLForArabic("Category");
        options.Add(option);
        foreach (ProductCategory productCategory in Cache.Instance.cachedData.allCategories)
        {
            option = new TMP_Dropdown.OptionData();
            option.text = ImportantMesthods.FixRTLForArabic(productCategory.name);
            options.Add(option);
        }

        categoriesDD.AddOptions(options);
        #endregion
    }

    public void ToggleAllSearchCommponent(bool enabled)
    {
        searchIN.RealText.text = (enabled) ? searchIN.RealText.text : "";
        searchPageIndex = (enabled) ? searchPageIndex : 0;
        ToggleSearchForNewSearch(enabled);
    }

    private void ToggleSearchForNewSearch(bool enabled)
    {
        searchPageIndex = (enabled) ? searchPageIndex : 0;
        filterCategoryID = (enabled) ? filterCategoryID : -1;
        searchPageIndexTxt.gameObject.SetActive(enabled);
        prevSearchPageBtn.gameObject.SetActive(enabled);
        nextSearchPageBtn.gameObject.SetActive(enabled);
        seachResultCountTxt.gameObject.SetActive(enabled);
        searchedBookContainer.gameObject.SetActive(enabled);
    }

    public void ResetFilters()
    {
        fairsDD.value = 0;
        publishersDD.value = 0;
        categoriesDD.value = 0;

        fairID = -1;
        publisherID = -1;
        categoryID = -1;
    }

    public void StartSearch(string searchWord)
    {
        if (Cache.Instance)
        {
            if (!string.IsNullOrEmpty(searchWord) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                ToggleSearchForNewSearch(false);
                endlessLoadingBar.SetActive(true);

                #region Filters
                fairID = (fairsDD.value != 0) ? Cache.Instance.cachedData.allFairs[fairsDD.value - 1].id : -1;
                publisherID = (publishersDD.value != 0) ? Cache.Instance.cachedData.allVendors[publishersDD.value - 1].id : -1;
                categoryID = (categoriesDD.value != 0) ? Cache.Instance.cachedData.allCategories[categoriesDD.value - 1].id : -1;
                #endregion

                Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchWord, categoryID, fairID, publisherID);
            }
        }
    }

    public void StartSearch(TMP_InputField searchIn)
    {
        if (Cache.Instance)
        {
            if (!string.IsNullOrEmpty(searchIn.text))
            {
                ToggleSearchForNewSearch(false);
                endlessLoadingBar.SetActive(true);

                #region Filters
                fairID = (fairsDD.value != 0) ? Cache.Instance.cachedData.allFairs[fairsDD.value - 1].id : -1;
                publisherID = (publishersDD.value != 0) ? Cache.Instance.cachedData.allVendors[publishersDD.value - 1].id : -1;
                categoryID = (categoriesDD.value != 0) ? Cache.Instance.cachedData.allCategories[categoriesDD.value - 1].id : -1;
                #endregion

                Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchIn.text, categoryID, fairID, publisherID);
            }
        }
    }

    public void SearchResultCallback(List<BookData> searchPageResult, int totalSearchedBooksCount)
    {
        if (totalSearchedBooksCount > 0)
        {
            this.searchPageResult = searchPageResult;
            this.totalSearchedBooksCount = totalSearchedBooksCount;

            seachResultCountTxt.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "نتيجة البحث " + totalSearchedBooksCount + ((totalSearchedBooksCount > 2) ? " كتب" : (totalSearchedBooksCount == 1) ? " كتاب" : " كتابان") :
                "Search Result " + totalSearchedBooksCount + ((totalSearchedBooksCount > 1) ? " books" : " book");
            searchPageIndexTxt.text = ((PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "صفحة " : "Page ") + (searchPageIndex + 1) + " / " + (Mathf.CeilToInt((float)totalSearchedBooksCount / (float)searchedBookContainer.childCount));

            endlessLoadingBar.SetActive(false);
            ToggleAllSearchCommponent(true);

            PutBooksDataOnUI();
            ManageSeachUIForSearchedResult();
        }
        else
        {
            endlessLoadingBar.SetActive(false);
            seachResultCountTxt.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "لا يوجد نتائج للبحث" : "There is no search result";
            seachResultCountTxt.gameObject.SetActive(true);
        }

    }

    private void CloseAllBooks()
    {
        foreach (Transform i in searchedBookContainer)
        {
            i.gameObject.SetActive(false);
        }
    }

    private void PutBooksDataOnUI()
    {
        // Close All Books
        CloseAllBooks();

        for (int i = 0; i < searchPageResult.Count; i++)
        {
            // Get the book
            Transform book = searchedBookContainer.GetChild(i);

            // Enable Book
            book.gameObject.SetActive(true);

            // Put Data on the book
            book.GetComponentInChildren<FixTextMeshPro>().text = searchPageResult[i].name;
            if (searchPageResult[i].texture)
            {
                book.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Sprite.Create(searchPageResult[i].texture, new Rect(0, 0, searchPageResult[i].texture.width, searchPageResult[i].texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                book.GetChild(0).GetChild(0).GetComponent<Image>().sprite = noBookCover;
            }
            book.GetComponent<Button>().onClick.RemoveAllListeners();
            string url = searchPageResult[i].url;
            book.GetComponent<PressHandler>().OnPress.AddListener(() => OpenURLInNewTab(url));
        }
    }

    public void NextOrPrevPage(bool isNext)
    {
        if (isNext)
        {
            searchPageIndex = (searchPageIndex + 1) % (Mathf.CeilToInt((float)totalSearchedBooksCount / (float)searchedBookContainer.childCount));
        }
        else
        {
            searchPageIndex = (searchPageIndex == 0) ? Mathf.CeilToInt((float)totalSearchedBooksCount / (float)searchedBookContainer.childCount) - 1 : searchPageIndex - 1;
        }
        endlessLoadingBar.SetActive(true);

        if (Cache.Instance)
        {
            Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchIN.RealText.text, categoryID, fairID, publisherID);
        }
    }

    private void ManageSeachUIForSearchedResult()
    {
        if (totalSearchedBooksCount == 0)
        {
            prevSearchPageBtn.interactable = false;
            nextSearchPageBtn.interactable = false;
        }
        else if (totalSearchedBooksCount > 0)
        {
            if (totalSearchedBooksCount <= searchedBookContainer.childCount)
            {
                prevSearchPageBtn.interactable = false;
                nextSearchPageBtn.interactable = false;
            }
            else
            {
                if (searchPageIndex > 0 && searchPageIndex < Mathf.CeilToInt((float)totalSearchedBooksCount / (float)searchedBookContainer.childCount) - 1)
                {
                    prevSearchPageBtn.interactable = true;
                    nextSearchPageBtn.interactable = true;
                }
                else if (searchPageIndex == 0)
                {
                    prevSearchPageBtn.interactable = false;
                    nextSearchPageBtn.interactable = true;
                }
                else if (searchPageIndex == Mathf.CeilToInt((float)totalSearchedBooksCount / (float)searchedBookContainer.childCount) - 1)
                {
                    prevSearchPageBtn.interactable = true;
                    nextSearchPageBtn.interactable = false;
                }
            }
        }
    }



    public void OpenURLInNewTab(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
#if !UNITY_EDITOR
    openWindow(url);
#else
            Application.OpenURL(url);
#endif
        }
        else
        {
            Debug.Log("Empty URL");
        }
    }

    public void OpenURLInTheSameTab(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.Log("Empty URL");
        }
    }
    #endregion

    #region FairMenu
    [Header("FairsMenu")]
    [SerializeField] private ScrollRect fairsScroll;

    public void PutFairsData()
    {
        if (Cache.Instance)
        {
            if (fairsScroll.content.childCount < Cache.Instance.cachedData.allFairs.Count)
            {
                for (int i = fairsScroll.content.childCount; i < Cache.Instance.cachedData.allFairs.Count; i++)
                {
                    GameObject go = Instantiate(toggleScrollItem, fairsScroll.content);
                    go.GetComponentInChildren<FixTextMeshPro>().text = Cache.Instance.cachedData.allFairs[i].fullName;
                    go.GetComponentInChildren<Toggle>().group = fairsScroll.content.GetComponent<ToggleGroup>();

                    if (Cache.Instance.cachedData.allFairs[i].id == Cache.Instance.getFairId())
                    {
                        go.GetComponentInChildren<Toggle>().isOn = true;
                        currentFairIndex = i;
                    }
                }
            }
        }
    }

    public void CheckFairChanged()
    {
        Toggle toggle = fairsScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First();
        if (toggle)
        {
            if (toggle.transform.parent.GetSiblingIndex() != currentFairIndex)
            {
                Cache.Instance.setFairId(Cache.Instance.cachedData.allFairs[toggle.transform.parent.GetSiblingIndex()].id);
                PlayerPrefs.SetInt(ImportantStrings.fairIDKey, Cache.Instance.getFairId());
                Destroy(Cache.Instance.gameObject);
                endlessLoadingBar.SetActive(true);
                endlessLoadingBar.GetComponent<Image>().color = Color.black;
                LoadLevel(ImportantStrings.splashScene, true);
            }
        }
    }
    #endregion

    #region SponsorsMenu
    [Header("SponsorsMenu")]
    [SerializeField] private ScrollRect SponsorsScroll;

    public void PutSponsorsData()
    {
        if (Cache.Instance)
        {
            if (SponsorsScroll.content.childCount < Cache.Instance.cachedData.allSponsors.Count)
            {
                for (int i = SponsorsScroll.content.childCount; i < Cache.Instance.cachedData.allSponsors.Count; i++)
                {
                    GameObject go = Instantiate(regularScrollItem, SponsorsScroll.content);
                    go.GetComponentInChildren<FixTextMeshPro>().text = Cache.Instance.cachedData.allSponsors[i].name;
                }
            }
        }
    }
    #endregion

    #region PublishersMenu
    [Header("PublishersMenu")]
    [SerializeField] private ScrollRect publishersScroll;

    public void PutPublishersData()
    {
        if (Cache.Instance)
        {
            if (publishersScroll.content.childCount < Cache.Instance.cachedData.allVendors.Count)
            {
                for (int i = publishersScroll.content.childCount; i < Cache.Instance.cachedData.allVendors.Count; i++)
                {
                    GameObject go = Instantiate(regularScrollItem, publishersScroll.content);
                    go.GetComponentInChildren<FixTextMeshPro>().text = Cache.Instance.cachedData.allVendors[i].name;
                }
            }
        }
    }
    #endregion
    #endregion
}
