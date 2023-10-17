using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUIRoot : RootObject
{
    protected override void Register()
    {
        UIManager.Instance.SceneUIRoot = transform;
    }
}
