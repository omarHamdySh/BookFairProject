using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

namespace mostafa
{

    [RequireComponent(typeof(ObjectAlignerOverPath))]
    public class Shelf : MonoBehaviour, IScrollable, IClickable
    {
        BookFair fair;
        public List<Book> bookList = new List<Book>();

        public float getScrollSpeed()
        {
            if (GameManager.Instance)
            {
                if (GameManager.Instance.pathData)
                {
                    return GameManager.Instance.pathData.ShelfScrollSpeed;
                }
            }
            return 0;
        }

        public void move()
        {
            print("move");
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
            print("focus");
        }

        public void unfocus()
        {
            print("focus");
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

        public bool getLandStatus()
        {
            throw new System.NotImplementedException();
        }
    }

}