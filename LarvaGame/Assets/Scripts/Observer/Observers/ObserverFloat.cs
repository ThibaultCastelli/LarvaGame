using UnityEngine;
using UnityEngine.Events;

namespace ObserverTC
{
    public class ObserverFloat : MonoBehaviour
    {
        [Tooltip("The Notifier to subscribe.")]
        [SerializeField] NotifierFloat notifier;

        [Tooltip("The reponse (function) to do when notify.")]
        public UnityEvent<float> response;

        // Add to the notifier's list of observers
        private void OnEnable()
        {
            if (notifier == null)
            {
                Debug.LogError("ERROR : There is no notifier to subscribe\nLocation : " + gameObject.name + ".");
                return;
            }

            notifier.Subscribe(this);
        }
        // Remove from the notifier's list of observers
        private void OnDisable()
        {
            if (notifier == null)
            {
                Debug.LogError("ERROR : There is no notifier to unsubscribe\nLocation : " + gameObject.name + ".");
                return;
            }

            notifier.Unsubscribe(this);
        }
    }
}

