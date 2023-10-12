using UnityEngine;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public Fairytale_Card card;

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
        var prefab = Resources.Load<GameObject>("Prefabs/TheHareAndTheTortoise");
        card = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
        card.cardData.cardType = Define.FairyTaleGameCardType.TheHareAndTheTortoise;
    }

    public void ThereAreAlwaysMemos()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/ThereAreAlwaysMemos");
        card = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<ThereAreAlwaysMemos>();
        card.cardData.cardType = Define.FairyTaleGameCardType.ThereAreAlwaysMemos;
    }

    public void ShowPlayerCard()
    {
        Overlay.UnsetActiveOverlay();
    }

    public void SetSceneGrayScale()
    {
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }
}