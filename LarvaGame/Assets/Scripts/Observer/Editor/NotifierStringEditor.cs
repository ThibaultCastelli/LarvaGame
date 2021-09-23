using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierString))]
    public class NotifierStringEditor : Editor
    {
        string valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            GUILayout.BeginHorizontal();

            // Text field to choose the value to notify
            GUILayout.Label("Value to notify (only for 'Notify Observers') :");
            valueToNotify = GUILayout.TextField(valueToNotify);

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierString notifier = (NotifierString)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
