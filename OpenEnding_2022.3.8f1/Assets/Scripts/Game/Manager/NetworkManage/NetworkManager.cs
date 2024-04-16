using System;
using System.Collections;
using System.Collections.Generic;
using Game.Manager.NetworkManage;
using Shatalmic;
using UnityEngine;

public partial class NetworkManager : Singleton<NetworkManager>
{
    [Header("Connect Info")]
    public string networkName = "OpenEnding";
    public string clientName = "None";
    public Define.ConnectType connectType = Define.ConnectType.None;
    
    public readonly List<Networking.NetworkDevice> connectedDeviceList = new List<Networking.NetworkDevice>();
    public readonly Networking.NetworkDevice ownDeviceData = new Networking.NetworkDevice();
    
    [Header("Networking")]
    private Networking networking = null;
    private bool isWritingData = false;

#if UNITY_ANDROID || UNITY_IOS

    protected override void Awake()
    {
        NetworkingInit();
    }
    
    public void NetworkingInit()
    {
        if (networking != null) Destroy(networking);
        
        networking = gameObject.AddComponent<Networking>();
        networking.Initialize(null, null);
    }

    #region Server Side

    public ServerSidePacketHandler serverSidePacketHandler;       
    
    public Action<Networking.NetworkDevice> OnDeviceReady = null;
    public Action<Networking.NetworkDevice> OnDeviceDisconnected = null;
    public Action<Networking.NetworkDevice, string, byte[]> OnReceiveDataFromClient = null;
    
    public void StartServer()
    {
        connectType = Define.ConnectType.Server;
        
        ownDeviceData.deviceListOrder = connectedDeviceList.Count;
        ownDeviceData.colorOrder = 0;
        
        connectedDeviceList.Add(ownDeviceData);

        networking.StartServer
        (
            networkName, 
            (connectedDevice) => OnDeviceReady?.Invoke(connectedDevice),
            (disconnectedDevice) => OnDeviceDisconnected?.Invoke(disconnectedDevice),
            (device, characteristic, bytes) => OnReceiveDataFromClient?.Invoke(device, characteristic, bytes)
        );
    }
    
    public IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) yield break;
   
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        if (targetDevice == ownDeviceData)
        {
            OnReceiveDataFromServer?.Invoke(null, null, bytes);
            isWritingData = false;
        }
        else
        {
            networking.WriteDevice(targetDevice, bytes, () => isWritingData = false);    
        }
    }

    public IEnumerator SendBytesToAllDevice(byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) yield break;
        
        yield return new WaitWhile(() => isWritingData);
        yield return new WaitForSecondsRealtime(0.3f);
        
        foreach (var targetDevice in connectedDeviceList)
        {
            yield return new WaitWhile(() => isWritingData);
            isWritingData = true;
            
            if (targetDevice == ownDeviceData)
            {
                OnReceiveDataFromServer?.Invoke(null, null, bytes);
                isWritingData = false;
            }
            else
            {
                networking.WriteDevice(targetDevice, bytes, () => isWritingData = false);    
                $"Send [{bytes[0]},{bytes[1]}] to {targetDevice.Name}".Log();
            }
        }
    }
    
    public void StopServer()
    {
        connectedDeviceList.Clear();
        networking.StopServer(null);
    }

    #endregion
    
    #region Client Side
    
    public ClientSidePacketHandler clientSidePacketHandler;
    
    public Action<string, string, byte[]> OnReceiveDataFromServer = null;
    
    public void StartClient()
    {
        connectType = Define.ConnectType.Client;
        
        networking.StartClient
        (
            networkName,
            clientName,
            null,
            (client, characteristic, bytes) => OnReceiveDataFromServer?.Invoke(client, characteristic, bytes)
        );
    }
    
    public IEnumerator SendBytesToServer(byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        if (connectType == Define.ConnectType.Client)
        {
            networking.SendFromClient(bytes);
            $"Send [{bytes[0]},{bytes[1]}] to Server".Log();
        }
        else if (connectType == Define.ConnectType.Server)
        {
            OnReceiveDataFromClient?.Invoke(null, null, bytes);
        }
        
        isWritingData = false;
    }
    
    public void StopClient()
    {
        networking.StopClient(null);
    }
    
    #endregion
    
#endif
}