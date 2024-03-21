using System.Collections;
using UnityEngine;

namespace Game.Manager.GameManage
{
    public abstract class GameMode : MonoBehaviour
    {
        protected Coroutine gameRoutine;
        
        protected virtual void Awake()
        {
            GameManager.Instance.GameMode = this;

            if (NetworkManager.Instance.connectType == Define.ConnectType.Server)
            {
                gameRoutine = StartCoroutine(GameRoutine());
            }
        }

        protected abstract IEnumerator GameRoutine();

        protected virtual void GameOver()
        {
            StopCoroutine(gameRoutine);   
        }
    }
}
