using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyTale_SuccessSceneUI : MonoBehaviour, IUIElement
{
    [SerializeField]
    private Image sceneUIImage;
    [SerializeField]
    private TMP_Text resultText;
    [SerializeField]
    private RectTransform resultTextRect;
    [SerializeField]
    private Image tortoiseImage;
    [SerializeField]
    private RectTransform tortoiseImageRect;

    public void Show()
    {
        resultText.alpha = 0f;
        tortoiseImage.color = Color.clear;

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(sceneUIImage.DOFade(1f, 1f).From(0f))
            .Append(resultText.DOFade(1f, 1f))
            .Join(resultTextRect.DOAnchorPosY(resultTextRect.anchoredPosition.y, 1f).From(Vector2.up * 600))
            .Append(tortoiseImage.DOColor(Color.white, 1f))
            .Join(tortoiseImageRect.DOAnchorPosY(tortoiseImageRect.anchoredPosition.y, 1f).From(Vector2.zero));
    }

    public void Hide()
    {
        
    }
}
