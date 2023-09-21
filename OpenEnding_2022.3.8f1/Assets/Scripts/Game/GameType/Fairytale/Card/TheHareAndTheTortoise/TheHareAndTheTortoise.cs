using System.Collections;
using UnityEngine;
public class TheHareAndTheTortoise : Fairytale_Card
{
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;
    
    protected override void Awake()
    {
        base.Awake();
        // var harePrefab = Resources.Load<GameObject>("Quirky/Rabbit");
        // hare = Instantiate(harePrefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
        //
        // var tortoisePrefab = Resources.Load<GameObject>("Quirky/Tortoise");
        // tortoise = Instantiate(tortoisePrefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ShowNextStep()
    {
        "Override ShowNextStep".Log();
        if (hare != null)
        {
            Define.Act nextAct = (Define.Act)((int)hare.curAction + 1);
            hare.ActNaturally(nextAct);
        }
        if (tortoise != null)
        {
            Define.Act nextAct = (Define.Act)((int)hare.curAction + 1);
            tortoise.ActNaturally(nextAct);    
        }
    }
}