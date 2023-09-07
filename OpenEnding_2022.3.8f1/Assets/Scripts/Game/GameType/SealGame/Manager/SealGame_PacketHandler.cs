using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class SealGame_PacketHandler : Singleton<SealGame_PacketHandler>
{
    // 데이터 포맷

    // index 0 : 실행할 로직의 Class
    // index 1 : 실행할 로직
    // index 2 : 실행할 로직의 param
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
        
        dataList.Add((byte)NetworkManager.Instance.myDeviceData.deviceListOrder);

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
        Function targetFunction = GetFunctionByPacket(bytes);
        targetFunction(bytes);
    }
}
