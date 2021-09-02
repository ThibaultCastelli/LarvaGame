using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event Bool", menuName = "Event/EventBool")]
public class EventBool : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerBool> eventListeners = new List<EventListenerBool>();

    public void Raise(bool value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(value);
    }

    public void SubscribeEvent(EventListenerBool eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerBool eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
