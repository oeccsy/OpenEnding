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
        private RomanGameScene RomanGameScene => (RomanGameScene)GameManager.Instance.GameScene;
        private RomanGameState RomanGameState => (RomanGameState)GameManager.Instance.GameState;
        private RomanGameMode RomanGameMode => GameManager.Instance.GameMode as RomanGameMode;

        protected override void Awake()
        {
            base.Awake();

            _instanceDict = new Dictionary<Type, object>
            {
                {typeof(RomanGameScene), RomanGameScene},
                {typeof(RomanGameState), RomanGameState},
                {typeof(DeviceUtils), null},
                {typeof(RomanGameMode), RomanGameMode}
            };
        }
    }
}