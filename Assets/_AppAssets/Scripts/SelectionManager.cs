using System.Collections;
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

        if (this.selectedObject != selectedObject)
        {
            deselectThis(selectedObject);
            this.selectedObject = selectedObject;
            this.selectedObject.focus();
        }

    }

    public void deselectThis(IClickable selectedObject)
    {


        if (this.selectedObject == selectedObject)
        {
            this.selectedObject.unfocus();
            this.selectedObject = null;
        }
    }

    public void deselectCurrent()
    {


        if (this.selectedObject != null)
        {
            this.selectedObject.unfocus();
            this.selectedObject = null;
        }
    }


}

