﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation.Examples;
using mostafa;
using Lean.Touch;

namespace mostafa
{
    
    public class Book : MonoBehaviour, IScrollable, IClickable
    {
        public string title;
        public Texture2D image;
        public PathNode pathNode;
        List<BookPage> pages;
        LeanSelectable leanSelectable;
        
        void Start()
        {
            leanSelectable = GetComponent<LeanSelectable>();
        }
        public float getScrollSpeed()
        {
            return 0;
        }

        public void select()
        {
            mostafa.SelectionManager.instance.selectThis(this);
        }
        public void onDeparture()
        {
            print("onDeparture");
        }

        public void onLand()
        {
            print("onLand");
        }

        public void onMoving()
        {
            print("onMoving");
        }

        public void focus()
        {
            CameraPath.instance.setTarget(pathNode);
            CameraPath.instance.gotoTarget();
        }

        public void unfocus()
        {
          
        }

        public void move(Vector3 destination, float duration)
        {
            throw new System.NotImplementedException();
        }

        public int getObjectIndex()
        {
            throw new System.NotImplementedException();
        }

        public void setObjectIndex(int objectIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}