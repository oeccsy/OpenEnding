using DG.Tweening;
using Game.GameType.Roman.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameType.Roman.ClientSide.UI
{
    public class CardInfoUI : MonoBehaviour, IUIElement
    {
        private TextMeshProUGUI _cardNameText;
        private TextMeshProUGUI _cardDescText;

        [SerializeField]
        private Image _lineImage;
        [SerializeField]
        private Image _mask;
        
        private void Awake()
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>();
            _cardNameText = texts[0];
            _cardDescText = texts[1];
        }

        private void Start()
        {
            Show();
        }
        
        public void RefreshUI(RomanCardInfo cardInfo)
        {
            _cardNameText.text = cardInfo.cardName;
            _cardDescText.text = cardInfo.cardDesc;
        }

        public void Show()
        {
            _lineImage.fillOrigin = 0;
            _lineImage.fillAmount = 0;
            _cardNameText.color = Color.clear;
            _cardDescText.color = Color.clear;
            float nameTextPosY = _cardNameText.transform.localPosition.y;
            float descTextPosY = _cardDescText.transform.localPosition.y;
            
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(_lineImage.DOFillAmount(1f, 1f).From(0).SetEase(Ease.InOutCirc))
                .AppendInterval(0.5f)
                .Append(_cardNameText.DOFade(1, 0.5f).From(0))
                .Join(_cardNameText.transform.DOLocalMoveY(nameTextPosY, 0.5f).From(nameTextPosY + 10))
                .Join(_cardDescText.DOFade(1, 0.5f).From(0))
                .Join(_cardDescText.transform.DOLocalMoveY(descTextPosY, 0.5f).From(descTextPosY + 10))
                .Join(_mask.DOFade(0, 0.5f).From(1));
        }

        public void Hide()
        {
            _lineImage.fillOrigin = 1;
            
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(_cardNameText.DOFade(0, 0.5f).From(1))
                .Join(_cardDescText.DOFade(0, 0.5f).From(1))
                .Join(_mask.DOFade(1, 0.5f).From(0))
                .Append(_lineImage.DOFillAmount(0f, 0.5f).SetEase(Ease.InOutCirc));
        }
    }
}