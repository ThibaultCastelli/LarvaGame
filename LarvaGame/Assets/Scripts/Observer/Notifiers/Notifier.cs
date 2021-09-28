using System.Collections.Generic;
using UnityEngine;

namespace ObserverTC
{
    [CreateAssetMenu(fileName = "Default Notifier", menuName = "Notifiers/Notifier")]
    public class Notifier : ScriptableObject
    {
        [SerializeField] [TextArea] string description;

        // List of observers
        List<Observer> observers = new List<Observer>();

        // Method to add an observer to the list
        public void Subscribe(Observer newObserver)
        {
            if (observers.Contains(newObserver))
                return;

            observers.Add(newObserver);
        }

        // Method to remove an observer from the list
        public void Unsubscribe(Observer observerToRemove)
        {
            if (!observers.Contains(observerToRemove))
                return;

            observers.Remove(observerToRemove);
        }

        // Method to noitfy all the observers
        public void Notify()
        {
            for (int i = observers.Count - 1; i >= 0; i--)
                observers[i].response?.Invoke();
        }

        public void LocateObservers()
        {
            Debug.Log($"Notifier '{name}' :");
            for (int i = 0; i < observers.Count; i++)
                Debug.Log($"Location of Observer n°{i} : {observers[i].gameObject.name}");
        }
    }
}
