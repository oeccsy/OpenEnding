using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SealGame_CardContainer
{
    public List<SealGame_CardData> deviceDataList = new List<SealGame_CardData>();

    public void InitDeviceDataList()
    {
        for (int i = 0; i < NetworkManager.Instance.connectedDeviceList.Count; i++)
        {
            deviceDataList.Add(new SealGame_CardData(i));
        }
    }

    public void SetDeviceData(int deviceIndex, Define.SealGameCardType targetCardType)
    {
        DebugText.Instance.AddText($"DeviceList Count : {NetworkManager.Instance.connectedDeviceList.Count}");
        
        deviceDataList[deviceIndex].cardType = targetCardType;
    }
}
