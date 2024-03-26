using Game.GameType.Roman;
using UnityEngine;
using Utility;

namespace Game.Manager.GameManage
{
    public abstract class PlayerController : MonoBehaviour
    {
        public Flip flip;
        public Shake shake;
        protected virtual void Awake()
        {
            GameManager.Instance.PlayerController = this;
        
            flip = gameObject.AddComponent<Flip>();
            shake = gameObject.AddComponent<Shake>();
        }
    }
}
