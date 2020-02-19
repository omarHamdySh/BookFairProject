using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : UIHandler
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

    #region SwitchFromUI to Game Mode
    [HideInInspector] public bool isUIOpen = false;

    public void ToggleUI(bool check)
    {
        isUIOpen = check;
    }
    #endregion
}
