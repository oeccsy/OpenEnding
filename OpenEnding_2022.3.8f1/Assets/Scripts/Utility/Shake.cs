using System.Collections;
using UnityEngine;

namespace Utility
{
    public class Shake : MonoBehaviour
    {
        private bool isShakeable = true;
        public float threshold = 2f;
        public WaitForSecondsRealtime throttle = new WaitForSecondsRealtime(2f);
        public delegate void ShakeHandler();

        public event ShakeHandler OnEveryShake;
        public event ShakeHandler OnNextShake;

        private void Update()
        {
            if (!isShakeable) return;
            
            var value = Input.acceleration.magnitude;
            
            if (value > threshold)
            {
                OnEveryShake?.Invoke();
                OnNextShake?.Invoke();
                OnNextShake = null;

                StartCoroutine(ThrottleRoutine());
            }
        }

        private IEnumerator ThrottleRoutine()
        {
            isShakeable = false;
            yield return throttle;
            isShakeable = true;
        }
    }
}