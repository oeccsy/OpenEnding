using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestEvent : MonoBehaviour
{
    public delegate void TestHandler();
    public event TestHandler OnTest;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
