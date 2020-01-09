using UnityEngine;
using UnityEngine.Events;

public class GameEventListener_Osama : MonoBehaviour
{
    public GameEvent_Osama Event;
    public UnityEvent Response;

    private void OnEnable() { Event.Register(this); }

    private void OnDisable() { Event.Unregister(this); }

    public void OnEventRaised() { Response.Invoke(); }
}