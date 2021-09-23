using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(Notifier))]
    public class NotifierEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            // Button to notify observers
            Notifier notifier = (Notifier)target;
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify();
        }
    }
}
