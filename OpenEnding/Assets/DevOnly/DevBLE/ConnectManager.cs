using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class ConnectManager : Singleton<ConnectManager>
{
    [Header("Connect Info")]
    public string networkName = "test";
    public Define.ConnectType connectType = Define.ConnectType.None;
    public List<Networking.NetworkDevice> connectedDeviceList = null;

    public BLE_Server bleServer;
    public BLE_Client bleClient;

    public void SetConnectType(Define.ConnectType type)
    {
        switch (type)
        {
            case Define.ConnectType.None:
                break;
            case Define.ConnectType.Server:
                connectType = Define.ConnectType.Server;
                bleServer = gameObject.AddComponent<BLE_Server>();
                break;
            case Define.ConnectType.Client:
                connectType = Define.ConnectType.Client;
                bleClient = gameObject.AddComponent<BLE_Client>(); 
                break;
        }
    }
}
