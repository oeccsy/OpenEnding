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

    [ContextMenu("FuncTest/ShowNextStep")]
    public override void ShowNextStep()
    {
        "Override ShowNextStep".Log();
        if (hare != null)
        {
            "Start Test1".Log();
            Define.Act nextAct = (Define.Act)(((int)hare.curAction + 1) % 17);
            hare.ActNaturally(nextAct);
            "Test1 Done".Log();
        }
        if (tortoise != null)
        {
            "Start Test2".Log();
            Define.Act nextAct = (Define.Act)(((int)hare.curAction + 1) % 17);
            tortoise.ActNaturally(nextAct);
            "Test2 Done".Log();
        }
    }
}