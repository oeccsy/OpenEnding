using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheHareAndTheTortoise : Fairytale_Card
{
    public int step = -1;
    public int targetGoal = 5;
    public int achievement = 0;
    public List<Define.Story> storyLine = new List<Define.Story>();
    public List<int> achievementProgress = new List<int>();
    
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;

    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
        CreateStoryLine(5, 15);
        hare.transform.position = Vector3.left + Vector3.forward * targetGoal * 0.5f;
        tortoise.transform.position = Vector3.zero;
        
    }

    public override void Update()
    {
#if !UNITY_EDITOR
        base.Update();
#endif
    }
    
    public override void CreateStoryLine(int goal, int runningTime)
    {
        int temporaryAchievement = 0;

        for (int i = 0; i < runningTime; i++)
        {
            SelectStory(i, runningTime, ref temporaryAchievement, goal);
        }
    }

    public override void SelectStory(int timeStep, int runningTime, ref int temporaryAchievement, int goal)
    {
        List<Define.Story> availableStory = new List<Define.Story>();

        if (timeStep + 1 == runningTime || temporaryAchievement + 1 != goal)
        {
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
        }

        if (temporaryAchievement + runningTime - timeStep - 1 >= goal)
        {
            availableStory.Add(Define.Story.Standstill);
            availableStory.Add(Define.Story.Standstill);
        }

        if (temporaryAchievement - 1 + runningTime - timeStep - 1 >= goal && temporaryAchievement > 0)
        {
            availableStory.Add(Define.Story.TakeStepBack);
            availableStory.Add(Define.Story.TakeStepBack);
        }

        if (runningTime - timeStep - 1 >= goal && temporaryAchievement > 0)
        {
            availableStory.Add(Define.Story.LoseAll);
        }

        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
            
        switch (availableStory[Random.Range(0, availableStory.Count)])
        {
            case Define.Story.LoseAll:
                temporaryAchievement = 0;
                storyLine.Add(Define.Story.LoseAll);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.Standstill:
                storyLine.Add(Define.Story.Standstill);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.TakeStepBack:
                temporaryAchievement -= 1;
                storyLine.Add(Define.Story.TakeStepBack);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.TakeOneStep:
                temporaryAchievement += 1;
                storyLine.Add(Define.Story.TakeOneStep);
                achievementProgress.Add(temporaryAchievement);
                break;
        }
    }

    private void TortoiseRunFast()
    {
        achievement += 1;
        tortoise.ActNaturally(Define.Act.Run);
        tortoise.Shape(Define.Shape.Eyes_Annoyed);
        
        hare.transform.position = Vector3.left + Vector3.forward * targetGoal * 0.5f;
        tortoise.transform.position = Vector3.forward * achievement * 0.5f;
    }

    private void TortoiseRunSlow()
    {
        achievement += 0;
        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Sad);
        
        hare.transform.position = Vector3.left + Vector3.forward * targetGoal * 0.5f;
        tortoise.transform.position = Vector3.forward * achievement * 0.5f;
    }

    private void TortoiseDance()
    {
        achievement -= 1;
        tortoise.ActNaturally(Define.Act.Spin);
        tortoise.Shape(Define.Shape.Eyes_Happy);
        
        hare.transform.position = Vector3.left + Vector3.forward * targetGoal * 0.5f;
        tortoise.transform.position = Vector3.forward * achievement * 0.5f;
        tortoise.speed = 0f;
    }

    private void TortoiseAccident()
    {
        achievement = 0;
        tortoise.ActNaturally(Define.Act.Death);
        tortoise.Shape(Define.Shape.Eyes_Dead);
        
        hare.transform.position = Vector3.left + Vector3.forward * targetGoal * 0.5f;
        tortoise.transform.position = Vector3.forward * achievement * 0.5f;
        tortoise.speed = 0f;
    }

    private void ShowResult()
    {
        
    }

    [ContextMenu("FuncTest/ShowNextStep")]
    public override void ShowNextStep()
    {
        if (achievement == targetGoal) return;
        
        step++;
        switch (storyLine[step])
        {
            case Define.Story.LoseAll:
                TortoiseAccident();
                break;
            case Define.Story.Standstill:
                TortoiseRunSlow();
                break;
            case Define.Story.TakeOneStep:
                TortoiseRunFast();
                break;
            case Define.Story.TakeStepBack:
                TortoiseDance();
                break;
        }

        if (achievement == targetGoal)
        {
            ShowResult();
        }
    }
}