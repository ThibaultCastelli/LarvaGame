using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(NotifierGameObject))]
    public class NotifierGameObjectEditor : Editor
    {
        GameObject valueToNotify;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            // Toggle to choose the value to notify
            valueToNotify = (GameObject)EditorGUILayout.ObjectField("Value to notify (only for 'Notify Observers')", valueToNotify, typeof(GameObject), true);

            EditorGUILayout.Space();

            // Button to notify observers
            NotifierGameObject notifier = (NotifierGameObject)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify(valueToNotify);
        }
    }
}
