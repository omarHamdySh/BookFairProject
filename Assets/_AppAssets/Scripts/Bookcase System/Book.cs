//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using PathCreation.Examples;

//[RequireComponent(typeof(BookcaseObjectAlignerOverPath), typeof(Lean.Touch.LeanSelectable))]
//public class Book : MonoBehaviour, IScrollable, IClickable
//{
//    public string title;
//    public Texture2D image;
//    List<BookPage> pages;
//    public CameraPathNode pathNode;

//    public float getScrollSpeed()
//    {
//        if (GameManager.Instance)
//        {
//            if (GameManager.Instance.pathData)
//            {
//                return GameManager.Instance.pathData.BookScrollSpeed;
//            }
//        }
//        return 0;
//    }

//    public void move()
//    {
//        print("move");
//    }

//    public void select()
//    {
//        SelectionManager.instance.selectThis(this);
//    }

//    public void onDeparture()
//    {
//        print("onDeparture");
//    }

//    public void onLand()
//    {
//        print("onLand");
//    }

//    public void onMoving()
//    {
//        print("onMoving");
//    }

//    public void focus()
//    {
//        CameraPath.instance.setTarget(pathNode);
//        CameraPath.instance.gotoTarget();
//    }

//    public void unfocus()
//    {
//        print("focus");
//    }

//    public void move(Vector3 destination, float duration)
//    {
//        throw new System.NotImplementedException();
//    }

//    public int getObjectIndex()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void setObjectIndex(int objectIndex)
//    {
//        throw new System.NotImplementedException();
//    }

//    public bool getLandStatus()
//    {
//        throw new System.NotImplementedException();
//    }
//}