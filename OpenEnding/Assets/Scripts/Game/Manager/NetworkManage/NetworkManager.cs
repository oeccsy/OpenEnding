using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

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
    private Action<Networking.NetworkDevice, string, byte[]> OnReceiveDataFromClient = null;
    private Action<string, string, byte[]> OnReceiveDataFromServer = null;

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

        // 서버 디바이스 등록 : index 0
        myDeviceData.indexOfDeviceList = connectedDeviceList.Count;
        connectedDeviceList.Add(myDeviceData);
        
        networking.StartServer(networkName, (connectedDevice) =>
        {
            DebugText.Instance.AddText($"{connectedDevice.Name} 접속 완료");
            if (!connectedDeviceList.Contains(connectedDevice))
            {
                connectedDevice.indexOfDeviceList = connectedDeviceList.Count;
                connectedDeviceList.Add(connectedDevice);
            }
        }, (disconnectedDevice) =>
        {
            DebugText.Instance.AddText($"{disconnectedDevice.Name} 연결 끊김");
            if (connectedDeviceList != null && connectedDeviceList.Contains(disconnectedDevice))
            {
                // indexOfDeviceList 꼬일 위험 점검
                connectedDeviceList.Remove(disconnectedDevice);
            }
                
        }, ((device, characteristic, bytes) =>
        {
            //OnReceiveDataFromClient(device, characteristic, bytes);
            SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
        }));
    }
    
    private IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;
        
        networking.WriteDevice(targetDevice, bytes, () =>
        {
            isWritingData = false;
        });
    }

    public IEnumerator ExecuteFuncOnTargetDevice(byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);

        if (bytes[1] == 0)
        {
            // 서버 디바이스 처리
            SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
        }
        else
        {
            // 클라이언트 디바이스 처리
            Networking.NetworkDevice targetDevice = connectedDeviceList[bytes[1]];
            yield return SendBytesToTargetDevice(targetDevice, bytes);
        }
    }

    public IEnumerator ExecuteFuncOnAllDevice(byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);

        // 서버 디바이스 처리
        SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
        
        // 클라이언트 디바이스 처리
        for (int i = 1; i < connectedDeviceList.Count; i++)
        {
            Networking.NetworkDevice targetDevice = connectedDeviceList[i];
            yield return SendBytesToTargetDevice(targetDevice, bytes);
        }
    }

    public IEnumerator ExecuteFuncExceptOneDevice(int skipIndex, Byte[] bytes)
    {
        yield return new WaitUntil(() => isWritingData);
        
        for (int i = 0; i < connectedDeviceList.Count; i++)
        {
            if(i == skipIndex) continue;
            yield return ExecuteFuncOnTargetDevice(bytes);
        }
    }

    #endregion
    
    #region Client Side
    
    public void StartClient()
    {
        networking.StartClient(networkName, "client100", () =>
        {
            // Advertising을 시작했을 때 실행되는 영역
            // DebugText.Instance.AddText("Start Client");
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
    
    #endregion
#endif
}
