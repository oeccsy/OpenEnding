using System.Collections;
using System.Collections.Generic;
using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.ClientSide.UI;
using Game.Manager.GameManage;
using UnityEngine;
using Utility.Hierarchy;

namespace Game.GameType.Roman.ClientSide
{
    public class RomanGameScene : GameScene
    {
        public RomanCard card;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void CreateCard(CardType cardType)
        {
            GameObject prefab = null;
            
            switch (cardType)
            {
                case CardType.A :
                    prefab = Resources.Load<GameObject>("Prefabs/Roman/A");
                    break;
                case CardType.B :
                    prefab = Resources.Load<GameObject>("Prefabs/Roman/B");
                    break;
                case CardType.C :
                    prefab = Resources.Load<GameObject>("Prefabs/Roman/C");
                    break;
                case CardType.D :
                    prefab = Resources.Load<GameObject>("Prefabs/Roman/D");
                    break;
                case CardType.E :
                    prefab = Resources.Load<GameObject>("Prefabs/Roman/E");
                    break;
            }

            card = Instantiate(prefab, GameObjectRoot.Transform).GetComponent<RomanCard>();
        }

        public void ReplaceCard(CardType cardType)
        {
            $"Replace to {cardType}".Log();
            if (card == null) return;

            IEnumerator ReplaceRoutine()
            {
                yield return card.Hide();
                yield return card.cardInfoUI.Hide();
                
                Destroy(card.gameObject);
                CreateCard(cardType);
            }

            StartCoroutine(ReplaceRoutine());
        }

        public void ShowResultPopup()
        {
            var instance = UIManager.Instance.ShowPopup("Prefabs/Roman/ResultPopup", 9);
            var resultPopup = instance.GetComponent<ResultPopup>();
            var gameState = GameManager.Instance.GameState as RomanGameState;
            
            resultPopup.RefreshResultText(gameState.winner);
            resultPopup.Show();
        }
    }
}