using UnityEngine;

namespace CustomVariablesTC
{
    [System.Serializable]
    public class IntReference
    {
        [SerializeField] bool useConstant = true;
        [SerializeField] int constant;
        [SerializeField] IntVariable reference;

        public int Value
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
