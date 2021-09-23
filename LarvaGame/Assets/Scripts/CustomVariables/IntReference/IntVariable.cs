using UnityEngine;

namespace CustomVariablesTC
{
    [CreateAssetMenu(fileName = "Default Int Variable", menuName = "Custom Variables/Int")]
    public class IntVariable : ScriptableObject
    {
        public int value;
    }
}
