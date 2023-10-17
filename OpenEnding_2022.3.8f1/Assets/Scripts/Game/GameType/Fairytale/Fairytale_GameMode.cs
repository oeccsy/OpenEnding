using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Fairytale_GameMode : GameMode
{
    public Fairytale_CardContainer cardContainer = new Fairytale_CardContainer();
    
    private int _timeStep = 0;
    private int _totalCardFlip = 0;
    private int _maxCardFlip = 0;

    private Coroutine _gameRoutine = null;
    private Coroutine _timer = null;
    private bool _isTimerExpired = false;

    private void Awake()
    {
        GameManager.Instance.GameMode = this;

        if (NetworkManager.Instance.connectType == Define.ConnectType.Server)
        {
            _gameRoutine = StartCoroutine(GameRoutine());    
        }
    }
    
    private IEnumerator GameRoutine()
    {
        SelectCardTypes();
        yield return new WaitForSecondsRealtime(0.5f);
        CreateStories();
        yield return new WaitForSecondsRealtime(0.5f);
        ShowPlayerCard();

        while (_totalCardFlip < _maxCardFlip)
        {
            yield return new WaitUntil(() => cardContainer.IsAllCardTail);
            TheCardStoriesUnfolds(_timeStep);
            yield return new WaitForSecondsRealtime(0.5f);
            NotifyCardFlipAvailable();
            StartTimerForDecide(10f);
            yield return new WaitUntil(() => _isTimerExpired || cardContainer.IsAllCardHead);
            ResetTimer();
            UpdateGameData();
            UpdateCardDataByFace();
            yield return new WaitForSecondsRealtime(0.5f);
            SynchronizeState();
            if (cardContainer.IsAllCardTail) break;
            yield return new WaitForSecondsRealtime(0.5f);
            NotifyCardFlipUnavailable();
        }

        yield return new WaitForSecondsRealtime(5f);
        ShowResult();
        yield return new WaitForSecondsRealtime(5f);
        HideResult();
        yield return new WaitForSecondsRealtime(5f);
        LoadConnectScene();
    }

    private void SelectCardTypes()
    {
        "GameReady".Log();
        
        var cardTypes = new List<Define.FairyTaleGameCardType>();
        cardTypes.Add(Define.FairyTaleGameCardType.TheHareAndTheTortoise);
        cardTypes.Add(Define.FairyTaleGameCardType.ThereAreAlwaysMemos);
        
        Utils.ListRandomShuffle(cardTypes);

        foreach (var device in NetworkManager.Instance.connectedDeviceList)
        {
            var newCard = new Fairytale_CardData(device);
            newCard.cardType = cardTypes[(int)newCard.Color];
            cardContainer.cardList.Add(newCard);
            
            StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(device, new byte[] {0, (byte)newCard.cardType}));
        }

        cardContainer.InitFaceCounter();

        _timeStep = 0;
        _totalCardFlip = 0;
        _maxCardFlip = Random.Range(21, 31);
        $"maxCardFlip : {_maxCardFlip}".Log();
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
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 2, 0 }));
    }
    
    private void TheCardStoriesUnfolds(int timeStep)
    {
        foreach (var card in cardContainer.cardList)
        {
            if (card.cardStatus == Define.FairyTaleGameCardStatus.Playing)
            {
                StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(card.networkDevice, new byte[] { 2, 1, (byte)timeStep }));    
            }
        }
    }

    private void NotifyCardFlipAvailable()
    {
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

        _timer = StartCoroutine(Timer());
    }

    private void ResetTimer()
    {
        if(_timer != null) StopCoroutine(_timer);
        _isTimerExpired = false;
    }

    private void UpdateGameData()
    {
        _timeStep++;

        foreach (var cardData in cardContainer.cardList)
        {
            if (cardData.displayedFace == Define.DisplayedFace.Head) _totalCardFlip++;
        }

        $"total card flip : {_totalCardFlip}".Log();
    }

    private void UpdateCardDataByFace()
    {
        foreach (var card in cardContainer.cardList)
        {
            if (card.displayedFace == Define.DisplayedFace.Tail && card.cardStatus == Define.FairyTaleGameCardStatus.Playing)
            {
                StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(card.networkDevice, new byte[] { 0, 3, 0 }));
                cardContainer.SetCardGiveUp(card.Color);
            }
        }
        
        foreach (var card in cardContainer.cardList)
        {
            if (card.displayedFace == Define.DisplayedFace.Head && card.runningTime - 1 == _timeStep)
            {
                StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(card.networkDevice, new byte[] { 0, 4, 0}));
                cardContainer.SetCardSuccess(card.Color);
            }
        }
        
        $"head : {cardContainer.headCount}".Log();
        $"tail : {cardContainer.tailCount}".Log();
    }

    private void SynchronizeState()
    {
        var gameState = GameManager.Instance.GameState as Fairytale_GameState;
        int successCardCount = gameState.successCardCount;
        int giveUpCardCount = gameState.giveUpCardCount;
        
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 3, 3, (byte)successCardCount, (byte)giveUpCardCount }));
    }

    private void NotifyCardFlipUnavailable()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 2, 0, 0 }));
    }

    private void ShowResult()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 5, 0 }));
    }
    
    private void HideResult()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 5, 0 }));
    }

    private void LoadConnectScene()
    {
        
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 0, 6, 0 }));
    }
}