using UnityEngine;

namespace Game.Manager.GameManage
{
    public class GameManager : Singleton<GameManager>
    {
        public GameMode GameMode { get; set; }
        public GameState GameState { get; set; }
        public GameFlow GameFlow { get; set; }
        public GameScene GameScene { get; set; }
        public PlayerController PlayerController { get; set; }
    }
}