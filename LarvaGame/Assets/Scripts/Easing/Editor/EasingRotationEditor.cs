using UnityEditor;
using UnityEngine;

namespace EasingTC
{
    [CustomEditor(typeof(EasingRotation))]
    public class EasingRotationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EasingRotation _target = (EasingRotation)target;

            EditorGUILayout.LabelField("ANIMATION CHOICE", EditorStyles.boldLabel);

            _target.animationType = (AnimationType)EditorGUILayout.EnumPopup(new GUIContent("Animation Type", "Ease In : Start slow.\nEase Out : End slow.\nEase In Out : Start and end slow.\nMirror : Go back and forth.\nSpecial Ease : Bounce or back effect."), _target.animationType);

            switch (_target.animationType)
            {
                case AnimationType.EaseIn:
                    _target.easeInType = (EaseIn)EditorGUILayout.EnumPopup(new GUIContent("Ease In Type", "Quad : Slow slope.\nCirc : Hard slope."), _target.easeInType);
                    break;

                case AnimationType.EaseOut:
                    _target.easeOutType = (EaseOut)EditorGUILayout.EnumPopup(new GUIContent("Ease Out Type", "Quad : Slow slope.\nCirc : Hard slope."), _target.easeOutType);
                    break;

                case AnimationType.EaseInOut:
                    _target.easeInOutType = (EaseInOut)EditorGUILayout.EnumPopup(new GUIContent("Ease In Out Type", "Quad : Slow slope.\nCirc : Hard slope."), _target.easeInOutType);
                    break;

                case AnimationType.Mirror:
                    _target.mirorType = (MirorType)EditorGUILayout.EnumPopup(new GUIContent("Mirror Type", "Quad : Slow slope.\nCirc : Hard slope."), _target.mirorType);
                    break;

                case AnimationType.SpecialEase:
                    _target.specialEaseType = (SpecialEase)EditorGUILayout.EnumPopup(new GUIContent("Special Ease Type", "Back In : Back effect at start and go to end value.\nBack Out : Go to end value and back effect.\nBounce Out : Go to end value and bounce effect."), _target.specialEaseType);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("ANIMATION INFOS", EditorStyles.boldLabel);

            _target.playOnAwake = EditorGUILayout.Toggle(new GUIContent("Play On Awake", "Select if the animation should automatically start when the game start."), _target.playOnAwake);
            
            _target.loop = EditorGUILayout.Toggle(new GUIContent("Loop", "Select if the animation should automatically loop."), _target.loop);
            if (_target.loop)
                _target.loopType = (LoopType)EditorGUILayout.EnumPopup(new GUIContent("Loop Type", "Simple : Loop the animation.\nMirror : Loop the animation back and forth. (can't work with Mirror animation type"), _target.loopType);

            EditorGUILayout.Space();
            _target.useAnotherStartValue = EditorGUILayout.Toggle(new GUIContent("Use Another Start Rotation", "Select if you want to use a different start value.\nUnselect if you want to use the current value of the object as the start value."), _target.useAnotherStartValue);
            _target.addRotation = EditorGUILayout.Toggle(new GUIContent("Add Rotation", "Select if you want to add this value to the start value.\nUnselect if you want the object to go to this end value."), _target.addRotation);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("ANIMATION VALUES", EditorStyles.boldLabel);

            if (_target.useAnotherStartValue)
                _target.startRot = EditorGUILayout.Vector3Field(new GUIContent("Start Rotation", "Set the value for the start of the animation."), _target.startRot);

            if (_target.addRotation)
                _target.addRot = EditorGUILayout.Vector3Field(new GUIContent("Add Rotation", "Set the value that will be add to the start value."), _target.addRot);
            else
                _target.endRot = EditorGUILayout.Vector3Field(new GUIContent("End Rotation", "Set the value that the object will reach."), _target.endRot);

            _target.duration = EditorGUILayout.Slider(new GUIContent("Duration", "Set the duration of the animation. (in s)"), _target.duration, 0.01f, 20f);
        }
    }
}