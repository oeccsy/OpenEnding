using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairytale_GameMode : GameMode
{
    public Fairytale_CardContainer cardContainer = new Fairytale_CardContainer();
    
    private int timeStep = 0;
    private int timeStepLimit = 0;
    
    private bool isAllCardTail = false;
    private bool isTimerExpired = false;
    private bool isAllCardHead = false;

    private Coroutine gameRoutine = null;
    private Coroutine timer = null;

    private void Awake()
    {
        GameManager.Instance.GameMode = this;
        
        cardContainer.OnAllCardHead += () => isAllCardHead = true;
        cardContainer.OnAllCardTail += () => isAllCardTail = true;
        
        SetFlipEvent();
        Flip.Instance.SetEnableGyroSensor(true);
        
        if (NetworkManager.Instance.connectType == Define.ConnectType.Server)
        {
            gameRoutine = StartCoroutine(GameRoutine());    
        }
    }

    #region Routine
    private IEnumerator GameRoutine()
    {
        GameReady();
        yield return new WaitForSecondsRealtime(0.5f);
        ShowPlayerCard();

        while (timeStep < timeStepLimit)
        {
            yield return new WaitUntil(() => isAllCardTail);
            NotifyCardFlipAvailable();
            StartTimerForDecide();
            yield return new WaitUntil(() => isTimerExpired || isAllCardHead);
            yield return new WaitForSecondsRealtime(5f);
            UpdateData();
        }
        
        ShowResult();
    }

    private void GameReady()
    {
        timeStepLimit = Random.Range(3, 5);

        var cardTypes = new List<Define.FairyTailGameCardType>();
        cardTypes.Add(Define.FairyTailGameCardType.TheHareAndTheTortoise);
        cardTypes.Add(Define.FairyTailGameCardType.TheNumber);

        foreach (var device in NetworkManager.Instance.connectedDeviceList)
        {
            var newCard = new Fairytale_CardData(device);
            newCard.cardType = cardTypes[(int)newCard.color];
            cardContainer.cardList.Add(newCard);

            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(device, new byte[] {0, (byte)newCard.cardType}));
        }
        
        cardContainer.InitFaceCounter();
    }

    private void ShowPlayerCard()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 2, 0 }));
    }

    private void NotifyCardFlipAvailable()
    {
        "NotifyCardFlipAvailable".Log();
        // 진동으로 알림
    }

    private void StartTimerForDecide()
    {
        "StartTimerForDecide".Log();
        IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(10f);
            isTimerExpired = true;
        }
        
        isTimerExpired = false;
        isAllCardHead = false;

        timer = StartCoroutine(Timer());
    }

    private void UpdateData()
    {
        isTimerExpired = false;
        isAllCardHead = false;

        timeStep++;
    }

    private void ShowResult()
    {
        "ShowResult".Log();
    }
    
    #endregion

    #region Interaction

    private void SetFlipEvent()
    {
        Flip.Instance.OnFlipToTail += NotifyFlipToTail;
        Flip.Instance.OnStartFlipToHead += NotifyStartFlipToHead;
    }

    private void UnsetFlipEvent()
    {
        Flip.Instance.OnFlipToTail -= NotifyFlipToTail;
        Flip.Instance.OnStartFlipToHead -= NotifyStartFlipToHead;
    }

    private void NotifyFlipToTail()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {1, 1, (byte)NetworkManager.Instance.ownDeviceData.colorOrder}));
    }
    
    private void NotifyStartFlipToHead()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToServer(new byte[] {1, 0, (byte)NetworkManager.Instance.ownDeviceData.colorOrder}));
    }

    #endregion
}