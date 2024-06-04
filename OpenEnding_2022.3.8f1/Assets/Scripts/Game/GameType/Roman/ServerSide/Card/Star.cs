using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class Star : RomanCard, IDiscoveredAbility
    {
        public Star()
        {
            cardType = CardType.Star;
        }
        
        public void DiscoveredAbility()
        {
            (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
        }

        public override void OnEnterField() {}
        public override void OnExitField() {}
    }
}