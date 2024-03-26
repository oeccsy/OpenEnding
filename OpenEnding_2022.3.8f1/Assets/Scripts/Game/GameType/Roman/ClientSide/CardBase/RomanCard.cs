using System;
using Game.GameType.Roman.ClientSide.UI;
using Shatalmic;
using TMPro;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.CardBase
{
    public abstract class RomanCard : MonoBehaviour
    {
        public CardType cardType = CardType.None;
        public string cardName;
        public string cardDesc;
                
        public Define.DisplayedFace displayedFace = Define.DisplayedFace.None;
        public Networking.NetworkDevice device;

        public CardInfoUI cardInfoUI;

        protected virtual void Awake()
        {
            var cardInfoUIPrefab = Resources.Load<GameObject>("Prefabs/Roman/CardInfoUI");
            cardInfoUI = Instantiate(cardInfoUIPrefab, SceneUIRoot.Transform).GetComponent<CardInfoUI>();
        }

        private void OnDestroy()
        {
            Destroy(cardInfoUI.gameObject);
        }
    }
}