using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event Int", menuName = "Event/EventInt")]
public class EventInt : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerInt> eventListeners = new List<EventListenerInt>();

    public void Raise(int value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(value);
    }

    public void SubscribeEvent(EventListenerInt eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerInt eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
