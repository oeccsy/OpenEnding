using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.OnBoarding
{
    public class OnBoardingUI : MonoBehaviour
    {
        public int contentIndex = 0;

        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private RectTransform contentRect;
        [SerializeField]
        private DragEventHandler contentDragEventHandler;
        [SerializeField]
        private Image[] progressDots;
    
        private void Awake()
        {
            contentDragEventHandler.OnDragEnd += SnapContent;
            contentDragEventHandler.OnDragEnd += UpdateProgressDot;
        }
    
        private void SnapContent(PointerEventData eventData)
        {
            scrollRect.velocity = Vector2.zero;
            scrollRect.enabled = false;
            contentDragEventHandler.dragable = false;
        
            var targetIndex = (int)(-1 * contentRect.anchoredPosition.x + 540) / 1080;
            Debug.Log(targetIndex);
        
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(contentRect.DOAnchorPos(new Vector2(-1080, 0) * targetIndex, 0.3f, true))
                .AppendCallback(() => scrollRect.enabled = true)
                .AppendCallback(() => contentDragEventHandler.dragable = true);
        }
    
        private void UpdateProgressDot(PointerEventData eventData)
        {
            var targetIndex = (int)(-1 * contentRect.anchoredPosition.x + 540) / 1080;
            var prevDot = progressDots[contentIndex];
            var nextDot = progressDots[targetIndex];

            if (targetIndex == contentIndex) return;
        
            contentIndex = targetIndex;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(prevDot.DOColor(ColorPalette.GetColor(ColorPalette.ColorName.ProgressDotDefault), 0.3f))
                .Join(nextDot.transform.DOPunchScale(Vector3.one, 0.3f))
                .Join(nextDot.DOColor(ColorPalette.GetColor(ColorPalette.ColorName.ProgressDotActive), 0.3f));
        }
    }
}
