using System;
using System.Collections.Generic;

[Serializable]
public class Fairytale_CardContainer
{
    public List<Fairytale_CardData> cardList = new List<Fairytale_CardData>();
    
    public delegate void CardFaceHandler();
    public event CardFaceHandler OnAllCardHead;
    public event CardFaceHandler OnAllCardTail;
    public event CardFaceHandler OnFaceMixed;
    
    public int headCount = 0;
    public int tailCount = 0;

    public void InitFaceCounter()
    {
        headCount = 0;
        tailCount = 0;
        
        foreach (var cardData in cardList)
        {
            switch (cardData.displayedFace)
            {
                case Define.DisplayedFace.Head :
                    headCount++;
                    break;
                case Define.DisplayedFace.Tail :
                    tailCount++;
                    break;
            }
        }

        OnAllCardHead += () => "OnAllCardHead".Log();
        OnAllCardTail += () => "OnAllCardTail".Log();
    }
    
    public void SetCardHead(ColorPalette.ColorName targetDeviceColor)
    {
        foreach (var cardData in cardList)
        {
            if (cardData.color == targetDeviceColor)
            {
                if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
                
                $"Set {targetDeviceColor} Head".Log();
                
                cardData.displayedFace = Define.DisplayedFace.Head;
                headCount++;
                tailCount--;
                
                if (tailCount == 0)
                {
                    OnAllCardHead?.Invoke();
                }
                else
                {
                    OnFaceMixed?.Invoke();
                }
            }
        }
    }

    public void SetCardTail(ColorPalette.ColorName targetDeviceColor)
    {
        foreach (var cardData in cardList)
        {
            if (cardData.color == targetDeviceColor)
            {
                if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
                
                $"Set {targetDeviceColor} Tail".Log();
                
                cardData.displayedFace = Define.DisplayedFace.Tail;
                headCount--;
                tailCount++;
                
                if (headCount == 0)
                {
                    OnAllCardTail?.Invoke();
                }
                else
                {
                    OnFaceMixed?.Invoke();
                }
            }
        }
    }

    public void SetCardSuccess(ColorPalette.ColorName targetDeviceColor)
    {
        $"Set {targetDeviceColor} Success".Log();
        
        foreach (var cardData in cardList)
        {
            if (cardData.color == targetDeviceColor)
            {
                cardData.cardStatus = Define.FairyTaleGameCardStatus.Success;
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
    }
    
    public void SetCardGiveUp(ColorPalette.ColorName targetDeviceColor)
    {
        $"Set {targetDeviceColor} GiveUp".Log();
        
        foreach (var cardData in cardList)
        {
            if (cardData.color == targetDeviceColor)
            {
                cardData.cardStatus = Define.FairyTaleGameCardStatus.GiveUp;
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
    }
}