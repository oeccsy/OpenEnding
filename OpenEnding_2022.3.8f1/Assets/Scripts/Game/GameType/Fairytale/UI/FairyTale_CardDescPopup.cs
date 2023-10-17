using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FairyTale_CardDescPopup : MonoBehaviour, IUIElement
{
    [SerializeField]
    private RectTransform popupRect;

    public IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(5f);
        Hide();
    }
    
    public void Show()
    {
        Dimmed.SetDimmed(true);
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(popupRect.DOAnchorPosY(popupRect.anchoredPosition.y, 1f).From(Vector2.up * 3200).SetEase(Ease.OutCubic));
    }

    public void Hide()
    {
        Dimmed.SetDimmed(false);
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(popupRect.DOScale(Vector3.zero, 0.3f))
            .AppendCallback(() => Destroy(this.gameObject));
    }
}
