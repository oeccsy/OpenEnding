using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Shatalmic;

public partial class NetworkManager
{
#if DEVELOPMENT_BUILD
    
    // 아래 UdpClinet는 LifeCycleManager에서 Application Quit인 경우에 Close() 처리
    private UdpClient server;
    private UdpClient client;
    
    #region Server Side

    public async void StartServer()
    {
        DebugText.Instance.AddText("Start Server");

        // 서버 디바이스 등록 : index 0
        myDeviceData.indexOfDeviceList = connectedDeviceList.Count;
        connectedDeviceList.Add(myDeviceData);
        
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
                        DebugText.Instance.AddText($"클라이언트 접속 : {result.RemoteEndPoint.Port}");

                        Networking.NetworkDevice newDevice = new Networking.NetworkDevice();
                        newDevice.indexOfDeviceList = connectedDeviceList.Count;
                        newDevice.endPoint = result.RemoteEndPoint;
                        connectedDeviceList.Add(newDevice);

                        break;
                    default:
                        SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
                        break;
                }
            }
            catch (SocketException e)
            {
                Debug.LogError(e);
            }
        }
    }
    
    private IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes) 
    {
        yield return new WaitWhile(() => isWritingData);
        isWritingData = true;

        server.Send(bytes, bytes.Length, targetDevice.endPoint);
        isWritingData = false;
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

    public IEnumerator ExecuteFuncOnAllDevice(byte[] bytes) // TODOv1
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

    public IEnumerator ExecuteFuncExceptOneDevice(int skipIndex, Byte[] bytes) // TODOv1
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
    
    public async void StartClient()
    {
        // UDP 클라이언트 초기화
        DebugText.Instance.AddText("Start Client");
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
                SealGame_PacketHandler.Instance.ExecuteFuncByPacket(bytes);
            }
            catch (SocketException e)
            {
                Debug.LogError(e);
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
    #endregion

    public void CloseUDPClient()
    {
        switch (connectType)
        {
            case Define.ConnectType.Server:
                server.Close();
                break;
            case Define.ConnectType.Client:
                client.Close();
                break;
        }
    }

#endif
}
