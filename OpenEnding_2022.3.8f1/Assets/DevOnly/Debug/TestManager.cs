#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TestManager : Singleton<TestManager>
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
        component.StoryUnfoldsByTimeStep(component.cardData.timeStep);
        component.cardData.timeStep++;
    }

    [MenuItem("FuncTest/F_GrayScale")]
    public static void GrayScale()
    {
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }
    
    [MenuItem("FuncTest/G_NetworkTest")]
    public void SendTest()
    {
        StartCoroutine(NetworkManager.Instance.SendBytesToAllDevice(new byte[] { 3, 0, 0 }));
    }
    
    [MenuItem("FuncTest/H_SelectMemoCard")]
    public static void SelectMemoCard()
    {
        Fairytale_Scene.Instance.ThereAreAlwaysMemos();
    }
    
    [MenuItem("FuncTest/I_CreateStory")]
    public static void CreateStory_Memo()
    {
        var component = FindObjectOfType<ThereAreAlwaysMemos>();
        component.CreateStoryLine(3, 15);
    }
    
    [MenuItem("FuncTest/J_ShowNextCard")]
    public static void ShowNextCard_Memo()
    {
        var component = FindObjectOfType<ThereAreAlwaysMemos>();
        component.StoryUnfoldsByTimeStep(component.cardData.timeStep);
        component.cardData.timeStep++;
    }

}
#endif