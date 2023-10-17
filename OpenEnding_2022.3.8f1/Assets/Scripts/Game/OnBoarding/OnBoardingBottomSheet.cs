using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnBoardingBottomSheet : MonoBehaviour, IUIElement
{
    public int contentIndex = 0;
    public bool isBottomSheetActive = true;
    
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private RectTransform bottomSheetRect;
    [SerializeField]
    private RectTransform contentRect;
    [SerializeField]
    private Image[] progressDots;

    [SerializeField]
    private DragEventHandler contentDragEventHandler;
    [SerializeField]
    private DragEventHandler handleDragEventHandler;
    
    private void Start()
    {
        contentDragEventHandler.OnDragEnd += SnapContent;
        contentDragEventHandler.OnDragEnd += UpdateProgressDot;

        handleDragEventHandler.OnWhileDrag += UpdateBottomSheetPos;
        handleDragEventHandler.OnDragEnd += SnapBottomSheet;
    }

    private void SnapContent(PointerEventData eventData)
    {
        scrollRect.velocity = Vector2.zero;
        scrollRect.enabled = false;
        contentDragEventHandler.dragable = false;
        
        var targetIndex = (int)(-1 * contentRect.anchoredPosition.x + 540) / 1080;

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

    private void SnapBottomSheet(PointerEventData eventData)
    {
        var targetPos = new Vector2(0, eventData.position.y - 2300 - 50);
        if (targetPos.y > -400) targetPos.y = -400;
        if (targetPos.y < -2300) targetPos.y = -2300;

        if (isBottomSheetActive)
        {
            if (targetPos.y < -1000)
            {
                isBottomSheetActive = false;
                Hide();
            }
            else
            {
                Show();
            }
            
        }
        else if (!isBottomSheetActive)
        {
            if (targetPos.y > -2000)
            {
                isBottomSheetActive = true;
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
    
    private void UpdateBottomSheetPos(PointerEventData eventData)
    {
        var targetPos = new Vector2(0, eventData.position.y / Screen.height * 2400 - 2300 - 50);
        if (targetPos.y > -400) targetPos.y = -400;
        if (targetPos.y < -2300) targetPos.y = -2300;
        
        bottomSheetRect.anchoredPosition = targetPos;
    }

    public void Show()
    {
        handleDragEventHandler.dragable = false;
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(bottomSheetRect.DOAnchorPos(new Vector2(0, -400), 0.3f, true))
            .AppendCallback(() => handleDragEventHandler.dragable = true);
        
        Dimmed.SetDimmed(true);
    }

    public void Hide()
    {
        handleDragEventHandler.dragable = false;
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(bottomSheetRect.DOAnchorPos(new Vector2(0, -2300), 0.3f, true))
            .AppendCallback(() => handleDragEventHandler.dragable = true);
        
        Dimmed.SetDimmed(false);
    }
}