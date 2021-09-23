using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierInt))]
    public class NotifierIntEditor : Editor
    {
        int valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            GUILayout.BeginHorizontal();

            // Text field to choose the value to notify
            GUILayout.Label("Value to notify (only for 'Notify Observers') :");
            valueToNotify = EditorGUILayout.IntField(valueToNotify);

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierInt notifier = (NotifierInt)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
