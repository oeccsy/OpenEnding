using Shatalmic;

public class Fairytale_CardData
{
    public Define.FairyTailGameCardType cardType = Define.FairyTailGameCardType.None;
    public Define.DisplayedFace displayedFace = Define.DisplayedFace.Head;
    public ColorPalette.ColorName color { get { return (ColorPalette.ColorName)networkDevice.colorOrder; } }
    public Networking.NetworkDevice networkDevice;

    public Fairytale_CardData(Networking.NetworkDevice device)
    {
        networkDevice = device;
    }
}