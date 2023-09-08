using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Connect_Scene : Singleton<Connect_Scene>
{
    public int n;
    public int r;
    
    public DeviceObject userOwnDeviceObject;
    
    public List<DeviceObject> deviceObjectList = new List<DeviceObject>();
    public GameObject plane;

    private void Awake()
    {
        // 지형 생성
        GameObject planePrefab = Resources.Load<GameObject>("Prefabs/Plane");
        Instantiate(planePrefab, GameObject.Find("GameObjectRoot").transform);
        
        GameObject deviceObjectPrefab = Resources.Load<GameObject>("Prefabs/DeviceObject");
        for (int i = 0; i < n; i++)
        {
            Vector3 targetPos = new Vector3(r * Mathf.Cos(((360f / n) * i - 90f) * Mathf.Deg2Rad), 0, r * Mathf.Sin(((360f / n) * i - 90f) * Mathf.Deg2Rad));
            Quaternion targetRot = Quaternion.Euler(-90, 0, -90 + ((180 * (n-2) / n) * i));
            
            var newObj = Instantiate(deviceObjectPrefab, targetPos, targetRot, GameObject.Find("GameObjectRoot").transform);
            var newDeviceObject = newObj.GetComponent<DeviceObject>();
            newDeviceObject.ownColor = (ColorPalette.ColorName)i;
            deviceObjectList.Add(newDeviceObject);
        }

        DeviceObject.OnTouchAnyDevice += SelectOwnDeviceWithAnimation;
    }

    private void SelectOwnDeviceWithAnimation(DeviceObject selectedDevice)
    {
        if (userOwnDeviceObject != null) return;
        
        userOwnDeviceObject = selectedDevice;
        
        selectedDevice.Flip();
        selectedDevice.SetDeviceColor(selectedDevice.ownColor);
        
        DeviceObject.OnTouchAnyDevice -= SelectOwnDeviceWithAnimation;
        DeviceObject.OnTouchAnyDevice += LeavePartyWithAnimation;
    }

    public void JoinPartyWithAnimation(List<ColorPalette.ColorName> deviceColorList)
    {
        foreach (var ownColor in deviceColorList)
        {
            var device = deviceObjectList[(int)ownColor];
            device.Flip();
            device.SetDeviceColor(ownColor);
        }
    }

    public void LeavePartyWithAnimation(DeviceObject selectedDevice)
    {
        if (selectedDevice != userOwnDeviceObject) return;
        
        userOwnDeviceObject = null;
        
        foreach (var device in deviceObjectList)
        {
            if (device.curColor != ColorPalette.ColorName.DeviceDefault)
            {
                device.Flip();
                device.SetDeviceColor(ColorPalette.ColorName.DeviceDefault);
            }
        }
        
        DeviceObject.OnTouchAnyDevice -= LeavePartyWithAnimation;
        DeviceObject.OnTouchAnyDevice += SelectOwnDeviceWithAnimation;
    }
    
    public IEnumerator SynchronizeDevicesRoutine()
    {
        yield return null;
    }
}
