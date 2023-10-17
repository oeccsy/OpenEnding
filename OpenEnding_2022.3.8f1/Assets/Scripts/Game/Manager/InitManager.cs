using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    private static bool isInit = false;
    private void Awake()
    {
        if (isInit) return;
        
        DOTween.Init(false, false, LogBehaviour.ErrorsOnly).SetCapacity(100, 20);
        ColorPalette.Init();

        var prefab = Resources.Load<GameObject>("Prefabs/DontDestroyOnLoadContainer");
        Instantiate(prefab);
    }

    private IEnumerator Start()
    {
        if (isInit) yield break;
        
        yield return new WaitForSecondsRealtime(2f);
        
        var bottomSheet = UIManager.Instance.ShowPopup("Prefabs/OnBoardingBottomSheet");
        bottomSheet.transform.SetParent(DontDestroyOnLoadContainer.Instance.transform);

        isInit = true;
        
#if DEVELOPMENT_BUILD || UNITY_EDITOR || UNITY_STANDALONE_WIN
        DebugCanvas.Instance.InitDebugCanvas();
#endif
    }
}