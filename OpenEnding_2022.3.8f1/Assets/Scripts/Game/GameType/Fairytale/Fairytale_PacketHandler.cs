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
    
    private Fairytale_Scene FairytaleScene => Fairytale_Scene.Instance;
    private Fairytale_CardContainer CardContainer => (GameManager.Instance.GameMode as Fairytale_GameMode)?.cardContainer;
    public Fairytale_Card OwnCard => Fairytale_Scene.Instance.card; 
    
    private Dictionary<byte, Function[]> _classDict;
    
    private delegate void Function(byte[] bytes);
    private Function[] _sceneFunctions;
    private Function[] _cardContainerFunctions;
    private Function[] _cardFunctions;
    private Function[] _utilFunctions;

    protected override void Awake()
    {
        base.Awake();

        _sceneFunctions = new Function[]
        {
            (bytes) => FairytaleScene.TheHareAndTheTortoise(),
            (bytes) => FairytaleScene.ThereAreAlwaysMemos(),
            (bytes) => FairytaleScene.ShowPlayerCard(),
            (bytes) => FairytaleScene.SetSceneGrayScale()
        };

        _cardContainerFunctions = new Function[]
        {
            (bytes) => CardContainer.SetCardHead((ColorPalette.ColorName)bytes[0]),
            (bytes) => CardContainer.SetCardTail((ColorPalette.ColorName)bytes[0])
        };
        
        _cardFunctions = new Function[]
        {
            (bytes) => DeviceUtils.Vibrate(),
            (bytes) => OwnCard.StoryUnfoldsByTimeStep(bytes[0]),
            (bytes) =>
            {
                OwnCard.cardData.goal = 3;
                OwnCard.cardData.runningTime = bytes[0];
                OwnCard.cardData.storyLine = Fairytale_StorylineFactory.GetStoryLine(3, bytes[0]);
            }
        };

        _utilFunctions = new Function[]
        {
            bytes => DeviceUtils.Vibrate()
        };
            
        _classDict = new Dictionary<byte, Function[]>
        {
            {0, _sceneFunctions},
            {1, _cardContainerFunctions},
            {2, _cardFunctions},
            {3, _utilFunctions}
        };
        
        NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
        NetworkManager.Instance.OnReceiveDataFromClient = ExecuteActionByPacket;
    }

    public void ExecuteActionByPacket(string clientName, string characteristic, byte[] bytes)
    {
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