using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierFloat))]
    public class NotifierFloatEditor : Editor
    {
        NotifierFloat notifier;
        float valueToNotify;

        private void OnEnable()
        {
            notifier = (NotifierFloat)target;
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
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            // Float field to choose the value to notify
            GUILayout.Label("Value to notify (only for 'Notify Observers') :");
            valueToNotify = EditorGUILayout.FloatField(valueToNotify);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            // Button to notify observers
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
