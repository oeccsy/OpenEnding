using UnityEngine;

public class Fairytale_Scene : Singleton<Fairytale_Scene>
{
    public void TheHareAndTheTortoise()
    {
        var prefab = Resources.Load<GameObject>("Quirky/TortoiseCard");
        var instance = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
        Fairytale_PacketHandler.Instance.ownCard = instance;
    }

    public void TheNumber()
    {
        var prefab = Resources.Load<GameObject>("Quirky/RabbitCard");
        var instance = Instantiate(prefab, GameObject.Find("GameObjectRoot").transform).GetComponent<TheHareAndTheTortoise>();
        Fairytale_PacketHandler.Instance.ownCard = instance;
    }

    public void ShowPlayerCard()
    {
        Overlay.UnsetActiveOverlay();
    }
}