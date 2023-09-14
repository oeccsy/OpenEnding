using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycleManager : Singleton<LifeCycleManager>
{
    private void Awake()
    {
        base.Awake();
        
        DebugCanvas.Instance.InitDebugCanvas();
#if DEVELOPMENT_BUILD_A
        
        
#if UNITY_STANDALONE_WIN
        Application.runInBackground = true;
#endif
#endif
    }

    private void OnApplicationFocus(bool hasFocus)
    {
#if UNITY_IOS
        Application.targetFrameRate = 60;
#endif
    }

    private void OnApplicationQuit()
    {
#if DEVELOPMENT_BUILD_A
        NetworkManager.Instance.StopServer();
        NetworkManager.Instance.StopClient();
#endif
    }
}
