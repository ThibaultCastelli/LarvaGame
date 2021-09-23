using UnityEngine;

namespace CustomVariablesTC
{
    [System.Serializable]
    public class StringReference
    {
        [SerializeField] bool useConstant = true;
        [SerializeField] string constant;
        [SerializeField] StringVariable reference;

        public string Value
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
