using System;
using System.Collections.Generic;
using Game.Manager.GameManage;
using Shatalmic;

namespace Game.Manager.NetworkManage
{
    public class ServerSidePacketHandler : PacketHandler
    {
        public ServerSidePacketHandler()
        {
            NetworkManager.Instance.serverSidePacketHandler = this;
            NetworkManager.Instance.OnReceiveDataFromClient = ExecuteActionByPacket;
        }
    }
}