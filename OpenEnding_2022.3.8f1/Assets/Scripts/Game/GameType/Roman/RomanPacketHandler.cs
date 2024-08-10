using System;
using System.Collections.Generic;
using Game.GameType.Roman.ClientSide;
using Game.GameType.Roman.ServerSide;
using Game.Manager.GameManage;
using Game.Manager.NetworkManage;
using UnityEngine;

namespace Game.GameType.Roman
{
    public class RomanPacketHandler : PacketHandler
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _instanceDict = new Dictionary<Type, object>
            {
                {typeof(RomanGameScene), GameManager.Instance.GameScene as RomanGameScene},
                {typeof(RomanGameState), GameManager.Instance.GameState as RomanGameState},
                {typeof(DeviceUtils), null},
                {typeof(RomanGameMode), GameManager.Instance.GameMode as RomanGameMode}
            };
        }
    }
}