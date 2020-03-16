using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

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

    #region Search
    [Header("Search")]
    [SerializeField] private Button prevSearchPageBtn;
    [SerializeField] private Button nextSearchPageBtn;
    [SerializeField] private FixTextMeshPro seachResultCountTxt;
    [SerializeField] private Transform searchedBookContainer;
    [SerializeField] private FixInputFieldMeshPro searchIN;
    [SerializeField] private FixTextMeshPro searchPageIndexTxt;

    private int searchPageIndex = 0;
    private int filterCategoryID = -1;
    private int totalSearchedBooksCount = 0;
    private List<BookData> searchPageResult;

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

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

    public void StartSearch(string searchWord)
    {
        if (Cache.Instance)
        {
            if (!string.IsNullOrEmpty(searchWord) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                ToggleSearchForNewSearch(false);
                endlessLoadingBar.SetActive(true);
                Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchWord);
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
                Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchIn.text);
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
                book.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(
                    book.GetChild(0).GetChild(0).GetComponent<Image>().color.r,
                    book.GetChild(0).GetChild(0).GetComponent<Image>().color.g,
                    book.GetChild(0).GetChild(0).GetComponent<Image>().color.b,
                    1.0f);
            }
            else
            {
                book.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(
                    book.GetChild(0).GetChild(0).GetComponent<Image>().color.r,
                    book.GetChild(0).GetChild(0).GetComponent<Image>().color.g,
                    book.GetChild(0).GetChild(0).GetComponent<Image>().color.b,
                    0.0f);
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
            Cache.Instance.search(SearchResultCallback, searchedBookContainer.childCount, searchPageIndex + 1, searchIN.RealText.text);
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
}
