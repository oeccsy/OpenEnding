using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    private static bool _isInit = false;
    
    private void Awake()
    {
        if (_isInit) return;
        
        DOTween.Init(false, false, LogBehaviour.ErrorsOnly).SetCapacity(100, 20);
        ColorPalette.Init();
        
        var dontDestroyOnLoadContainerPrefab = Resources.Load<GameObject>("Prefabs/DontDestroyOnLoadContainer");
        Instantiate(dontDestroyOnLoadContainerPrefab);
    }

    private IEnumerator Start()
    {
        if (_isInit) yield break;
        
        yield return new WaitForSecondsRealtime(2f);
        
        // var bottomSheet = UIManager.Instance.ShowPopup("Prefabs/OnBoardingBottomSheet", 9);
        // bottomSheet.transform.SetParent(DontDestroyOnLoadContainer.Instance.transform);
        
#if DEVELOPMENT_BUILD || UNITY_EDITOR || UNITY_STANDALONE_WIN
        DebugCanvas.Instance.InitDebugCanvas();
#endif
        
        _isInit = true;
    }
}