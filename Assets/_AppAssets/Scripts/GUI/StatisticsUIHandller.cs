using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StatisticsUIHandller : MonoBehaviour
{
    #region Logic
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private MeshRenderer statisticsBoard;
    [SerializeField] private GameObject[] containers;
    [SerializeField] private GameObject statisticsItemPrefab;
    [SerializeField] private Button skipBtn;
    [SerializeField] private float delayBetweenContainers;

    private List<int> randomIndexList;
    private bool skipBtnPressed;
    private float timer;

    private void Start()
    {
        // PrepareData
        if (Cache.Instance)
        {
            Cache cache = Cache.Instance;
            ChangeNumberPublisher(cache.cachedData.allVendors.Count);
            if (cache.cachedData.allVendors[0].bookcaseData != null && cache.cachedData.allVendors[0].bookcaseData.categories != null)
                ChangeNumberOfBooksInPublisher(cache.numBooksInVendor(cache.cachedData.allVendors[0].bookcaseData));
            ChangeNumberOfBooksInFair(cache.cachedData.allFairs[cache.cachedData.allFairs.FindIndex((x) => x.id == cache.getFairId())].booksCount);
            PrepareBestSellingSV(cache.cachedData.BestSellers);
            PrepareFairSponsorsSV(cache.cachedData.allSponsors);
        }

        // Prepare Randomization
        GenerateRandom();

        timer = delayBetweenContainers;

        StartCoroutine(ShowStatistics());

    }

    private void Update()
    {
        delayBetweenContainers -= Time.deltaTime;
    }

    private void GenerateRandom()
    {
        randomIndexList = new List<int>();
        List<int> listIndexPool = new List<int>();
        for (int i = 0; i < containers.Length; i++)
        {
            listIndexPool.Add(i);
        }

        int index = 0;
        randomIndexList.Clear();
        for (int i = 0; i < containers.Length; i++)
        {
            index = Random.Range(0, listIndexPool.Count);
            randomIndexList.Add(listIndexPool[index]);
            listIndexPool.RemoveAt(index);
        }
    }

    private void CloseAllContainers()
    {
        foreach (GameObject i in containers)
        {
            i.SetActive(false);
        }
    }

    public void SkipBtn()
    {
        skipBtnPressed = true;
    }

    IEnumerator ShowStatistics()
    {
        int i = 0;

        while (true)
        {
            CloseAllContainers();
            containers[i].SetActive(true);

            yield return new WaitUntil(() => ((timer <= 0) || skipBtnPressed));
            i = (i + 1) % randomIndexList.Count;
            timer = delayBetweenContainers;
            skipBtnPressed = false;
        }
    }

    public void ToggleAllStatisticsUI(bool enabled)
    {
        EventSystem.current.SetSelectedGameObject(null);
        myCanvas.enabled = enabled;
        statisticsBoard.enabled = enabled;
    }

    #endregion

    #region NumberOfPublisher
    [Header("Number of Publisher Menu")]
    [SerializeField] private FixTextMeshPro numberOfPublisherTxt;

    private void ChangeNumberPublisher(int count)
    {
        numberOfBooksPublisherTxt.SetText(((PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "عدد الكتب في دار النشر : " : "Nubmer of book in Publisher : ") + count);
    }
    #endregion

    #region NumberOfBooksInPublisher
    [Header("Number of books in Publisher Menu")]
    [SerializeField] private FixTextMeshPro numberOfBooksPublisherTxt;

    public void ChangeNumberOfBooksInPublisher(int count)
    {
        numberOfBooksPublisherTxt.SetText(((PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "عدد الكتب في دار النشر : " : "Nubmer of book in Publisher : ") + count);
    }
    #endregion

    #region NumberOfBooksInFair
    [Header("Number of  books in Fair Menu")]
    [SerializeField] private FixTextMeshPro numberOfBooksFairTxt;

    private void ChangeNumberOfBooksInFair(int count)
    {
        numberOfBooksPublisherTxt.SetText(((PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ? "عدد الكتب في دار النشر : " : "Nubmer of book in Publisher : ") + count);
    }
    #endregion

    #region BestSellingBooksInfair
    [Header("Best Selling Books In fair Menu")]
    [SerializeField] private ScrollRect bestSellingBooksSV;

    private void PrepareBestSellingSV(List<BookData> bestSellers)
    {
        for (int i = 0; i < bestSellers.Count; i++)
        {
            GameObject go = Instantiate(statisticsItemPrefab, bestSellingBooksSV.content);
            go.GetComponentInChildren<FixTextMeshPro>().SetText(i + 1 + "." + bestSellers[i].name);
        }
    }
    #endregion

    #region FairSponsors
    [Header("Fair Sponsors Menu")]
    [SerializeField] private ScrollRect fairSponsorsSV;

    private void PrepareFairSponsorsSV(List<Sponsor> allSponsors)
    {
        for (int i = 0; i < allSponsors.Count; i++)
        {
            GameObject go = Instantiate(statisticsItemPrefab, fairSponsorsSV.content);
            go.GetComponentInChildren<FixTextMeshPro>().SetText(allSponsors[i].name);
        }
    }
    #endregion
}
