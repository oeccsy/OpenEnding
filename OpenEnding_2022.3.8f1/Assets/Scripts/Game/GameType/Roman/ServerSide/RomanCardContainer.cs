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
            availableCards.Add(CardType.Star, new Star());
            availableCards.Add(CardType.Telescope, new Telescope());
            availableCards.Add(CardType.Artwork, new Artwork());
            availableCards.Add(CardType.Sprout, new Sprout());
            availableCards.Add(CardType.RoleModel, new RoleModel());
        }

        public void UseCard(CardType cardType, ColorPalette.ColorName deviceColor)
        {
            if (!availableCards.ContainsKey(cardType)) return;
            
            RomanCard cardData = availableCards[cardType];
            availableCards.Remove(cardType);

            cardData.deviceColor = deviceColor;
            usedCards.Add(cardType, cardData);
            
            cardData.OnEnterField();
        }

        public RomanCard ReplaceCard(CardType cardType)
        {
            if (!usedCards.ContainsKey(cardType)) return null;

            List<RomanCard> availableCardList = availableCards.Values.ToList();
            RomanCard newCard = availableCardList[Random.Range(0, availableCardList.Count)];
            availableCards.Remove(newCard.cardType);
            
            RomanCard prevCard = usedCards[cardType];
            usedCards.Remove(cardType);
            
            newCard.deviceColor = prevCard.deviceColor;
            prevCard.deviceColor = ColorPalette.ColorName.DeviceDefault;
            
            usedCards.Add(newCard.cardType, newCard);
            availableCards.Add(cardType, prevCard);
            
            prevCard.OnExitField();
            newCard.OnEnterField();

            return newCard;
        }

        public RomanCard GetCard(CardType cardType)
        {
            if (!usedCards.ContainsKey(cardType)) return null;
            
            return usedCards[cardType];
        }
        
        public List<T> GetCards<T>()
        {
            List<T> cards = new List<T>();

            foreach (var cardData in usedCards)
            {
                if (cardData.Value is T card)
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

        public bool IsAllHide()
        {
            int cardCount = usedCards.Count;
            int hideCount = usedCards.Count(cardData => cardData.Value.displayedFace == Define.DisplayedFace.Tail);
                
            return hideCount == cardCount;
        }
    }
}