using System.Collections.Generic;
using UnityEngine;

public static class ColorPalette
{
    public static void Init() {}

    private static Dictionary<ColorName, Color> colorDict = new Dictionary<ColorName, Color>()
    {
        { ColorName.Pink, new Color(232/255f, 174/255f, 181/255f, 232/255f) },
        { ColorName.Green, new Color(214/255f, 245/255f, 171/255f, 245/255f) },
        { ColorName.Violet, new Color(195/255f, 182/255f, 252/255f, 252/255f) },
        { ColorName.Sky, new Color(178/255f,247/255f, 242/255f, 247/255f) },
        { ColorName.Gray, new Color(111/255f, 111/255f, 111/255f, 255/255f) },
        { ColorName.Beige, new Color(255/255f, 227/255f, 179/255f, 255/255f) },
        { ColorName.DeviceDefault, new Color(207/255f, 207/255f, 207/255f, 255/255f) },
        { ColorName.DisplayDefault, new Color(248/255f, 248/255f, 248/255f) },
        { ColorName.ProgressDotDefault, new Color(222/255f, 222/255f, 222/255f) },
        { ColorName.ProgressDotActive, new Color(92/255f, 92/255f, 92/255f) }
        
    };

    public static Color GetColor(ColorName color)
    {
        return colorDict[color];
    }
    public enum ColorName
    {
        Pink,
        Green,
        Violet,
        Sky,
        Gray,
        Beige,
        DeviceDefault,
        DisplayDefault,
        ProgressDotDefault,
        ProgressDotActive
    }
}