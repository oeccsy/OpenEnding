using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class A : RomanCard
    {
        public A()
        {
            cardType = CardType.A;
        }
        
        public override void FlipAbility() {}
        public override void ShakeAbility() {}
        public override void DiscoveredAbility()
        {
            (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
        }

        public override void SetActive(bool active) {}
    }
}