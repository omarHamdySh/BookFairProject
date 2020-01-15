using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfState : IGameplayState
{
    GameplayState stateName = GameplayState.Shelf;
    public GameplayFSMManager gameplayFSMManager;


    public void OnStateEnter()
    {

    }

    public void OnStateExit()
    {

    }

    public void OnStateUpdate()
    {

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
