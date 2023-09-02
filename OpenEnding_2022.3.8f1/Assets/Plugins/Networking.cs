using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Shatalmic
{
    public class Networking : MonoBehaviour
    {
        private float _timeout = 0f;
        private States _state = States.None;
        private NetworkDevice _deviceToConnect = null;
        private NetworkDevice _deviceToDisconnect = null;

        public class Characteristic
        {
            public string ServiceUUID;
            public string CharacteristicUUID;
            public bool Found;
        }

        public static List<Characteristic> Characteristics = new List<Characteristic>
        {
            new Characteristic { ServiceUUID = "37200001-7638-4216-B629-96AD40F79AA1", CharacteristicUUID = "37200002-7638-4216-B629-96AD40F79AA1", Found = false },
            new Characteristic { ServiceUUID = "37200001-7638-4216-B629-96AD40F79AA1", CharacteristicUUID = "37200003-7638-4216-B629-96AD40F79AA1", Found = false },
        };

        public Characteristic SampleCharacteristic = Characteristics[0];
        public Characteristic CommandCharacteristic = Characteristics[1];

        public bool AllCharacteristicsFound { get { return !(Characteristics.Where(c => c.Found == false).Any()); } }
        public Characteristic GetCharacteristic(string serviceUUID, string characteristicsUUID)
        {
            return Characteristics.Where(c => IsEqual(serviceUUID, c.ServiceUUID) && IsEqual(characteristicsUUID, c.CharacteristicUUID)).FirstOrDefault();
        }

        public class NetworkDevice
        {
            public string Name;
            public string Address;
            public bool Connected;
            public bool DoDisconnect;
        }

        public List<NetworkDevice> NetworkDeviceList;

        enum States
        {
            None,
            StartScan,
            RestartScan,
            Connect,
            RequestMTU,
            WriteMTUToClient,
            Subscribe,
            NextUpdateCharactersticValuePacket,
            Disconnect,
            Disconnecting,
        }

        void SetState(States newState, float timeout)
        {
            _state = newState;
            _timeout = timeout;
        }

        void Reset()
        {
            _timeout = 0f;
            _state = States.None;
            _networkName = null;
            _deviceToConnect = null;
            _deviceToDisconnect = null;
            NetworkDeviceList = new List<NetworkDevice>();
        }

        public Action<string> OnError = (error) =>
        {
            BluetoothLEHardwareInterface.Log("Error: " + error);
        };

        public Action<string> OnStatusMessage = (message) =>
        {
            BluetoothLEHardwareInterface.Log(message);
        };

        public Action<NetworkDevice> OnDeviceReady;
        public Action<NetworkDevice> OnDeviceDisconnected;
        public Action<NetworkDevice, string, byte[]> OnDeviceData;

        public void Initialize(Action<string> onError, Action<string> onStatusMessage)
        {
            BluetoothLEHardwareInterface.Log("BLNE: Bluetooth Low Energy Networking Initialize");

            if (onError != null)
                OnError = onError;
            if (onStatusMessage != null)
                OnStatusMessage = onStatusMessage;

            BluetoothLEHardwareInterface.Initialize(true, true, () =>
            {

            }, (error1) =>
            {
                if (error1.Contains("Peripheral mode is Not Available"))
                {
                    if (OnError != null)
                        OnError("Client mode not available");

                    // try central only mode
                    BluetoothLEHardwareInterface.Initialize(true, false, () =>
                    {

                    }, (error2) =>
                    {
                        if (OnError != null)
                            OnError(error2);
                    });
                }
                else
                {
                    if (OnError != null)
                        OnError(error1);
                }
            });
        }

        bool _serverStarted = false;
        string _networkName;
        public void StartServer(string networkName, Action<NetworkDevice> onDeviceReady, Action<NetworkDevice> onDeviceDisconnected, Action<NetworkDevice, string, byte[]> onDeviceData)
        {
            BluetoothLEHardwareInterface.Log(string.Format("server network: {0}", networkName));

            if (!_serverStarted)
            {
                Reset();

                _serverStarted = true;
                _networkName = networkName;

                OnDeviceReady = onDeviceReady;
                OnDeviceDisconnected = onDeviceDisconnected;
                OnDeviceData = onDeviceData;

                SetState(States.StartScan, 0.1f);
            }
        }

        public Action OnFinishedStoppingServer = null;
        public void StopServer(Action onFinishedStoppingServer)
        {
            if (_serverStarted)
            {
                BluetoothLEHardwareInterface.StopScan();

                if (NetworkDeviceList != null && NetworkDeviceList.Count > 0)
                {
                    OnFinishedStoppingServer = onFinishedStoppingServer;
                    foreach (var device in NetworkDeviceList)
                        device.DoDisconnect = true;
                    SetState(States.Disconnect, 0.1f);
                }
                else if (onFinishedStoppingServer != null)
                {
                    onFinishedStoppingServer();
                    _serverStarted = false;
                }
            }
            else if (onFinishedStoppingServer != null)
            {
                onFinishedStoppingServer();
                _serverStarted = false;
            }
        }

        public void WriteDevice(NetworkDevice device, byte[] bytes, Action onWritten)
        {
            BluetoothLEHardwareInterface.WriteCharacteristic(device.Address, SampleCharacteristic.ServiceUUID, SampleCharacteristic.CharacteristicUUID, bytes, bytes.Length, true, (Characteristic) =>
            {
                if (onWritten != null)
                    onWritten();
            });
        }

        public bool ClientAdvertising;

        public void StartClient(string networkName, string clientName, Action onStartedAdvertising, Action<string, string, byte[]> onCharacteristicWritten)
        {
            Reset();

            BluetoothLEHardwareInterface.Log(string.Format("client network: {0}, client name: {1}", networkName, clientName));

            BluetoothLEHardwareInterface.PeripheralName(networkName + ":" + clientName);

            BluetoothLEHardwareInterface.RemoveServices();
            BluetoothLEHardwareInterface.RemoveCharacteristics();

            BluetoothLEHardwareInterface.CBCharacteristicProperties properties =
                BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyRead |
                BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyWrite |
                BluetoothLEHardwareInterface.CBCharacteristicProperties.CBCharacteristicPropertyNotify;

            BluetoothLEHardwareInterface.CBAttributePermissions permissions =
                BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsReadable |
                BluetoothLEHardwareInterface.CBAttributePermissions.CBAttributePermissionsWriteable;

            Action<string, byte[]> action = (characteristic, bytes) =>
            {
                if (characteristic.ToUpper().Equals(CommandCharacteristic.CharacteristicUUID))
                {
                    if (bytes.Length >= 2 && bytes[0] == 0xA5 && bytes[1] > 0)
                    {
                        _mtu = bytes[1];
                        StatusMessage = $"Client MTU set to {_mtu}";
                    }
                }
                else
                {
                    if (onCharacteristicWritten != null)
                        onCharacteristicWritten(clientName, characteristic, bytes);
                }
            };

            BluetoothLEHardwareInterface.CreateCharacteristic(SampleCharacteristic.CharacteristicUUID, properties, permissions, null, 5, action);
            BluetoothLEHardwareInterface.CreateCharacteristic(CommandCharacteristic.CharacteristicUUID, properties, permissions, null, 5, action);

            BluetoothLEHardwareInterface.CreateService(SampleCharacteristic.ServiceUUID, true, (characteristic) =>
            {
                StatusMessage = "Created service";
            });

            BluetoothLEHardwareInterface.StartAdvertising(() =>
            {
                if (onStartedAdvertising != null)
                    onStartedAdvertising();

                ClientAdvertising = true;
            });
        }

        private byte[] _totalBytes = null;
        private int _sendPosition = 0;
        private int _mtu = 17;
        protected void _sendFromClient()
        {
            if (_totalBytes != null)
            {
                int numberOfBytes = _totalBytes.Length - _sendPosition;
                if (numberOfBytes > 0)
                {
                    byte[] bytesToSend = null;
                    if (numberOfBytes > _mtu)
                        numberOfBytes = _mtu;

                    bytesToSend = new byte[numberOfBytes];
                    Array.Copy(_totalBytes, _sendPosition, bytesToSend, 0, numberOfBytes);

                    StatusMessage = "SendFromClient next packet";
                    BluetoothLEHardwareInterface.UpdateCharacteristicValue(SampleCharacteristic.CharacteristicUUID, bytesToSend, bytesToSend.Length);
                    _sendPosition += bytesToSend.Length;

                    if (_sendPosition < _totalBytes.Length)
                        SetState(States.NextUpdateCharactersticValuePacket, 0.05f);
                }
            }
        }

        public void SendFromClient(byte[] bytes)
        {
            StatusMessage = "SendFromClient";

            if (bytes.Length <= _mtu)
            {
                StatusMessage = "Direct send from client";
                BluetoothLEHardwareInterface.UpdateCharacteristicValue(SampleCharacteristic.CharacteristicUUID, bytes, bytes.Length);
            }
            else
            {
                _totalBytes = bytes;
                _sendPosition = 0;

                _sendFromClient();
            }
        }

        public void StopClient(Action onStoppedAdvertising)
        {
            BluetoothLEHardwareInterface.StopAdvertising(() =>
            {
                if (onStoppedAdvertising != null)
                    onStoppedAdvertising();

                ClientAdvertising = false;
            });
        }

        public string StatusMessage
        {
            set
            {
                if (OnStatusMessage != null)
                    OnStatusMessage(value);
            }
        }

        void Start()
        {
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            if (_timeout > 0f)
            {
                _timeout -= Time.deltaTime;
                if (_timeout <= 0f)
                {
                    _timeout = 0f;

                    switch (_state)
                    {
                        case States.None:
                            if (_deviceToConnect == null)
                            {
                                StatusMessage = "Can connected";
                                _deviceToConnect = NetworkDeviceList.Where(d => !d.Connected).Select(d => d).FirstOrDefault();
                                if (_deviceToConnect != null)
                                {
                                    StatusMessage = string.Format("Need connect: {0}", _deviceToConnect.Name);
                                    SetState(States.Connect, 0.1f);
                                }
                            }
                            break;

                        case States.StartScan:
                            SetState(States.RestartScan, 5f);
                            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, deviceName) =>
                            {
                                if (deviceName.StartsWith(_networkName))
                                {
                                    StatusMessage = "Found " + address;

                                    if (NetworkDeviceList == null)
                                        NetworkDeviceList = new List<NetworkDevice>();

                                    var checkForDevice = NetworkDeviceList.Where(d => d.Name.Equals(deviceName)).Select(d => d).FirstOrDefault();
                                    if (checkForDevice == null)
                                    {
                                        NetworkDeviceList.Add(new NetworkDevice { Name = deviceName, Address = address, Connected = false });

                                        if (_deviceToConnect == null)
                                            SetState(States.None, 0.01f);
                                    }
                                }

                            });
                            break;

                        case States.RestartScan:
                            StatusMessage = "Restarting scanning";
                            BluetoothLEHardwareInterface.StopScan();
                            SetState(States.StartScan, 0.01f);
                            break;

                        case States.Connect:
                            if (_deviceToConnect != null)
                            {
                                StatusMessage = string.Format("Connecting to {0}, {1}...", _deviceToConnect.Name, _deviceToConnect.Address);

                                BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceToConnect.Address, (deviceAddress) =>
                                {
                                    StatusMessage = string.Format("Connect to {0}...", deviceAddress);
                                }, (serviceDeviceAddress, serviceDeviceUUID) =>
                                {
                                    StatusMessage = string.Format("Service Discovered {0}, {1}...", serviceDeviceAddress, serviceDeviceUUID);
                                }, (address, serviceUUID, characteristicUUID) =>
                                {
                                    StatusMessage = string.Format("Characteristic Discovered {0}, {1}, {2}...", address, serviceUUID, characteristicUUID);
                                    var characteristic = GetCharacteristic(serviceUUID, characteristicUUID);
                                    if (characteristic != null)
                                    {
                                        StatusMessage = string.Format("A characteristics was found {0}", characteristicUUID);
                                        characteristic.Found = true;

                                        if (AllCharacteristicsFound)
                                        {
                                            StatusMessage = string.Format("All characteristics found");
                                            _deviceToConnect.Connected = true;
                                            SetState(States.RequestMTU, 2f);
                                        }
                                    }
                                }, (disconnectAddress) =>
                                {
                                    var networkDevice = NetworkDeviceList.Where(d => d.Address.Equals(disconnectAddress)).Select(d => d).FirstOrDefault();
                                    if (networkDevice != null)
                                    {
                                        StatusMessage = "Disconnected from " + networkDevice.Name;
                                        if (OnDeviceDisconnected != null && OnDeviceDisconnected != null)
                                            OnDeviceDisconnected(networkDevice);

                                        NetworkDeviceList.Remove(networkDevice);
                                        StatusMessage = string.Format("1 device count: {0}", NetworkDeviceList.Count);
                                        if (networkDevice == _deviceToDisconnect)
                                        {
                                            _deviceToDisconnect = null;
                                            SetState(States.Disconnect, 0.1f);
                                        }
                                    }
                                });
                            }
                            break;

                        case States.RequestMTU:
                            {
                                _state = States.None;
                                StatusMessage = "Request MTU";
                                BluetoothLEHardwareInterface.RequestMtu(_deviceToConnect.Address, 128, (deviceAddress, newMTU) =>
                                {
                                    _mtu = newMTU - 3;
                                    StatusMessage = "MTU set to " + newMTU.ToString();
                                    SetState(States.WriteMTUToClient, 0.01f);
                                });
                            }
                            break;

                        case States.WriteMTUToClient:
                            {
                                _state = States.None;
                                StatusMessage = "Writing MTU to client";
                                BluetoothLEHardwareInterface.WriteCharacteristic(_deviceToConnect.Address, CommandCharacteristic.ServiceUUID, CommandCharacteristic.CharacteristicUUID, new byte[] { 0xA5, (byte)_mtu }, 2, true, (characteristicWritten) =>
                                {
                                    StatusMessage = "MTU Written to client";
                                    SetState(States.Subscribe, 0.05f);
                                });
                            }
                            break;

                        case States.Subscribe:
                            _state = States.None;
                            StatusMessage = "Subscribe to device";
                            BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceToConnect.Address, SampleCharacteristic.ServiceUUID, SampleCharacteristic.CharacteristicUUID, (deviceAddressNotify, characteristicNotify) =>
                            {
                                var networkDevice = NetworkDeviceList.Where(d => d.Address.Equals(deviceAddressNotify)).Select(d => d).FirstOrDefault();
                                if (networkDevice != null && OnDeviceData != null)
                                {
                                    OnDeviceReady(networkDevice);
                                    _deviceToConnect = null;
                                    StatusMessage = string.Format("Device completely connected to {0} command", networkDevice.Name);
                                    SetState(States.None, 0.1f);
                                }
                            }, (deviceAddressData, characteristricData, bytes) =>
                            {
                                var networkDevice = NetworkDeviceList.Where(d => d.Address.Equals(deviceAddressData)).Select(d => d).FirstOrDefault();
                                if (networkDevice != null && OnDeviceData != null)
                                    OnDeviceData(networkDevice, characteristricData, bytes);
                            });
                            break;

                        case States.NextUpdateCharactersticValuePacket:
                            _state = States.None;
                            _sendFromClient();
                            break;

                        case States.Disconnect:
                            _deviceToDisconnect = NetworkDeviceList.Where(d => d.DoDisconnect).Select(d => d).FirstOrDefault();
                            if (_deviceToDisconnect != null)
                            {
                                SetState(States.Disconnecting, 5f);
                                if (_deviceToDisconnect.Connected)
                                {
                                    BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceToDisconnect.Address, (address) =>
                                    {
                                        // since we have a callback for disconnect in the connect method above, we don't
                                        // need to process the callback here.
                                    });
                                }
                                else
                                {
                                    NetworkDeviceList.Remove(_deviceToDisconnect);
                                    StatusMessage = string.Format("2 device count: {0}", NetworkDeviceList.Count);

                                    _deviceToDisconnect = null;
                                    _state = States.None;
                                }
                            }
                            else
                            {
                                _state = States.None;
                                if (OnFinishedStoppingServer != null)
                                {
                                    OnFinishedStoppingServer();
                                    OnFinishedStoppingServer = null;
                                }

                                _serverStarted = false;
                            }
                            break;

                        case States.Disconnecting:
                            if (_deviceToDisconnect != null && NetworkDeviceList != null && NetworkDeviceList.Contains(_deviceToDisconnect))
                            {
                                StatusMessage = string.Format("3 device count: {0}", NetworkDeviceList.Count);

                                NetworkDeviceList.Remove(_deviceToDisconnect);
                                _deviceToDisconnect = null;
                            }

                            // if we got here we timed out disconnecting, so just go to disconnected state
                            SetState(States.Disconnect, 0.1f);
                            break;
                    }
                }
            }
        }

        bool IsEqual(string uuid1, string uuid2)
        {
            return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
        }
    }
}
