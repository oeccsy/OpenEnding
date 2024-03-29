using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.Data;
using Game.Manager.GameManage;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.Card
{
    public class A : RomanCard
    {
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.A;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_A");
            cardInfoUI.RefreshUI(cardInfo);
        }
    }
}