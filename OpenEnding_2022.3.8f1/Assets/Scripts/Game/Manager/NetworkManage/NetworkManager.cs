using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class NetworkManager : Singleton<NetworkManager>
{
    [Header("Connect Info")]
    public string networkName = "test";
    public Define.ConnectType connectType = Define.ConnectType.None;
    public List<Networking.NetworkDevice> connectedDeviceList = new List<Networking.NetworkDevice>();
    public Networking.NetworkDevice myDeviceData = new Networking.NetworkDevice();  // Plugins Custom
    
    [Header("Networking")]
    [SerializeField]
    private Networking networking = null;                                           // Plugins
    [SerializeField]
    private bool isWritingData = false;                                             // true이면 현재 Writing(Sending) 중 임을 나타내는 변수

    private Action<Networking.NetworkDevice> OnDeviceReady = null;
    private Action<Networking.NetworkDevice, string, byte[]> OnReceiveDataFromClient = null;
    private Action<string, string, byte[]> OnReceiveDataFromServer = null;
    // 추가
    
#if !DEVELOPMENT_BUILD    
    private void Awake()
    {
        networking = GetComponent<Networking>();
        NetworkingInit();
    }
    
    private void NetworkingInit()
    {
        networking.Initialize((error) =>
        {
            switch (error)
            { 
                case "Bluetooth LE Not Enabled" :
                case "Bluetooth LE Not Powered Off" :
                case "Bluetooth LE Not Available" :
                case "Bluetooth LE Not Supported" :
                    "error".Log();
                    break;
            }
        }, (message) =>
        {
            //message.Log();
        });
    }

    #region Server Side

    public void StartServer()
    {
        "StartServer".Log();

        myDeviceData.deviceListOrder = connectedDeviceList.Count;
        connectedDeviceList.Add(myDeviceData);
        
        networking.StartServer(networkName, (connectedDevice) =>
        {
            $"{connectedDevice.Name} 접속 완료".Log();
            if (!connectedDeviceList.Contains(connectedDevice))
            {
                connectedDevice.deviceListOrder = connectedDeviceList.Count;
                connectedDeviceList.Add(connectedDevice);
            }

            StartCoroutine(ConnectManager.Instance.SynchronizeDevicesRoutine());
        }, (disconnectedDevice) =>
        {
            $"{disconnectedDevice.Name} 연결 끊김".Log();
            if (connectedDeviceList != null && connectedDeviceList.Contains(disconnectedDevice))
            {
                // indexOfDeviceList 꼬일 위험 점검
                connectedDeviceList.Remove(disconnectedDevice);
            }
                
        }, ((device, characteristic, bytes) =>
        {
            OnReceiveDataFromClient(device, characteristic, bytes);
            SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
        }));
    }
    
    public IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;
        
        networking.WriteDevice(targetDevice, bytes, () =>
        {
            isWritingData = false;
        });
    }

    public IEnumerator SendBytesToAllDevice(Byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        foreach (var targetDevice in connectedDeviceList)
        {
            networking.WriteDevice(targetDevice, bytes, () =>
            {
                
            });    
        }
        
        isWritingData = false;
    }
    
    public IEnumerator SendBytesExceptOneDevice(Networking.NetworkDevice skipDevice, Byte[] bytes)
    {
        yield return new WaitUntil(() => isWritingData);
        isWritingData = true;
        
        foreach (var device in connectedDeviceList)
        {
            if (device == skipDevice) continue;
            networking.WriteDevice(device, bytes, () =>
            {
                
            });
        }
        
        isWritingData = false;
    }

    public void StopServer()
    {
        networking.StopServer(() =>
        {
            
        });
    }

    #endregion
    
    #region Client Side
    
    public void StartClient()
    {
        "Start Client".Log();
        networking.StartClient(networkName, "client100", () =>
        {
            "Start Client".Log();
        }, (clientName, characteristic, bytes)=>
        {
            //OnReceiveDataFromServer(clientName, characteristic, bytes);
            SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
        });
    }
    
    public IEnumerator SendBytesToServer(Byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;
        
        networking.SendFromClient(bytes);
        
        isWritingData = false;
    }
    
    public IEnumerator RequestSendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes)
    {
        yield return new WaitUntil(() => isWritingData);
        isWritingData = true;

        if (NetworkManager.Instance.connectType == Define.ConnectType.Server)
        {
            yield return SendBytesToTargetDevice(targetDevice, bytes);
        }
        else
        {
            var request = new Byte[5]; // TODO
            yield return SendBytesToServer(request);
        }

        isWritingData = false;
    }

    public void StopClient()
    {
        networking.StopClient(() =>
        {
            
        });
    }
    
    #endregion


    public void SetActionByScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "ConnectScene":
                break;
            default:
                break;
        }
        
    }
#endif
}