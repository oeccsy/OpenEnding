using System;
using UnityEngine;

namespace Game.GameType.Roman.Card
{
    [Serializable]
    public abstract class RomanCard : MonoBehaviour
    {
        protected RomanCardData _cardData = new RomanCardData();

        protected abstract void OwnAbility();

        protected virtual void ChangeCard()
        {
            
        }
    }
}