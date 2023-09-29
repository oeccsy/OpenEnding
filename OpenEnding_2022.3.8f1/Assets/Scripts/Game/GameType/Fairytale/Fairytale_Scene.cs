using UnityEngine;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public Fairytale_Card card;
    
    public void TheHareAndTheTortoise()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/TheHareAndTheTortoise");
        card = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
    }

    public void TheNumber()
    {
        var prefab = Resources.Load<GameObject>("Quirky/RabbitCard");
        card = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
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