using TMPro;
using UnityEngine;

namespace Game.GameType.Roman
{
    public class Shake : MonoBehaviour
    {
        public TextMeshProUGUI debugText;
        public float threshold = 10f;
        public delegate void ShakeHandler();

        public event ShakeHandler OnEveryShake;
        public event ShakeHandler OnNextShake;

        private void Update()
        {
            var value = Input.acceleration.magnitude;
            
            if (value > 10)
            {
                OnEveryShake?.Invoke();
                OnNextShake?.Invoke();
                OnNextShake = null;
                DebugCanvas.Instance.AddText(value);
            }

            debugText.text = value.ToString();
        }
    }
}