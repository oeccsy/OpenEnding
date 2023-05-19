using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BytesTest : MonoBehaviour
{
    void Start()
    {
        var bytes = new byte[4];
        bytes[0] = (byte)'A';
        bytes[1] = (byte)'A';
        bytes[2] = (byte)'A';
        bytes[3] = (byte)'A';
        Debug.Log(bytes);
        Debug.Log(bytes.ToString());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
