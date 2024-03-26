using System.Collections;
using UnityEngine;

namespace Game.Manager.GameManage
{
    public abstract class GameMode : MonoBehaviour
    {
        public int playerCount;
        protected Coroutine gameRoutine;
        
        protected virtual void Awake()
        {
            GameManager.Instance.GameMode = this;
            playerCount = NetworkManager.Instance.connectedDeviceList.Count;

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
