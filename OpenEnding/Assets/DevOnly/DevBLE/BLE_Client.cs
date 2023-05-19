using System;
using Shatalmic;
using UnityEngine;

public class BLE_Client : MonoBehaviour
{
    // 네트워킹 컴포넌트
    [SerializeField]
    private Networking networking = null;
    
    // true이면 현재 Writing(Sending) 중 임을 나타내는 변수
    [SerializeField]
    private bool isWritingData = false;
    
    // 데이터를 받았을 때 진행할 Interaction
    public Action<string, string, byte[]> OnReceiveData = null;
    
    private void Awake()
    {
        networking = GetComponent<Networking>();
    }

    private void Start()
    {
        NetworkingInit();
        StartClient();
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
            
            BluetoothLEHardwareInterface.Log("Error: " + error);
        }, (message) =>
        {
            // Status 상태가 바뀔 때 실행되는 코드
            // Client MTU set to 125 이후에 교환 진행할 것
            DebugText.Instance.AddText(message);
            BluetoothLEHardwareInterface.Log("Message: " + message);
        });
    }

    private void StartClient()
    {
        networking.StartClient("testtest", "client11", () =>
        {
            // Advertising을 시작했을 때 실행되는 영역
            networking.StatusMessage = "Started advertising";
            DebugText.Instance.AddText("서버 탐색 시작");
        }, (s1, s2, bytes)=>
        {
            OnReceiveData(s1, s2, bytes); // 액션을 바로 넣으니까 안됐음 주의
        });
    }

    public void SendBytesToServer(Byte[] bytes)
    {
        if (isWritingData) return; // 전송 가능한 상태인지 체크
        isWritingData = true;
        
        // 클라이언트는 서버에 데이터를 보낸다.
        networking.SendFromClient(bytes);
        
        isWritingData = false;
    }
    
    #region Test

    public void SendTestText()
    {
        if (isWritingData) return; // 전송 가능한 상태인지 체크
        isWritingData = true;

        var bytes = new byte[4];
        bytes[0] = 1;
        bytes[1] = 1;
        bytes[2] = 1;
        bytes[3] = 1;
        
        //서버가 아니라면 networking 클래스를 통해 전송에 도움을 받는다.
        networking.SendFromClient(bytes);
        
        DebugText.Instance.AddText("서버로 데이터 전송됨");
        isWritingData = false;
    }
    
    #endregion
}