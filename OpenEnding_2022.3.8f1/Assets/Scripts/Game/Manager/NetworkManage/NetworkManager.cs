using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class NetworkManager : Singleton<NetworkManager>
{
    [Header("Connect Info")]
    public string networkName = "OpenEnding";
    public string clientName = "None";
    public Define.ConnectType connectType = Define.ConnectType.None;
    public readonly List<Networking.NetworkDevice> connectedDeviceList = new List<Networking.NetworkDevice>();
    public readonly Networking.NetworkDevice ownDeviceData = new Networking.NetworkDevice();  // Plugins Custom
    
    [Header("Networking")]
    [SerializeField]
    private Networking networking = null;                                           // Plugins
    [SerializeField]
    private bool isWritingData = false;                                             // true이면 현재 Writing(Sending) 중 임을 나타내는 변수

    
    public Action<Networking.NetworkDevice> OnDeviceReady = null;
    public Action<Networking.NetworkDevice> OnDeviceDisconnected = null;
    public Action<Networking.NetworkDevice, string, byte[]> OnReceiveDataFromClient = null;
    
    public Action<string, string, byte[]> OnReceiveDataFromServer = null;

#if UNITY_ANDROID || UNITY_IOS 
    protected override void Awake()
    {
        base.Awake();
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
        connectType = Define.ConnectType.Server;
        
        ownDeviceData.deviceListOrder = connectedDeviceList.Count;
        ownDeviceData.colorOrder = 0;
        
        connectedDeviceList.Add(ownDeviceData);

        networking.StartServer(networkName, (connectedDevice) =>
        {
            "OnDeviceReady".Log();
            OnDeviceReady?.Invoke(connectedDevice);
        }, (disconnectedDevice) =>
        {
            "OnDeviceDisconnected".Log();
            OnDeviceDisconnected?.Invoke(disconnectedDevice);
        }, (device, characteristic, bytes) =>
        {
            "OnDeviceData".Log();
            OnReceiveDataFromClient?.Invoke(device, characteristic, bytes);
        });
    }
    
    public IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) yield break;
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        $"Try Send To {(ColorPalette.ColorName)targetDevice.colorOrder}".Log();
        
        if (targetDevice == ownDeviceData)
        {
            OnReceiveDataFromServer?.Invoke(null, null, bytes);
        }
        else
        {
            networking.WriteDevice(targetDevice, bytes, () =>
            {
                isWritingData = false;
            });    
        }
    }

    public IEnumerator SendBytesToAllDevice(Byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) yield break;
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;
        
        $"Device Count : {connectedDeviceList.Count}".Log();

        foreach (var targetDevice in connectedDeviceList)
        {
            $"Try Send To {(ColorPalette.ColorName)targetDevice.colorOrder}".Log();
            
            if (targetDevice == ownDeviceData)
            {
                OnReceiveDataFromServer?.Invoke(null, null, bytes);
            }
            else
            {
                networking.WriteDevice(targetDevice, bytes, () =>
                {
                    
                });     
            }
        }

        isWritingData = false;
    }
    
    public IEnumerator SendBytesExceptOneDevice(Networking.NetworkDevice skipDevice, Byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) yield break;
        yield return new WaitUntil(() => isWritingData);
        isWritingData = true;
        
        foreach (var targetDevice in connectedDeviceList)
        {
            if (targetDevice == skipDevice) continue;
            
            if (targetDevice == ownDeviceData)
            {
                OnReceiveDataFromServer?.Invoke(null, null, bytes);
            }
            else
            {
                networking.WriteDevice(targetDevice, bytes, () =>
                {
                    
                });    
            }
        }
        
        isWritingData = false;
    }

    public void StopServer()
    {
        networking.StopServer(() =>
        {
            "StopServer".Log();
        });
    }

    #endregion
    
    #region Client Side
    
    public void StartClient()
    {
        "Start Client".Log();
        connectType = Define.ConnectType.Client;
        
        networking.StartClient(networkName, clientName, () =>
        {
            "Start Client Advertising".Log();
        }, (client, characteristic, bytes) =>
        {
            OnReceiveDataFromServer?.Invoke(client, characteristic, bytes);
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

        if (connectType == Define.ConnectType.Server)
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
            "StopClient".Log();
        });
    }
    
    #endregion
    
#endif
}