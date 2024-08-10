using System;
using System.Collections;
using Game.GameType.Roman.ClientSide;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
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

        private int _shakeLimit = 1;
        private int _shakeCount = 0;
        
        private void Start()
        {
            GameManager.Instance.GameState = new RomanGameState();
        }
        
        protected override IEnumerator GameRoutine()
        {
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => curStep == GameStep.InitGame);
            InitDeviceOwnCard();
            yield return SelectStartPlayer();
            RequestFlipToTail();
            yield return new WaitUntil(() => cardContainer.IsAllHide());
            
            while (curStep != GameStep.GameOver)
            {
                yield return NotifyNewPlayerTurn();
                yield return new WaitUntil(() => curStep == GameStep.SelectCard);
                yield return new WaitUntil(() => curStep == GameStep.FlipOrShake);
                _shakeCount = 0;
                
                yield return new WaitUntil(() => curStep == GameStep.ShowCard);
                yield return WaitForCardCheck();
                yield return new WaitUntil(() => curStep == GameStep.HideCard);
                yield return new WaitUntil(() => cardContainer.IsAllHide());

                turnCount++;
            }
        }

        private void InitDeviceOwnCard()
        {
            var randomNumbers = Utils.GetCombinationInt(1, 5, 5); // TODO
            Utils.ShuffleList(randomNumbers);

            for (int i = 0; i < NetworkManager.Instance.connectedDeviceList.Count; i++)
            {
                CardType cardType = (CardType)randomNumbers[i];
                ColorPalette.ColorName deviceColor = NetworkManager.Instance.connectedDeviceList[i];
                
                cardContainer.UseCard(cardType, deviceColor);

                NetworkManager.Instance.ClientRpcCall(deviceColor, typeof(RomanGameScene), "CreateCard", cardType);
            }
        }
        
        private IEnumerator SelectStartPlayer()
        {
            ColorPalette.ColorName startPlayerColor = (ColorPalette.ColorName)Random.Range(0, base.playerCount);

            NetworkManager.Instance.ClientRpcCall(startPlayerColor, typeof(RomanGameScene), "ShowStartPlayerPopup", null);
            yield return new WaitForSecondsRealtime(5f);
        }

        private void RequestFlipToTail()
        {
            NetworkManager.Instance.ClientRpcCall(typeof(RomanGameScene), "ShowRequestFlipToTailPopup", null);
        }

        private IEnumerator NotifyNewPlayerTurn()
        {
            yield return new WaitForSeconds(1f);
            
            curPlayer = (ColorPalette.ColorName)(((int)startPlayer + turnCount) % base.playerCount);
            NetworkManager.Instance.ClientRpcCall(typeof(RomanGameState), "SynchronizeCurPlayer", curPlayer);
            NetworkManager.Instance.ClientRpcCall(typeof(DeviceUtils), "Vibrate", null);
            
            curStep = GameStep.SelectCard;
            $"{curStep.ToString()}".Log();
            NetworkManager.Instance.ClientRpcCall(typeof(RomanGameState), "SynchronizeGameStep", curStep);
        }
        
        private IEnumerator WaitForCardCheck()
        {
            yield return new WaitForSecondsRealtime(3f);
            curStep = GameStep.HideCard;
            $"{curStep.ToString()}".Log();
            NetworkManager.Instance.ClientRpcCall(typeof(RomanGameState), "SynchronizeGameStep", curStep);
        }
        
        public void FlipCard(CardType cardType, Define.DisplayedFace face)
        {
            $"Flip {cardType} to {face}".Log();
            cardContainer.SetCardFace(cardType, face);
        
            if (curStep == GameStep.SelectCard && face == Define.DisplayedFace.Stand)
            {
                ColorPalette.ColorName targetDeviceColor = cardContainer.GetCard(cardType).deviceColor;
                NetworkManager.Instance.ClientRpcCall(targetDeviceColor, typeof(RomanGameScene), "ShowCard", null);
                
                curStep = GameStep.FlipOrShake;
                $"{curStep.ToString()}".Log();
                NetworkManager.Instance.ClientRpcCall(targetDeviceColor, typeof(RomanGameState), "SynchronizeGameStep", curStep);
                return;
            }
        
            if (curStep == GameStep.FlipOrShake && face == Define.DisplayedFace.Head)
            {
                var flippedCard = cardContainer.GetCard(cardType);
                if(flippedCard is IFlipAbility card) card.FlipAbility();
                OnCardFlipped?.Invoke(cardType);

                curStep = GameStep.ShowCard;
                $"{curStep.ToString()}".Log();
                NetworkManager.Instance.ClientRpcCall(typeof(RomanGameState), "SynchronizeGameStep", curStep);
                return;
            }
            
            if (curStep == GameStep.HideCard && face == Define.DisplayedFace.Tail)
            {
                curStep = GameStep.SelectCard;
                $"{curStep.ToString()}".Log();
                NetworkManager.Instance.ClientRpcCall(typeof(RomanGameState), "SynchronizeGameStep", curStep);
                return;
            }
        }

        public void ShakeCard(CardType cardType)
        {
            $"Shake {cardType}".Log();
            if (curStep != GameStep.FlipOrShake) return;
            if (_shakeCount >= _shakeLimit) return;

            _shakeCount++;
            var shakenCard = cardContainer.GetCard(cardType);
            
            if(shakenCard is IShakeAbility card) card.ShakeAbility();
            OnCardShaken?.Invoke(cardType);
            
            var newCard = cardContainer.ReplaceCard(cardType);

            NetworkManager.Instance.ClientRpcCall(newCard.deviceColor, typeof(RomanGameScene), "ReplaceCard", newCard.cardType);
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
            
            NetworkManager.Instance.ClientRpcCall(typeof(RomanGameScene), "ShowResultPopup", null);
        }
        
        public void Victory(ColorPalette.ColorName playerColor)
        {
            GameOver();
            OnGameOver?.Invoke();
            
            NetworkManager.Instance.ClientRpcCall(typeof(RomanGameScene), "ShowResultPopup", null); 
        }
    }
}