using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fairytale_GameMode : GameMode
{
    public Fairytale_CardContainer cardContainer = new Fairytale_CardContainer();
    
    private int _timeStep = 0;
    private int _totalCardFlip = 0;
    private int _maxCardFlip = 0;
    
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
        CreateStories();
        yield return new WaitForSecondsRealtime(0.5f);
        ShowPlayerCard();

        while (_totalCardFlip < _maxCardFlip)
        {
            yield return new WaitUntil(() => _isAllCardTail);
            TheCardStoriesUnfolds(_timeStep);
            yield return new WaitForSecondsRealtime(0.5f);
            NotifyCardFlipAvailable();
            
            StartTimerForDecide(10f);
            yield return new WaitUntil(() => _isTimerExpired || _isAllCardHead);
            ResetTimer();
            
            RemoveTailCardFromGame();
            UpdateDoneCard();
            yield return new WaitForSecondsRealtime(0.5f);
            NotifyCardFlipUnavailable();
            if (_isAllCardTail) break;
            _timeStep++;
        }
        
        ShowResult();
    }

    private void GameReady()
    {
        "GameReady".Log();
        
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

        _timeStep = 0;
        _totalCardFlip = 0;
        _maxCardFlip = Random.Range(21, 31);
    }

    private void CreateStories()
    {
        foreach (var cardData in cardContainer.cardList)
        {
            cardData.runningTime += 10;
        }

        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            var randomCardIndex = Random.Range(0, cardContainer.cardList.Count); 
            cardContainer.cardList[randomCardIndex].runningTime += 1;
        }
        
        foreach (var cardData in cardContainer.cardList)
        {
            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(cardData.networkDevice, new byte[] { 2, 2, (byte)cardData.runningTime }));    
        }
    }

    private void ShowPlayerCard()
    {
        "ShowPlayerCard".Log();
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 2, 0 }));
    }
    
    private void TheCardStoriesUnfolds(int timeStep)
    {
        foreach (var card in cardContainer.cardList)
        {
            if (card.cardStatus == Define.FairyTailGameCardStatus.None)
            {
                StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(card.networkDevice, new byte[] { 2, 1, (byte)timeStep }));    
            }
        }
    }

    private void NotifyCardFlipAvailable()
    {
        "NotifyCardFlipAvailable".Log();
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 2, 0, 0 }));
    }

    private void StartTimerForDecide(float timer)
    {
        IEnumerator Timer()
        {
            yield return new WaitForSecondsRealtime(timer);
            _isTimerExpired = true;
        }
        
        _isTimerExpired = false;
        _isAllCardHead = false;

        _timer = StartCoroutine(Timer());
    }

    private void ResetTimer()
    {
        if(_timer != null) StopCoroutine(_timer);
        _isTimerExpired = false;
        _timeStep += cardContainer.cardList.Count;
    }

    private void RemoveTailCardFromGame()
    {
        foreach (var card in cardContainer.cardList)
        {
             if (card.displayedFace == Define.DisplayedFace.Tail)
             {
                 StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(card.networkDevice, new byte[] { 0, 3, 0 }));
                 cardContainer.SetCardGiveUp(card.color);
             }
        }
    }

    private void UpdateDoneCard()
    {
        foreach (var card in cardContainer.cardList)
        {
            if (card.runningTime - 1 == _timeStep && card.displayedFace == Define.DisplayedFace.Head)
            {
                cardContainer.SetCardSuccess(card.color);
            }
        }
    }
    
    private void NotifyCardFlipUnavailable()
    {
        "NotifyCardFlipUnavailable".Log();
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 2, 0, 0 }));
    }

    private void ShowResult()
    {
        "ShowResult".Log();
        "ShowResult".Log();
        "ShowResult".Log();
        "ShowResult".Log();
        "ShowResult".Log();
        "ShowResult".Log();
    }
}