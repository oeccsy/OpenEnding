using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
using UnityEngine;

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
            // 이 카드를 뒤집으면 성장합니다.
            // 3번째 성장하면 승리합니다.
            
            Grow();
            if(_growthCount >= _victoryThreshold) (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
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