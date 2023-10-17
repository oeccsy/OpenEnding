using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text a;
    public TMP_Text b;
    public TMP_Text c;
    public TMP_Text d;
    public TMP_Text e;
    public TMP_Text f;
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.gyro.enabled)
        {
            Vector3 acceleration = Input.acceleration;
            Vector3 gyroRotationRate = Input.gyro.rotationRate;
            Vector3 gyroGravity = Input.gyro.gravity; // z축 값 -1~1로 판단하는게 제일 오차가 적어보이며 값도 깔끔해보임.
            Vector3 gyroAttitude = Input.gyro.attitude.eulerAngles;
            Vector3 gyroAccel = Input.gyro.userAcceleration;
            Vector3 gyroUnbiased = Input.gyro.rotationRateUnbiased;

            a.SetText($"Accel : {acceleration.ToString()}");
            b.SetText($"rotaRate : {gyroRotationRate.ToString()}");
            c.SetText($"gyroUnbias : {gyroUnbiased.ToString()}");
            d.SetText($"gravity : {gyroGravity.ToString()}");
            e.SetText($"gyroAtt : {gyroAttitude.ToString()}");
            f.SetText($"gyroAccel : {gyroAccel.ToString()}");
        }
    }
}
