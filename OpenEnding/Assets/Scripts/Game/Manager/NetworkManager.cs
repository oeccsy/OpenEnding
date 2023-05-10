using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    [Header("Connect Info")]
    public string networkName = "test";
    public Define.ConnectType connectType = Define.ConnectType.None;
    public List<Networking.NetworkDevice> connectedDeviceList = new List<Networking.NetworkDevice>();
    
    [Header("Networking")]
    [SerializeField]
    private Networking networking = null;
    private bool isSendable = true;
    private Action<Networking.NetworkDevice, string, byte[]> OnReceiveDataFromClient = null;
    private Action<string, string, byte[]> OnReceiveDataFromServer = null;

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
                    DebugText.Instance.AddText("error");
                    break;
            }
        }, (message) =>
        {
            // Status 상태가 바뀔 때 실행되는 영역
            // DebugText.Instance.AddText(message);
            // Debug.Log(message);
        });
    }

    #region Server Side

    public void StartServer()
    {
        DebugText.Instance.AddText("Start Server");
        networking.StartServer(networkName, (connectedDevice) =>
        {
            DebugText.Instance.AddText($"{connectedDevice.Name} 접속 완료");
            if (!connectedDeviceList.Contains(connectedDevice))
            {
                connectedDeviceList.Add(connectedDevice);
            }
        }, (disconnectedDevice) =>
        {
            DebugText.Instance.AddText($"{disconnectedDevice.Name} 연결 끊김");
            if (connectedDeviceList != null && connectedDeviceList.Contains(disconnectedDevice))
            {
                connectedDeviceList.Remove(disconnectedDevice);
            }
                
        }, ((device, characteristic, bytes) =>
        {
            OnReceiveDataFromClient(device, characteristic, bytes);
        }));
    }
    
    public IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes)
    {
        yield return new WaitUntil(() => isSendable);
        isSendable = false;
        
        networking.WriteDevice(targetDevice, bytes, () =>
        {
            isSendable = true;
        });
    }
    
    public IEnumerator SendBytesToAllDevice(Byte[] bytes)
    {
        yield return new WaitUntil(() => isSendable);
        
        for (int i = 0; i < connectedDeviceList.Count; i++)
        {
            yield return SendBytesToTargetDevice(connectedDeviceList[i], bytes);
        }
    }
    
    public IEnumerator SendBytesExceptOneDevice(int skipIndex, Byte[] bytes)
    {
        yield return new WaitUntil(() => isSendable);
        
        for (int i = 0; i < connectedDeviceList.Count; i++)
        {
            if(i == skipIndex) continue;
            yield return SendBytesToTargetDevice(connectedDeviceList[i], bytes);
        }
    }
    

    #endregion
    
    #region Client Side
    
    private void StartClient()
    {
        networking.StartClient(networkName, "client100", () =>
        {
            // Advertising을 시작했을 때 실행되는 영역
            // DebugText.Instance.AddText("Start Client");
        }, (clientName, characteristic, bytes)=>
        {
            OnReceiveDataFromServer(clientName, characteristic, bytes);
        });
    }
    
    public IEnumerator SendBytesToServer(Byte[] bytes)
    {
        yield return new WaitUntil(() => isSendable);
        isSendable = false;
        
        networking.SendFromClient(bytes);
        
        isSendable = true;
    }
    
    #endregion
}
