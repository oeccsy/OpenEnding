using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : Singleton<Flip>
{
    private Define.DisplayedFace curFace = Define.DisplayedFace.Head;
    public delegate void FlipHandler();
    public event FlipHandler OnFlipToHead;
    public event FlipHandler OnFlipToTail;
    public event FlipHandler OnStartFlipToHead;
    public event FlipHandler OnStartFlipToTail;

    private void Awake()
    {
        Input.gyro.enabled = false;
    }

    public void SetEnableGyroSensor(bool enable)
    {
        Input.gyro.enabled = enable;
    }
    
    private void Update()
    {
        if (Input.gyro.enabled)
        {
            if (curFace == Define.DisplayedFace.Tail && Input.gyro.gravity.z < 0.7f)
            {
                OnStartFlipToHead?.Invoke();
            }
            
            if (curFace == Define.DisplayedFace.Tail && Input.gyro.gravity.z < -0.95f)
            {
                curFace = Define.DisplayedFace.Head;
                OnFlipToHead?.Invoke();
            }

            if (curFace == Define.DisplayedFace.Head && Input.gyro.gravity.z > 0f)
            {
                OnStartFlipToTail?.Invoke();
            }

            if (curFace == Define.DisplayedFace.Head && Input.gyro.gravity.z > 0.95f)
            {
                curFace = Define.DisplayedFace.Tail;
                OnFlipToTail?.Invoke();
            }
        }
    }
}