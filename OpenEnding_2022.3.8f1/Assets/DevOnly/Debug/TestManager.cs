#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [MenuItem("FuncTest/A_LoadFairytaleScene")]
    public static void LoadFairytaleScene()
    {
        Connect_Scene.Instance.LoadFairytaleScene(null);
    }

    [MenuItem("FuncTest/B_UnsetOverlay")]
    public static void UnsetOverlay()
    {
        Overlay.UnsetActiveOverlay();
    }
    
    [MenuItem("FuncTest/C_SelectTortoiseCard")]
    public static void SelectTortoiseCard()
    {
        Fairytale_Scene.Instance.TheHareAndTheTortoise();
    }
    
    [MenuItem("FuncTest/D_ShowNextCard")]
    public static void ShowNextCard()
    {
        var component = FindObjectOfType<TheHareAndTheTortoise>();
        component.ShowNextStep();
    }
}
#endif