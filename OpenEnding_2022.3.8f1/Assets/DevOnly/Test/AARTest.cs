using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AARTest : MonoBehaviour
{
    private AndroidJavaClass _androidUtilsClass;
    private AndroidJavaObject _androidUtilsInstance;

    private AndroidJavaClass _bluetoothLEUtilsClass;
    private AndroidJavaObject _bluetoothLEUtilsInstance;
    
    private AndroidJavaClass _networkingClass;
    private AndroidJavaObject _networkingInstance;

    private AndroidJavaClass _centralClass;
    private AndroidJavaObject _centralInstance;

    private AndroidJavaClass _peripheralClass;
    private AndroidJavaObject _peripheralInstance;

    private void Awake()
    {
        _androidUtilsClass = new AndroidJavaClass("com.oeccsy.openending_ble.AndroidUtils");
        _androidUtilsInstance = _androidUtilsClass.CallStatic<AndroidJavaObject>("getInstance");
        
        _bluetoothLEUtilsClass = new AndroidJavaClass("com.oeccsy.openending_ble.BluetoothLEUtils");
        _bluetoothLEUtilsInstance = _bluetoothLEUtilsClass.CallStatic<AndroidJavaObject>("getInstance");
        
        _networkingClass = new AndroidJavaClass("com.oeccsy.openending_ble.Networking");
        _networkingInstance = _networkingClass.CallStatic<AndroidJavaObject>("getInstance");

        _centralClass = new AndroidJavaClass("com.oeccsy.openending_ble.Central");
        _centralInstance = _centralClass.CallStatic<AndroidJavaObject>("getInstance");

        _peripheralClass = new AndroidJavaClass("com.oeccsy.openending_ble.Peripheral");
        _peripheralInstance = _peripheralClass.CallStatic<AndroidJavaObject>("getInstance");
        
        Toast("Hello from android!");
    }

    public void Toast(string text)
    {
        _androidUtilsClass.CallStatic("toast", text);
    }

    public void ReturnInt()
    {
        Toast(_androidUtilsClass.CallStatic<int>("returnInt").ToString());
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

    public void StartScanning()
    {
        _centralInstance.Call("startScanning");
    }
    
    public void StopScanning()
    {
        _centralInstance.Call("stopScanning");
    }
    
    public void Write()
    {
        string deviceName = "Test";
        byte[] data = new byte[] {116, 101, 115, 116};
        _centralInstance.Call("write", deviceName, data);
    }



    public void InitPeripheralBluetoothSystem()
    {
        _peripheralInstance.Call("initBluetoothSystem");
    }

    public void StartAdvertising()
    {
        string deviceName = "Test";
        _peripheralInstance.Call("startAdvertising", deviceName, 0);
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

    public void Indicate()
    {
        byte[] data = new byte[] {116, 101, 115, 116};
        _peripheralInstance.Call("indicate", data);
    }
    
    
    public void OnDataReceive(string encodedData)
    {
        Debug.Log(encodedData);
        Toast(encodedData);

        byte[] bytes = System.Convert.FromBase64String(encodedData);
        
        Debug.Log(bytes.ToString());
        Toast(bytes[0].ToString());
    }
}
