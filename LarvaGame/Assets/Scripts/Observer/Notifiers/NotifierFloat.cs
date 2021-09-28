using System.Collections.Generic;
using UnityEngine;

namespace ObserverTC
{
    [CreateAssetMenu(fileName = "Default Notifier Float", menuName = "Notifiers/Notifier Float")]
    public class NotifierFloat : ScriptableObject
    {
        [SerializeField] [TextArea] string description;

        // List of observers
        List<ObserverFloat> observers = new List<ObserverFloat>();

        // Method to add an observer to the list
        public void Subscribe(ObserverFloat newObserver)
        {
            if (observers.Contains(newObserver))
                return;

            observers.Add(newObserver);
        }

        // Method to remove an observer from the list
        public void Unsubscribe(ObserverFloat observerToRemove)
        {
            if (!observers.Contains(observerToRemove))
                return;

            observers.Remove(observerToRemove);
        }

        // Method to noitfy all the observers
        public void Notify(float value)
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


