using Game.GameType.Roman.Data;
using TMPro;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.UI
{
    public class CardInfoUI : MonoBehaviour
    {
        private TextMeshProUGUI _cardNameText;
        private TextMeshProUGUI _cardDescText;

        private void Awake()
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>();
            _cardNameText = texts[0];
            _cardDescText = texts[1];
        }
        
        public void RefreshUI(RomanCardInfo cardInfo)
        {
            _cardNameText.text = cardInfo.cardName;
            _cardDescText.text = cardInfo.cardDesc;
        }
    }
}