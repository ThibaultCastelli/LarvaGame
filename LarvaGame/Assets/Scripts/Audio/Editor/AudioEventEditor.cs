using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioEvent), true)]
public class AudioEventEditor : Editor
{
    AudioSource _previewer;

    private void OnEnable()
    {
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio Preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        DestroyImmediate(_previewer.gameObject);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioEvent audioEvent = (AudioEvent)target;

        if (GUILayout.Button("Preview"))
            audioEvent.Play(_previewer);
    }
}
