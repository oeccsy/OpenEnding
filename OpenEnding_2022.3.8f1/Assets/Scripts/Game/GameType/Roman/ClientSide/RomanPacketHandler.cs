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
                {Tuple.Create<byte, byte>(10, 0), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard((CardType)bytes[0])},
                {Tuple.Create<byte, byte>(10, 1), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard((CardType)bytes[0])}
            };
            
            NetworkManager.Instance.clientSidePacketHandler = this;
            NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
        }
    }
}