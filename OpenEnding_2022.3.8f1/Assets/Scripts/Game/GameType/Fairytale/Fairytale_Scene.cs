using UnityEngine;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public Fairytale_Card card;
    
    public void TheHareAndTheTortoise()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/TheHareAndTheTortoise");
        card = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
    }

    public void ThereAreAlwaysMemos()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/ThereAreAlwaysMemos");
        card = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<ThereAreAlwaysMemos>();
    }

    public void ShowPlayerCard()
    {
        Overlay.UnsetActiveOverlay();
    }

    public void SetSceneGrayScale()
    {
        $"SetSceneGrayScale".Log();
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }
}