using UnityEngine;

namespace CustomVariablesTC
{
    [CreateAssetMenu(fileName = "Default String Variable", menuName = "Custom Variables/String")]
    public class StringVariable : ScriptableObject
    {
        public string value;
    }
}
