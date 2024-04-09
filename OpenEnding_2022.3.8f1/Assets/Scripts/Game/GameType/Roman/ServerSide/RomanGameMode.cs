using System;
using System.Collections;
using Game.GameType.Roman.ClientSide;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
using Shatalmic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GameType.Roman.ServerSide
{
    public class RomanGameMode : GameMode
    {
        public RomanCardContainer cardContainer = new RomanCardContainer();

        public delegate void CardEventHandler(CardType cardType);
        public event CardEventHandler OnCardFlipped;
        public event CardEventHandler OnCardDiscovered;
        public event CardEventHandler OnCardShaken;
        
        public delegate void GameEventHandler();
        public event GameEventHandler OnGameOver;
        
        public ColorPalette.ColorName curPlayer;
        public GameStep curStep = GameStep.InitGame;
        
        private void Start()
        {
            NetworkManager.Instance.clientSidePacketHandler = new ClientSide.RomanPacketHandler();
            NetworkManager.Instance.serverSidePacketHandler = new RomanPacketHandler();

            GameManager.Instance.GameState = new RomanGameState();
        }
        
        protected override IEnumerator GameRoutine()
        {
            yield return new WaitUntil(() => curStep == GameStep.InitGame);
            yield return InitDeviceOwnCard();
            yield return InitPlayerTurn();
            yield return new WaitUntil(() => cardContainer.IsAllHide());
            
            while (curStep != GameStep.GameOver)
            {
                yield return NotifyNewPlayerTurn();
                yield return new WaitUntil(() => curStep == GameStep.SelectCard);
                yield return new WaitUntil(() => curStep == GameStep.FlipOrShake);
                yield return new WaitUntil(() => curStep == GameStep.ShowCard);
                yield return new WaitUntil(() => curStep == GameStep.HideCard);
                
                curPlayer = (ColorPalette.ColorName)(((int)curPlayer + 1) % playerCount);
            }
        }

        private IEnumerator InitDeviceOwnCard()
        {
            var randomNumbers = Utils.GetCombinationInt(1, 5, 3);
            Utils.ShuffleList(randomNumbers);

            for (int i = 0; i < NetworkManager.Instance.connectedDeviceList.Count; i++)
            {
                CardType cardType = (CardType)randomNumbers[i];
                Networking.NetworkDevice device = NetworkManager.Instance.connectedDeviceList[i];
                
                cardContainer.UseCard(cardType, device);

                yield return new WaitForSecondsRealtime(0.5f);
                yield return NetworkManager.Instance.SendBytesToTargetDevice(device, new byte[] { 10, 0, (byte)cardType });
            }
        }

        private IEnumerator InitPlayerTurn()
        {
            curPlayer = (ColorPalette.ColorName)Random.Range(0, NetworkManager.Instance.connectedDeviceList.Count);
            yield return null;
        }

        private IEnumerator NotifyNewPlayerTurn()
        {
            curStep = GameStep.SelectCard;
            yield return null;
        }
        
        public void FlipCard(CardType cardType, Define.DisplayedFace face)
        {
            $"Flip {cardType} to {face}".Log();
            cardContainer.SetCardFace(cardType, face);
        
            if (curStep == GameStep.SelectCard && face == Define.DisplayedFace.Stand)
            {
                curStep = GameStep.FlipOrShake;
                return;
            }
        
            if (curStep == GameStep.FlipOrShake && face == Define.DisplayedFace.Head)
            {
                var flippedCard = cardContainer.GetCard(cardType);
                if(flippedCard is IFlipAbility card) card.FlipAbility();
                OnCardFlipped?.Invoke(cardType);

                curStep = GameStep.ShowCard;
                return;
            }
            
            if (curStep == GameStep.HideCard && face == Define.DisplayedFace.Tail)
            {
                curStep = GameStep.SelectCard;
                return;
            }
        }

        public void ShakeCard(CardType cardType)
        {
            $"Shake {cardType}".Log();
            if (curStep != GameStep.FlipOrShake) return;
            
            var shakenCard = cardContainer.GetCard(cardType);
            
            if(shakenCard is IShakeAbility card) card.ShakeAbility();
            OnCardShaken?.Invoke(cardType);
            
            var newCard = cardContainer.ReplaceCard(cardType);

            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(newCard.device, new byte[] { 10, 1, (byte)newCard.cardType }));
        }
        
        public void DiscoverCard(CardType cardType)
        {
            $"Discover {cardType}".Log();
            var discoveredCard = cardContainer.GetCard(cardType);
            
            if(discoveredCard is IDiscoveredAbility card) card.DiscoveredAbility();
            OnCardDiscovered?.Invoke(cardType);
        }

        public void Victory()
        {
            DebugCanvas.Instance.AddText("게임 종료!");
            GameOver();
            OnGameOver?.Invoke();

            StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 11, 0 }));
        }
    }
}