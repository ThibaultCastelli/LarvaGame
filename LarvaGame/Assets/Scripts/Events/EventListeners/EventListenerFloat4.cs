using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerFloat4 : MonoBehaviour
{
    public EventFloat4 gameEvent;
    public UnityEvent<float, float, float, float> responseInt;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(float value1, float value2, float value3, float value4)
    {
        responseInt?.Invoke(value1, value2, value3, value4);
    }
}
