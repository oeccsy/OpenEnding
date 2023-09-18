using System.Collections;
using UnityEngine;
public class TheHareAndTheTortoise : MonoBehaviour
{
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;
    private void Awake()
    {
        // var harePrefab = Resources.Load<GameObject>("Quirky/Rabbit");
        // hare = Instantiate(harePrefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
        //
        // var tortoisePrefab = Resources.Load<GameObject>("Quirky/Tortoise");
        // tortoise = Instantiate(tortoisePrefab, GameObject.Find("GameObjectRoot").transform).GetComponent<Animal>();
    }

    public void ShowNextStep()
    {
        tortoise.ActNaturally(tortoise.curAction++);
    }
}