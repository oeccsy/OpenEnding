using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Flip flip;
    
    private void Awake()
    {
        GameManager.Instance.PlayerController = this;
        
        flip = GetComponent<Flip>();
        
        flip.OnFlipToTail += NotifyFlipToTail;
        flip.OnStartFlipToHead += NotifyStartFlipToHead;
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
