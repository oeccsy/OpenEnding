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

        public void SetActive(bool active)
        {
            if (active)
            {
                var gameMode = (GameManager.Instance.GameMode as RomanGameMode);
                if(gameMode != null) gameMode.OnCardFlipped += CheckGrow;
            }
            else
            {
                var gameMode = (GameManager.Instance.GameMode as RomanGameMode);
                if(gameMode != null) gameMode.OnCardFlipped -= CheckGrow;
            }
        }
        
        public void CheckGrow(CardType flippedCardType)
        {
            if (flippedCardType != CardType.RoleModel) Grow();
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