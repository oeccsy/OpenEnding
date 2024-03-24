using TMPro;
using UnityEngine;

namespace Game.GameType.Roman
{
    public class Shake : MonoBehaviour
    {
        public float threshold = 2f;
        public delegate void ShakeHandler();

        public event ShakeHandler OnEveryShake;
        public event ShakeHandler OnNextShake;

        private void Update()
        {
            var value = Input.acceleration.magnitude;
            
            if (value > threshold)
            {
                OnEveryShake?.Invoke();
                OnNextShake?.Invoke();
                OnNextShake = null;
            }
        }
    }
}