using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Dimmed : MonoBehaviour, IUIElement
{
    public static Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
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
    
    public void Show()
    {
        image.DOFade(0.7f, 0.3f).SetEase(Ease.OutCirc);
    }

    public void Hide()
    {
        image.DOFade(0f, 0.3f).SetEase(Ease.OutCirc);   
    }
}
