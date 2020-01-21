using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSpeed : MonoBehaviour
{
    private float swipeTime;
    public float LastSwipeTime;

    #region Singleton
    public static SwipeSpeed instance { private set; get; }
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

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void add()
    {
        swipeTime = Time.time;
    }

    public void calculateTime()
    {
        LastSwipeTime = Time.time - swipeTime;
        Debug.Log(LastSwipeTime);
        swipeTime = 0;
    }
    public void reset()
    {
        
    }
}
