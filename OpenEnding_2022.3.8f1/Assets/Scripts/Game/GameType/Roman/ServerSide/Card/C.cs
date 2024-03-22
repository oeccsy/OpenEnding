using Game.Manager.GameManage;
using UnityEngine;

namespace Game.GameType.Roman.ServerSide.Card
{
    public class C : RomanCard, IGrowable
    {
        private int _growthCount = 0;
        private int _victoryThreshold = 3;

        public override void FlipAbility()
        {
            // 이 카드를 뒤집으면 성장합니다.
            // 3번째 성장하면 승리합니다.
            
            Grow();
            if(_growthCount >= _victoryThreshold) (GameManager.Instance.GameMode as RomanGameMode)?.Victory();
        }

        public override void ShakeAbility() {}
        public override void DiscoveredAbility() {}
        public override void SetActive(bool active) {}
        
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