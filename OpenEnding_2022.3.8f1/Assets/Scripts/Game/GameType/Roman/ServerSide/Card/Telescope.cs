using System;
using System.Collections;
using System.Linq;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;
using Game.GameType.Roman.ClientSide;
using Game.GameType.Roman.ServerSide;
using UnityEngine;

namespace Game.GameType.Roman
{
    public class Telescope : RomanCard, IFlipAbility
    {
        public Telescope()
        {
            cardType = CardType.Telescope;
        }
        
        public void FlipAbility()
        {
            IEnumerator DiscoverRoutine()
            {
                var gameMode = GameManager.Instance.GameMode as RomanGameMode;
                var cards = gameMode.cardContainer.GetCards<RomanCard>();
                cards = cards.Where(card => card.cardType != CardType.Telescope).ToList();
            
                var targetCard = cards[UnityEngine.Random.Range(0, cards.Count)];
                NetworkManager.Instance.ClientRpcCall(deviceColor, typeof(RomanGameScene), "DiscoverCard", targetCard.cardType);

                yield return new WaitForSeconds(7f);
                gameMode.DiscoverCard(targetCard.cardType);
            }

            GameManager.Instance.StartCoroutine(DiscoverRoutine());
        }

        public override void OnEnterField() {}
        public override void OnExitField() {}
    }
}