using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookState : IGameplayState
{
    GameplayState stateName = GameplayState.Book;

    public GameplayFSMManager gameplayFSMManager;


    public void OnStateEnter()
    {

    }

    public void OnStateExit()
    {

    }

    public void OnStateUpdate()
    {
        GameManager.Instance.pathData.BookScrollSpeed = SwipeSpeed.instance.horizontalScrollSpeed;
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
