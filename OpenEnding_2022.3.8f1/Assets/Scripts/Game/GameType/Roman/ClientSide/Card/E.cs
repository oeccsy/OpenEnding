using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.Data;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.Card
{
    public class E : RomanCard
    {
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.E;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_E");
            cardInfoUI.RefreshUI(cardInfo);
        }
    }
}