using System;
using System.Collections;
using System.Collections.Generic;
using Shatalmic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SealGame_GameFramework : GameFramework
{
    [SerializeField]
    private Define.SealGameStep curStep = Define.SealGameStep.GameReady;
    [SerializeField]
    private SealGame_CardContainer sealGameCardContainer;
    [SerializeField]
    private SealGame_ByteToFunc sealGameByteToFunc;

    protected override IEnumerator GameFrameworkCoroutine()
    {
        yield return GameReady();                            // 각각은 Game Step으로 생각하기, Step 에 따라 BLEAction이 달라지도록
        yield return SelectOneSeal();
        yield return ShowCard();
    }

    private IEnumerator GameReady()
    {
        DebugText.Instance.AddText("GameReady");
        if (ConnectManager.Instance.connectType == Define.ConnectType.Server)
        {
            SealGame_CardContainer.Instance.InitDeviceDataList();    
        }
        
        yield return new WaitForSecondsRealtime(1f);
    }

    private IEnumerator SelectOneSeal()
    {
        DebugText.Instance.AddText("SelectOneSeal");
        if (ConnectManager.Instance.connectType == Define.ConnectType.Server)
        {
            // device 선택을 위한 device index list 생성
            List<int> deviceIndex = new List<int>();
            
            for (int i = 0; i < SealGame_CardContainer.Instance.deviceDataList.Count; i++)
            {
                deviceIndex.Add(i);
            }
            
            // 무작위로 device index 선택
            int randomNum = Random.Range(0, deviceIndex.Count);
            int sealIndex = deviceIndex[randomNum];
            deviceIndex.RemoveAt(randomNum);
            
            // 선택한 device는 Seal 당첨
            SealGame_CardContainer.Instance.SetDeviceData(sealIndex, Define.SealGameCardType.Seal);

            DebugText.Instance.AddText($"Device Num {sealIndex} 물개 당첨!");
            // // 다른 device는 Hunter 당첨
            // for (int i = 0; i < deviceIndex.Count; i++)
            // {
            //     SealGame_CardContainer.Instance.SetDeviceData(deviceIndex[i], Define.SealGameCardType.Hunter);
            // }
        }
        
        yield return new WaitForSecondsRealtime(1f);
    }

    private IEnumerator ShowCard()
    {
        DebugText.Instance.AddText("ShowCard");
        // Client Set Action
        if (ConnectManager.Instance.connectType == Define.ConnectType.Client)
        {
            ConnectManager.Instance.bleClient.OnReceiveData += SealGame_DeviceCardManager.Instance.SetCardImage;
        }
        
        yield return new WaitForSecondsRealtime(2f);

        // Send Bytes to Clients
        if (ConnectManager.Instance.connectType == Define.ConnectType.Server)
        {
            for (int i = 0; i < SealGame_CardContainer.Instance.deviceDataList.Count; i++)
            {
                Networking.NetworkDevice targetDevice = SealGame_CardContainer.Instance.deviceDataList[i].networkDevice;
                byte[] bytes = new byte[1];

                if (SealGame_CardContainer.Instance.deviceDataList[i].cardType == Define.SealGameCardType.Seal)
                {
                    bytes[0] = 1;
                }
                else
                {
                    bytes[0] = 2;
                }
                
                yield return ConnectManager.Instance.bleServer.SendBytesToTargetDevice(targetDevice, bytes);
            }
        }
        
        // Check server Card
        if (ConnectManager.Instance.connectType == Define.ConnectType.Server && SealGame_CardContainer.Instance.deviceDataList[1].cardType == Define.SealGameCardType.Seal)
        {
            SealGame_DeviceCardManager.Instance.SetCardImageSeal();
        }
    }
}
