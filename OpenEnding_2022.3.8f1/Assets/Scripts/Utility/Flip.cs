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
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Input.gyro.enabled = true;
#endif
        }
    
        private void Update()
        {
            if (!Input.gyro.enabled) return;
            
            if (Input.gyro.gravity.z < -0.95f)
            {
                if (curFace == Define.DisplayedFace.Head) return;
                
                curFace = Define.DisplayedFace.Head;
                OnFlipToHead?.Invoke();
                OnNextHead?.Invoke();
                OnNextHead = null;
            }

            if (-0.7f < Input.gyro.gravity.z && Input.gyro.gravity.z < 0.7f)
            {
                if (curFace == Define.DisplayedFace.Stand) return;
                
                curFace = Define.DisplayedFace.Stand;
                OnStand?.Invoke();
                OnNextStand?.Invoke();
                OnNextStand = null;
            }
            
            if (Input.gyro.gravity.z > 0.95f)
            {
                if (curFace == Define.DisplayedFace.Tail) return;
                
                curFace = Define.DisplayedFace.Tail;
                OnFlipToTail?.Invoke();
                OnNextTail?.Invoke();
                OnNextTail = null;
            }
        }
    }
}