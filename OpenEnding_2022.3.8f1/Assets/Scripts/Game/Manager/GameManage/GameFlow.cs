using System.Collections;
using Game.Manager.GameManage;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.GameFlow = this;
        
        ConnectManager.Instance.OnAllDeviceConnected += () => StartCoroutine(GameJoinRoutine());
    }

    private IEnumerator GameJoinRoutine()
    {
        if (NetworkManager.Instance.connectType != Define.ConnectType.Server) yield break;
        
        yield return new WaitForSecondsRealtime(2f);

        if (Connect_Scene.Instance.n != NetworkManager.Instance.connectedDeviceList.Count) yield break;

        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] {1, 0, 2}));
    }
    
    public void LoadScene(Define.SceneType targetScene)
    {
        IEnumerator LoadFairytaleSceneRoutine()
        {
            Overlay.SetActiveOverlay();
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene((int)targetScene);
            yield return new WaitForSecondsRealtime(1f);
            Overlay.UnsetActiveOverlay();
        }

        StartCoroutine(LoadFairytaleSceneRoutine());
    }
}
