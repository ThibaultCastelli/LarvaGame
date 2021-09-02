using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event Float", menuName = "Event/EventFloat")]
public class EventFloat : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerFloat> eventListeners = new List<EventListenerFloat>();

    public void Raise(float value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(value);
    }

    public void SubscribeEvent(EventListenerFloat eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerFloat eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
