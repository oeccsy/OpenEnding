using Game.Manager.GameManage;
using UnityEngine;

namespace Game.GameType.Roman.Card
{
    public class C : RomanCard
    {
        private int _growthCount = 0;
        private int _victoryThreshold = 3;
        
        private void Awake()
        {
            GameManager.Instance.PlayerController.flip.OnFlipToHead += FlipAbility;
        }
        // 이 카드를 뒤집으면 성장합니다.
        // 3번째 성장하면 승리합니다.
        protected override void FlipAbility()
        {
            _growthCount++;
            if(_growthCount > _victoryThreshold) Debug.Log("NotifyVictory");
        }

        protected override void ShakeAbility()
        {
            throw new System.NotImplementedException();
        }

        protected override void DiscoveredAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}