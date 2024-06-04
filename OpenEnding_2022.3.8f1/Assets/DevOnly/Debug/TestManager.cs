#if UNITY_EDITOR
using System.Collections;
using Base;
using Game.GameType.Roman;
using Game.GameType.Roman.ClientSide;
using Game.GameType.Roman.ClientSide.Card;
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
        (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard(CardType.RoleModel);
    }
    
    [MenuItem("FuncTest/Roman/C_ReplaceCard")]
    public static void ReplaceCard()
    {
        (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.RoleModel);
    }
    
    [MenuItem("FuncTest/Roman/D_AnimTest")]
    public static void AnimTest()
    {
        IEnumerator Routine()
        {
            (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard(CardType.Star);
            yield return (GameManager.Instance.GameScene as RomanGameScene)?.card.Show();
            yield return new WaitForSeconds(7f);
            
            for (int i = 0; i < 30; i++)
            {
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.Telescope);
                yield return new WaitForSeconds(10f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.Artwork);
                yield return new WaitForSeconds(7f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.Sprout);
                yield return new WaitForSeconds(10f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.RoleModel);
                yield return new WaitForSeconds(7f);
                (GameManager.Instance.GameScene as RomanGameScene)?.ReplaceCard(CardType.Star);
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
    
    [MenuItem("FuncTest/Roman/F_ShowRequestFlipToTail")]
    public static void ShowStartPlayer()
    {
        (GameManager.Instance.GameScene as RomanGameScene)?.ShowRequestFlipToTailPopup();
    }
    
    [MenuItem("FuncTest/Roman/G_ShowCard")]
    public static void ShowCard()
    {
        (GameManager.Instance.GameScene as RomanGameScene)?.ShowCard();
    }
    
    [MenuItem("FuncTest/Roman/H_HideCard")]
    public static void HideCard()
    {
        IEnumerator Routine()
        {
            yield return (GameManager.Instance.GameScene as RomanGameScene)?.card.Hide();
            yield return (GameManager.Instance.GameScene as RomanGameScene)?.card.cardInfoUI.Hide();
        }
        
        GameManager.Instance.GameScene.StartCoroutine(Routine());
    }
    
    [MenuItem("FuncTest/Roman/I_EndterDiscoveryMode")]
    public static void EndterDiscoveryMode()
    {
        IEnumerator Routine()
        {
            (GameManager.Instance.GameScene as RomanGameScene)?.CreateCard(CardType.Telescope);
            yield return (GameManager.Instance.GameScene as RomanGameScene)?.card.Show();
        }
        
        GameManager.Instance.GameScene.StartCoroutine(Routine());
    }
    
    #endregion
    
    [MenuItem("FuncTest/Roman/_GameExit")]
    public static void GameExit()
    {
        GameManager.Instance.GameFlow.LoadScene(Define.SceneType.ConnectScene);
    }
}
#endif