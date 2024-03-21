using System.Collections;
using System.Collections.Generic;
using Game.GameType.Roman.Card;
using Game.Manager.GameManage;
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
                var newCardData = new RomanCardData();
                newCardData.cardType = (CardType)randomNumbers[i];
                newCardData.networkDevice = NetworkManager.Instance.connectedDeviceList[i];
                
                _cardContainer.cardList.Add(newCardData);
                StartCoroutine(NetworkManager.Instance.SendBytesToTargetDevice(newCardData.networkDevice, new byte[] {0, (byte)newCardData.cardType}));
            }
        }
    }
}