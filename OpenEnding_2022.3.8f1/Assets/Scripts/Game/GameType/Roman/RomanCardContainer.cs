using System;
using System.Collections.Generic;
using Game.GameType.Roman.Card;

namespace Game.GameType.Roman
{
    [Serializable]
    public class RomanCardContainer
    {
        public List<RomanCardData> cardList = new List<RomanCardData>();
    }
}