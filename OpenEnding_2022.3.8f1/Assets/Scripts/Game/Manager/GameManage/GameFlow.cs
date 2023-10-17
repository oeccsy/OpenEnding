using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        
        yield return new WaitForSeconds(3f);

        if (Connect_Scene.Instance.n != NetworkManager.Instance.connectedDeviceList.Count) yield break;

        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] {0, 1, 0}));
    }
    
    public void LoadFairytaleScene()
    {
        IEnumerator LoadFairytaleSceneRoutine()
        {
            Overlay.SetActiveOverlay();
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("FairytaleGameScene");
            yield return new WaitForSeconds(1f);
            Overlay.UnsetActiveOverlay();
        }

        StartCoroutine(LoadFairytaleSceneRoutine());
    }
    
    public void LoadConnectScene()
    {
        IEnumerator LoadConnectSceneRoutine()
        {
            Overlay.SetActiveOverlay();
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("ConnectScene");
            yield return new WaitForSeconds(1f);
            Overlay.UnsetActiveOverlay();
        }

        StartCoroutine(LoadConnectSceneRoutine());
    }
}
