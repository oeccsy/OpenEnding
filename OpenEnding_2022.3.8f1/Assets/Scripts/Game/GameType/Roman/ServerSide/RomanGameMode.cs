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
        
        public ColorPalette.ColorName startPlayer;
        public ColorPalette.ColorName curPlayer;
        public int turnCount = 0;
        public GameStep curStep = GameStep.InitGame;
        
        private void Start()
        {
            NetworkManager.Instance.clientSidePacketHandler = new ClientSide.RomanPacketHandler();
            NetworkManager.Instance.serverSidePacketHandler = new RomanPacketHandler();
        
            GameManager.Instance.GameState = new RomanGameState();
        }
        
        protected override IEnumerator GameRoutine()
        {
            yield return new WaitForSeconds(3f);
            yield return new WaitUntil(() => curStep == GameStep.InitGame);
            yield return InitDeviceOwnCard();
            yield return SelectStartPlayer();
            yield return RequestFlipToTail();
            yield return new WaitUntil(() => cardContainer.IsAllHide());
            
            while (curStep != GameStep.GameOver)
            {
                yield return NotifyNewPlayerTurn();
                yield return new WaitUntil(() => curStep == GameStep.SelectCard);
                yield return new WaitUntil(() => curStep == GameStep.FlipOrShake);
                yield return new WaitUntil(() => curStep == GameStep.ShowCard);
                yield return new WaitUntil(() => curStep == GameStep.HideCard);

                turnCount++;
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
                yield return NetworkManager.Instance.SendBytesToTargetDevice(device, new byte[] { 10, 2, (byte)cardType });
            }
        }
        
        private IEnumerator SelectStartPlayer()
        {
            int startPlayerNumber = Random.Range(0, base.playerCount);
            
            startPlayer = (ColorPalette.ColorName)startPlayerNumber;
            Networking.NetworkDevice targetDevice = null;

            foreach (var device in NetworkManager.Instance.connectedDeviceList)
            {
                if (device.colorOrder != startPlayerNumber) continue;
                targetDevice = device;
                break;
            }

            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(targetDevice, new byte[] { 10, 0 }));
            yield return new WaitForSecondsRealtime(5f);
        }

        private IEnumerator RequestFlipToTail()
        {
            yield return NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 10, 1 });
        }

        private IEnumerator NotifyNewPlayerTurn()
        {
            curPlayer = (ColorPalette.ColorName)(((int)startPlayer + turnCount) % base.playerCount);
            
            yield return NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 11, 0 });
            
            curStep = GameStep.SelectCard;
            yield return null;
        }
        
        public void FlipCard(CardType cardType, Define.DisplayedFace face)
        {
            $"Flip {cardType} to {face}".Log();
            cardContainer.SetCardFace(cardType, face);
        
            if (curStep == GameStep.SelectCard && face == Define.DisplayedFace.Stand)
            {
                Networking.NetworkDevice targetDevice = cardContainer.GetCard(cardType).device;
                StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(targetDevice, new byte[] {11, 1}));
                
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

            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(newCard.device, new byte[] { 11, 2, (byte)newCard.cardType }));
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
            GameOver();
            OnGameOver?.Invoke();
            
            StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 12, 0, (byte)curPlayer })); 
        }
    }
}