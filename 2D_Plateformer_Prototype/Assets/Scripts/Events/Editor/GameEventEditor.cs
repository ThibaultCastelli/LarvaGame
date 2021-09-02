using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Event))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Only enable the raise button on play mode
        GUI.enabled = Application.isPlaying;

        Event gameEvent = (Event)target;
        if (GUILayout.Button("Raise"))
            gameEvent.Raise();
    }
}
