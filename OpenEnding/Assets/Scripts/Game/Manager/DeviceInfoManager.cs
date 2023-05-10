public class DeviceInfoManager : Singleton<DeviceInfoManager>
{
    public string deviceName = "testDevice";
    public int deviceIndex = -1;

    public void SetDeviceIndex(int index)
    {
        deviceIndex = index;
    }
}
