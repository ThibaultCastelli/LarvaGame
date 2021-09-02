using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event Vector2", menuName = "Event/EventVector2")]
public class EventVector2 : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerVector2> eventListeners = new List<EventListenerVector2>();

    public void Raise(Vector2 dir)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(dir);
    }

    public void SubscribeEvent(EventListenerVector2 eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerVector2 eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
