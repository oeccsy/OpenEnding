using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class Artwork : RomanCard, IFlipAbility, IGrowable
    {
        public Artwork()
        {
            cardType = CardType.Artwork;
        }
        
        private int _growthCount = 0;
        private int _victoryThreshold = 3;

        public void FlipAbility()
        {
            Grow();
            if(_growthCount >= _victoryThreshold) (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
        }
        
        public override void OnEnterField() {}
        public override void OnExitField() {}
        
        public void Grow()
        {
            _growthCount++;
        }

        public void Regress()
        {
            _growthCount--;
        }
    }
}