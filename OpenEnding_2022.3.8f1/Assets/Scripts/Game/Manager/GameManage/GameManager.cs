using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameMode GameMode { get; set; }
    public GameState GameState { get; set; }
    public GameFlow GameFlow { get; set; }
    public PlayerController PlayerController { get; set; }
}