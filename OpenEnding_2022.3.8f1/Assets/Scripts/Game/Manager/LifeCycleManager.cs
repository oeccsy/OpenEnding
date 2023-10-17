using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycleManager : Singleton<LifeCycleManager>
{
    protected override void Awake()
    {
        base.Awake();

#if UNITY_STANDALONE_WIN
        Application.runInBackground = true;
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
