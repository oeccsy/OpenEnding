using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SceneType
    {
        ConnectScene,
        FairytaleScene,
        RomanScene
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
        None,
        Head,
        Tail,
        Stand
    }

    public enum FairyTaleGameCardType
    {
        None = -1,
        TheHareAndTheTortoise = 0,
        ThereAreAlwaysMemos
    }

    public enum FairyTaleGameCardStatus
    {
        Playing = 0,
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
        UI,
        Main
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
        Eyes_Rabid = 10,
        Eyes_Sad,
        Eyes_Shrink,
        Eyes_Sleep,
        Eyes_Spin,
        Eyes_Squint,
        Eyes_Trauma,
        Sweat_L,
        Sweat_R,
        Teardrop_L,
        Teardrop_R = 20
    }

    public enum Story
    {
        LoseAll,
        Standstill,
        TakeStepBack,
        TakeOneStep
    }

    public enum MemoType
    {
        Book,
        Drawing,
        Studying,
    }
}

namespace Game.GameType.Roman
{
    public enum GameStep
    {
        InitGame,
        SelectCard,
        FlipOrShake,
        ShowCard,
        HideCard,
        GameOver,
        Pause
    }
    
    public enum CardType
    {
        None,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J
    }
}