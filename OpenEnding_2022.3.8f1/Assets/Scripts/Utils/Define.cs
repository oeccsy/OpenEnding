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

    public enum DisplayedFace
    {
        Head,
        Tail
    }

    public enum FairyTailGameCardType
    {
        None = -1,
        TheHareAndTheTortoise = 0,
        TheNumber
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

    public enum PacketIndex
    {
        Class = 0,
        Function,
        ParamBegin
    }

    public enum Act
    {
        Attack,
        Bounce,
        Clicked,
        Death,
        Eat,
        Fear,
        Fly,
        Hit,
        Idle_A,
        Idle_B,
        Idle_C,
        Jump,
        Roll,
        Run,
        Sit,
        Spin,
        Swim,
        Walk
    }
    
    public enum Shape
    {
        Annoyed,
        Blink,
        Cry,
        Dead,
        Excited,
        Happy,
        LookDown,
        LookIn,
        LookOut,
        LookUp,
        Rabid,
        Sad,
        Shrink,
        Sleep,
        Spin,
        Squint,
        Trauma,
        Sweat_L,
        Sweat_R,
        Teardrop_L,
        Teardrop_R
    }
}
