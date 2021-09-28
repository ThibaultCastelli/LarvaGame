using UnityEngine;
using UnityEditor;

namespace ObserverTC
{
    [CustomEditor(typeof(Notifier))]
    public class NotifierEditor : Editor
    {
        Notifier notifier;

        private void OnEnable()
        {
            notifier = (Notifier)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Only able to click when the game is running
            GUI.enabled = Application.isPlaying;

            EditorGUILayout.Space();
            if (GUILayout.Button("Locate Observers", GUILayout.Height(30)))
                notifier.LocateObservers();

            EditorGUILayout.Space();
            if (GUILayout.Button("Notify Observers", GUILayout.Height(50)))
                notifier.Notify();
        }
    }
}
