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
        $"Set {targetDeviceColor} Head".Log();
        
        foreach (var cardData in cardList)
        {
            if (cardData.color == targetDeviceColor)
            {
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
        $"Set {targetDeviceColor} Tail".Log();
        
        foreach (var cardData in cardList)
        {
            if (cardData.color == targetDeviceColor)
            {
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
}