using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycleManager : Singleton<LifeCycleManager>
{
    protected override void Awake()
    {
        base.Awake();
        
#if DEVELOPMENT_BUILD || UNITY_EDITOR || UNITY_STANDALONE_WIN
        DebugCanvas.Instance.InitDebugCanvas();
        
#if UNITY_STANDALONE_WIN
        Application.runInBackground = true;
#endif
#endif
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Application.targetFrameRate = 60;
    }

    private void OnApplicationQuit()
    {
#if DEVELOPMENT_BUILD
        NetworkManager.Instance.StopServer();
        NetworkManager.Instance.StopClient();
#endif
    }
}
