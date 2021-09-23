using UnityEngine;
using UnityEditor;

namespace SFXTC
{
    [CustomPropertyDrawer(typeof(RangedFloat))]
    public class RangedFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get values of the property
            SerializedProperty minProperty = property.FindPropertyRelative("minValue");
            SerializedProperty maxProperty = property.FindPropertyRelative("maxValue");

            float minValue = minProperty.floatValue;
            float maxValue = maxProperty.floatValue;

            // Set min and max range
            float minRange = 0;
            float maxRange = 1;

            // Check if the porperty has a MixMaxRangeAttribute
            var range = (MinMaxRangeAttribute)System.Attribute.GetCustomAttribute(fieldInfo, typeof(MinMaxRangeAttribute));
            if (range != null)
            {
                // If so, change the default min and max range
                minRange = range.Min;
                maxRange = range.Max;
            }

            const float rangeLabelWidth = 40;

            // Label
            position = EditorGUI.PrefixLabel(position, label);

            // Label for min range
            var rangeLabelMin = new Rect(position);
            rangeLabelMin.width = rangeLabelWidth;
            GUI.Label(rangeLabelMin, minRange.ToString("F2"));
            position.xMin += rangeLabelWidth;

            // Label for max range
            var rangeLabelMax = new Rect(position);
            rangeLabelMax.xMin = rangeLabelMax.xMax - rangeLabelWidth;
            GUI.Label(rangeLabelMax, maxRange.ToString("F2"));
            position.xMax -= rangeLabelWidth;

            // Slider
            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, minRange, maxRange);
            if (EditorGUI.EndChangeCheck())
            {
                minProperty.floatValue = minValue;
                maxProperty.floatValue = maxValue;
            }

            EditorGUI.EndProperty();
        }
    }
}
