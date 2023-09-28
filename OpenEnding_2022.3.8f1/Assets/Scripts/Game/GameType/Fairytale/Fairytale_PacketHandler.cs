using System;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class Fairytale_PacketHandler : Singleton<Fairytale_PacketHandler>
{
    // 데이터 포맷
    
    // index 0 : 실행할 로직의 Class
    // index 1 : 실행할 로직
    // index 2 : 실행할 로직의 param
    // . . .
    
    private Fairytale_Scene _fairytaleScene;
    private Fairytale_CardContainer _cardContainer;
    public Fairytale_Card ownCard;
    
    private Dictionary<byte, Function[]> _classDict;
    
    private delegate void Function(byte[] bytes);
    private Function[] _sceneFunctions;
    private Function[] _cardContainerFunctions;
    private Function[] _cardFunctions;

    protected override void Awake()
    {
        base.Awake();
        
        _fairytaleScene = Fairytale_Scene.Instance;
        _cardContainer = GameObject.Find("FairytaleManager").GetComponent<Fairytale_GameMode>().cardContainer;

        _sceneFunctions = new Function[]
        {
            (bytes) => _fairytaleScene.TheHareAndTheTortoise(),
            (bytes) => _fairytaleScene.TheNumber(),
            (bytes) => _fairytaleScene.ShowPlayerCard(),
            (bytes) => _fairytaleScene.SetSceneGrayScale()
        };

        _cardContainerFunctions = new Function[]
        {
            (bytes) => _cardContainer.SetCardHead((ColorPalette.ColorName)bytes[0]),
            (bytes) => _cardContainer.SetCardTail((ColorPalette.ColorName)bytes[0])
        };
        
        _cardFunctions = new Function[]
        {
            (bytes) => { if(ownCard != null) ownCard.Vibrate(); },
            (bytes) => { if (ownCard != null) ownCard.ShowNextStep(); }
        };
            
        _classDict = new Dictionary<byte, Function[]>
        {
            {0, _sceneFunctions},
            {1, _cardContainerFunctions},
            {2, _cardFunctions}
        };
        
        NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
        NetworkManager.Instance.OnReceiveDataFromClient = ExecuteActionByPacket;
    }

    public void ExecuteActionByPacket(string clientName, string characteristic, byte[] bytes)
    {
        $"Execute {bytes[0]}{bytes[1]}".Log();
        
        var targetClass = _classDict[bytes[(byte)Define.PacketIndex.Class]];
        var targetFunction = targetClass[bytes[(byte)Define.PacketIndex.Function]];

        List<Byte> byteList = new List<byte>(bytes);
        targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
    }

    public void ExecuteActionByPacket(Networking.NetworkDevice device, string characteristic, byte[] bytes)
    {
        var targetClass = _classDict[bytes[(byte)Define.PacketIndex.Class]];
        var targetFunction = targetClass[bytes[(byte)Define.PacketIndex.Function]];

        List<Byte> byteList = new List<byte>(bytes);
        targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
    }
}