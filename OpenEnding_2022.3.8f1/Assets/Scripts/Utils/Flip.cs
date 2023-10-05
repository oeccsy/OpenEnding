using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : Singleton<Flip>
{
    [SerializeField]
    private Define.DisplayedFace curFace = Define.DisplayedFace.Head;
    [SerializeField]
    private bool isStartFlip = false;
    public delegate void FlipHandler();
    public event FlipHandler OnFlipToHead;
    public event FlipHandler OnFlipToTail;
    public event FlipHandler OnStartFlipToHead;
    public event FlipHandler OnStartFlipToTail;

    public delegate void NextFlipHandler();
    public event NextFlipHandler OnFlipToHeadOnlyNextTime;
    public event NextFlipHandler OnFlipToTailOnlyNextTime;
    public event NextFlipHandler OnStartFlipToHeadOnlyNextTime;
    public event NextFlipHandler OnStartFlipToTailOnlyNextTime;

    protected override void Awake()
    {
        base.Awake();
        
        Input.gyro.enabled = false;
    }
    
#if DEVELOPMENT_BUILD
    private void Start()
    {
        SetEnableGyroSensor(true);
        // OnFlipToHead += () => $"OnFlipToHead".Log();
        // OnFlipToTail += () => $"OnFlipToTail".Log();
        // OnStartFlipToHead += () => $"OnStartFlipToHead".Log();
        // OnStartFlipToTail += () => $"OnStartFlipToTail".Log();
    }
#endif

    public void SetEnableGyroSensor(bool enable)
    {
        Input.gyro.enabled = enable;
    }
    
    private void Update()
    {
        if (Input.gyro.enabled)
        {
            if (!isStartFlip && curFace == Define.DisplayedFace.Tail && Input.gyro.gravity.z < 0.7f)
            {
                isStartFlip = true;
                OnStartFlipToHead?.Invoke();
                OnStartFlipToHeadOnlyNextTime?.Invoke();
                OnStartFlipToHeadOnlyNextTime = null;
            }
            
            if (curFace == Define.DisplayedFace.Tail && Input.gyro.gravity.z < -0.95f)
            {
                curFace = Define.DisplayedFace.Head;
                isStartFlip = false;
                OnFlipToHead?.Invoke();
                OnFlipToHeadOnlyNextTime?.Invoke();
                OnFlipToHeadOnlyNextTime = null;
            }

            if (!isStartFlip && curFace == Define.DisplayedFace.Head && Input.gyro.gravity.z > 0f)
            {
                isStartFlip = true;
                OnStartFlipToTail?.Invoke();
                OnStartFlipToTailOnlyNextTime?.Invoke();
                OnStartFlipToTailOnlyNextTime = null;
            }

            if (curFace == Define.DisplayedFace.Head && Input.gyro.gravity.z > 0.95f)
            {
                curFace = Define.DisplayedFace.Tail;
                isStartFlip = false;
                OnFlipToTail?.Invoke();
                OnFlipToTailOnlyNextTime?.Invoke();
                OnFlipToTailOnlyNextTime = null;
            }
        }
    }
}