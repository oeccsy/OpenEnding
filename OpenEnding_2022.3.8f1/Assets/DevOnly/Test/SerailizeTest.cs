using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class SerailizeTest : MonoBehaviour
{
    Command command1;

    // Start is called before the first frame update
    void Start()
    {
        ColorPalette.ColorName[] deviceColors = new ColorPalette.ColorName[] { ColorPalette.ColorName.Pink, ColorPalette.ColorName.Green };
        object[] objectArray = Array.ConvertAll(deviceColors, data => (object)data);
        List<ColorPalette.ColorName> list = new List<ColorPalette.ColorName>() { ColorPalette.ColorName.Pink, ColorPalette.ColorName.Green };
        TestCommandWithSerializer(typeof(Connect_Scene), "SynchronizeDevicesWithAnimation", list);
    }

    public void Test(Type type, string methodName, object[] param)
    {
        Command command = new Command();
        command.sourceDeviceColor = ColorPalette.ColorName.Pink;
        command.typeName = type.Name;
        command.methodName = methodName;
        command.param = new object[] {0, 1, "test", 3.14, true};

        TempCommand tc = new TempCommand();

        string json = JsonUtility.ToJson(command);
        Debug.Log(json);
        Debug.Log(json.Length);
    }

    public void TestCommandWithSerializer(Type type, string methodName, params object[] param)
    {
        Command command = new Command();
        command.sourceDeviceColor = ColorPalette.ColorName.Pink;
        command.typeName = type.Name;
        command.methodName = methodName;
        command.param = param;
        
        Debug.Log($"command.param Lenght : {command.param.Length}");

        byte[] bytes = CommandSerializer.Serialize(command);
        Debug.Log(bytes.Length);
        Debug.Log(bytes);
    }

    [Serializable]
    public class TempCommand
    {
        public ColorPalette.ColorName sourceDeviceColor;
        public string typeName;
        public string methodName;
        public object[] param;
    }

    public static byte[] Serialize(TempCommand command)
    {
        using(MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, command);
            return memoryStream.ToArray();
        }
    }

    public static TempCommand Deserialize(byte[] packet)
    {
        MemoryStream memoryStream = new MemoryStream(packet);
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter.Deserialize(memoryStream) as TempCommand;
    }
}
