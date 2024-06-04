using UnityEngine;

namespace Utility.Hierarchy
{
    public abstract class RootObject : MonoBehaviour
    {
        private void Awake()
        {
            Register();
        }
    
        protected abstract void Register();
    }
}
