using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Shatalmic;

public partial class NetworkManager
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IOS    
    // 아래 UdpClinet는 LifeCycleManager에서 Application Quit인 경우에 Close() 처리
    private UdpClient server;
    private UdpClient client;
    
    #region Server Side

    public async void StartServer()
    {
        DebugCanvas.Instance.AddText("Start Server");

        // 서버 디바이스 등록 : index 0
        ownDeviceData.deviceListOrder = connectedDeviceList.Count;
        connectedDeviceList.Add(ownDeviceData);
        
        // UDP 서버 초기화
        server = new UdpClient(8888);

        while (true)
        {
            try
            {
                UdpReceiveResult result = await server.ReceiveAsync();
                byte[] bytes = result.Buffer;

                switch (bytes[0])
                {
                    case 255:
                        // On Device Connected
                        DebugCanvas.Instance.AddText($"Client Join : {result.RemoteEndPoint.Port}");

                        Networking.NetworkDevice newDevice = new Networking.NetworkDevice();
                        newDevice.deviceListOrder = connectedDeviceList.Count;
                        newDevice.endPoint = result.RemoteEndPoint;
                        connectedDeviceList.Add(newDevice);

                        break;
                    default:
                        break;
                }
            }
            catch (SocketException e)
            {
                Debug.LogError(e);
            }
        }
    }
    
    public IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes) 
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        server.Send(bytes, bytes.Length, targetDevice.endPoint);
        isWritingData = false;
    }

    public IEnumerator SendBytesToAllDevice(Byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        foreach (var targetDevice in connectedDeviceList)
        {
            yield return SendBytesToTargetDevice(targetDevice, bytes);
        }
        
        isWritingData = false;
    }

    public IEnumerator SendBytesExceptOneDevice(Networking.NetworkDevice skipDevice, Byte[] bytes)
    {
        yield return new WaitUntil(() => isWritingData);
        isWritingData = true;
        
        foreach (var targetDevice in connectedDeviceList)
        {
            if (targetDevice == skipDevice) continue;
      
            yield return SendBytesToTargetDevice(targetDevice, bytes);
        }
        
        isWritingData = false;
    }

    public void StopServer()
    {
        server?.Close();
    }

    #endregion
    
    #region Client Side
    
    public async void StartClient()
    {
        // UDP 클라이언트 초기화
        DebugCanvas.Instance.AddText("Start Client");
        client = new UdpClient();
        
        // 서버로 클라이언트 정보를 전달합니다.
        byte[] testBytes = new byte[5] { 255, 2, 3, 4, 5 };
        client.Send(testBytes, testBytes.Length, "127.0.0.1", 8888);
        
        while (true)
        {
            try
            {
                UdpReceiveResult result = await client.ReceiveAsync();
                byte[] bytes = result.Buffer;
            }
            catch (SocketException e)
            {
                Debug.LogError($"Client Error : {e}");
            }
        }
    }
    
    public IEnumerator SendBytesToServer(Byte[] bytes)
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;
        
        client.Send(bytes, bytes.Length, "127.0.0.1", 8888);
        
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
        client?.Close();
    }
    
    #endregion
#endif
}
