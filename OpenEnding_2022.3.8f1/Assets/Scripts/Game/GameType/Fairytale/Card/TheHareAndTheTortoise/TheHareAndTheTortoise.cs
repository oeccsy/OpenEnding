using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheHareAndTheTortoise : Fairytale_Card
{
    public List<Define.Story> storyLine = new List<Define.Story>();
    public List<int> achievementProgress = new List<int>();
    private Coroutine currentStoryRoutine = null;
    
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;

    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
    }

    public override void Update()
    {
#if !UNITY_EDITOR
        base.Update();
#endif
    }
    
    public override void CreateStoryLine(int goal, int runningTime)
    {
        DebugCanvas.Instance.SetText("");
        $"Create Story Line : {goal}, {runningTime}".Log();
        cardData.runningTime = runningTime;
        cardData.goal = goal;

        int temporaryAchievement = 0;

        for (int i = 0; i < runningTime; i++)
        {
            SelectStory(i, runningTime, ref temporaryAchievement, goal);
            //$"story : {storyLine[i]}".Log();
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

    private IEnumerator TortoiseRunFast()
    {
        "OneStep".Log();
        cardData.achievement += 1;
        
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Run);
        tortoise.Shape(Define.Shape.Eyes_Annoyed);
        tortoise.speed = 8f;
        
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("Done");

        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
    }

    private IEnumerator TortoiseRunSlow()
    {
        "Slow".Log();
        yield return null;
        cardData.achievement += 0;
        
        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Sad);
    }

    private IEnumerator TortoiseDance()
    {
        "Dance".Log();
        cardData.achievement -= 1;
        
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Spin);
        tortoise.Shape(Define.Shape.Eyes_Happy);
        tortoise.speed = 0f;
        
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("Done");
        
        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
    }

    private IEnumerator TortoiseAccident()
    {
        "Accident".Log();
        hare.transform.position = Vector3.left + Vector3.forward * cardData.goal * 0.5f;
        tortoise.transform.position = Vector3.forward * cardData.achievement * 0.5f;

        var delay = cardData.achievement * 2f;
        cardData.achievement = 0;
        
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Death);
        tortoise.Shape(Define.Shape.Eyes_Dead);
        tortoise.speed = 0f;
        
        yield return new WaitForSecondsRealtime(delay);
        Debug.Log("Done");
        
        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
    }

    private void ShowResult()
    {
        
    }

    [ContextMenu("FuncTest/ShowNextStep")]
    public override void StoryUnfoldsByTimeStep(int timeStep)
    {
        if (cardData.cardStatus != Define.FairyTailGameCardStatus.None) return;
        if (cardData.runningTime <= timeStep) return;
        
        "StoryUnFolds".Log();
        
        cardData.timeStep = timeStep;
        
        switch (storyLine[timeStep])
        {
            case Define.Story.LoseAll:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseAccident());
                break;
            case Define.Story.Standstill:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseRunSlow());
                break;
            case Define.Story.TakeOneStep:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseRunFast());
                break;
            case Define.Story.TakeStepBack:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseDance());
                break;
        }
        
        "StoryUnFolds Done".Log();

        if (cardData.achievement == cardData.goal)
        {
            ShowResult();
        }
    }
}