#if UNITY_EDITOR
using System.Collections;
using Game.GameType.Roman;
using Game.GameType.Roman.ClientSide;
using Game.Manager.GameManage;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestManager : Singleton<TestManager>
{
    #region Fairytale
    [MenuItem("FuncTest/Fairytale/A_LoadFairytaleScene")]
    public static void LoadFairytaleScene()
    {
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.FairytaleScene);
    }

    [MenuItem("FuncTest/Fairytale/B_UnsetOverlay")]
    public static void UnsetOverlay()
    {
        Overlay.UnsetActiveOverlay();
    }
    
    [MenuItem("FuncTest/Fairytale/C_SelectTortoiseCard")]
    public static void SelectTortoiseCard()
    {
        Fairytale_Scene.Instance.TheHareAndTheTortoise();
    }
    
    [MenuItem("FuncTest/Fairytale/D_CreateStory")]
    public static void CreateStory()
    {
        Fairytale_Scene.Instance.card.cardData.storyLine = Fairytale_StorylineFactory.GetStoryLine(3, 12);
    }
    
    [MenuItem("FuncTest/Fairytale/E_ShowNextCard")]
    public static void ShowNextCard()
    {
        var timeStep = Fairytale_Scene.Instance.card.cardData.timeStep;
        Fairytale_Scene.Instance.card.StoryUnfoldsByTimeStep(timeStep);
        Fairytale_Scene.Instance.card.cardData.timeStep++;
    }

    [MenuItem("FuncTest/Fairytale/F_GrayScale")]
    public static void GrayScale()
    {
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }

    [MenuItem("FuncTest/Fairytale/H_SelectMemoCard")]
    public static void SelectMemoCard()
    {
        Fairytale_Scene.Instance.ThereAreAlwaysMemos();
    }
    
    [MenuItem("FuncTest/Fairytale/I_CreateStory")]
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
    
    [MenuItem("FuncTest/Fairytale/J_ShowNextCard")]
    public static void ShowNextCard_Memo()
    {
        var timeStep = Fairytale_Scene.Instance.card.cardData.timeStep;
        Fairytale_Scene.Instance.card.StoryUnfoldsByTimeStep(timeStep);
        Fairytale_Scene.Instance.card.cardData.timeStep++;
    }
    
    [MenuItem("FuncTest/Fairytale/K_ShowSuccessSceneUI")]
    public static void ShowSuccessSceneUI()
    {
        UIManager.Instance.SceneUIRoot = GameObject.Find("SceneUIRoot").transform;
        UIManager.Instance.ShowSceneUI("Prefabs/SuccessUICanvas", 0);
    }
    
    [MenuItem("FuncTest/Fairytale/L_ResultPopup")]
    public static void ShowResultPopup()
    {
        UIManager.Instance.PopupUIRoot = GameObject.Find("PopupUIRoot").transform;
        UIManager.Instance.ShowPopup("Prefabs/ResultPopupCanvas", 9);
    }
    #endregion
    
    #region Roman
    
    [MenuItem("FuncTest/Roman/A_LoadRomanScene")]
    public static void LoadRomanScene()
    {
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.RomanScene);
    }
    
    [MenuItem("FuncTest/Roman/B_CreateCard")]
    public static void CreateCard()
    {
        (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard(CardType.A);
    }
    
    [MenuItem("FuncTest/Roman/C_ReplaceCard")]
    public static void ReplaceCard()
    {
        (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.E);
    }
    
    [MenuItem("FuncTest/Roman/D_AnimTest")]
    public static void AnimTest()
    {
        IEnumerator Routine()
        {
            (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard(CardType.A);
            yield return new WaitForSeconds(7f);
            
            for (int i = 0; i < 30; i++)
            {
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.B);
                yield return new WaitForSeconds(10f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.C);
                yield return new WaitForSeconds(7f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.D);
                yield return new WaitForSeconds(10f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.E);
                yield return new WaitForSeconds(7f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.A);
                yield return new WaitForSeconds(10f);
            }
        }

        GameManager.Instance.GameScene.StartCoroutine(Routine());
    }
    
    [MenuItem("FuncTest/Roman/E_ShowResultPopup")]
    public static void ShowResultPopupRoman()
    {
        (GameManager.Instance.GameScene as RomanGameScene)?.ShowResultPopup();
    }
    #endregion
    
    [MenuItem("FuncTest/Roman/_GameExit")]
    public static void GameExit()
    {
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.ConnectScene);
    }
}
#endif