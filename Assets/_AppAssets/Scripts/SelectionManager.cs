﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SelectionManager : MonoBehaviour
{
    public IClickable selectedObject;

    #region Singleton
    public static SelectionManager instance { private set; get; }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    public void selectThis(IClickable selectedObject)
    {
            deselectThis(this.selectedObject);
            this.selectedObject = selectedObject;
            this.selectedObject.focus();
    }

    public void deselectThis(IClickable selectedObject)
    {
        this.selectedObject = null;

    }

    public void deselectCurrent()
    {

        if (this.selectedObject != null)
        {

            this.selectedObject.unfocus();

        }
    }


}

