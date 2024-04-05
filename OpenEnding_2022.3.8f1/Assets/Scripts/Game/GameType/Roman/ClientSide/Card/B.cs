﻿using System.Collections;
using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.Data;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.Card
{
    public class B : RomanCard
    {
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.B;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_B");
            cardInfoUI.RefreshUI(cardInfo);
        }

        public override IEnumerator Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}