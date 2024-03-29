using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class A : RomanCard, IDiscoveredAbility
    {
        public A()
        {
            cardType = CardType.A;
        }
        
        public void DiscoveredAbility()
        {
            (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
        }
    }
}