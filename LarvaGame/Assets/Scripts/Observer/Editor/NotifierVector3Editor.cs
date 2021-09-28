using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierVector3))]
    public class NotifierVector3Editor : Editor
    {
        NotifierVector3 notifier;
        Vector3 valueToNotify;

        private void OnEnable()
        {
            notifier = (NotifierVector3)target;
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

            // Vector2 field to choose the value to notify on X
            GUILayout.Label("Value to notify(only for 'Notify Observers')");
            valueToNotify = EditorGUILayout.Vector3Field("", valueToNotify);

            EditorGUILayout.Space();
            // Button to notify observers
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
