using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    private static Image _image;
    private Canvas _canvas;
    private static Sequence _overlaySequence;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _canvas = transform.parent.GetComponent<Canvas>();

        SceneManager.sceneLoaded += (scene, mode) => SetCanvas();
    }

    public static void SetActiveOverlay()
    {
        if (_overlaySequence.IsActive()) return;

        _overlaySequence = DOTween.Sequence()
            .Append(_image.DOFade(1f, 2f));
    }

    public static void UnsetActiveOverlay()
    {
        if (_overlaySequence.IsActive()) return;

        _overlaySequence = DOTween.Sequence()
            .Append(_image.DOFade(0f, 2f));
    }

    private void SetCanvas()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.allCameras[(int)Define.CameraIndex.UI];
    }
}
