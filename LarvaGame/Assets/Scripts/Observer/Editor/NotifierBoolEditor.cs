using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierBool))]
    public class NotifierBoolEditor : Editor
    {
        NotifierBool notifier;
        bool valueToNotify;

        private void OnEnable()
        {
            notifier = (NotifierBool)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            EditorGUILayout.Space();
            if (GUILayout.Button("Locate Observers", GUILayout.Height(30)))
                notifier.LocateObservers();

            EditorGUILayout.Space(10);

            // Toggle to choose the value to notify
            valueToNotify = GUILayout.Toggle(valueToNotify, " Value to notify (only for 'Notify Observers')");

            // Button to notify observers
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
