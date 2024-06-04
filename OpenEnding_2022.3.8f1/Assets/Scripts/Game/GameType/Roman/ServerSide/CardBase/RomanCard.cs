using System;
using Shatalmic;

namespace Game.GameType.Roman.ServerSide.CardBase
{
    [Serializable]
    public abstract class RomanCard
    {
        public CardType cardType = CardType.None;
        public string cardName;
        public string cardDesc;
        
        public Define.DisplayedFace displayedFace = Define.DisplayedFace.None;
        public Networking.NetworkDevice device;

        public delegate void CardEventHandler(RomanCard card);
        public static event CardEventHandler OnCardFlipped;
        public static event CardEventHandler OnCardDiscovered;
        public static event CardEventHandler OnCardShaken;

        public abstract void OnEnterField();
        public abstract void OnExitField();
    }
}