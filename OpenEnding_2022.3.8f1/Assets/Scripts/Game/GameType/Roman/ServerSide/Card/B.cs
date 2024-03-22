namespace Game.GameType.Roman.ServerSide.Card
{
    public class B : RomanCard
    {
        public B()
        {
            cardType = CardType.B;
        }
        
        public override void FlipAbility()
        {
            // 이 카드를 뒤집으면
            // 다른 카드를 선택하여 확인할 수 있습니다.
        }

        public override void ShakeAbility() {}
        public override void DiscoveredAbility() {}
        
        public override void SetActive(bool active) {}
    }
}