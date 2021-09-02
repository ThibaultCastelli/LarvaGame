using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event", menuName = "Event/Event")]
public class Event : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListener> eventListeners = new List<EventListener>();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise();
    }

    public void SubscribeEvent(EventListener eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListener eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
