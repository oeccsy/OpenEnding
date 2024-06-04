using System.Collections;
using Game.GameType.Roman.ClientSide.UI;
using Shatalmic;
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

        public virtual IEnumerator Show()
        {
            yield return cardInfoUI.Show();
            yield return ShowPolygons();
        }
        
        protected abstract IEnumerator ShowPolygons();
        public abstract IEnumerator Hide();

        protected virtual void OnDestroy()
        {
            if (cardInfoUI == null) return;
            Destroy(cardInfoUI.gameObject);
        }
    }
}