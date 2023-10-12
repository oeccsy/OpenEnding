using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init(false, false, LogBehaviour.ErrorsOnly).SetCapacity(100, 20);
        ColorPalette.Init();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        var bottomSheet = UIManager.Instance.ShowPopup("Prefabs/OnBoardingBottomSheet");
        bottomSheet.transform.SetParent(DontDestroyContainer.Instance.transform);
    }
}