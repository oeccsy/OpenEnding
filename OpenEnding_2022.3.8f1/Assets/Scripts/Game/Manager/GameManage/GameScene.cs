using UnityEngine;

namespace Game.Manager.GameManage
{
    public abstract class GameScene : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GameManager.Instance.GameScene = this;
        }
    }
}