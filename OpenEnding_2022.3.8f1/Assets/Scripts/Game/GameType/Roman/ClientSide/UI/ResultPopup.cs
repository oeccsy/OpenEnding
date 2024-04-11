using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameType.Roman.ClientSide.UI
{
    public class ResultPopup : MonoBehaviour, IUIElement
    {
        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private Image lineImage;
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private TextMeshProUGUI resultText;

        private void Awake()
        {
            backgroundImage.color = Color.clear;
            lineImage.fillAmount = 0;
            titleText.alpha = 0;
            resultText.alpha = 0;
        }
        
        public void RefreshResultText(ColorPalette.ColorName winPlayer)
        {
            resultText.text = $"{winPlayer.ToString()} 승리";
        }
        
        public void Show()
        {
            lineImage.fillOrigin = 0;
            
            float titleTextPosY = titleText.transform.localPosition.y;
            float resultTextPosY = resultText.transform.localPosition.y;
            
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(backgroundImage.DOColor(new Color(253/255f, 253/255f, 253/255f), 1f))
                .Append(lineImage.DOFillAmount(1f, 1f).From(0).SetEase(Ease.InOutCirc))
                .AppendInterval(0.5f)
                .Append(titleText.DOFade(1, 0.5f).From(0))
                .Join(titleText.transform.DOLocalMoveY(titleTextPosY, 0.5f).From(titleTextPosY + 10))
                .Join(resultText.DOFade(1, 0.5f).From(0))
                .Join(resultText.transform.DOLocalMoveY(resultTextPosY, 0.5f).From(resultTextPosY + 10));

        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}