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

    public enum ConnectStatus
    {
        LeaveParty,
        TryingToJoin,
        JoinParty
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

    public enum PastelColor
    {
        Pink,
        Green,
        Violet,
        Sky,
        Gray,
        Beige
    }

    public enum CameraIndex
    {
        Main,
        UI
    }
}
