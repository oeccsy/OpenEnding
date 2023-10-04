using System;
using Shatalmic;

[Serializable]
public class Fairytale_CardData
{
    public Define.FairyTaleGameCardType cardType = Define.FairyTaleGameCardType.None;
    public Define.FairyTaleGameCardStatus cardStatus = Define.FairyTaleGameCardStatus.Playing;
    public Define.DisplayedFace displayedFace = Define.DisplayedFace.Head;
    
    public ColorPalette.ColorName color { get { return (ColorPalette.ColorName)networkDevice.colorOrder; } }
    public Networking.NetworkDevice networkDevice;

    public int runningTime;
    public int timeStep;
    public int goal;
    public int achievement;
    

    public Fairytale_CardData(Networking.NetworkDevice device)
    {
        networkDevice = device;

        runningTime = 0;
        timeStep = 0;
        goal = 0;
        achievement = 0;
    }
}