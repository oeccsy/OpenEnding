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
    private void Awake()
    {
        _androidUtilsClass = new AndroidJavaClass("com.oeccsy.openending_ble.AndroidUtils");
        _androidUtilsInstance = _androidUtilsClass.CallStatic<AndroidJavaObject>("getInstance");
        
        _bluetoothLEUtilsClass = new AndroidJavaClass("com.oeccsy.openending_ble.BluetoothLEUtils");
        _bluetoothLEUtilsInstance = _bluetoothLEUtilsClass.CallStatic<AndroidJavaObject>("getInstance");
        
        _networkingClass = new AndroidJavaClass("com.oeccsy.openending_ble.Networking");
        _networkingInstance = _networkingClass.CallStatic<AndroidJavaObject>("getInstance");
        
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



    public void InitBluetoothSystem()
    {
        _networkingInstance.Call("initBluetoothSystem");
    }
    
    public void StartAdvertising()
    {
        _networkingInstance.Call("startAdvertising");
    }
    
    public void StopAdvertising()
    {
        _networkingInstance.Call("stopAdvertising");
    }

    public void StartServer()
    {
        _networkingInstance.Call("startServer");
    }
    
    public void StopServer()
    {
        _networkingInstance.Call("stopServer");
    }
    
  
    
    public void StartScanning()
    {
        _networkingInstance.Call("startScanning");
    }
    
    public void StopScanning()
    {
        _networkingInstance.Call("stopScanning");
    }
    
    
    public void Write()
    {
        _networkingInstance.Call("write");
    }
}
