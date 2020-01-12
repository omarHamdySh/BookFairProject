using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mostafa;

namespace Mostafa
{
    public class Book : MonoBehaviour, IClickable
    {
        public string title;
        public Texture2D image;

        List<Page> pages;

        public void focus()
        {
        
        }

        public void unfocus()
        {
          
        }
    }
}