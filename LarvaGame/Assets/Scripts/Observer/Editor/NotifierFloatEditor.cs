using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierFloat))]
    public class NotifierFloatEditor : Editor
    {
        float valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            GUILayout.BeginHorizontal();

            // Float field to choose the value to notify
            GUILayout.Label("Value to notify (only for 'Notify Observers') :");
            valueToNotify = EditorGUILayout.FloatField(valueToNotify);

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierFloat notifier = (NotifierFloat)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
