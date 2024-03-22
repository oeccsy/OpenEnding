using System;
using System.Collections.Generic;
using Game.Manager.GameManage;
using Game.Manager.NetworkManage;

namespace Game.GameType.Roman.ServerSide
{
    public class RomanPacketHandler : ServerSidePacketHandler
    {
        public RomanPacketHandler()
        {
            _funcDict = new Dictionary<Tuple<byte, byte>, Action<byte[]>>
            {
                {Tuple.Create<byte, byte>(0, 0), (bytes) => (GameManager.Instance.GameMode as RomanGameMode)?.SetCardFace((CardType)bytes[0], (Define.DisplayedFace)bytes[1])}
            };
        }
    }
}