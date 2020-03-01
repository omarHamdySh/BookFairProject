using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }
    #endregion

    #region LoadingBar
    [Header("LoadingBar")]
    public GameObject loadingBar;
    #endregion

    #region SwitchFromUI to Game Mode
    [HideInInspector] public bool isUIOpen = false;

    public void ToggleUI(bool check)
    {
        isUIOpen = check;
    }
    #endregion

    #region Search
    [Header("Search")]
    [SerializeField] private Button prevSearchPageBtn, nextSearchPageBtn;
    [SerializeField] private TextMeshProUGUI seachResultCountTxt;
    [SerializeField] private Transform searchedBookContainer;

    private int searchPageIndex = 0;
    private int filterCategoryID = -1;
    private int searchedBookCount = 0;


    public void NextOrPrevPage(NextOrPrev nextOrPrev)
    {
        if (nextOrPrev == NextOrPrev.Next)
        {
            //Cache.Instance.search()
        }
        else
        {

        }
    }

    private void ManageSeachUIForSearchedResult()
    {
        if (searchedBookCount == 0)
        {
            prevSearchPageBtn.interactable = false;
            nextSearchPageBtn.interactable = false;
        }
        else if (searchedBookCount > 0)
        {
            if (searchPageIndex * searchedBookContainer.childCount <= searchedBookCount)
            {
                prevSearchPageBtn.interactable = false;
                nextSearchPageBtn.interactable = false;
            }
            else if (searchPageIndex > 2 && searchPageIndex < s)
            {

            }
        }
    }
    #endregion
}

public enum NextOrPrev
{
    Next,
    Previous
}
