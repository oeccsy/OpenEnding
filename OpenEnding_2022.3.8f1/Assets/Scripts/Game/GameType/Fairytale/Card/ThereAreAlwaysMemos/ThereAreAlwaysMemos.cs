using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThereAreAlwaysMemos : Fairytale_Card
{
    public List<Memo> memoList = new List<Memo>();
    public List<Vector2> availableMemoPosList = new List<Vector2>();
    
    public List<Define.Story> storyLine = new List<Define.Story>();
    public List<int> achievementProgress = new List<int>();
    private Coroutine currentStoryRoutine = null;
    
    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
        InitMemo();
        cardData.goal = 3;
        cardData.achievement = 0;
    }

    public override void Update()
    {
#if !UNITY_EDITOR
        base.Update();
#endif
    }

    private void InitMemo()
    {
        availableMemoPosList = PoissonDiscSampling.GeneratePoints(1.8f, new Vector2(8, 5));
        var prefab = Resources.Load<GameObject>("Prefabs/Memo");

        for (int i = 0; i < 3; i++)
        {
            var instance = Instantiate(prefab, transform);
            instance.transform.position = new Vector3(availableMemoPosList[i].x, availableMemoPosList[i].y, 0);
            memoList.Add(instance.GetComponent<Memo>());
        }
    }

    private void AddMemo()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Memo");
        var instance = Instantiate(prefab, transform);
        instance.transform.position = new Vector3(availableMemoPosList[cardData.goal - cardData.achievement].x, availableMemoPosList[cardData.goal - cardData.achievement].y, 0);
        memoList.Add(instance.GetComponent<Memo>());
        
        cardData.achievement--;
    }

    private void RemoveMemo()
    {
        var targetMemo = memoList[cardData.goal - cardData.achievement - 1];
        memoList.Remove(targetMemo);
        Destroy(targetMemo.gameObject);

        cardData.achievement++;
    }
    
    public override void CreateStoryLine(int goal, int runningTime)
    {
        $"Create Story Line : {goal}, {runningTime}".Log();
        cardData.runningTime = runningTime;
        cardData.goal = goal;

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
        }

        if (temporaryAchievement + runningTime - timeStep - 1 >= goal)
        {
            availableStory.Add(Define.Story.Standstill);
        }

        if (temporaryAchievement - 1 + runningTime - timeStep - 1 >= goal && temporaryAchievement > 0)
        {
            availableStory.Add(Define.Story.TakeStepBack);
        }

        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
            
        switch (availableStory[Random.Range(0, availableStory.Count)])
        {
            case Define.Story.Standstill:
                storyLine.Add(Define.Story.Standstill);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.TakeStepBack:
                temporaryAchievement--;
                storyLine.Add(Define.Story.TakeStepBack);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.TakeOneStep:
                temporaryAchievement++;
                storyLine.Add(Define.Story.TakeOneStep);
                achievementProgress.Add(temporaryAchievement);
                break;
        }
    }
    
    private void ShowResult()
    {
        
    }
    
    [ContextMenu("FuncTest/ShowNextStep")]
    public override void StoryUnfoldsByTimeStep(int timeStep)
    {
        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
        if (cardData.runningTime <= timeStep) return;

        cardData.timeStep = timeStep;
        
        Debug.Log($"{timeStep} : {storyLine[timeStep].ToString()}");
        
        switch (storyLine[timeStep])
        {
            case Define.Story.TakeOneStep:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                //currentStoryRoutine = StartCoroutine(TortoiseRunFast());
                RemoveMemo();
                break;
            case Define.Story.TakeStepBack:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                //currentStoryRoutine = StartCoroutine(TortoiseDance());
                AddMemo();
                break;
        }

        if (cardData.achievement == cardData.goal)
        {
            ShowResult();
        }
    }
}
