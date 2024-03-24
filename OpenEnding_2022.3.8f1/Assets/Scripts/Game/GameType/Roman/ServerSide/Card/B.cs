using Game.GameType.Roman.ServerSide.CardBase;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class B : RomanCard, IFlipAbility
    {
        public B()
        {
            cardType = CardType.B;
        }
        
        public void FlipAbility()
        {
            // 이 카드를 뒤집으면
            // 다른 카드를 선택하여 확인할 수 있습니다.
            
            // Client에게 로직 노티
        }
    }
}