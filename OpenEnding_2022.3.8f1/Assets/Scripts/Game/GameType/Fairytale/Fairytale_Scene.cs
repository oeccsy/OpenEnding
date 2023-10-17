using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public Fairytale_Card card;
    public FairyTale_ResultPopup resultPopup;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Update()
    {
        SetOverlayByDeviceAngle();
    }

    private void SetOverlayByDeviceAngle()
    {
        if (Overlay.isOverlayActive) return;
        if (!Input.gyro.enabled) return;

        float alpha = 1.086956f * Input.gyro.gravity.z;
        Overlay.image.color = Color.black * alpha;
    }

    public void TheHareAndTheTortoise()
    {
        var cardPrefab = Resources.Load<GameObject>("Prefabs/TheHareAndTheTortoise");
        card = Instantiate(cardPrefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
        card.cardData.cardType = Define.FairyTaleGameCardType.TheHareAndTheTortoise;

        UIManager.Instance.ShowPopup("Prefabs/TortoiseCardDescPopupCanvas", 9);
    }

    public void ThereAreAlwaysMemos()
    {
        var cardPrefab = Resources.Load<GameObject>("Prefabs/ThereAreAlwaysMemos");
        card = Instantiate(cardPrefab, GameObject.Find("GameObjectRoot").transform).GetComponent<ThereAreAlwaysMemos>();
        card.cardData.cardType = Define.FairyTaleGameCardType.ThereAreAlwaysMemos;
        
        UIManager.Instance.ShowPopup("Prefabs/MemoCardDescPopupCanvas", 9);
    }

    public void SetSceneGrayScale()
    {
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }

    public void ShowSuccessSceneUI()
    {
        IEnumerator ShowRoutine()
        {
            yield return new WaitForSecondsRealtime(5f);
            UIManager.Instance.ShowSceneUI("Prefabs/SuccessUICanvas", 0);    
        }

        StartCoroutine(ShowRoutine());
    }

    public void ShowResultPopup()
    {
        var instance = UIManager.Instance.ShowPopup("Prefabs/ResultPopupCanvas", 9);
        resultPopup = instance.GetComponentInChildren<FairyTale_ResultPopup>();
        
        var gameState = GameManager.Instance.GameState as Fairytale_GameState;
        var successCardCount = gameState.successCardCount;
        resultPopup.RefreshSuccessCountText(successCardCount);
    }

    public void HideResultPopup()
    {
        resultPopup.Hide();
    }
}