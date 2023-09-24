using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Fairytale_Card : MonoBehaviour
{
    public Fairytale_CardData cardData;
    protected virtual void Awake()
    {
        SetFlipEvent();
        Flip.Instance.SetEnableGyroSensor(true);
        Fairytale_PacketHandler.Instance.ownCard = this;
    }

    public virtual void Update()
    {
        if (Overlay.isOverlayActive) return;
        if (!Input.gyro.enabled) return;

        float alpha = 1.086956f * Input.gyro.gravity.z;
        Overlay.image.color = Color.black * alpha;
    }

    public void SetFlipEvent()
    {
        Flip.Instance.OnFlipToTail += NotifyFlipToTail;
        Flip.Instance.OnStartFlipToHead += NotifyStartFlipToHead;
    }

    public void UnsetFlipEvent()
    {
        Flip.Instance.OnFlipToTail -= NotifyFlipToTail;
        Flip.Instance.OnStartFlipToHead -= NotifyStartFlipToHead;
    }

    public void NotifyFlipToTail()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {1, 1, (byte)NetworkManager.Instance.ownDeviceData.colorOrder}));
    }
    
    public void NotifyStartFlipToHead()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {1, 0, (byte)NetworkManager.Instance.ownDeviceData.colorOrder}));
    }

    public void Vibrate()
    {
        Handheld.Vibrate();
    }

    public virtual void ShowNextStep() { }
    
    public virtual void CreateStoryLine(int goal, int runningTime) { }
    
    public virtual void SelectStory(int timeStep, int runningTime, ref int cur, int goal) { }
}