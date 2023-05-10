using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SealGame_GameStartButton : MonoBehaviour
{
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> GameManager.Instance.GameFramework.StartGame());

        if (ConnectManager.Instance.connectType != Define.ConnectType.Server)
        {
            gameObject.SetActive(false);
        }
    }
}
