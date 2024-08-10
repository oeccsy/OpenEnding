using System;
using System.Collections.Generic;
using Game.Manager.GameManage;
using Shatalmic;
using UnityEditor;
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
    public Fairytale_GameState GameState => GameManager.Instance.GameState as Fairytale_GameState;
    public GameFlow GameFlow => GameManager.Instance.GameFlow;

    private delegate void Function(byte[] bytes);
    private Dictionary<Tuple<byte,byte>, Function> _funcDict;


    protected override void Awake()
    {
        base.Awake();
        
        _funcDict = new Dictionary<Tuple<byte,byte>, Function>
        {
            {Tuple.Create<byte, byte>(0, 0), (bytes) => FairytaleScene.TheHareAndTheTortoise()},
            {Tuple.Create<byte, byte>(0, 1), (bytes) => FairytaleScene.ThereAreAlwaysMemos()},
            {Tuple.Create<byte, byte>(0, 2), (bytes) => FairytaleScene.SetSceneGrayScale()},
            {Tuple.Create<byte, byte>(0, 3), (bytes) => FairytaleScene.ShowSuccessSceneUI()},
            {Tuple.Create<byte, byte>(0, 4), (bytes) => FairytaleScene.ShowResultPopup()},
            {Tuple.Create<byte, byte>(0, 5), (bytes) => FairytaleScene.HideResultPopup()},

            {Tuple.Create<byte, byte>(1, 0), (bytes) => CardContainer.SetCardHead((ColorPalette.ColorName)bytes[0])},
            {Tuple.Create<byte, byte>(1, 1), (bytes) => CardContainer.SetCardTail((ColorPalette.ColorName)bytes[0])},
            
            {Tuple.Create<byte, byte>(2, 0), (bytes) => OwnCard.StoryUnfoldsByTimeStep(bytes[0])},
            {Tuple.Create<byte, byte>(2, 1), (bytes) => OwnCard.InitCardStory(3, bytes[0])},
            {Tuple.Create<byte, byte>(2, 2), (bytes) => OwnCard.GiveUp()},

            {Tuple.Create<byte, byte>(3, 0), (bytes) => DeviceUtils.Vibrate()},
            
            {Tuple.Create<byte, byte>(4, 0), (bytes) => GameState.SetGameState(bytes[0], bytes[1])},

            {Tuple.Create<byte, byte>(5, 0), (bytes) => GameFlow.LoadScene(Define.SceneType.ConnectScene)}
        };
    }

    public void ExecuteActionByPacket(string clientName, string characteristic, byte[] bytes)
    {
        var funcKey = Tuple.Create<byte, byte>(bytes[0], bytes[1]);
        var targetFunction = _funcDict[funcKey];

        List<Byte> byteList = new List<byte>(bytes);
        targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
    }

    public void ExecuteActionByPacket(Networking.NetworkDevice device, string characteristic, byte[] bytes)
    {
        var funcKey = Tuple.Create<byte, byte>(bytes[0], bytes[1]);
        var targetFunction = _funcDict[funcKey];

        List<Byte> byteList = new List<byte>(bytes);
        targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
    }
}