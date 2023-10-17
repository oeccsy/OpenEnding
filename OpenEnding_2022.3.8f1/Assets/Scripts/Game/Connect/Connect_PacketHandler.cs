using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Connect_PacketHandler : Singleton<Connect_PacketHandler>
{
    // 데이터 포맷
    
    // index 0 : 실행할 로직의 Class
    // index 1 : 실행할 로직
    // index 2 : 실행할 로직의 param
    // . . .

    private Connect_Scene ConnectScene => Connect_Scene.Instance;
    private GameFlow GameFlow => GameManager.Instance.GameFlow;
    
    private delegate void Function(byte[] bytes);
    private Dictionary<Tuple<byte,byte>, Function> _funcDict;

    protected override void Awake()
    {
        base.Awake();
        
        _funcDict = new Dictionary<Tuple<byte,byte>, Function>
        {
            {Tuple.Create<byte, byte>(0, 0), (bytes) => ConnectScene.SynchronizeDevicesWithAnimation(bytes)},
            
            {Tuple.Create<byte, byte>(1, 0), (bytes) => GameFlow.LoadFairytaleScene()}
        };

        NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
    }

    public void ExecuteActionByPacket(string clientName, string characteristic, byte[] bytes)
    {
        var funcKey = Tuple.Create<byte, byte>(bytes[0], bytes[1]);
        var targetFunction = _funcDict[funcKey];

        List<byte> byteList = new List<byte>(bytes);
        targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
    }
}
