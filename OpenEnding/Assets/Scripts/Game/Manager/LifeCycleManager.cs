using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycleManager : Singleton<LifeCycleManager>
{
    private void Awake()
    {
#if DEVELOPMENT_BUILD
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
#if DEVELOPMENT_BUILD
        NetworkManager.Instance.CloseUDPClient();
#endif
    }
}
