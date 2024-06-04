using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Hierarchy;

public class SceneUIRoot : RootObject
{
    public static Transform Transform;
    protected override void Register()
    {
        Transform = transform;
        UIManager.Instance.SceneUIRoot = Transform;
    }
}
