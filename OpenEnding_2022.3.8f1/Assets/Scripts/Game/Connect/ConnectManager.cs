using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;
using UnityEngine.tvOS;

public class ConnectManager : Singleton<ConnectManager>
{
    [SerializeField]
    private DeviceObject userOwnDeviceObject;
    [SerializeField]
    private Define.ConnectStatus connectStatus = Define.ConnectStatus.LeaveParty;

    private void Awake()
    {
        DeviceObject.OnTouchAnyDevice += StartConnect;
    }

    private void StartConnect(DeviceObject selectedDevice)
    {
        if (connectStatus != Define.ConnectStatus.LeaveParty) return;
        if (selectedDevice.curColor != ColorPalette.ColorName.DeviceDefault) return;

        switch (selectedDevice.ownColor)
        {
            case ColorPalette.ColorName.Pink:
                NetworkManager.Instance.connectType = Define.ConnectType.Server;
                NetworkManager.Instance.StartServer();
                break;
            default:
                NetworkManager.Instance.connectType = Define.ConnectType.Client;
                NetworkManager.Instance.StartClient();
                break;
        }
        
        connectStatus = Define.ConnectStatus.TryingToJoin;
        
        DeviceObject.OnTouchAnyDevice -= StartConnect;
        DeviceObject.OnTouchAnyDevice += StopConnect;
    }

    private void StopConnect(DeviceObject selectedDevice)
    {
        if (connectStatus == Define.ConnectStatus.LeaveParty) return;
        if (selectedDevice != userOwnDeviceObject) return;

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
        
        DeviceObject.OnTouchAnyDevice -= StopConnect;
        DeviceObject.OnTouchAnyDevice += StartConnect;
    }

    public IEnumerator SynchronizeDevicesRoutine()
    {
        yield return null;
    }

    public void ClientSendOwnColorToServer()
    {
        
    }
}