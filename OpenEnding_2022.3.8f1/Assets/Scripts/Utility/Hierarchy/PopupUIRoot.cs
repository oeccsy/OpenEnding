using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Hierarchy;

public class PopupUIRoot : RootObject
{
    protected override void Register()
    {
        UIManager.Instance.PopupUIRoot = transform;
    }
}
