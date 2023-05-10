using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 게임을 관리하는 Root Class
    public GameFramework _gameFramework = null;
    public GameFramework GameFramework
    {
        get
        {
            if (_gameFramework == null)
                _gameFramework = FindObjectOfType<GameFramework>(); //GameObject.Find("Systems").GetComponentInChildren<GameFramework>();

            return _gameFramework;
        }
    }

    //public GameMode gameMode = GameMode.Normal;
    //public GameState gameState = GameState.GamePlay; 
}