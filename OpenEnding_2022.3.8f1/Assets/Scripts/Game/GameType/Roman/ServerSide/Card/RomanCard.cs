using System;
using UnityEngine;

namespace Game.GameType.Roman.Card
{
    [Serializable]
    public abstract class RomanCard : MonoBehaviour
    {
        protected RomanCardData _cardData = new RomanCardData();

        protected abstract void FlipAbility();
        protected abstract void ShakeAbility();
        protected abstract void DiscoveredAbility();
        

        protected virtual void ChangeCard()
        {
            
        }
    }
}