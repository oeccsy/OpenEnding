using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Fairytale_CardContainer
{
    public List<Fairytale_CardData> cardList = new List<Fairytale_CardData>();
    
    public delegate void CardFaceHandler();
    public event CardFaceHandler OnAllCardHead;
    public event CardFaceHandler OnAllCardTail;
    
    public int playingCardCount = 0;
    public int headCount = 0;
    public int tailCount = 0;
    
    public bool IsAllCardHead => headCount == playingCardCount;
    public bool IsAllCardTail => tailCount == playingCardCount;
    
    public void InitFaceCounter()
    {
        playingCardCount = cardList.Count(cardData => cardData.cardStatus == Define.FairyTaleGameCardStatus.Playing);
        headCount = cardList.Count(cardData => cardData.displayedFace == Define.DisplayedFace.Head);
        tailCount = cardList.Count(cardData => cardData.displayedFace == Define.DisplayedFace.Tail);
    }

    public void SetCardHead(ColorPalette.ColorName targetColor)
    {
        var cardData = cardList.Single(cardData => cardData.Color == targetColor);
        
        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
        if (cardData.displayedFace == Define.DisplayedFace.Head) return;
        
        cardData.displayedFace = Define.DisplayedFace.Head;
        
        headCount++;
        tailCount--;
                
        if (headCount == playingCardCount)
        {
            OnAllCardHead?.Invoke();
        }
    }
    
    public void SetCardTail(ColorPalette.ColorName targetColor)
    {
        var cardData = cardList.Single(cardData => cardData.Color == targetColor);

        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
        if (cardData.displayedFace == Define.DisplayedFace.Tail) return;
        
        cardData.displayedFace = Define.DisplayedFace.Tail;
        
        headCount--;
        tailCount++;
                
        if (tailCount == playingCardCount)
        {
            OnAllCardTail?.Invoke();
        }
    }
    
    public void SetCardSuccess(ColorPalette.ColorName targetColor)
    {
        var cardData = cardList.Single(cardData => cardData.Color == targetColor);

        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;

        cardData.cardStatus = Define.FairyTaleGameCardStatus.Success;
        playingCardCount--;
        
        switch (cardData.displayedFace)
        {
            case Define.DisplayedFace.Head :
                headCount--;
                break;
            case Define.DisplayedFace.Tail :
                tailCount--;
                break;
        }
    }
    
    public void SetCardGiveUp(ColorPalette.ColorName targetColor)
    {
        var cardData = cardList.Single(cardData => cardData.Color == targetColor);

        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;

        cardData.cardStatus = Define.FairyTaleGameCardStatus.GiveUp;
        playingCardCount--;
        
        switch (cardData.displayedFace)
        {
            case Define.DisplayedFace.Head :
                headCount--;
                break;
            case Define.DisplayedFace.Tail :
                tailCount--;
                break;
        }
    }
}