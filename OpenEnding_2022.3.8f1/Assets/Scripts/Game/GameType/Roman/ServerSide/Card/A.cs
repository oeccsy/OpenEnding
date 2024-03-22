using UnityEngine;

namespace Game.GameType.Roman.Card
{
    public class A : RomanCard
    {
        protected override void FlipAbility() {}
        protected override void ShakeAbility() {}
        protected override void DiscoveredAbility()
        {
            // B로 이 카드를 발견하면 승리합니다.
        }
    }
}