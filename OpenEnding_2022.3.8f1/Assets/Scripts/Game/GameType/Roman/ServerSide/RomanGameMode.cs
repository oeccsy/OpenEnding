using System.Collections;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
using Shatalmic;
using UnityEngine;

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
        
        
        private void Start()
        {
            NetworkManager.Instance.clientSidePacketHandler = new ClientSide.RomanPacketHandler();
            NetworkManager.Instance.serverSidePacketHandler = new RomanPacketHandler();
        }
        
        protected override IEnumerator GameRoutine()
        {
            yield return InitDeviceOwnCard();
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
                DebugCanvas.Instance.AddText(device.Name + " Start");
                yield return new WaitForSecondsRealtime(0.5f);
                yield return NetworkManager.Instance.SendBytesToTargetDevice(device, new byte[] { 10, 0, (byte)cardType });
            }
        }

        public void FlipCard(CardType cardType, Define.DisplayedFace face)
        {
            cardContainer.SetCardFace(cardType, face);

            if (face == Define.DisplayedFace.Head)
            {
                var flippedCard = cardContainer.GetCard(cardType);
            
                if(flippedCard is IFlipAbility card) card.FlipAbility();
                OnCardFlipped?.Invoke(cardType);    
            }
        }
        
        public void ShakeCard(CardType cardType)
        {
            var shakenCard = cardContainer.GetCard(cardType);
            
            if(shakenCard is IShakeAbility card) card.ShakeAbility();
            OnCardShaken?.Invoke(cardType);
            
            var newCard = cardContainer.ReplaceCard(cardType);

            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(newCard.device, new byte[] { 10, 0, (byte)newCard.cardType }));
        }

        public void DiscoverCard(CardType cardType)
        {
            var discoveredCard = cardContainer.GetCard(cardType);
            
            if(discoveredCard is IDiscoveredAbility card) card.DiscoveredAbility();
            OnCardDiscovered?.Invoke(cardType);
        }

        public void Victory()
        {
            DebugCanvas.Instance.AddText("게임 종료!");
            GameOver();
            OnGameOver?.Invoke();
        }
    }
}