using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierVector2))]
    public class NotifierVector2Editor : Editor
    {
        Vector2 valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            // Vector2 field to choose the value to notify on X
            GUILayout.Label("Value to notify(only for 'Notify Observers')");
            valueToNotify = EditorGUILayout.Vector2Field("", valueToNotify);

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierVector2 notifier = (NotifierVector2)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
