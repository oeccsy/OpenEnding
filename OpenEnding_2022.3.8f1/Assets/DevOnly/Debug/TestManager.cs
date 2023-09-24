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
    
    [MenuItem("FuncTest/SelectTortoiseCard")]
    public static void SelectTortoiseCard()
    {
        Fairytale_Scene.Instance.TheHareAndTheTortoise();
    }
    
    [MenuItem("FuncTest/TheNumberCard")]
    public static void SelectNumberCard()
    {
        Fairytale_Scene.Instance.TheNumber();
    }
}
#endif