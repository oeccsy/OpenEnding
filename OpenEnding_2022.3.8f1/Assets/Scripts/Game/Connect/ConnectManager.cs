using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;

public class ConnectManager : Singleton<ConnectManager>
{
    [SerializeField]
    private Define.ConnectStatus connectStatus = Define.ConnectStatus.LeaveParty;
    
    public delegate void ConnectHandler();
    public event ConnectHandler OnAllDeviceConnected;
    
    private void Start()
    {
        NetworkManager.Instance.OnDeviceReady += RegisterDevice;
        NetworkManager.Instance.OnDeviceReady += RequestSynchronizeDevices;
        NetworkManager.Instance.OnDeviceDisconnected += UnRegisterDevice;
        NetworkManager.Instance.OnDeviceDisconnected += RequestSynchronizeDevices;
        
        foreach (var deviceObject in Connect_Scene.Instance.deviceObjectList)
        {
            deviceObject.OnTouchDevice += StartConnect;
        }
    }

    private void StartConnect(DeviceObject selectedDevice)
    {
        if (connectStatus != Define.ConnectStatus.LeaveParty) return;

        NetworkManager.Instance.clientName = selectedDevice.ownColor.ToString();
        NetworkManager.Instance.ownDeviceData.colorOrder = (int)selectedDevice.ownColor;
        
        
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

    private void RegisterDevice(Networking.NetworkDevice connectedDevice)
    {
        $"{connectedDevice.Name} Join Done".Log();
        
        List<Networking.NetworkDevice> connectedDeviceList = NetworkManager.Instance.connectedDeviceList;

        var colorString = connectedDevice.Name.Split(':')[1];
        connectedDevice.colorOrder = (int)(ColorPalette.ColorName)Enum.Parse(typeof(ColorPalette.ColorName), colorString);

        if (!connectedDeviceList.Contains(connectedDevice))
        {
            connectedDevice.deviceListOrder = connectedDeviceList.Count;
            connectedDeviceList.Add(connectedDevice);
        }

        if (Connect_Scene.Instance.n == connectedDeviceList.Count)
        {
            OnAllDeviceConnected?.Invoke();    
        }
    }

    private void UnRegisterDevice(Networking.NetworkDevice disconnectedDevice)
    {
        $"{disconnectedDevice.Name} Disconnect".Log();
        
        List<Networking.NetworkDevice> connectedDeviceList = NetworkManager.Instance.connectedDeviceList;

        if (connectedDeviceList != null && connectedDeviceList.Contains(disconnectedDevice))
        {
            connectedDeviceList.Remove(disconnectedDevice);
        }
    }

    private void RequestSynchronizeDevices(Networking.NetworkDevice temp)
    {
        var packet = new List<byte> {0, 0};
        
        foreach (var device in NetworkManager.Instance.connectedDeviceList)
        {
            packet.Add((byte)device.colorOrder);
        }

        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(packet.ToArray()));
    }
}