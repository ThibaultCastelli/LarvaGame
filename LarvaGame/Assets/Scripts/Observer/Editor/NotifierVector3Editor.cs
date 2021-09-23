using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierVector3))]
    public class NotifierVector3Editor : Editor
    {
        Vector3 valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            // Vector2 field to choose the value to notify on X
            GUILayout.Label("Value to notify(only for 'Notify Observers')");
            valueToNotify = EditorGUILayout.Vector3Field("", valueToNotify);

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierVector3 notifier = (NotifierVector3)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
