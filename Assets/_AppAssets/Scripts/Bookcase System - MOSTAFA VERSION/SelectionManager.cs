using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mostafa
{
    public class SelectionManager : MonoBehaviour
    {
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

        [HideInInspector] public IClickable selectedObject;

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
    }
}