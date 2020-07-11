using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using System.Linq;
using DG.Tweening;

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
        currentCameraPos = cameraTransform.localPosition;
        currentCameraRot = cameraTransform.localEulerAngles;

        cameraTransform.localPosition = cameraUIPos;
        cameraTransform.localEulerAngles = cameraUIRot;

        if (Cache.Instance)
        {
            InitSearchFilters();
        }
    }



    #region LoadingBar
    public GameObject endlessLoadingBar;
    #endregion

    #region Colored UI
    [Header("Colored UI")]
    [SerializeField] private Color[] colors;
    [SerializeField] private Sprite[] searchColoredUIPanel, searchColoredLogoContainerUI, miniPanelUI;

    public int indexOfColor;

    #region search
    public void ChangeColorForSearchColoredUI()
    {
        foreach (Image i in searchColoredUI)
        {
            i.color = colors[indexOfColor];
        }
    }

    public void ChangeSpriteForSearchPanelImages()
    {
        searchPanel.sprite = searchColoredUIPanel[indexOfColor];
        searchLogoContainer.sprite = searchColoredLogoContainerUI[indexOfColor];
    }
    #endregion

    #region Fair
    public void ChangeColorForFairColoredUI()
    {
        foreach (Image i in fairColoredUI)
        {
            i.color = colors[indexOfColor];
        }
    }

    public void ChangeSpriteForFairPanelImages()
    {
        fairPanel.sprite = miniPanelUI[indexOfColor];
        fairLogoContainer.sprite = searchColoredLogoContainerUI[indexOfColor];
    }
    #endregion

    #region Sponsors
    public void ChangeColorForSponsorsColoredUI()
    {
        foreach (Image i in sponsorsColoredUI)
        {
            i.color = colors[indexOfColor];
        }
    }

    public void ChangeSpriteForSponsorsPanelImages()
    {
        sponsorsPanel.sprite = miniPanelUI[indexOfColor];
        sponsorsLogoContainer.sprite = searchColoredLogoContainerUI[indexOfColor];
    }
    #endregion

    #region Publishers
    public void ChangeColorForPublishersColoredUI()
    {
        foreach (Image i in publishersColoredUI)
        {
            i.color = colors[indexOfColor];
        }
    }

    public void ChangeSpriteForPublishersPanelImages()
    {
        publishersPanel.sprite = miniPanelUI[indexOfColor];
        publishersLogoContainer.sprite = searchColoredLogoContainerUI[indexOfColor];
    }
    #endregion

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
    [SerializeField] private BookcasePathHandller_Bendary BookcasePathHandller;

    #region Gameplay
    [Header("Gameplay")]
    public Button backToUIModeBtn;
    public GameObject backFromPageModeBtn;
    [SerializeField] private Image[] gameplayColoredUI;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraUIPos;
    [SerializeField] private Vector3 cameraUIRot;
    [SerializeField] private float teleportDelayBetweenUI_game;

    private Vector3 currentCameraPos;
    private Vector3 currentCameraRot;

    public void TeleportToUI()
    {
        ToggleUI(true);

        currentCameraPos = cameraTransform.localPosition;
        currentCameraRot = cameraTransform.localEulerAngles;

        cameraTransform.DOLocalMove(cameraUIPos, teleportDelayBetweenUI_game);
        cameraTransform.DOLocalRotate(cameraUIRot, teleportDelayBetweenUI_game);
    }

    public void TeleportToGamplay()
    {
        cameraTransform.DOLocalMove(currentCameraPos, teleportDelayBetweenUI_game).OnComplete(CloseUI);
        cameraTransform.DOLocalRotate(currentCameraRot, teleportDelayBetweenUI_game);
    }

    private void CloseUI()
    {
        ToggleUI(false);
    }
    #endregion

    #region SearchMenu
    [Header("SearchMenu")]
    [SerializeField] private Button prevSearchPageBtn;
    [SerializeField] private Button nextSearchPageBtn;
    [SerializeField] private FixTextMeshPro seachResultCountTxt;
    [SerializeField] private Transform searchedBookContainer;
    [SerializeField] private FixInputFieldMeshPro searchIN;
    [SerializeField] private TextMeshProUGUI searchPageIndexTxt;
    [SerializeField] private Sprite noBookCover;
    [SerializeField] private TMP_Dropdown fairsDD;
    [SerializeField] private TMP_Dropdown publishersDD;
    [SerializeField] private TMP_Dropdown categoriesDD;
    [SerializeField] private Image[] searchColoredUI;
    [SerializeField] private Image searchPanel, searchLogoContainer;

    private int searchPageIndex = 0;
    private int filterCategoryID = -1;
    private int totalSearchedBooksCount = 0;
    private List<BookData> searchPageResult;
    private int searchFairID, searchPublisherID, searchCategoryID;
    private bool isSearching;

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

        searchFairID = -1;
        searchPublisherID = -1;
        searchCategoryID = -1;
    }

    public void StartSearch(string searchWord)
    {
        if (Cache.Instance && !isSearching)
        {
            if (!string.IsNullOrEmpty(searchWord) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                isSearching = true;

                ToggleSearchForNewSearch(false);
                endlessLoadingBar.SetActive(true);

                #region Filters
                searchFairID = (fairsDD.value != 0) ? Cache.Instance.cachedData.allFairs[fairsDD.value - 1].id : -1;
                searchPublisherID = (publishersDD.value != 0) ? Cache.Instance.cachedData.allVendors[publishersDD.value - 1].id : -1;
                searchCategoryID = (categoriesDD.value != 0) ? Cache.Instance.cachedData.allCategories[categoriesDD.value - 1].id : -1;
                #endregion

                Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchWord, searchCategoryID, searchFairID, searchPublisherID);
            }
        }
    }

    public void StartSearch(TMP_InputField searchIn)
    {
        if (Cache.Instance && !isSearching)
        {
            if (!string.IsNullOrEmpty(searchIn.text))
            {
                isSearching = true;

                ToggleSearchForNewSearch(false);
                endlessLoadingBar.SetActive(true);

                #region Filters
                searchFairID = (fairsDD.value != 0) ? Cache.Instance.cachedData.allFairs[fairsDD.value - 1].id : -1;
                searchPublisherID = (publishersDD.value != 0) ? Cache.Instance.cachedData.allVendors[publishersDD.value - 1].id : -1;
                searchCategoryID = (categoriesDD.value != 0) ? Cache.Instance.cachedData.allCategories[categoriesDD.value - 1].id : -1;
                #endregion

                Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchIn.text, searchCategoryID, searchFairID, searchPublisherID);
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
            searchPageIndexTxt.text = (searchPageIndex + 1) + " / " + (Mathf.CeilToInt((float)totalSearchedBooksCount / (float)searchedBookContainer.childCount));

            ToggleAllSearchCommponent(true);

            PutBooksDataOnUI();
            ManageSeachUIForSearchedResult();
        }
        else
        {
            seachResultCountTxt.text = (PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "لا يوجد نتائج للبحث" : "There is no search result";
            seachResultCountTxt.gameObject.SetActive(true);
        }

        endlessLoadingBar.SetActive(false);
        isSearching = false;

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
            Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchIN.RealText.text, searchCategoryID, searchFairID, searchPublisherID);
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
    [SerializeField] private Image[] fairColoredUI;
    [SerializeField] private Image fairPanel, fairLogoContainer;
    [SerializeField] private Button goBtn;

    private int currentFairIndex;

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
                    //go.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = colors[indexOfColor];

                    if (Cache.Instance.cachedData.allFairs[i].id == Cache.Instance.getFairId())
                    {
                        go.GetComponentInChildren<Toggle>().isOn = true;
                        currentFairIndex = i;
                    }
                    go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(CheckFaireOptionChanged);
                }
            }
        }
    }

    public void CheckFaireOptionChanged(bool enabled)
    {
        if (fairsScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First())
        {
            Toggle toggle = fairsScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First();
            if (toggle)
            {
                if (toggle.transform.parent.GetSiblingIndex() != currentFairIndex)
                {
                    goBtn.interactable = true;
                }
                else
                {
                    goBtn.interactable = false;
                }
            }
        }
    }

    public void GoToSelectedFair()
    {
        Toggle toggle = fairsScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First();
        Cache.Instance.setFairId(Cache.Instance.cachedData.allFairs[toggle.transform.parent.GetSiblingIndex()].id);
        PlayerPrefs.SetInt(ImportantStrings.fairIDKey, Cache.Instance.getFairId());
        Destroy(Cache.Instance.gameObject);
        endlessLoadingBar.SetActive(true);
        endlessLoadingBar.GetComponent<Image>().color = Color.black;
        LoadLevel(ImportantStrings.splashScene, true);
    }

    public void CancelNewSelectedFair()
    {
        if (fairsScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First())
        {
            Toggle toggle = fairsScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First();
            if (toggle)
            {
                if (toggle.transform.parent.GetSiblingIndex() != currentFairIndex)
                {
                    fairsScroll.content.GetChild(currentFairIndex).GetComponentInChildren<Toggle>().isOn = true;
                }
            }
        }
    }
    #endregion

    #region SponsorsMenu
    [Header("SponsorsMenu")]
    [SerializeField] private ScrollRect SponsorsScroll;
    [SerializeField] private Image[] sponsorsColoredUI;
    [SerializeField] private Image sponsorsPanel, sponsorsLogoContainer;

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
    [SerializeField] private Image[] publishersColoredUI;
    [SerializeField] private Image publishersPanel, publishersLogoContainer;

    private int currentPublisherIndex;

    public void PutPublishersData()
    {
        if (Cache.Instance)
        {
            if (publishersScroll.content.childCount < Cache.Instance.cachedData.allVendors.Count)
            {
                for (int i = publishersScroll.content.childCount; i < Cache.Instance.cachedData.allVendors.Count; i++)
                {
                    GameObject go = Instantiate(toggleScrollItem, publishersScroll.content);
                    go.GetComponentInChildren<FixTextMeshPro>().text = Cache.Instance.cachedData.allVendors[i].name;
                    go.GetComponentInChildren<Toggle>().group = publishersScroll.content.GetComponent<ToggleGroup>();
                    go.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = colors[indexOfColor];

                    if (i == BookcasePathHandller.vendorIndex)
                    {
                        go.GetComponentInChildren<Toggle>().isOn = true;
                        currentPublisherIndex = i;
                    }
                }
            }
            UpdateCurrentPublisherOnUI();
        }
    }

    private void UpdateCurrentPublisherOnUI()
    {
        publishersScroll.content.GetChild(BookcasePathHandller.vendorIndex).GetComponentInChildren<Toggle>().isOn = true;
        currentPublisherIndex = BookcasePathHandller.vendorIndex;
    }

    public void CheckPublisherChanged()
    {
        if (publishersScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First())
        {
            Toggle toggle = publishersScroll.content.GetComponent<ToggleGroup>().ActiveToggles().First();
            if (toggle)
            {
                if (toggle.transform.parent.GetSiblingIndex() != currentPublisherIndex)
                {
                    endlessLoadingBar.SetActive(true);
                    int stepsCount = 0;
                    NearstDir nearstDir = CheckNearstDir(toggle.transform.parent.GetSiblingIndex(), ref stepsCount);

                    BookcasePathHandller.MoveToIndex(nearstDir, stepsCount);
                }
            }
        }
    }

    private NearstDir CheckNearstDir(int newIndex, ref int stepsCount)
    {
        int right = 0, left = 0, currentIndex = currentPublisherIndex;
        while (currentIndex != newIndex)
        {
            currentIndex = (currentIndex == 0) ? Cache.Instance.cachedData.allVendors.Count - 1 : currentIndex - 1;
            right++;
        }

        currentIndex = currentPublisherIndex;
        while (currentIndex != newIndex)
        {
            currentIndex = (currentIndex + 1) % Cache.Instance.cachedData.allVendors.Count;
            left++;
        }

        if (right < left)
        {
            stepsCount = right;
            return NearstDir.right;
        }
        else if (right > left)
        {
            stepsCount = left;
            return NearstDir.left;
        }
        else
        {
            stepsCount = right;
            return NearstDir.even;
        }
    }

    #endregion
    #endregion
}

public enum NearstDir
{
    right = 0,
    left = 1,
    even,
}
