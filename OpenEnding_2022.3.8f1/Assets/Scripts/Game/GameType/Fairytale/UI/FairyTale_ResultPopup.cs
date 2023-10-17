using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyTale_ResultPopup : MonoBehaviour, IUIElement
{
    [SerializeField]
    private RectTransform popupRect;
    [SerializeField]
    private TMP_Text successCountText;
    [SerializeField]
    private RectTransform tortoiseImageRect;

    public void RefreshSuccessCountText(int count)
    {
        successCountText.text = $"x{count}";
    }
    
    public void Show()
    {
        successCountText.alpha = 0f;
        
        var gameState = GameManager.Instance.GameState as Fairytale_GameState;
        var successCardCount = gameState.successCardCount;
        successCountText.text = $"x{successCardCount}";

        Dimmed.SetDimmed(true);
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(popupRect.DOAnchorPosY(popupRect.anchoredPosition.y, 2f).From(Vector2.up * 3200).SetEase(Ease.OutBounce))
            .Append(successCountText.DOFade(1f, 1f))
            .Append(tortoiseImageRect.DOPunchScale(Vector3.one * 0.1f, 1f));
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
