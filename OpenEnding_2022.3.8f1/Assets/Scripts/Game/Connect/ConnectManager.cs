using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : Singleton<ConnectManager>
{
    [SerializeField]
    private Define.ConnectStatus connectStatus = Define.ConnectStatus.LeaveParty;
    
    public delegate void ConnectHandler();
    public event ConnectHandler OnAllDeviceConnected;
    
    private void Start()
    {
        InitConnect();
        
        NetworkManager.Instance.OnBluetoothDeviceConnected += RegisterDevice;
        NetworkManager.Instance.OnBluetoothDeviceConnected += RequestSynchronizeDevices;
        NetworkManager.Instance.OnBluetoothDeviceDisconnected += UnRegisterDevice;
        NetworkManager.Instance.OnBluetoothDeviceDisconnected += RequestSynchronizeDevices;
        
        foreach (var deviceObject in Connect_Scene.Instance.deviceObjectList)
        {
            deviceObject.OnTouchDevice += StartConnect;
        }
    }

    private void InitConnect()
    {
        if (connectStatus == Define.ConnectStatus.LeaveParty) return;
        
        switch (NetworkManager.Instance.connectType)
        {
            case Define.ConnectType.Server:
                NetworkManager.Instance.connectType = Define.ConnectType.None;
                NetworkManager.Instance.StopServer();
                break;
            case Define.ConnectType.Client:
                NetworkManager.Instance.connectType = Define.ConnectType.None;
                NetworkManager.Instance.StopClient();
                break;
        }
        
        connectStatus = Define.ConnectStatus.LeaveParty;
    }

    private void StartConnect(DeviceObject selectedDevice)
    {
        if (connectStatus != Define.ConnectStatus.LeaveParty) return;

        NetworkManager.Instance.ownDeviceColor = selectedDevice.ownColor;
        
        switch (selectedDevice.ownColor)
        {
            case ColorPalette.ColorName.Pink:
                NetworkManager.Instance.StartServer();
                break;
            default:
                NetworkManager.Instance.StartClient();
                break;
        }
        
        connectStatus = Define.ConnectStatus.TryingToJoin;
        
        selectedDevice.OnTouchDevice -= StartConnect;
        selectedDevice.OnTouchDevice += StopConnect;
        
        "StartConnect".Log();   
    }

    private void StopConnect(DeviceObject selectedDevice)
    {
        if (connectStatus == Define.ConnectStatus.LeaveParty) return;
        
        switch (NetworkManager.Instance.connectType)
        {
            case Define.ConnectType.Server:
                NetworkManager.Instance.connectType = Define.ConnectType.None;
                NetworkManager.Instance.StopServer();
                break;
            case Define.ConnectType.Client:
                NetworkManager.Instance.connectType = Define.ConnectType.None;
                NetworkManager.Instance.StopClient();
                break;
        }
        
        connectStatus = Define.ConnectStatus.LeaveParty;
        
        selectedDevice.OnTouchDevice -= StopConnect;
        selectedDevice.OnTouchDevice += StartConnect;
        
        "StopConnect".Log();
    }

    private void RegisterDevice(string deviceName)
    {
        $"{deviceName} Join Done".Log();
        
        List<ColorPalette.ColorName> connectedDeviceList = NetworkManager.Instance.connectedDeviceList;

        ColorPalette.ColorName deviceColor = (ColorPalette.ColorName)Enum.Parse(typeof(ColorPalette.ColorName), deviceName);

        if (!connectedDeviceList.Contains(deviceColor))
        {
            connectedDeviceList.Add(deviceColor);
        }

        if (Connect_Scene.Instance.n == connectedDeviceList.Count)
        {
            OnAllDeviceConnected?.Invoke();
        }
    }

    private void UnRegisterDevice(string deviceName)
    {
        $"{deviceName} Disconnect".Log();
        
        List<ColorPalette.ColorName> connectedDeviceList = NetworkManager.Instance.connectedDeviceList;

        ColorPalette.ColorName deviceColor = (ColorPalette.ColorName)Enum.Parse(typeof(ColorPalette.ColorName), deviceName);

        if (connectedDeviceList != null && connectedDeviceList.Contains(deviceColor))
        {
            connectedDeviceList.Remove(deviceColor);
        }
    }

    private void RequestSynchronizeDevices(string deviceName)
    {
        ColorPalette.ColorName[] deviceColors = NetworkManager.Instance.connectedDeviceList.ToArray();

        NetworkManager.Instance.ClientRpcCall(typeof(Connect_Scene), "SynchronizeDevicesWithAnimation", deviceColors);
    }
}