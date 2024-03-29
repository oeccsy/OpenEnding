using Game.Manager.GameManage;
using UnityEngine;

public abstract class GameState
{
    public GameState()
    {
        GameManager.Instance.GameState = this;
    }
}
