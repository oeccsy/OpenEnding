using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SealGame_DeviceCardManager : Singleton<SealGame_DeviceCardManager>
{
    private Image cardImage;

    private void Awake()
    {
        cardImage = GetComponentInChildren<Image>();
    }

    public void SetCardImage(string s1, string s2, byte[] bytes)
    {
        DebugText.Instance.AddText($"데이터 수신 : {bytes[0]}");
        if (bytes[0] == 1)
        {
            cardImage.sprite = Resources.Load<Sprite>("Sprites/Seal");
        }
    }

    public void SetCardImageSeal()
    {
        DebugText.Instance.AddText("물개 카드 !");
        cardImage.sprite = Resources.Load<Sprite>("Sprites/Seal");
    }
}
