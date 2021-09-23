using UnityEngine;
using UnityEditor;

namespace CustomVariablesTC
{
    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get the bool to prompt good field
            bool useConstant = property.FindPropertyRelative("useConstant").boolValue;

            // Label
            position = EditorGUI.PrefixLabel(position, label);

            // Box to choose constant or reference
            var box = new Rect(position.position, Vector2.one * 20);
            if (EditorGUI.DropdownButton(box, GUIContent.none, FocusType.Passive))
            {
                // Create the menu and add items
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Constant"), useConstant, () => SetBoolValue(property, true));
                menu.AddItem(new GUIContent("Reference"), !useConstant, () => SetBoolValue(property, false));
                menu.ShowAsContext();
            }

            // Set position and size of input field
            position.position += Vector2.right * 25;
            var fieldPos = new Rect(position.position.x, position.position.y, 230, 20);

            if (useConstant)
            {
                int constantValue = property.FindPropertyRelative("constant").intValue;

                // Prompt the current constant value
                string newValue = EditorGUI.TextField(fieldPos, constantValue.ToString());

                // Get the value from the text field and apply it to the property
                int.TryParse(newValue, out constantValue);
                property.FindPropertyRelative("constant").intValue = constantValue;
            }
            else
                EditorGUI.ObjectField(fieldPos, property.FindPropertyRelative("reference"), GUIContent.none);

            EditorGUI.EndProperty();
        }

        // Leave space for next property
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }

        // Set the bool value of the property
        void SetBoolValue(SerializedProperty property, bool useConstant)
        {
            property.FindPropertyRelative("useConstant").boolValue = useConstant;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
