using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheHareAndTheTortoise : Fairytale_Card
{
    public List<Define.Story> storyLine = new List<Define.Story>();
    
    public int achievement = 0;
    public List<int> achievementProgress = new List<int>();
    
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }
    
    public override void CreateStoryLine(int goal, int runningTime)
    {
        achievement = 0;

        for (int i = 0; i < runningTime; i++)
        {
            SelectStory(i, runningTime, ref achievement, goal);
        }
    }

    public override void SelectStory(int timeStep, int runningTime, ref int curAchievement, int goal)
    {
        List<Define.Story> availableStory = new List<Define.Story>();

        if (timeStep + 1 == runningTime || curAchievement + 1 != goal)
        {
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
        }

        if (curAchievement + runningTime - timeStep - 1 >= goal)
        {
            availableStory.Add(Define.Story.Standstill);
            availableStory.Add(Define.Story.Standstill);
        }

        if (curAchievement - 1 + runningTime - timeStep - 1 >= goal && curAchievement > 0)
        {
            availableStory.Add(Define.Story.TakeStepBack);
            availableStory.Add(Define.Story.TakeStepBack);
        }

        if (runningTime - timeStep - 1 >= goal && curAchievement > 0)
        {
            availableStory.Add(Define.Story.LoseAll);
        }

        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
            
        switch (availableStory[Random.Range(0, availableStory.Count)])
        {
            case Define.Story.LoseAll:
                curAchievement = 0;
                storyLine.Add(Define.Story.LoseAll);
                achievementProgress.Add(curAchievement);
                break;
            case Define.Story.Standstill:
                storyLine.Add(Define.Story.Standstill);
                achievementProgress.Add(curAchievement);
                break;
            case Define.Story.TakeStepBack:
                curAchievement -= 1;
                storyLine.Add(Define.Story.TakeStepBack);
                achievementProgress.Add(curAchievement);
                break;
            case Define.Story.TakeOneStep:
                curAchievement += 1;
                storyLine.Add(Define.Story.TakeOneStep);
                achievementProgress.Add(curAchievement);
                break;
        }
    }

    [ContextMenu("FuncTest/ShowNextStep")]
    public override void ShowNextStep()
    {
        "Override ShowNextStep".Log();
        if (hare != null)
        {
            "Start Test1".Log();
            Define.Act nextAct = (Define.Act)(((int)hare.curAction + 1) % 17);
            hare.ActNaturally(nextAct);
            "Test1 Done".Log();
        }
        if (tortoise != null)
        {
            "Start Test2".Log();
            Define.Act nextAct = (Define.Act)(((int)hare.curAction + 1) % 17);
            tortoise.ActNaturally(nextAct);
            "Test2 Done".Log();
        }
    }
}