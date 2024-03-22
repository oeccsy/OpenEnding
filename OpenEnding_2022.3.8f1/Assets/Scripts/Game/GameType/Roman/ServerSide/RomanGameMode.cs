using System.Collections;
using System.Collections.Generic;
using Game.GameType.Roman.Card;
using Game.Manager.GameManage;
using Shatalmic;
using UnityEngine;

namespace Game.GameType.Roman
{
    public class RomanGameMode : GameMode
    {
        private RomanCardContainer _cardContainer = new RomanCardContainer();
        
        protected override IEnumerator GameRoutine()
        {
            InitDeviceOwnCard();
            
            while (true)
            {
                yield return null;    
            }
        }

        private void InitDeviceOwnCard()
        {
            var randomNumbers = Utils.GetCombinationInt(1, 10, 2);
            Utils.ShuffleList(randomNumbers);

            for (int i = 0; i < NetworkManager.Instance.connectedDeviceList.Count; i++)
            {
                CardType cardType = (CardType)randomNumbers[i];
                Networking.NetworkDevice device = NetworkManager.Instance.connectedDeviceList[i];
                
                _cardContainer.UseCard(cardType, device);
            }
        }
    }
}