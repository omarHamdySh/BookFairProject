using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

[RequireComponent(typeof(ObjectAlignerOverPath))]
public class Floor : MonoBehaviour, IScrollable
{
    public BookFair fair;
    public List<Bookcase> bookcases;

    public int getObjectIndex()
    {
        throw new System.NotImplementedException();
    }

    public float getScrollSpeed()
    {
        if(GameManager.Instance)
        {
            if(GameManager.Instance.pathData)
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

    public void move(Vector3 destination, float duration)
    {
        throw new System.NotImplementedException();
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

    public void setObjectIndex(int objectIndex)
    {
        throw new System.NotImplementedException();
    }

    public bool getLandStatus()
    {
        throw new System.NotImplementedException();
    }

    public void move(Vector3 destination, float duration, bool visibility)
    {
        throw new System.NotImplementedException();
    }
}