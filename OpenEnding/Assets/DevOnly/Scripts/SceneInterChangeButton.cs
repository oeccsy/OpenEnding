using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneInterChangeButton : MonoBehaviour
{
    public enum SceneType
    {
        None,
        Client,
        Server
    }

    public SceneType targetSceneType = SceneType.None;
    private Button button;
    
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> SceneChange(targetSceneType));
    }

    private void SceneChange(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.None:
                Debug.Log("ButtonType : None");
                break;
            case SceneType.Client:
                SceneManager.LoadScene("Client");
                break;
            case SceneType.Server:
                SceneManager.LoadScene("Server");
                break;
        }
    }
}
