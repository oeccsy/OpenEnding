﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Fairytale_PacketHandler : Singleton<Fairytale_PacketHandler>
{
    // 데이터 포맷
    
    // index 0 : 실행할 로직의 Class
    // index 1 : 실행할 로직
    // index 2 : 실행할 로직의 param
    // . . .
    
    private Dictionary<byte, Function[]> _classDict;
    private delegate void Function(byte[] bytes);
    private Function[] _sceneFunctions;

    protected override void Awake()
    {
        base.Awake();
        
        _sceneFunctions = new Function[]
        {
            (bytes) => Fairytale_Scene.Instance.TheHareAndTheTortoise(),
            (bytes) => Fairytale_Scene.Instance.TheNumber(),
            (bytes) => Fairytale_Scene.Instance.ShowPlayerCard()
        };
        
        _classDict = new Dictionary<byte, Function[]>
        {
            {0, _sceneFunctions}
        };
        
        NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
    }
    
    public void ExecuteActionByPacket(string clientName, string characteristic, byte[] bytes)
    {
        var targetClass = _classDict[bytes[(byte)Define.PacketIndex.Class]];
        var targetFunction = targetClass[bytes[(byte)Define.PacketIndex.Function]];

        List<Byte> byteList = new List<byte>(bytes);
        targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
    }
}