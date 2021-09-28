using System.Collections.Generic;
using UnityEngine;

namespace ObserverTC
{
    [CreateAssetMenu(fileName = "Default Notifier Bool", menuName = "Notifiers/Notifier Bool")]
    public class NotifierBool : ScriptableObject
    {
        [SerializeField] [TextArea] string description;

        // List of observers
        List<ObserverBool> observers = new List<ObserverBool>();

        // Method to add an observer to the list
        public void Subscribe(ObserverBool newObserver)
        {
            if (observers.Contains(newObserver))
                return;

            observers.Add(newObserver);
        }

        // Method to remove an observer from the list
        public void Unsubscribe(ObserverBool observerToRemove)
        {
            if (!observers.Contains(observerToRemove))
                return;

            observers.Remove(observerToRemove);
        }

        // Method to noitfy all the observers
        public void Notify(bool value)
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