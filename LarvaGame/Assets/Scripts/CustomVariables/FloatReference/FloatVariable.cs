using UnityEngine;

namespace CustomVariablesTC
{
    [CreateAssetMenu(fileName = "Default Float Variable", menuName = "Custom Variables/Float")]
    public class FloatVariable : ScriptableObject
    {
        public float value;
    }
}
