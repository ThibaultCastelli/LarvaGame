using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerVector2 : MonoBehaviour
{
    public EventVector2 gameEvent;
    public UnityEvent<Vector2> responsePlayer;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(Vector2 dir)
    {
        responsePlayer?.Invoke(dir);
    }
}
