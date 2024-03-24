using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
    public class Flip : MonoBehaviour
    {
        [SerializeField]
        private Define.DisplayedFace curFace = Define.DisplayedFace.None;
        public delegate void FlipHandler();
        public event FlipHandler OnFlipToHead;
        public event FlipHandler OnFlipToTail;
        public event FlipHandler OnStand;

        public delegate void NextFlipHandler();
        public event NextFlipHandler OnNextHead;
        public event NextFlipHandler OnNextTail;
        public event NextFlipHandler OnNextStand;

        private void Awake()
        {
            Input.gyro.enabled = false;
        }
    
        private void Update()
        {
            if (!Input.gyro.enabled) return;
            
            if (Input.gyro.gravity.z < -0.95f)
            {
                curFace = Define.DisplayedFace.Head;
                OnFlipToHead?.Invoke();
                OnNextHead?.Invoke();
                OnNextHead = null;
            }

            if (-0.7f < Input.gyro.gravity.z && Input.gyro.gravity.z < 0.7f)
            {
                curFace = Define.DisplayedFace.Stand;
                OnStand?.Invoke();
                OnNextStand?.Invoke();
                OnNextStand = null;
            }
            
            if (Input.gyro.gravity.z > 0.95f)
            {
                curFace = Define.DisplayedFace.Tail;
                OnFlipToTail?.Invoke();
                OnNextTail?.Invoke();
                OnNextTail = null;
            }
        }
    }
}