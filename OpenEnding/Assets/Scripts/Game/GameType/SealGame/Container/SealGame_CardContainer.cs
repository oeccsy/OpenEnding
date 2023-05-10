using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealGame_CardContainer : Singleton<SealGame_CardContainer>
{
    public List<SealGame_DeviceData> deviceDataList = new List<SealGame_DeviceData>();

    public void InitDeviceDataList()
    {
        // Add Client Device Data
        for (int i = 0; i < ConnectManager.Instance.connectedDeviceList.Count; i++)
        {
            deviceDataList.Add(new SealGame_DeviceData(i));
        }
        
        // Add Server Device Data
        int serverIndex = deviceDataList.Count;
        deviceDataList.Add(new SealGame_DeviceData(serverIndex));
    }

    public void SetDeviceData(int deviceIndex, Define.SealGameCardType targetCardType)
    {
        deviceDataList[deviceIndex].cardType = targetCardType;
    }
}
