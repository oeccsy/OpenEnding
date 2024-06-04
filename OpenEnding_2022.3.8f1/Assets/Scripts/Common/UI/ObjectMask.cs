using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMask : MonoBehaviour
{
    private static Image _image;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
    }
    
    public static void SetMaskColor(Color color, float duration = 2f)
    {
        if (_image == null) return;
        
        _image.DOColor(color, 2f);
    }
}
