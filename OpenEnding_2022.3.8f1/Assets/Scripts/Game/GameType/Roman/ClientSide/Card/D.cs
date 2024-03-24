using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.Data;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.Card
{
    public class D : RomanCard
    {
        protected override void Awake()
        {
            base.Awake();
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_D");
            cardInfoUI.RefreshUI(cardInfo);
        }
    }
}