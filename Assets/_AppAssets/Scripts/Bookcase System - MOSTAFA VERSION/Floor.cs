using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;


namespace mostafa
{

    [RequireComponent(typeof(ObjectAlignerOverPath))]
    public class Floor : MonoBehaviour, IScrollable
    {
        public BookFair fair;
        public List<Bookcase> bookcases;

        public float getScrollSpeed()
        {
            if (GameManager.Instance)
            {
                if (GameManager.Instance.pathData)
                {
                    return GameManager.Instance.pathData.FloorScrollSpeed;
                }
            }
            //Implemented by omar
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
    }
}