using Game.Manager.GameManage;

namespace Game.GameType.Fairytale
{
    public class Fairytale_PlayerController : PlayerController
    {
        protected override void Awake()
        {
            base.Awake();
            flip.OnFlipToTail += NotifyFlipToTail;
            flip.OnStand += NotifyStartFlipToHead;
        }

#if !UNITY_EDITOR
    private void Start()
    {
        flip.SetEnableGyroSensor(true);
    }
#endif
    
        public void NotifyFlipToTail()
        {
            StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {1, 1, (byte)NetworkManager.Instance.ownDeviceData.colorOrder}));
        }
    
        public void NotifyStartFlipToHead()
        {
            StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {1, 0, (byte)NetworkManager.Instance.ownDeviceData.colorOrder}));
        }
    }
}
