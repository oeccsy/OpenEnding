using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyContainer : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
