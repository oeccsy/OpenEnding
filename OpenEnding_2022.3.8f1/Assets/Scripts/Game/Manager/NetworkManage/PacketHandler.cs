using System;
using System.Collections.Generic;
using Game.Manager.GameManage;
using Shatalmic;

namespace Game.Manager.NetworkManage
{
    public abstract class PacketHandler
    {
        protected Dictionary<Tuple<byte, byte>, Action<byte[]>> _funcDict;
        
        protected void ExecuteActionByPacket(Networking.NetworkDevice device, string characteristic, byte[] bytes)
        {
            var funcKey = Tuple.Create<byte, byte>(bytes[0], bytes[1]);
            var targetFunction = _funcDict[funcKey];

            List<Byte> byteList = new List<byte>(bytes);
            targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
        }
        
        protected void ExecuteActionByPacket(string clientName, string characteristic, byte[] bytes)
        {
            var funcKey = Tuple.Create<byte, byte>(bytes[0], bytes[1]);
            var targetFunction = _funcDict[funcKey];

            List<Byte> byteList = new List<byte>(bytes);
            targetFunction.Invoke(byteList.GetRange(2, byteList.Count - 2).ToArray());
        }
    }
}