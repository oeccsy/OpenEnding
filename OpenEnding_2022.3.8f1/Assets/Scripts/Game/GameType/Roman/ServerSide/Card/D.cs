using System;
using Game.Manager.GameManage;
using Random = UnityEngine.Random;

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

            int randomIndex = Random.Range(0, growableCards.Count);
            growableCards[randomIndex].Regress();
        }

        public override void ShakeAbility() {}

        public override void DiscoveredAbility() {}

        public override void SetActive(bool active) {}
    }
}