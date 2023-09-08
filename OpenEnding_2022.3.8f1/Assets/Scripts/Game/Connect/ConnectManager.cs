using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;
using UnityEngine.tvOS;

public class ConnectManager : Singleton<ConnectManager>
{
    [SerializeField]
    private Define.ConnectStatus connectStatus = Define.ConnectStatus.LeaveParty;

    private void Start()
    {
        foreach (var deviceObject in Connect_Scene.Instance.deviceObjectList)
        {
            deviceObject.OnTouchDevice += StartConnect;
        }
    }

    private void StartConnect(DeviceObject selectedDevice)
    {
        if (connectStatus != Define.ConnectStatus.LeaveParty) return;
        "Start".Log();   
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
        
        selectedDevice.OnTouchDevice -= StartConnect;
        selectedDevice.OnTouchDevice += StopConnect;
    }

    private void StopConnect(DeviceObject selectedDevice)
    {
        if (connectStatus == Define.ConnectStatus.LeaveParty) return;
        "Stop".Log();
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
    }

    public IEnumerator SynchronizeDevicesRoutine()
    {
        yield return null;
    }

    public void ClientSendOwnColorToServer()
    {
        
    }
}