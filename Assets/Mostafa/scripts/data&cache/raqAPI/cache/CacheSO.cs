﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CacheSO", menuName = "Data/Cache")]
public class CacheSO : ScriptableObject
{
    public List<ProductCategory> allCategories;
    public List<BookcaseData> bookcasesData;
    public List<Vendor> allVendors;
}