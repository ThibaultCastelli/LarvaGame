using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierBool))]
    public class NotifierBoolEditor : Editor
    {
        bool valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            // Toggle to choose the value to notify
            valueToNotify = GUILayout.Toggle(valueToNotify, " Value to notify (only for 'Notify Observers')");

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierBool notifier = (NotifierBool)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
