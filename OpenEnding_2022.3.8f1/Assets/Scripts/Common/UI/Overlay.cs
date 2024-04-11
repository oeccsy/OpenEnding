using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public static RawImage image;
    public static Sequence overlaySequence;
    public static bool isOverlayActive = false;
    
    private Canvas _canvas;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        _canvas = transform.parent.GetComponent<Canvas>();

        SceneManager.sceneLoaded += (scene, mode) => SetCanvas();
    }

    public static void SetActiveOverlay()
    {
        if (overlaySequence.IsActive()) return;

        isOverlayActive = true;
        
        overlaySequence = DOTween.Sequence()
            .Append(image.DOFade(1f, 1f));
    }

    public static void UnsetActiveOverlay()
    {
        if (overlaySequence.IsActive()) return;

        overlaySequence = DOTween.Sequence()
            .Append(image.DOFade(0f, 1f))
            .AppendCallback( ()=> isOverlayActive = false );
    }

    private void SetCanvas()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.allCameras[(int)Define.CameraIndex.UI];
    }
}