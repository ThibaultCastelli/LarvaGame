using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Event GameObject", menuName = "Event/EventGameObject")]
public class EventGameObject : ScriptableObject
{
    [SerializeField] [Multiline] string description;

    private readonly List<EventListenerGameObject> eventListeners = new List<EventListenerGameObject>();

    public void Raise(GameObject obj)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaise(obj);
    }

    public void SubscribeEvent(EventListenerGameObject eventListener)
    {
        if (!eventListeners.Contains(eventListener))
            eventListeners.Add(eventListener);
    }

    public void UnsubscribeEvent(EventListenerGameObject eventListener)
    {
        if (eventListeners.Contains(eventListener))
            eventListeners.Remove(eventListener);
    }
}
