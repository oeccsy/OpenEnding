using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Manager.NetworkManage
{
    public abstract class PacketHandler : MonoBehaviour
    {
        protected Dictionary<Type, object> _instanceDict;

        protected virtual void Awake()
        {
            NetworkManager.Instance.PacketHandler = this;
        }

        private void OnEnable()
        {
            NetworkManager.Instance.OnBluetoothDataReceive += ExcuteMethodByPacket;
        }

        private void ExcuteMethodByPacket(byte[] packet)
        {
            Command command = CommandSerializer.Deserialize(packet);
            $"{command.methodName} arrived packet : {packet.Length}, param : {command.param?.Length}".Log();
            
            Type type = Type.GetType(command.typeName);
            MethodInfo methodInfo = type.GetMethod(command.methodName);
            
            object instance = _instanceDict[type];
            methodInfo.Invoke(instance, command.param);
        }

        private void OnDisable()
        {
            NetworkManager.Instance.OnBluetoothDataReceive -= ExcuteMethodByPacket;
        }
    }
}