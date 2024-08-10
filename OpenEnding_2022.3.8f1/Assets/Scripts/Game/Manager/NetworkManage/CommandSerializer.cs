using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CommandSerializer
{
    public static byte[] Serialize(Command command)
    {
        using(MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, command);
            return memoryStream.ToArray();
        }
    }

    public static Command Deserialize(byte[] packet)
    {
        packet.Length.Log();
        using(MemoryStream memoryStream = new MemoryStream(packet))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Command)formatter.Deserialize(memoryStream);
        }
    }
}