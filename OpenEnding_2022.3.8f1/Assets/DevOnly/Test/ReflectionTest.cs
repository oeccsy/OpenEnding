using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using UnityEngine;

public class ReflectionTest : MonoBehaviour
{
    protected Dictionary<Type, object> _instanceDict = new Dictionary<Type, object>();
    public Command command1;
    public Command command2;

    void Start()
    {
        _instanceDict.Add(typeof(ReflectionTest), this);
        Test();
    }

    public void Test()
    {
        Type type = Type.GetType("ReflectionTest");
        byte[] param = {19, 23};

        object[] parameters = new object[param.Length];
        for(int i=0; i<parameters.Length; i++)
        {
            parameters[i] = param[i];
        }
            
        object instance = _instanceDict[type];
        MethodInfo methodInfo = instance.GetType().GetMethod("Log");
        methodInfo.Invoke(instance, parameters);

        Debug.Log(instance.GetType().FullName);
    }

    public void Log(int a, int b)
    {
        Debug.Log($"TestTestTest {a} {b}");
    }

    
}
