using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierFloat4))]
    public class NotifierFloat4Editor : Editor
    {
        float valueToNotify1;
        float valueToNotify2;
        float valueToNotify3;
        float valueToNotify4;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            GUILayout.BeginHorizontal();

            // Float field to choose the value to notify
            GUILayout.Label("Value to notify (only for 'Notify Observers') :");
            valueToNotify1 = EditorGUILayout.FloatField(valueToNotify1);
            valueToNotify2 = EditorGUILayout.FloatField(valueToNotify2);
            valueToNotify3 = EditorGUILayout.FloatField(valueToNotify3);
            valueToNotify4 = EditorGUILayout.FloatField(valueToNotify4);

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierFloat4 notifier = (NotifierFloat4)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify1, valueToNotify2, valueToNotify3, valueToNotify4);
        }
    }
}
