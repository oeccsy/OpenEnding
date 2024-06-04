using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
using UnityEngine;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class Lost : RomanCard, IFlipAbility
    {
        public Lost()
        {
            cardType = CardType.F;
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
        
        public override void OnEnterField() {}
        public override void OnExitField() {}
    }
}