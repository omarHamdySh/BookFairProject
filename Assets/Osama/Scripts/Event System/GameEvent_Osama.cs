using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent_Osama", menuName = "Events/GameEvent_Osama", order = 1)]
public class GameEvent_Osama : ScriptableObject
{
    private List<GameEventListener_Osama> listeners = new List<GameEventListener_Osama>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Register(GameEventListener_Osama listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void Unregister(GameEventListener_Osama listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}