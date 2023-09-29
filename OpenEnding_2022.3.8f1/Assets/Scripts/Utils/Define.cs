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

    public enum FairyTailGameCardStatus
    {
        None = 0,
        Success = 1,
        GiveUp = 2
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

    public enum PostProcess
    {
        None,
        GrayScale
    }

    public enum PacketIndex
    {
        Class = 0,
        Function,
        ParamBegin
    }

    public enum Act
    {
        Attack = 0,
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
        Walk = 17
    }
    
    public enum Shape
    {
        Eyes_Annoyed = 0,
        Eyes_Blink,
        Eyes_Cry,
        Eyes_Dead,
        Eyes_Excited,
        Eyes_Happy,
        Eyes_LookDown,
        Eyes_LookIn,
        Eyes_LookOut,
        Eyes_LookUp,
        Eyes_Rabid,
        Eyes_Sad,
        Eyes_Shrink,
        Eyes_Sleep,
        Eyes_Spin,
        Eyes_Squint,
        Eyes_Trauma,
        Eyes_Sweat_L,
        Eyes_Sweat_R,
        Eyes_Teardrop_L,
        Eyes_Teardrop_R = 20
    }

    public enum Story
    {
        LoseAll,
        Standstill,
        TakeStepBack,
        TakeOneStep
    }
}
