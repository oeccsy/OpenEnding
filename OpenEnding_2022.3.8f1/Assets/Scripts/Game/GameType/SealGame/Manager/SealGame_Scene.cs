using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SealGame_Scene : Singleton<SealGame_Scene>
{
    [SerializeField]
    private Image cardImage;

    private void Awake()
    {
        base.Awake();
        
        switch (NetworkManager.Instance.connectType)
        {
            case Define.ConnectType.Server:
                NetworkManager.Instance.StartServer();
                break;
            case Define.ConnectType.Client:
                NetworkManager.Instance.StartClient();
                break;
        }
    }
    public void SetCardImageSeal(byte[] bytes)
    {
        DebugCanvas.Instance.AddText("물개 카드 !");
        cardImage.sprite = Resources.Load<Sprite>("Sprites/Seal");
    }
}
