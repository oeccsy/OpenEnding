using System;
using System.Collections.Generic;
using Game.GameType.Roman.ServerSide;
using Game.Manager.GameManage;
using Game.Manager.NetworkManage;

namespace Game.GameType.Roman.ClientSide
{
    public class RomanPacketHandler : ClientSidePacketHandler
    {
        public RomanPacketHandler()
        {
            _funcDict = new Dictionary<Tuple<byte, byte>, Action<byte[]>>
            {
                {Tuple.Create<byte, byte>(10, 0), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.ShowStartPlayerPopup()},
                {Tuple.Create<byte, byte>(10, 1), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.ShowRequestFlipToTailPopup()},
                {Tuple.Create<byte, byte>(10, 2), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard((CardType)bytes[0])},
                
                {Tuple.Create<byte, byte>(11, 0), (bytes) => DeviceUtils.Vibrate()},
                {Tuple.Create<byte, byte>(11, 1), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.ShowCard()},
                {Tuple.Create<byte, byte>(11, 2), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard((CardType)bytes[0])},
                
                {Tuple.Create<byte, byte>(12, 0), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.ShowResultPopup()}
            };
            
            NetworkManager.Instance.clientSidePacketHandler = this;
            NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
        }
    }
}