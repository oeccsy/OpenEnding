#if UNITY_EDITOR
using Game.Manager.GameManage;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    [MenuItem("FuncTest/A_LoadRomanScene")]
    public static void LoadRomanScene()
    {
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.RomanScene);
    }
    
    [MenuItem("FuncTest/A_LoadFairytaleScene")]
    public static void LoadFairytaleScene()
    {
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.FairytaleScene);
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
        var timeStep = Fairytale_Scene.Instance.card.cardData.timeStep;
        Fairytale_Scene.Instance.card.StoryUnfoldsByTimeStep(timeStep);
        Fairytale_Scene.Instance.card.cardData.timeStep++;
    }

    [MenuItem("FuncTest/F_GrayScale")]
    public static void GrayScale()
    {
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }

    [MenuItem("FuncTest/H_SelectMemoCard")]
    public static void SelectMemoCard()
    {
        Fairytale_Scene.Instance.ThereAreAlwaysMemos();
    }
    
    [MenuItem("FuncTest/I_CreateStory")]
    public static void CreateStory_Memo()
    {
        Fairytale_Scene.Instance.card.InitCardStory(3, 12);
        // Fairytale_Scene.Instance.card.cardData.storyLine.Clear();
        //
        // for (int i = 0; i < 10; i++)
        // {
        //     Fairytale_Scene.Instance.card.cardData.storyLine.Add(Define.Story.TakeStepBack);
        //     Fairytale_Scene.Instance.card.cardData.storyLine.Add(Define.Story.TakeOneStep);    
        // }
        
    }
    
    [MenuItem("FuncTest/J_ShowNextCard")]
    public static void ShowNextCard_Memo()
    {
        var timeStep = Fairytale_Scene.Instance.card.cardData.timeStep;
        Fairytale_Scene.Instance.card.StoryUnfoldsByTimeStep(timeStep);
        Fairytale_Scene.Instance.card.cardData.timeStep++;
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
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.ConnectScene);
    }
}
#endif