using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairytale_GameMode : GameMode
{
    public Fairytale_CardContainer cardContainer = new Fairytale_CardContainer();
    
    private int _timeStep = 0;
    private int _timeStepLimit = 1;
    
    private bool _isAllCardTail = false;
    private bool _isTimerExpired = false;
    private bool _isAllCardHead = false;

    private Coroutine _gameRoutine = null;
    private Coroutine _timer = null;

    private void Awake()
    {
        GameManager.Instance.GameMode = this;
        
        cardContainer.OnAllCardHead += () =>
        {
            _isAllCardHead = true;
            _isAllCardTail = false;
        };
        cardContainer.OnAllCardTail += () =>
        {
            _isAllCardHead = false;
            _isAllCardTail = true;
        };
        cardContainer.OnFaceMixed += () =>
        {
            _isAllCardHead = false;
            _isAllCardTail = false;
        };

        if (NetworkManager.Instance.connectType == Define.ConnectType.Server)
        {
            _gameRoutine = StartCoroutine(GameRoutine());    
        }
    }
    
    private IEnumerator GameRoutine()
    {
        GameReady();
        yield return new WaitForSecondsRealtime(0.5f);
        ShowPlayerCard();

        while (_timeStep < _timeStepLimit)
        {
            yield return new WaitUntil(() => _isAllCardTail);
            NotifyCardFlipAvailable();
            yield return new WaitForSecondsRealtime(0.5f);
            UpdateCard();
            StartTimerForDecide();
            yield return new WaitUntil(() => _isTimerExpired || _isAllCardHead);
            UpdateData();
            UpdateTailCard();
            NotifyCardFlipUnavailable();
            if (_isAllCardTail) break;
        }
        
        ShowResult();
    }

    private void GameReady()
    {
        "GameReady".Log();
        _timeStepLimit = Random.Range(3, 5);

        var cardTypes = new List<Define.FairyTailGameCardType>();
        cardTypes.Add(Define.FairyTailGameCardType.TheHareAndTheTortoise);
        cardTypes.Add(Define.FairyTailGameCardType.TheNumber);
        
        Utils.ListRandomShuffle(cardTypes);

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
        "ShowPlayerCard".Log();
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 2, 0 }));
    }

    private void NotifyCardFlipAvailable()
    {
        "NotifyCardFlipAvailable".Log();
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 2, 0, 0 }));
    }

    private void UpdateCard()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 2, 1, 0 }));
    }

    private void StartTimerForDecide()
    {
        IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(10f);
            _isTimerExpired = true;
        }
        
        _isTimerExpired = false;
        _isAllCardHead = false;

        _timer = StartCoroutine(Timer());
    }

    private void UpdateData()
    {
        if(_timer != null) StopCoroutine(_timer);
        _isTimerExpired = false;
        _timeStep++;
    }

    private void UpdateTailCard()
    {
        
    }
    
    private void NotifyCardFlipUnavailable()
    {
        "NotifyCardFlipUnavailable".Log();
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 2, 0, 0 }));
    }

    private void ShowResult()
    {
        "ShowResult".Log();
    }
}