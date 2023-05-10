using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectScene_ConnectUI : MonoBehaviour
{
    [SerializeField]
    private Image connectStatusImage;
    [SerializeField]
    private TMP_Text targetNetworkName;

    [SerializeField]
    private Button startServerButton;
    [SerializeField]
    private Button startClientButton;

    private void Awake()
    {
        connectStatusImage = transform.GetChild(0).GetComponent<Image>();
        targetNetworkName = transform.GetChild(1).GetComponent<TMP_Text>();
        startServerButton = transform.GetChild(2).GetComponent<Button>();
        startClientButton = transform.GetChild(3).GetComponent<Button>();
    }

    private void Start()
    {
        SetButtonEvent();
    }

    private void SetButtonEvent()
    {
        startServerButton.onClick.AddListener( ()=> ConnectManager.Instance.SetConnectType(Define.ConnectType.Server));
        startServerButton.onClick.AddListener( ()=> SceneManager.LoadScene(Define.SceneType.SealGameScene.ToString()));
        
        startClientButton.onClick.AddListener( ()=> ConnectManager.Instance.SetConnectType(Define.ConnectType.Client));
        startClientButton.onClick.AddListener( ()=> SceneManager.LoadScene(Define.SceneType.SealGameScene.ToString()));
    }
}
