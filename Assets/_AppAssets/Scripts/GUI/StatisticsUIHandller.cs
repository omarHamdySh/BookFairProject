using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsUIHandller : MonoBehaviour
{
    #region Logic
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private GameObject[] containers;
    [SerializeField] private GameObject statisticsItemPrefab;
    [SerializeField] private Button skipBtn;



    #endregion

    #region NumberOfPublisher
    [Header("Number of Publisher Menu")]
    [SerializeField] private FixTextMeshPro numberOfPublisherTxt;
    #endregion

    #region NumberOfBooksInPublisher
    [Header("Number of books in Publisher Menu")]
    [SerializeField] private FixTextMeshPro numberOfBooksPublisherTxt;
    #endregion

    #region NumberOfBooksInFair
    [Header("Number of  books in Fair Menu")]
    [SerializeField] private FixTextMeshPro numberOfBooksFairTxt;
    #endregion

    #region BestSellingBooksInfair
    [Header("Best Selling Books In fair Menu")]
    [SerializeField] private ScrollRect bestSellingBooksSV;
    #endregion

    #region FairSponsors
    [Header("Fair Sponsors Menu")]
    [SerializeField] private ScrollRect fairSponsorsSV;
    #endregion
}
