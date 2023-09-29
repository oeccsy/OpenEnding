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
    
    [MenuItem("FuncTest/D_CreateStory")]
    public static void CreateStory()
    {
        var component = FindObjectOfType<TheHareAndTheTortoise>();
        component.CreateStoryLine(5, 15);
    }
    
    [MenuItem("FuncTest/E_ShowNextCard")]
    public static void ShowNextCard()
    {
        var component = FindObjectOfType<TheHareAndTheTortoise>();
        component.StoryUnfoldsByTimeStep(0);
    }
}
#endif