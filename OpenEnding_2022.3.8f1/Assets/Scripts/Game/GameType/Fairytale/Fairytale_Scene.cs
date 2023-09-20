using UnityEngine;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public void TheHareAndTheTortoise()
    {
        var prefab = Resources.Load<GameObject>("Quirky/TortoiseCard");
        Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
    }

    public void TheNumber()
    {
        var prefab = Resources.Load<GameObject>("Quirky/RabbitCard");
        Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
    }

    public void ShowPlayerCard()
    {
        Overlay.UnsetActiveOverlay();
    }
}