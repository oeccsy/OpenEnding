﻿using Game.GameType.Roman.ServerSide;
using Game.Manager.GameManage;

namespace Game.GameType.Roman.ClientSide
{
    public class RomanPlayerController : PlayerController
    {
        protected override void Awake()
        {
            base.Awake();
            flip.OnFlipToHead += NotifyFlipToHead;
            flip.OnFlipToTail += NotifyFlipToTail;
            flip.OnStand += NotifyStand;
            shake.OnEveryShake += NotifyShake;
        }
        
        public void NotifyFlipToHead()
        {
            if ((GameManager.Instance.GameScene as RomanGameScene)?.card == null) return;
            
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            byte displayedFace = (byte)Define.DisplayedFace.Head;
            NetworkManager.Instance.ServerRpcCall(typeof(RomanGameMode), "FlipCard", cardType, displayedFace);
        }
        
        public void NotifyFlipToTail()
        {
            if ((GameManager.Instance.GameScene as RomanGameScene)?.card == null) return;
            
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            byte displayedFace = (byte)Define.DisplayedFace.Tail;
            NetworkManager.Instance.ServerRpcCall(typeof(RomanGameMode), "FlipCard", cardType, displayedFace);
        }

        public void NotifyStand()
        {
            if ((GameManager.Instance.GameScene as RomanGameScene)?.card == null) return;
            
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            byte displayedFace = (byte)Define.DisplayedFace.Stand;
            NetworkManager.Instance.ServerRpcCall(typeof(RomanGameMode), "FlipCard", cardType, displayedFace);
        }

        public void NotifyShake()
        {
            if ((GameManager.Instance.GameScene as RomanGameScene)?.card == null) return;
            
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            NetworkManager.Instance.ServerRpcCall(typeof(RomanGameMode), "ShakeCard", cardType);
        }
    }
}