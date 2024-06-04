using UnityEngine;

namespace Utility
{
    public class Gyro : MonoBehaviour
    {
        public Vector3 gravity = Vector3.zero;
        
        private void Awake()
        {
            Input.gyro.enabled = false;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Input.gyro.enabled = true;
#endif
        }

        private void Update()
        {
            if (!Input.gyro.enabled) return;

            gravity = Input.gyro.gravity;
        }
    }
}