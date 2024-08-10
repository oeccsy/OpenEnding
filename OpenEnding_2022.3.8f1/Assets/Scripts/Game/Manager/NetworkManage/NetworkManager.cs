using System;
using System.Collections.Generic;
using Game.Manager.NetworkManage;

public partial class NetworkManager : Singleton<NetworkManager>
{

    public Define.ConnectType connectType = Define.ConnectType.None;
    
    public ColorPalette.ColorName ownDeviceColor = ColorPalette.ColorName.DeviceDefault;
    public readonly List<ColorPalette.ColorName> connectedDeviceList = new List<ColorPalette.ColorName>();

    public delegate void BluetoothConnectHandler(string deviceName);
    public event BluetoothConnectHandler OnBluetoothDeviceConnected;
    public event BluetoothConnectHandler OnBluetoothDeviceDisconnected;

    public delegate void BluetoothDataHandler(byte[] data);
    public event BluetoothDataHandler OnBluetoothDataReceive;

    public PacketHandler PacketHandler { get; set; }

#if UNITY_ANDROID || UNITY_IOS
    protected void Start()
    {
#if UNITY_ANDROID
        AndroidConnection.Instance.OnBluetoothDeviceConnected += (deviceName) => OnBluetoothDeviceConnected.Invoke(deviceName);
        AndroidConnection.Instance.OnBluetoothDeviceDisconnected += (deviceName) => OnBluetoothDeviceDisconnected.Invoke(deviceName);
        AndroidConnection.Instance.OnBluetoothDataReceive += (data) => OnBluetoothDataReceive.Invoke(data);
        AndroidConnection.Instance.RequestPermissions();
        AndroidConnection.Instance.InitCentralBluetoothSystem();
        AndroidConnection.Instance.InitPeripheralBluetoothSystem();
#endif
    }

    #region Server Side

    public void StartServer()
    {
        connectType = Define.ConnectType.Server;

        ownDeviceColor = ColorPalette.ColorName.Pink;
        connectedDeviceList.Add(ownDeviceColor);

#if UNITY_ANDROID
        AndroidConnection.Instance.StartScanning(ownDeviceColor.ToString());
#endif
    }

    public void ClientRpcCall(ColorPalette.ColorName targetDeviceColor, Type type, string methodName, params object[] param)
    {
        Command command = new Command();
        command.sourceDeviceColor = ownDeviceColor;
        command.typeName = type.FullName;
        command.methodName = methodName;
        command.param = param;

        byte[] packet = CommandSerializer.Serialize(command);

        $"{methodName} RPC Call packet len : {packet.Length}".Log();
    
        SendBytesToTargetDevice(targetDeviceColor, packet);
    }

    public void ClientRpcCall(Type type, string methodName, params object[] param)
    {
        Command command = new Command();
        command.sourceDeviceColor = ownDeviceColor;
        command.typeName = type.FullName;
        command.methodName = methodName;
        command.param = param;

        byte[] packet = CommandSerializer.Serialize(command);

        $"{methodName} RPC Call packet len : {packet.Length}".Log();

        SendBytesToAllDevice(packet);
    }
    
    public void SendBytesToTargetDevice(ColorPalette.ColorName targetDevice, byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) return;
   
        if (targetDevice == ownDeviceColor)
        {
            OnBluetoothDataReceive?.Invoke(bytes);
        }
        else
        {
            AndroidConnection.Instance.Write(targetDevice.ToString(), bytes);    
        }
    }

    public void SendBytesToAllDevice(byte[] bytes)
    {
        if (connectType != Define.ConnectType.Server) return;
        
        foreach (var targetDeviceColor in connectedDeviceList)
        {
            if (targetDeviceColor == ownDeviceColor)
            {
                OnBluetoothDataReceive?.Invoke(bytes);
            }
            else
            {
                AndroidConnection.Instance.Write(targetDeviceColor.ToString(), bytes);    
            }
        }
    }
    
    public void StopServer()
    {
        connectedDeviceList.Clear();
#if UNITY_ANDROID
        AndroidConnection.Instance.StopServer();
#endif
    }

    #endregion
    
    #region Client Side
    public void StartClient()
    {
        connectType = Define.ConnectType.Client;

#if UNITY_ANDROID
        AndroidConnection.Instance.StartServer();
        AndroidConnection.Instance.StartAdvertising(ownDeviceColor.ToString());
#endif
    }

    public void ServerRpcCall(Type type, string methodName, params object[] param)
    {
        Command command = new Command();
        command.sourceDeviceColor = ownDeviceColor;
        command.typeName = type.FullName;
        command.methodName = methodName;
        command.param = param;

        byte[] packet = CommandSerializer.Serialize(command);
        $"{methodName} RPC Call packet len : {packet.Length}".Log();
        SendBytesToServer(packet);
    }
    
    public void SendBytesToServer(byte[] bytes)
    {
        if (connectType == Define.ConnectType.Client)
        {
            AndroidConnection.Instance.Indicate(bytes);
        }
        else if (connectType == Define.ConnectType.Server)
        {
            OnBluetoothDataReceive?.Invoke(bytes);
        }
    }
    
    public void StopClient()
    {
#if UNITY_ANDROID
        AndroidConnection.Instance.StopServer();
        AndroidConnection.Instance.InitPeripheralBluetoothSystem();
#endif
    }

    #endregion
    
#endif
}