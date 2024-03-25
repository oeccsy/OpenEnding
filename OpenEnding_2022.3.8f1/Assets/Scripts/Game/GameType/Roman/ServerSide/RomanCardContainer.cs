using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameType.Roman.ServerSide.Card;
using Game.GameType.Roman.ServerSide.CardBase;
using Shatalmic;
using Random = UnityEngine.Random;

namespace Game.GameType.Roman.ServerSide
{
    [Serializable]
    public class RomanCardContainer
    {
        private Dictionary<CardType, RomanCard> availableCards = new Dictionary<CardType, RomanCard>();
        private Dictionary<CardType, RomanCard> usedCards = new Dictionary<CardType, RomanCard>();

        public RomanCardContainer()
        {
            availableCards.Add(CardType.A, new A());
            availableCards.Add(CardType.B, new B());
            availableCards.Add(CardType.C, new C());
            availableCards.Add(CardType.D, new D());
            availableCards.Add(CardType.E, new E());
        }

        public void UseCard(CardType cardType, Networking.NetworkDevice device)
        {
            if (!availableCards.ContainsKey(cardType)) return;
            
            var cardData = availableCards[cardType];
            availableCards.Remove(cardType);

            cardData.device = device;
            usedCards.Add(cardType, cardData);
        }

        public void ReplaceCard(CardType cardType)
        {
            if (!usedCards.ContainsKey(cardType)) return;

            List<RomanCard> availableCardList = availableCards.Values.ToList();
            var newCard = availableCardList[Random.Range(0, availableCardList.Count)];
            availableCards.Remove(newCard.cardType);
            
            var prevCard = usedCards[cardType];
            usedCards.Remove(cardType);
            
            newCard.device = prevCard.device;
            prevCard.device = null;
            
            usedCards.Add(newCard.cardType, newCard);
            availableCards.Add(cardType, prevCard);
        }

        public RomanCard GetCard(CardType cardType)
        {
            if (!usedCards.ContainsKey(cardType)) return null;
            
            return usedCards[cardType];
        }
        
        public List<IGrowable> GetGrowableCards()
        {
            List<IGrowable> cards = new List<IGrowable>();

            foreach (var cardData in usedCards)
            {
                if (cardData.Value is IGrowable card)
                {
                    cards.Add(card);   
                }
            }

            return cards;
        }

        public void SetCardFace(CardType cardType, Define.DisplayedFace face)
        {
            if (!usedCards.ContainsKey(cardType)) return;

            var cardData = usedCards[cardType];
            cardData.displayedFace = face;
        }
    }
}