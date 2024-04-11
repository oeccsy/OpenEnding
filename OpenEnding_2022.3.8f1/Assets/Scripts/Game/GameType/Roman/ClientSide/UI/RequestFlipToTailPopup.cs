using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.UI
{
    public class RequestFlipToTailPopup : MonoBehaviour, IUIElement
    {
        [SerializeField]
        private RectTransform popupRect;
        
        public void Show()
        {
            Dimmed.SetDimmed(true);
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(popupRect.DOAnchorPosY(0f, 1f).From(Vector2.up * 3200).SetEase(Ease.OutCubic));
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
}