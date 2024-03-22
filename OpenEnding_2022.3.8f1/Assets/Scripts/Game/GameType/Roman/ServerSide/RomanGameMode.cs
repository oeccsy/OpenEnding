using System.Collections;
using Game.Manager.GameManage;
using Shatalmic;

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
            InitDeviceOwnCard();
            
            while (true)
            {
                yield return null;    
            }
        }

        private void InitDeviceOwnCard()
        {
            var randomNumbers = Utils.GetCombinationInt(1, 10, 2);
            Utils.ShuffleList(randomNumbers);

            for (int i = 0; i < NetworkManager.Instance.connectedDeviceList.Count; i++)
            {
                CardType cardType = (CardType)randomNumbers[i];
                Networking.NetworkDevice device = NetworkManager.Instance.connectedDeviceList[i];
                
                cardContainer.UseCard(cardType, device);
            }
        }

        public void FlipCard(CardType cardType)
        {
            var card = cardContainer.GetCard(cardType);
            card.FlipAbility();
            OnCardFlipped?.Invoke(cardType);
        }
        
        public void ShakeCard(CardType cardType)
        {
            var card = cardContainer.GetCard(cardType);
            card.ShakeAbility();
            OnCardShaken?.Invoke(cardType);
        }

        public void DiscoverCard(CardType cardType)
        {
            var card = cardContainer.GetCard(cardType);
            card.DiscoveredAbility();
            OnCardDiscovered?.Invoke(cardType);
        }
        
        public void SetCardFace(CardType cardType, Define.DisplayedFace face)
        {
            cardContainer.SetCardFace(cardType, face);
        }

        public void Victory()
        {
            GameOver();
            OnGameOver?.Invoke();
        }
    }
}