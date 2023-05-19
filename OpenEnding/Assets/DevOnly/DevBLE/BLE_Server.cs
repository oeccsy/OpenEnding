using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class BLE_Server : MonoBehaviour
{
    // 네트워킹을 담당하는 클래스
    [SerializeField]
    private Networking networking = null;

    // true이면 현재 Writing(Sending) 중 임을 나타내는 변수 // TODO isSendable 변수로 바꾸는게 좋아보인다.
    public bool isWritingData = false;

    // 데이터를 받았을 때 진행할 Interaction
    private Action<Networking.NetworkDevice, string, byte[]> OnReceiveData;

    private IEnumerator Start()
    {
        networking = GetComponent<Networking>();
        NetworkingInit();

        yield return new WaitForSecondsRealtime(3f);
        StartServer();
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
            // Status 상태가 바뀔 때 실행되는 코드
            //DebugText.Instance.AddText(message);
            //Debug.Log(message);
        });
    }

    public void StartServer()
    {
        DebugText.Instance.AddText("서버로직 시작");
        networking.StartServer("testtest", (connectedDevice) =>
        {
            DebugText.Instance.AddText("클라이언트 접속 완료");
            if (ConnectManager.Instance.connectedDeviceList == null)
                ConnectManager.Instance.connectedDeviceList = new List<Networking.NetworkDevice>();

            if (!ConnectManager.Instance.connectedDeviceList.Contains(connectedDevice))
            {
                ConnectManager.Instance.connectedDeviceList.Add(connectedDevice);
            }
            
            DebugText.Instance.AddText(connectedDevice.Name);
            DebugText.Instance.AddText(connectedDevice.Address);
        }, (disconnectedDevice) =>
        {
            DebugText.Instance.AddText("연결끊김");
            // 연결 끊겼을 경우 connectDeviceList Manage
            if (ConnectManager.Instance.connectedDeviceList != null && ConnectManager.Instance.connectedDeviceList.Contains(disconnectedDevice))
                ConnectManager.Instance.connectedDeviceList.Remove(disconnectedDevice);
        }, ((device, c, bytes) =>
        {
            
        }));
    }

    public IEnumerator SendBytesToTargetDevice(Networking.NetworkDevice targetDevice, Byte[] bytes)
    {
        yield return new WaitUntil(() => isWritingData == false);
        isWritingData = true;
        
        networking.WriteDevice(targetDevice, bytes, () =>
        {
            isWritingData = false;
        });
    }

    public IEnumerator SendBytesToAllDevice(Byte[] bytes)
    {
        bool isSendable = true;
        
        for (int i = 0; i < ConnectManager.Instance.connectedDeviceList.Count; i++)
        {
            yield return new WaitUntil(() => isSendable);
            isSendable = false;
            networking.WriteDevice(ConnectManager.Instance.connectedDeviceList[i], bytes, () =>
            {
                isSendable = true;
                DebugText.Instance.AddText("Semd Bytes Done");
            });
        }
        
        isWritingData = false;
    }



    #region Test

    //Server -> 하나의 Client 로의 데이터 전송
    private void SendTestText(Networking.NetworkDevice targetDevice)
    {
        if (isWritingData) return; // 전송 가능한 상태인지 체크
        isWritingData = true;

        var bytes = new byte[4]; // 전송할 데이터 마련
        bytes[0] = (byte)'A';
        bytes[1] = (byte)'A';
        bytes[2] = (byte)'A';
        bytes[3] = (byte)'A';

        networking.WriteDevice(targetDevice, bytes, () =>
        {
            DebugText.Instance.AddText("클라이언트로 데이터 전송됨");
            isWritingData = false;
        });
    }

    public void Test()
    {
        Networking.NetworkDevice target = ConnectManager.Instance.connectedDeviceList[0];
        SendTestText(target);
    }
    
    #endregion
}
