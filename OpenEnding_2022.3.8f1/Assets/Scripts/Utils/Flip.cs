using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : Singleton<Flip>
{
    public delegate void FlipHandler();
    public event FlipHandler OnFlip;

    private Vector3 beforeEulerAngle;
    private Vector3 curEulerAngle;

    private float flipRemainingTime; // 2초 이상 유지할 경우 Flip으로 확인
    private Coroutine remainingTimeCheckRoutine = null;
    
    // 뒤집혔다고 판단하는 각도
    private const float thresholdAngle = 150f;
    public void StartCheckFlip()
    {
        Input.gyro.enabled = true;
        beforeEulerAngle = Input.gyro.attitude.eulerAngles;
    }

    public IEnumerator CheckFlip()
    {
        //2초동안 150 이상 유지 할 경우 Flip으로 확인

        bool isStartCheckFlipTime = false;
        
        while (true)
        {
            if (beforeEulerAngle.y - curEulerAngle.y > thresholdAngle)
            {
                if (isStartCheckFlipTime)
                {
                    flipRemainingTime += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    
                }


            }
            else
            {
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.gyro.enabled)
        {
            DebugCanvas.Instance.SetText(Input.gyro.attitude.eulerAngles.ToString());
        }
    }
}
