using System;
using System.Collections;
using DG.Tweening;
using Game.Manager.GameManage;
using UnityEngine;

public abstract class Fairytale_Card : MonoBehaviour
{
    public Fairytale_CardData cardData;

    protected virtual void Awake()
    {
        cardData = new Fairytale_CardData(NetworkManager.Instance.ownDeviceColor);
    }

    private void Start()
    {
        var flip = GameManager.Instance.PlayerController.flip;
        
        flip.OnFlipToHead += () => cardData.displayedFace = Define.DisplayedFace.Head;
        flip.OnFlipToTail += () => cardData.displayedFace = Define.DisplayedFace.Tail;
    }

    public void InitCardStory(int goal, int runningTime)
    {
        cardData.goal = goal;
        cardData.runningTime = runningTime;
        cardData.storyLine = Fairytale_StorylineFactory.GetStoryLine(goal, runningTime);
    }

    public abstract void StoryUnfoldsByTimeStep(int timeStep);
    public abstract void GiveUp();
}