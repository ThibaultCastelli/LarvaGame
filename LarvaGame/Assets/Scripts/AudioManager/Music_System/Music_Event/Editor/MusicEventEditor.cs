using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MusicTC
{
    [CustomEditor(typeof(MusicEvent))]
    public class MusicEventEditor : Editor
    {
        MusicEvent _target;
        AudioSource[] _previewers;

        private void OnEnable()
        {
            _target = (MusicEvent)target;
            _previewers = new AudioSource[_target.MusicLayers.Length];

            for (int i = 0; i < _target.MusicLayers.Length; i++)
                _previewers[i] = EditorUtility.CreateGameObjectWithHideFlags("Audio Preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            foreach (AudioSource source in _previewers)
                DestroyImmediate(source);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(20);

            // Only able to click when the game is not running
            GUI.enabled = !Application.isPlaying;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Preview", GUILayout.Height(50)))
                _target.PlayPreview(_previewers);

            if (GUILayout.Button("Stop", GUILayout.Height(50)))
                _target.StopPreview(_previewers);

            if (GUILayout.Button("Increase Layer", GUILayout.Height(50)))
                _target.IncreaseLayerPreview(_previewers);

            if (GUILayout.Button("Decrease Layer", GUILayout.Height(50)))
                _target.DecreaseLayerPreview(_previewers);
            GUILayout.EndHorizontal();

            
        }
    }
}
