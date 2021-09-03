using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerInt : MonoBehaviour
{
    public EventInt gameEvent;
    public UnityEvent<int> responseInt;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(int value)
    {
        responseInt?.Invoke(value);
    }
}
