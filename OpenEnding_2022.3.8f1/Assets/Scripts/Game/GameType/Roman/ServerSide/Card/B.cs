using Game.Manager.GameManage;
using UnityEngine;

namespace Game.GameType.Roman.Card
{
    public class B : RomanCard
    {
        private void Awake()
        {
            GameManager.Instance.PlayerController.flip.OnFlipToHead += FlipAbility;
        }
        // 이 카드를 뒤집으면
        // 다른 카드를 선택하여 확인할 수 있습니다.
        protected override void FlipAbility()
        {
         
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