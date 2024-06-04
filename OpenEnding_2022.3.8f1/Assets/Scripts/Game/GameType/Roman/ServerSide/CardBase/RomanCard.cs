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

        public abstract void OnEnterField();
        public abstract void OnExitField();
    }
}