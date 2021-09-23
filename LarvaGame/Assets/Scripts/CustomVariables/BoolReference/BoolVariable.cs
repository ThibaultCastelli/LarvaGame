using UnityEngine;

namespace CustomVariablesTC
{
    [CreateAssetMenu(fileName = "Default Bool Variable", menuName = "Custom Variables/Bool")]
    public class BoolVariable : ScriptableObject
    {
        public bool value;
    }
}
