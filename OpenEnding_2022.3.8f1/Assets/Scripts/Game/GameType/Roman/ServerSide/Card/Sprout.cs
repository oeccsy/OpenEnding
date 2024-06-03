using System;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
using Random = UnityEngine.Random;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class Sprout : RomanCard, IFlipAbility
    {
        public Sprout()
        {
            cardType = CardType.Sprout;
        }
        
        public void FlipAbility()
        {
            var gameMode = GameManager.Instance.GameMode as RomanGameMode;
            if (gameMode == null) return;

            var growableCards = gameMode.cardContainer.GetCards<IGrowable>();
            if (growableCards.Count == 0) return;

            int randomIndex = Random.Range(0, growableCards.Count);
            growableCards[randomIndex].Regress();
        }
    }
}