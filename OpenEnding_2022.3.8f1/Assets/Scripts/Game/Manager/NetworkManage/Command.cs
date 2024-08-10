using System;

[Serializable]
public class Command
{
  public ColorPalette.ColorName sourceDeviceColor;
  public string typeName;
  public string methodName;
  public object[] param;
}