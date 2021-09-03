using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event Player", menuName = "Event/EventPlayer")]
public class EventPlayer : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerPlayer> eventListeners = new List<EventListenerPlayer>();

    public void Raise(Player player)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(player);
    }

    public void SubscribeEvent(EventListenerPlayer eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerPlayer eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
