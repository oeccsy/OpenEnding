using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameMode GameMode { get; set; }
    public GameFlow GameFlow { get; set; }

    protected override void Awake()
    {
        GameFlow = GetComponent<GameFlow>();
    }
}