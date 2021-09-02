using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerGameObject : MonoBehaviour
{
    public EventGameObject gameEvent;
    public UnityEvent<GameObject> responsePlayerGameObject;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(GameObject obj)
    {
        responsePlayerGameObject?.Invoke(obj);
    }
}
