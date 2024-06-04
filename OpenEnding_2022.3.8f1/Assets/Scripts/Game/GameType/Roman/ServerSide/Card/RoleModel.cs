using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class RoleModel : RomanCard, IFlipAbility, IGrowable
    {
        public RoleModel()
        {
            cardType = CardType.RoleModel;
        }
        
        private int _growthCount = 0;
        private int _victoryThreshold = 3;
        
        public void FlipAbility()
        {
            if(_growthCount >= _victoryThreshold) (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
        }
        
        public override void OnEnterField()
        {
            var gameMode = GameManager.Instance.GameMode as RomanGameMode;
            gameMode.OnCardFlipped += CheckGrow;
        }

        public override void OnExitField()
        {
            var gameMode = GameManager.Instance.GameMode as RomanGameMode;
            gameMode.OnCardFlipped -= CheckGrow;
        }
        
        public void CheckGrow(CardType cardType)
        {
            if (cardType != CardType.RoleModel) Grow();
        }

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