using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierInt))]
    public class NotifierIntEditor : Editor
    {
        NotifierInt notifier;
        int valueToNotify;

        private void OnEnable()
        {
            notifier = (NotifierInt)target;
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

            GUILayout.BeginHorizontal();
            // Text field to choose the value to notify
            GUILayout.Label("Value to notify (only for 'Notify Observers') :");
            valueToNotify = EditorGUILayout.IntField(valueToNotify);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            // Button to notify observers
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
