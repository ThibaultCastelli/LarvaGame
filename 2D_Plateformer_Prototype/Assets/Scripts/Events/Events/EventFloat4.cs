using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event Floatx4", menuName = "Event/EventFloatx4")]
public class EventFloat4 : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerFloat4> eventListeners = new List<EventListenerFloat4>();

    public void Raise(float value1, float value2, float value3, float value4)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(value1, value2, value3, value4);
    }

    public void SubscribeEvent(EventListenerFloat4 eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerFloat4 eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
