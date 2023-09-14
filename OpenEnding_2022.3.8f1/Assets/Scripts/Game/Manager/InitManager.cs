using DG.Tweening;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init(false, false, LogBehaviour.ErrorsOnly).SetCapacity(100, 20);
        ColorPalette.Init();
    }
}