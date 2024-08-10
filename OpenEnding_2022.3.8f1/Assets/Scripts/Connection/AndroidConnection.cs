using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidConnection : Singleton<AndroidConnection>
{
  private AndroidJavaClass _androidUtilsClass;
  private AndroidJavaObject _androidUtilsInstance;

  private AndroidJavaClass _bluetoothLEUtilsClass;
  private AndroidJavaObject _bluetoothLEUtilsInstance;

  private AndroidJavaClass _centralClass;
  private AndroidJavaObject _centralInstance;

  private AndroidJavaClass _peripheralClass;
  private AndroidJavaObject _peripheralInstance;

  public delegate void BluetoothConnectHandler(string deviceName);
  public event BluetoothConnectHandler OnBluetoothDeviceConnected;
  public event BluetoothConnectHandler OnBluetoothDeviceDisconnected;

  public delegate void BluetoothDataHandler(byte[] data);
  public event BluetoothDataHandler OnBluetoothDataReceive;

  protected override void Awake()
  {
    DontDestroyOnLoad(gameObject);

    _androidUtilsClass = new AndroidJavaClass("com.oeccsy.openending_ble.AndroidUtils");
    _androidUtilsInstance = _androidUtilsClass.CallStatic<AndroidJavaObject>("getInstance");

    _bluetoothLEUtilsClass = new AndroidJavaClass("com.oeccsy.openending_ble.BluetoothLEUtils");
    _bluetoothLEUtilsInstance = _bluetoothLEUtilsClass.CallStatic<AndroidJavaObject>("getInstance");

    _centralClass = new AndroidJavaClass("com.oeccsy.openending_ble.Central");
    _centralInstance = _centralClass.CallStatic<AndroidJavaObject>("getInstance");

    _peripheralClass = new AndroidJavaClass("com.oeccsy.openending_ble.Peripheral");
    _peripheralInstance = _peripheralClass.CallStatic<AndroidJavaObject>("getInstance");

    Toast("Hello From Android!");
}

  public void Toast(string text)
  {
    _androidUtilsClass.CallStatic("toast", text);
  }

  public void IsBluetoothLEFeatureExist()
  {
    Toast(_bluetoothLEUtilsInstance.Call<bool>("isBluetoothLEFeatureExist").ToString());
  }

  public void HasBluetoothLEPermissions()
  {
    Toast(_bluetoothLEUtilsInstance.Call<bool>("hasBluetoothLEPermissions").ToString());
  }

  public void IsBluetoothLEEnable()
  {
    Toast(_bluetoothLEUtilsInstance.Call<bool>("isBluetoothLEEnable").ToString());
  }

  public void RequestPermissions()
  {
    _bluetoothLEUtilsInstance.Call("requestPermissions");
  }

  public void InitCentralBluetoothSystem()
  {
    _centralInstance.Call("initBluetoothSystem");
  }

  public void StartScanning(string deviceName)
  {
    _centralInstance.Call("startScanning", deviceName);
  }

  public void StopScanning()
  {
    _centralInstance.Call("stopScanning");
  }

  public void Write(string deviceName, byte[] data)
  {
    _centralInstance.Call("write", deviceName, data);
  }


  public void InitPeripheralBluetoothSystem()
  {
    _peripheralInstance.Call("initBluetoothSystem");
  }

  public void StartAdvertising(string deviceName)
  {
    _peripheralInstance.Call("startAdvertising", deviceName);
  }

  public void StopAdvertising()
  {
    _peripheralInstance.Call("stopAdvertising");
  }

  public void StartServer()
  {
    _peripheralInstance.Call("startServer");
  }

  public void StopServer()
  {
    _peripheralInstance.Call("stopServer");
  }

  public void Indicate(byte[] data)
  {
    _peripheralInstance.Call("indicate", data);
  }


  public void OnDeviceConnected(string deviceName)
  {
    OnBluetoothDeviceConnected?.Invoke(deviceName);
  }

  public void OnDeviceDisconnected(string deviceName)
  {
    OnBluetoothDeviceDisconnected?.Invoke(deviceName);
  }

  public void OnDataReceive(string encodedData)
  {
    byte[] bytes = System.Convert.FromBase64String(encodedData);
    OnBluetoothDataReceive?.Invoke(bytes);
  }
}
