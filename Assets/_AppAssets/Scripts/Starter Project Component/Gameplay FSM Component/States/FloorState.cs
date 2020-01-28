using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorState : IGameplayState
{
    GameplayState stateName = GameplayState.Floor;

    public GameplayFSMManager gameplayFSMManager;


    public void OnStateEnter()
    {
        
    }

    public void OnStateExit()
    {

    }

    public void OnStateUpdate()
    {
        GameManager.Instance.pathData.FloorScrollSpeed = SwipeSpeed.instance.verticalScrollSpeed;
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
