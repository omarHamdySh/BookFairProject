using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCaseState : IGameplayState
{
    GameplayState stateName = GameplayState.BookCase;

    public GameplayFSMManager gameplayFSMManager;


    public void OnStateEnter()
    {

    }

    public void OnStateExit()
    {

    }

    public void OnStateUpdate()
    {
        Debug.Log("zebby");
        GameManager.Instance.pathData.BookcaseScrollSpeed = SwipeSpeed.instance.horizontalScrollSpeed; 
    }
    string ToString()
    {
        return stateName.ToString();
    }

    public GameplayState GetState()
    {
        return stateName;
    }
}
