using UnityEngine;

namespace SFXTC
{
    // Attribute to set the max and min values of RangedFloat
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMaxRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}
