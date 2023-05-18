using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class SealGame_PacketHandler : Singleton<SealGame_PacketHandler>
{
    // 데이터 포맷
    
    // index 0 : 최초 송신 디바이스 번호
    // index 1 : 최종 수신 디바이스 번호
    // index 2 : 실행할 로직의 Class
    // index 3 : 실행할 로직
    // index 4 : 실행할 로직의 param
    // . . .
    
    public delegate void Function(byte[] bytes);

    private Function[] sceneFunctions = new Function[1];

    private void Awake()
    {
        sceneFunctions[0] = SealGame_Scene.Instance.SetCardImageSeal;
    }

    public Function GetFunctionByPacket(byte[] bytes)
    {
        Function[] targetClass;
        
        switch (bytes[2])
        {
            case 0:
                targetClass = sceneFunctions;
                break;
            default:
                targetClass = sceneFunctions;
                break;
        }

        return targetClass[bytes[3]];
    }
    
    public byte[] GeneratePacket(byte targetDeviceNum, byte targetClass, byte targetFunction, byte[] param)
    {
        List<byte> dataList = new List<byte>();
        
        dataList.Add((byte)NetworkManager.Instance.myDeviceData.indexOfDeviceList);

        switch (NetworkManager.Instance.connectType)
        {
            case Define.ConnectType.Server:
                dataList.Add(1);
                break;
            case Define.ConnectType.Client:
                dataList.Add(0);
                break;
        }

        dataList.Add(targetClass);
        dataList.Add(targetFunction);
        dataList.AddRange(param);
        
        byte[] bytes = dataList.ToArray();
        return bytes;
    }

    public void ExecuteFuncByPacket(byte[] bytes)
    {
        // 아래 코드 불필요
        
        // // 서버는 본인이 받을 데이터가 아니면 목표 클라이언트에게 전송
        // // 클라이언트는 본인이 받을 데이터가 아니면 에러로그
        // switch (NetworkManager.Instance.connectType)
        // {
        //     case Define.ConnectType.Server:
        //         if (bytes[1] != NetworkManager.Instance.myDeviceData.indexOfDeviceList)
        //         {
        //             Networking.NetworkDevice targetDevice = NetworkManager.Instance.connectedDeviceList[bytes[1]];
        //             yield return NetworkManager.Instance.ExecuteFuncOnTargetDevice(bytes);
        //         }
        //         break;
        //     case Define.ConnectType.Client:
        //         DebugText.Instance.AddText("Network Error");
        //         break;
        // }
        
        // 내가 받을 데이터라면 수행한다.
        Function targetFunction = GetFunctionByPacket(bytes);
        targetFunction(bytes);
    }
}
