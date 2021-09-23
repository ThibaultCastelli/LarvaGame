using UnityEngine;

namespace CustomVariablesTC
{
    [System.Serializable]
    public class BoolReference
    {
        [SerializeField] bool useConstant = true;
        [SerializeField] bool constant;
        [SerializeField] BoolVariable reference;

        public bool Value
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
