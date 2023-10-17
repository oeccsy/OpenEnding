using System.Collections.Generic;
using UnityEngine;

public class Fairytale_GameState : GameState
{
    public int successCardCount = 0;
    public int giveUpCardCount = 0;

    public List<Define.FairyTaleGameCardType> successCardList = new List<Define.FairyTaleGameCardType>();
    public List<Define.FairyTaleGameCardType> giveUpCardList = new List<Define.FairyTaleGameCardType>();

    private void Awake()
    {
        GameManager.Instance.GameState = this;
    }
    
    public void AddSuccessCard(Define.FairyTaleGameCardType cardType)
    {
        successCardList.Add(cardType);
        successCardCount++;
    }

    public void AddGiveUpCard(Define.FairyTaleGameCardType cardType)
    {
        giveUpCardList.Add(cardType);
        giveUpCardCount++;
    }
    
    public void SetGameState(int successCardCount, int giveUpCardCount)
    {
        $"SetGameState".Log();
        this.successCardCount = successCardCount;
        this.giveUpCardCount = giveUpCardCount;

        $"SetGameState : {this.successCardCount}, {this.giveUpCardCount}".Log();
    }
}
