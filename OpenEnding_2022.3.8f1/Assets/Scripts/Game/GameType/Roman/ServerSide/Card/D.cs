using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class D : RomanCard
    {
        public override void FlipAbility()
        {
            var gameMode = GameManager.Instance.GameMode as RomanGameMode;
            if (gameMode == null) return;

            var growableCards = gameMode.cardContainer.GetGrowableCards();
            if (growableCards.Count == 0) return;
            
            Utils.ShuffleList(growableCards);
            growableCards[0].Regress();
        }

        public override void ShakeAbility() {}

        public override void DiscoveredAbility() {}

        public override void SetActive(bool active) {}
    }
}