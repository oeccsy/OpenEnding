using System;
using System.Collections.Generic;
using Game.GameType.Roman;
using Game.GameType.Roman.ClientSide;
using Game.Manager.GameManage;

namespace Game.Manager.NetworkManage
{
    public class ClientSidePacketHandler : PacketHandler
    {
        public ClientSidePacketHandler()
        {
            _funcDict = new Dictionary<Tuple<byte, byte>, Action<byte[]>>
            {
                {Tuple.Create<byte, byte>(0, 0), (bytes) => (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard((CardType)bytes[0])}
            };
            
            NetworkManager.Instance.clientSidePacketHandler = this;
            NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
        }
    }
}