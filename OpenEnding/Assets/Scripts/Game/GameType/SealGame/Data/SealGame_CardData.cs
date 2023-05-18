using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

[Serializable]
public class SealGame_CardData
{
    public int deviceNum = -1;
    public Define.SealGameCardType cardType = Define.SealGameCardType.None;
    public bool isMine = false;
    public Networking.NetworkDevice networkDevice;

    public SealGame_CardData(int deviceNum)
    {
        this.deviceNum = deviceNum;
        cardType = Define.SealGameCardType.None;
        isMine = false;

        if (deviceNum < NetworkManager.Instance.connectedDeviceList.Count)
        {
            networkDevice = NetworkManager.Instance.connectedDeviceList[deviceNum];    
        }
        else
        {
            networkDevice = null;
        }
    }
}
