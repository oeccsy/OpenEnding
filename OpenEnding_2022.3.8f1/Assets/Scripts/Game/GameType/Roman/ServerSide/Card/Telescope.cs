using System;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class Telescope : RomanCard, IFlipAbility
    {
        public Telescope()
        {
            cardType = CardType.Telescope;
        }
        
        public void FlipAbility()
        {
            var gameMode = GameManager.Instance.GameMode as RomanGameMode;
            var cards = gameMode.cardContainer.GetCards<RomanCard>();

            var targetCard = cards[UnityEngine.Random.Range(0, cards.Count)];
            gameMode.DiscoverCard(targetCard.cardType);
        }

        public override void OnEnterField() {}
        public override void OnExitField() {}
    }
}