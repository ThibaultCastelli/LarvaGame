using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerBool : MonoBehaviour
{
    public EventBool gameEvent;
    public UnityEvent<bool> responseBool;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(bool value)
    {
        responseBool?.Invoke(value);
    }
}
