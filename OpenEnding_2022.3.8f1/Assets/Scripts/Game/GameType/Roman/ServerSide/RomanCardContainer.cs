using System;
using System.Collections.Generic;
using Game.GameType.Roman.Card;
using Shatalmic;
using Unity.VisualScripting;

namespace Game.GameType.Roman
{
    [Serializable]
    public class RomanCardContainer
    {
        public Dictionary<CardType, RomanCardData> availableCards = new Dictionary<CardType, RomanCardData>();
        public Dictionary<CardType, RomanCardData> usedCards = new Dictionary<CardType, RomanCardData>();

        public RomanCardContainer()
        {
            for (int i = 0; i < 5; i++)
            {
                var romanCardData = new RomanCardData
                {
                    cardType = (CardType)i
                };
                
                availableCards.Add(romanCardData.cardType, romanCardData);
            }
        }

        public void UseCard(CardType cardType, Networking.NetworkDevice device)
        {
            if (!availableCards.ContainsKey(cardType)) return;
            
            var cardData = availableCards[cardType];
            availableCards.Remove(cardType);

            cardData.networkDevice = device;
            usedCards.Add(cardType, cardData);
        }

        public void RetrieveCard(CardType cardType)
        {
            if (!usedCards.ContainsKey(cardType)) return;
            
            var cardData = usedCards[cardType];
            usedCards.Remove(cardType);

            cardData.networkDevice = null;
            availableCards.Add(cardType, cardData);
        }
    }
}