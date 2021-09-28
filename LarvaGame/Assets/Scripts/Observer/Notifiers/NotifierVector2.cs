using System.Collections.Generic;
using UnityEngine;

namespace ObserverTC
{
    [CreateAssetMenu(fileName = "Default Notifier Vector2", menuName = "Notifiers/Notifier Vector2")]
    public class NotifierVector2 : ScriptableObject
    {
        [SerializeField] [TextArea] string description;

        // List of observers
        List<ObserverVector2> observers = new List<ObserverVector2>();

        // Method to add an observer to the list
        public void Subscribe(ObserverVector2 newObserver)
        {
            if (observers.Contains(newObserver))
                return;

            observers.Add(newObserver);
        }

        // Method to remove an observer from the list
        public void Unsubscribe(ObserverVector2 observerToRemove)
        {
            if (!observers.Contains(observerToRemove))
                return;

            observers.Remove(observerToRemove);
        }

        // Method to noitfy all the observers
        public void Notify(Vector2 value)
        {
            for (int i = observers.Count - 1; i >= 0; i--)
                observers[i].response?.Invoke(value);
        }

        public void LocateObservers()
        {
            Debug.Log($"Notifier '{name}' :");
            for (int i = 0; i < observers.Count; i++)
                Debug.Log($"Location of Observer n°{i} : {observers[i].gameObject.name}");
        }
    }
}