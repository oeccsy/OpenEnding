#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [MenuItem("FuncTest/LoadFairytaleScene")]
    public static void LoadFairytaleScene()
    {
        Connect_Scene.Instance.LoadFairytaleScene(null);
    }
}
#endif