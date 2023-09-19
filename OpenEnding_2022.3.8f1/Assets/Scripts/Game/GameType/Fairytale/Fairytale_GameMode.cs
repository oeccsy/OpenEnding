using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairytale_GameMode : GameMode
{
    [SerializeField]
    private Fairytale_CardContainer cardContainer = new Fairytale_CardContainer();
    
    private int timeStep = 0;
    [SerializeField]
    private int timeStepLimit;
    
    private bool isAllCardTale = false;
    private bool isTimerExpired = false;
    private bool isAllCardHead = false;

    private Coroutine gameRoutine = null;
    private Coroutine timer = null;

    private void Awake()
    {
        GameManager.Instance.GameMode = this;
        if (NetworkManager.Instance.connectType == Define.ConnectType.Server)
        {
            gameRoutine = StartCoroutine(GameRoutine());    
        }
    }

    private IEnumerator GameRoutine()
    {
        GameReady(); // 시간 설정, 카드 설정, 카드 가치 설정
        yield return ShowPlayerCard();

        while (timeStep < timeStepLimit)
        {
            yield return new WaitUntil(() => isAllCardTale);
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
        timeStepLimit = Random.Range(10, 20);

        var cardTypes = new List<Define.FairyTailGameCardType>();
        cardTypes.Add(Define.FairyTailGameCardType.TheHareAndTheTortoise);
        cardTypes.Add(Define.FairyTailGameCardType.TheNumber);

        foreach (var device in NetworkManager.Instance.connectedDeviceList)
        {
            var newCard = new Fairytale_CardData(device);
            newCard.cardType = cardTypes[(int)newCard.color];
            cardContainer.cardList.Add(newCard);

            $"{newCard.color.ToString()} : {newCard.cardType}".Log();
            $"isMyDevice : {device == NetworkManager.Instance.ownDeviceData}".Log();

            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(device, new byte[] {0, (byte)newCard.cardType}));
        }
    }

    private IEnumerator ShowPlayerCard()
    {
        yield return new WaitForSecondsRealtime(3f);
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 2, 0 }));
    }

    private void NotifyCardFlipAvailable()
    {
        // 진동으로 알림
    }

    private void StartTimerForDecide()
    {
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
    }

    private void ShowResult()
    {
        
    }
}
