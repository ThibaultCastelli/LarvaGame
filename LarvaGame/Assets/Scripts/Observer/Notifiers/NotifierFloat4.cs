using System.Collections.Generic;
using UnityEngine;

namespace ObserverTC
{
    [CreateAssetMenu(fileName = "Default Notifier Float4", menuName = "Notifiers/Notifier Float4")]
    public class NotifierFloat4 : ScriptableObject
    {
        [SerializeField] [TextArea] string description;

        // List of observers
        List<ObserverFloat4> observers = new List<ObserverFloat4>();

        // Method to add an observer to the list
        public void Subscribe(ObserverFloat4 newObserver)
        {
            if (observers.Contains(newObserver))
                return;

            observers.Add(newObserver);
        }

        // Method to remove an observer from the list
        public void Unsubscribe(ObserverFloat4 observerToRemove)
        {
            if (!observers.Contains(observerToRemove))
                return;

            observers.Remove(observerToRemove);
        }

        // Method to noitfy all the observers
        public void Notify(float value1, float value2, float value3, float value4)
        {
            for (int i = observers.Count - 1; i >= 0; i--)
                observers[i].response?.Invoke(value1, value2, value3, value4);
        }
    }
}


