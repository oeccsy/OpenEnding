using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public abstract class GameFramework : MonoBehaviour
{
    protected Coroutine gameFramework = null;
    protected abstract IEnumerator GameFrameworkCoroutine();

    public void Start()
    {
        if (ConnectManager.Instance.connectType == Define.ConnectType.Client)
        {
            DebugText.Instance.AddText("Set OnRecieve Data");
            ConnectManager.Instance.bleClient.OnReceiveData += (s1, s2, t) => StartGame();
        }
    }
    public void StartGame()
    {
        DebugText.Instance.AddText("StartGame");
        if (ConnectManager.Instance.connectType == Define.ConnectType.Server)
        {
            StartCoroutine(ConnectManager.Instance.bleServer.SendBytesToAllDevice(new byte[1]));
            StartCoroutine(GameFrameworkCoroutine());
        }
        else
        {
            StartCoroutine(GameFrameworkCoroutine());
            ConnectManager.Instance.bleClient.OnReceiveData = null;
        }
        
    }
}
