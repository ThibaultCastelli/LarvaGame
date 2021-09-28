using System.Collections.Generic;
using UnityEngine;

namespace ObserverTC
{
    [CreateAssetMenu(fileName = "Default Notifier String", menuName = "Notifiers/Notifier String")]
    public class NotifierString : ScriptableObject
    {
        [SerializeField] [TextArea] string description;

        // List of observers
        List<ObserverString> observers = new List<ObserverString>();

        // Method to add an observer to the list
        public void Subscribe(ObserverString newObserver)
        {
            if (observers.Contains(newObserver))
                return;

            observers.Add(newObserver);
        }

        // Method to remove an observer from the list
        public void Unsubscribe(ObserverString observerToRemove)
        {
            if (!observers.Contains(observerToRemove))
                return;

            observers.Remove(observerToRemove);
        }

        // Method to noitfy all the observers
        public void Notify(string value)
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