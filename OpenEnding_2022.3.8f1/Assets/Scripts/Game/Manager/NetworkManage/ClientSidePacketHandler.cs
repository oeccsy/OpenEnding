using System;
using System.Collections.Generic;
using Game.Manager.GameManage;
using Shatalmic;

namespace Game.Manager.NetworkManage
{
    public class ClientSidePacketHandler : PacketHandler
    {
        public ClientSidePacketHandler()
        {
            NetworkManager.Instance.clientSidePacketHandler = this;
            NetworkManager.Instance.OnReceiveDataFromServer = ExecuteActionByPacket;
        }
    }
}