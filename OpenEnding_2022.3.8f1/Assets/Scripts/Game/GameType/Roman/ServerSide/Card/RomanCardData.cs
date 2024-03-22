using System;
using Shatalmic;

namespace Game.GameType.Roman.Card
{
    [Serializable]
    public class RomanCardData
    {
        public CardType cardType = CardType.None;
        public string cardName;
        public string cardDesc;
        
        public Define.DisplayedFace displayedFace = Define.DisplayedFace.Head;
        public Networking.NetworkDevice networkDevice;
    }
}