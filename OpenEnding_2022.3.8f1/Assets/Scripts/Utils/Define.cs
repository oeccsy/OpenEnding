using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SceneType
    {
        OnBoardingScene,
        ConnectScene,
        SealGameScene
    }

    public enum ConnectType
    {
        None,
        Server,
        Client
    }
    
    public enum SealGameStep
    {
        GameReady,
        ChooseSeal
    }

    public enum SealGameCardType
    {
        None,
        Hunter,
        Seal
    }

    public enum CameraIndex
    {
        Main,
        UI
    }
}
