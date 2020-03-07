using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CacheSO", menuName = "Data/Cache")]
public class CacheSO : ScriptableObject
{
    public List<ProductCategory> allCategories;
    public List<Vendor> allVendors;
    public List<BookData> searchResult;
    public List<Sponsor> allSponsors;
}
