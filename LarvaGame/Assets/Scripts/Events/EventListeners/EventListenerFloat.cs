using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerFloat : MonoBehaviour
{
    public EventFloat gameEvent;
    public UnityEvent<float> responseInt;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(float value)
    {
        responseInt?.Invoke(value);
    }
}
