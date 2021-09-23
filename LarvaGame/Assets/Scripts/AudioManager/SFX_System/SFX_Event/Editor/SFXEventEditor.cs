using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SFXTC
{
    [CustomEditor(typeof(SFXEvent))]
    public class SFXEventEditor : Editor
    {
        SFXEvent _target;

        AudioSource _previewer;

        SerializedProperty randomVolumeProperty;
        SerializedProperty randomPanProperty;
        SerializedProperty randomPitchProperty;
        SerializedProperty randomReverbProperty;

        private void OnEnable()
        {
            _target = (SFXEvent)target;

            randomVolumeProperty = serializedObject.FindProperty("randomVolume");
            randomPanProperty = serializedObject.FindProperty("randomPan");
            randomPitchProperty = serializedObject.FindProperty("randomPitch");
            randomReverbProperty = serializedObject.FindProperty("randomReverbZoneMix");

            _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio Preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            DestroyImmediate(_previewer.gameObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            _target.useRandomVolume = EditorGUILayout.Toggle(new GUIContent("Use Random Volume", "Chose to use a fixed or a random value"), _target.useRandomVolume);
            if (_target.useRandomVolume)
                EditorGUILayout.PropertyField(randomVolumeProperty, new GUIContent("Random Volume", "Select the volume of the clip.\nA random value will be chosen each it will be played.\n0 = no sound | 1 = full sound"), true);
            else
                _target.volume = EditorGUILayout.Slider(new GUIContent("Volume", "Select the volume of the clip.\n0 = no sound | 1 = full sound"), _target.volume, 0, 1);

            EditorGUILayout.Space();

            _target.useRandomPan = EditorGUILayout.Toggle(new GUIContent("Use Random Pan", "Chose to use a fixed or a random value"), _target.useRandomPan);
            if (_target.useRandomPan)
                EditorGUILayout.PropertyField(randomPanProperty, new GUIContent("Random Pan", "Select the pan (earing from left or right) of the clip.\nA random value will be chosen each it will be played.\n-1 = left | 0 = center | 1 = right"));
            else
                _target.pan = EditorGUILayout.Slider(new GUIContent("Pan", "Select the pan (earing from left or right) of the clip.\n-1 = left | 0 = center | 1 = right"), _target.pan, -1, 1);

            EditorGUILayout.Space();

            _target.useRandomPitch = EditorGUILayout.Toggle(new GUIContent("Use Random Pitch", "Chose to use a fixed or a random value"), _target.useRandomPitch);
            if (_target.useRandomPitch)
                EditorGUILayout.PropertyField(randomPitchProperty, new GUIContent("Random Pitch", "Select the pitch (lower or higher frequency) of the clip.\nA random value will be chosen each it will be played.\n0 = low | 1 = normal | 2 = high"));
            else
                _target.pitch = EditorGUILayout.Slider(new GUIContent("Pitch", "Select the pitch (lower or higher frequency) of the clip.\n0 = low | 1 = normal | 2 = high"), _target.pitch, 0, 2);

            EditorGUILayout.Space();

            _target.useRandomReverbZoneMix = EditorGUILayout.Toggle(new GUIContent("Use Random Reverb Zone Mix", "Chose to use a fixed or a random value"), _target.useRandomReverbZoneMix);
            if (_target.useRandomReverbZoneMix)
                EditorGUILayout.PropertyField(randomReverbProperty, new GUIContent("Random Reverb Zone Mix", "Select how much the reverb zone affect the clip.\nA random value will be chosen each it will be played.\n0 = no reverb | 1 = normal"));
            else
                _target.reverbZoneMix = EditorGUILayout.Slider(new GUIContent("Reverb Zone Mix", "Select how much the reverb zone affect the clip.\n0 = no reverb | 1 = normal"), _target.reverbZoneMix, 0, 1);

            serializedObject.ApplyModifiedProperties();

            // Preview Button
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Preview", GUILayout.Height(50)))
                _target.Preview(_previewer);

            if (GUILayout.Button("Stop", GUILayout.Height(50)))
                _target.StopPreview(_previewer);
            GUILayout.EndHorizontal();
        }
    }
}
