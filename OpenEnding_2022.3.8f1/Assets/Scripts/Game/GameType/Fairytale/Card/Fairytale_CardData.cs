using System;
using System.Collections.Generic;
using Shatalmic;

[Serializable]
public class Fairytale_CardData
{
    public Define.FairyTaleGameCardType cardType = Define.FairyTaleGameCardType.None;
    public Define.FairyTaleGameCardStatus cardStatus = Define.FairyTaleGameCardStatus.Playing;
    public Define.DisplayedFace displayedFace = Define.DisplayedFace.Head;

    public ColorPalette.ColorName Color => (ColorPalette.ColorName)networkDevice.colorOrder;
    public Networking.NetworkDevice networkDevice;

    public int runningTime;
    public int timeStep;
    public int goal;
    public int achievement;
    
    public List<Define.Story> storyLine = new List<Define.Story>();
    public List<int> achievementProgress = new List<int>();

    public Fairytale_CardData(Networking.NetworkDevice device)
    {
        networkDevice = device;
    }
}