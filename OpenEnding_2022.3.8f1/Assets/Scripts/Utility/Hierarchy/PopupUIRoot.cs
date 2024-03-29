using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIRoot : RootObject
{
    protected override void Register()
    {
        UIManager.Instance.PopupUIRoot = transform;
    }
}
