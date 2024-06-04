using System.Collections;
using System.Collections.Generic;
using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.ClientSide.UI;
using Game.Manager.GameManage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using Utility.Hierarchy;

namespace Game.GameType.Roman.ClientSide
{
    public class RomanGameScene : GameScene
    {
        public RomanCard card;
        public RomanCard discoveryCard;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void ShowStartPlayerPopup()
        {
            IEnumerator ShowRoutine()
            {
                StartPlayerPopup popup = UIManager.Instance.ShowPopup("Prefabs/Roman/StartPlayerPopup", 9).GetComponentInChildren<StartPlayerPopup>();
                yield return new WaitForSeconds(4f);
                popup.Hide();    
            }

            StartCoroutine(ShowRoutine());
        }

        public void ShowRequestFlipToTailPopup()
        {
            RequestFlipToTailPopup popup = UIManager.Instance.ShowPopup("Prefabs/Roman/RequestFlipToTailPopup", 9).GetComponentInChildren<RequestFlipToTailPopup>();
            
            (GameManager.Instance.PlayerController as RomanPlayerController).flip.OnNextTail += popup.Hide;
        }

        public void CreateCard(CardType cardType)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Roman/{cardType.ToString()}");
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
                
                yield return new WaitForSeconds(0.5f);
                
                CreateCard(cardType);
                yield return card.Show();
            }

            StartCoroutine(ReplaceRoutine());
        }

        public void DiscoverCard(CardType cardType)
        {
            $"Discover {cardType}".Log();
            if (card == null) return;
            
            IEnumerator DiscoverRoutine()
            {
                yield return card.Hide();
                yield return card.cardInfoUI.Hide();
                
                yield return new WaitForSeconds(0.5f);
                
                var prefab = Resources.Load<GameObject>($"Prefabs/Roman/{cardType.ToString()}");
                discoveryCard = Instantiate(prefab, GameObjectRoot.Transform).GetComponent<RomanCard>();
                yield return discoveryCard.Show();
                
                RomanGameState gameState = GameManager.Instance.GameState as RomanGameState;
                yield return new WaitUntil(() => gameState?.curStep == GameStep.HideCard);
                
                GameManager.Instance.PlayerController.flip.OnNextTail += ()=> StartCoroutine(discoveryCard.Hide());
                GameManager.Instance.PlayerController.flip.OnNextTail += ()=> StartCoroutine(discoveryCard.cardInfoUI.Hide());
            }
            
            StartCoroutine(DiscoverRoutine());
        }

        public void ShowCard()
        {
            IEnumerator ShowRoutine()
            {
                yield return card.Show();
                
                RomanGameState gameState = GameManager.Instance.GameState as RomanGameState;
                yield return new WaitUntil(() => gameState?.curStep == GameStep.HideCard);
                
                GameManager.Instance.PlayerController.flip.OnNextTail += ()=> StartCoroutine(card.Hide());
                GameManager.Instance.PlayerController.flip.OnNextTail += ()=> StartCoroutine(card.cardInfoUI.Hide());
            }

            StartCoroutine(ShowRoutine());
        }

        public void ShowResultPopup()
        {
            var instance = UIManager.Instance.ShowPopup("Prefabs/Roman/ResultPopup", 9);
            var resultPopup = instance.GetComponent<ResultPopup>();
            var gameState = GameManager.Instance.GameState as RomanGameState;
            
            resultPopup.RefreshResultText(gameState.curPlayer);
            resultPopup.Show();
            
            resultPopup.button.onClick.AddListener(()=> SceneManager.LoadScene(Define.SceneType.ConnectScene.ToString()));
            resultPopup.button.onClick.AddListener(()=> resultPopup.button.onClick.RemoveAllListeners());
        }
    }
}