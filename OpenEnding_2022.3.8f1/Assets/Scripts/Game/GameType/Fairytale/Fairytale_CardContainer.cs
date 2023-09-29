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
    
    private int _headCount = 0;
    private int _tailCount = 0;

    public void InitFaceCounter()
    {
        _headCount = 0;
        _tailCount = 0;
        
        foreach (var cardData in cardList)
        {
            switch (cardData.displayedFace)
            {
                case Define.DisplayedFace.Head :
                    _headCount++;
                    break;
                case Define.DisplayedFace.Tail :
                    _tailCount++;
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
                if (cardData.cardStatus != Define.FairyTailGameCardStatus.None) return;
                
                $"Set {targetDeviceColor} Head".Log();
                
                cardData.displayedFace = Define.DisplayedFace.Head;
                _headCount++;
                _tailCount--;
                
                if (_tailCount == 0)
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
                if (cardData.cardStatus != Define.FairyTailGameCardStatus.None) return;
                
                $"Set {targetDeviceColor} Tail".Log();
                
                cardData.displayedFace = Define.DisplayedFace.Tail;
                _headCount--;
                _tailCount++;
                
                if (_headCount == 0)
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
                cardData.cardStatus = Define.FairyTailGameCardStatus.Success;
                switch (cardData.displayedFace)
                {
                    case Define.DisplayedFace.Head :
                        _headCount--;
                        break;
                    case Define.DisplayedFace.Tail :
                        _tailCount--;
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
                cardData.cardStatus = Define.FairyTailGameCardStatus.GiveUp;
                switch (cardData.displayedFace)
                {
                    case Define.DisplayedFace.Head :
                        _headCount--;
                        break;
                    case Define.DisplayedFace.Tail :
                        _tailCount--;
                        break;
                }
            }
        }
    }
}