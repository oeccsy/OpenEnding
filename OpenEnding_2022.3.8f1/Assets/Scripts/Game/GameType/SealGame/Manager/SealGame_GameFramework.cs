using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SealGame_GameFramework : GameFramework
{
    [SerializeField]
    private SealGame_CardContainer sealGameCardContainer = new SealGame_CardContainer();
    [SerializeField]
    private int cardIndexOfSeal;
    protected override IEnumerator GameFrameworkCoroutine()
    {
        GameReady();
        SelectOneSeal();
        yield return ShowCard();
    }

    private void GameReady()
    {
        DebugCanvas.Instance.AddText("GameReady");
        sealGameCardContainer.InitDeviceDataList();
    }

    private void SelectOneSeal()
    {
        DebugCanvas.Instance.AddText("SelectOneSeal");
        cardIndexOfSeal = Random.Range(0, sealGameCardContainer.deviceDataList.Count);
        DebugCanvas.Instance.AddText($"Device Num {cardIndexOfSeal} 물개 당첨!");
        sealGameCardContainer.SetDeviceData(cardIndexOfSeal, Define.SealGameCardType.Seal);
    }

    private IEnumerator ShowCard()
    {
        DebugCanvas.Instance.AddText("ShowCard");
        yield return NetworkManager.Instance.ExecuteFuncOnTargetDevice(new byte[] {0,(byte)cardIndexOfSeal,0,0,0});
    }
}
