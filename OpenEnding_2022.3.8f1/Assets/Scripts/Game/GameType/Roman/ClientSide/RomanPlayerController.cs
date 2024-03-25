using Game.Manager.GameManage;

namespace Game.GameType.Roman.ClientSide
{
    public class RomanPlayerController : PlayerController
    {
        protected override void Awake()
        {
            base.Awake();
            flip.OnFlipToHead += NotifyFlipToHead;
            flip.OnFlipToTail += NotifyFlipToTail;
            flip.OnStand += NotifyStand;
            shake.OnEveryShake += NotifyShake;
        }
        
        public void NotifyFlipToHead()
        {
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            byte displayedFace = (byte)Define.DisplayedFace.Head;
            StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {0, 0, cardType, displayedFace}));
        }
        
        public void NotifyFlipToTail()
        {
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            byte displayedFace = (byte)Define.DisplayedFace.Tail;
            StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {0, 0, cardType, displayedFace}));
        }

        public void NotifyStand()
        {
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            byte displayedFace = (byte)Define.DisplayedFace.Stand;
            StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {0, 0, cardType, displayedFace}));
        }

        public void NotifyShake()
        {
            byte cardType = (byte)(GameManager.Instance.GameScene as RomanGameScene).card.cardType;
            StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {0, 1, cardType}));
        }
    }
}