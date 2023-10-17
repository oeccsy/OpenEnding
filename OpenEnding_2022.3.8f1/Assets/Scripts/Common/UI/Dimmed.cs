using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dimmed : MonoBehaviour, IUIElement
{
    public static Image image;
    private Canvas _canvas;
    
    private void Awake()
    {
        image = GetComponent<Image>();
        _canvas = transform.parent.GetComponent<Canvas>();

        SceneManager.sceneLoaded += (scene, mode) => SetCanvas();
    }

    public static void SetDimmed(bool active)
    {
        if (active)
        {
            image.DOFade(0.7f, 0.3f).SetEase(Ease.OutCirc);
        }
        else
        {
            image.DOFade(0f, 0.3f).SetEase(Ease.OutCirc);
        }
    }
    
    private void SetCanvas()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.allCameras[(int)Define.CameraIndex.UI];
    }
    
    public void Show()
    {
        image.DOFade(0.7f, 0.3f).SetEase(Ease.OutCirc);
    }

    public void Hide()
    {
        image.DOFade(0f, 0.3f).SetEase(Ease.OutCirc);   
    }
}
