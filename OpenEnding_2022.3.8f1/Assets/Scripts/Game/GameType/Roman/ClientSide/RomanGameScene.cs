﻿using System.Collections;
using System.Collections.Generic;
using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.ClientSide.UI;
using Game.Manager.GameManage;
using UnityEngine;
using Utility;
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
                
                yield return new WaitForSeconds(0.5f);
                
                CreateCard(cardType);
                yield return card.Show();
            }

            StartCoroutine(ReplaceRoutine());
        }

        public void ShowCard()
        {
            StartCoroutine(card.Show());
            GameManager.Instance.PlayerController.flip.OnNextTail += ()=> StartCoroutine(card.Hide());
            GameManager.Instance.PlayerController.flip.OnNextTail += ()=> StartCoroutine(card.cardInfoUI.Hide());
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