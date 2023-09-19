using UnityEngine;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public void TheHareAndTheTortoise()
    {
        "TorToiseCard".Log();
        var prefab = Resources.Load<GameObject>("Quirky/TortoiseCard");
        Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
    }

    public void TheNumber()
    {
        "TheNumber".Log();
        var prefab = Resources.Load<GameObject>("Quirky/RabbitCard");
        Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
    }

    public void ShowPlayerCard()
    {
        "ShowPlayerCard".Log();
        Overlay.UnsetActiveOverlay();
    }
}