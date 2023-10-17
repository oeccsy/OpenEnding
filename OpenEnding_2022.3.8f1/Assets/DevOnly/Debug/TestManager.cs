#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    [MenuItem("FuncTest/A_LoadFairytaleScene")]
    public static void LoadFairytaleScene()
    {
        GameManager.Instance.GameFlow.LoadFairytaleScene();
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
        Fairytale_Scene.Instance.card.cardData.storyLine = Fairytale_StorylineFactory.GetStoryLine(3, 12);
    }
    
    [MenuItem("FuncTest/E_ShowNextCard")]
    public static void ShowNextCard()
    {

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
        Fairytale_Scene.Instance.card.cardData.storyLine = Fairytale_StorylineFactory.GetStoryLine(3, 12);
    }
    
    [MenuItem("FuncTest/J_ShowNextCard")]
    public static void ShowNextCard_Memo()
    {

    }
    
    [MenuItem("FuncTest/K_ShowSuccessSceneUI")]
    public static void ShowSuccessSceneUI()
    {
        UIManager.Instance.SceneUIRoot = GameObject.Find("SceneUIRoot").transform;
        UIManager.Instance.ShowSceneUI("Prefabs/SuccessUICanvas", 0);
    }
    
    [MenuItem("FuncTest/L_ResultPopup")]
    public static void ShowResultPopup()
    {
        UIManager.Instance.PopupUIRoot = GameObject.Find("PopupUIRoot").transform;
        UIManager.Instance.ShowPopup("Prefabs/ResultPopupCanvas", 9);
    }
    
    [MenuItem("FuncTest/M_GameExit")]
    public static void GameExit()
    {
        GameManager.Instance.GameFlow.LoadConnectScene();
    }
    
    

}
#endif