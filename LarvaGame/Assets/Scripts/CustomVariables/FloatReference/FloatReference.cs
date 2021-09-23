using UnityEngine;

namespace CustomVariablesTC
{
    [System.Serializable]
    public class FloatReference
    {
        [SerializeField] bool useConstant = true;
        [SerializeField] float constant;
        [SerializeField] FloatVariable reference;

        public float Value
        {
            get { return useConstant ? constant : reference.value; }
            set
            {
                if (useConstant)
                    constant = value;
                else
                    reference.value = value;
            }
        }
    }
}
