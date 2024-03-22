using System;
using Shatalmic;

namespace Game.GameType.Roman.ServerSide.Card
{
    [Serializable]
    public abstract class RomanCard
    {
        public CardType cardType = CardType.None;
        public string cardName;
        public string cardDesc;
        
        public Define.DisplayedFace displayedFace = Define.DisplayedFace.None;
        public Networking.NetworkDevice device;
        
        public abstract void FlipAbility();
        public abstract void ShakeAbility();
        public abstract void DiscoveredAbility();
        
        public virtual void ChangeCard()
        {
            
        }
        
        public abstract void SetActive(bool active);
    }
}